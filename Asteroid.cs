using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Asteroid : MonoBehaviour
{
    public Sprite[] _sprites;
    public GameObject _asteroidParticlePrefab;

    /****************************************/

    private Vector3 _target;
    private float _rotation = 0f;
    private float _rotationSpeed;
    private float _speed;
    private Vector3 _direction;
    private bool _paused = false;
    private bool _isStar = false;

    /****************************************/

    public void OnPause(bool pause)
    {
        _paused = pause;
    }

    public void OnHit()
    {
        for (int i = 0; i < Constants.instance.ASTEROID_PARTICLES_PER_HIT; ++i)
        {
            Instantiate(_asteroidParticlePrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    public void OnStarHit()
    {
        _paused = true;
        GetComponent<CircleCollider2D>().enabled = false;
        transform.DOScale(1.5f, Constants.instance.ASTEROID_STAR_ANIMATION_TIME);

        GetComponent<SpriteRenderer>().DOFade(0f, Constants.instance.ASTEROID_STAR_ANIMATION_TIME).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

    /****************************************/

    private void Start()
    {
        _target = new Vector3(Random.Range(-Constants.instance.ASTEROID_TARGET_LIMIT, Constants.instance.ASTEROID_TARGET_LIMIT), -8f, 0f);
        _speed = Random.Range
        (
            Game.instance.GAME_CURRENT_SPEED * Constants.instance.ASTEROID_START_SPEED_MODIFIER,
            Game.instance.GAME_CURRENT_SPEED * Constants.instance.ASTEROID_START_SPEED_MODIFIER
        );
        
        _rotationSpeed = Random.Range(40f, 60f);
        transform.localScale *= Random.Range(0.8f, 1.2f);

        GetComponent<SpriteRenderer>().sprite = _sprites[Random.Range(0, _sprites.Length)];
        transform.position = new Vector3(Random.Range(-Constants.instance.ASTEROID_TARGET_LIMIT, Constants.instance.ASTEROID_TARGET_LIMIT), 8f, 0f);

        _direction = (_target - transform.position).normalized;
        _isStar = CompareTag("Star");
    }

    private void Update()
    {
        if (!_paused)
        {
            transform.localRotation = Quaternion.AngleAxis(_rotation, Vector3.forward);
            _rotation += Time.deltaTime * _rotationSpeed;

            if (_isStar)
            {
                transform.position += _direction * Time.deltaTime * (Game.instance.GAME_NORMAL_SPEED + _speed) * Constants.instance.ASTEROID_STAR_SPEED_MODIFIER;
            }
            else
            {
                transform.position += _direction * Time.deltaTime * (Game.instance.GAME_CURRENT_SPEED + _speed);
            }

            if (transform.position.y < -Constants.instance.ASTEROID_TARGET_LIMIT) GameObject.Destroy(gameObject);

            if (_rotation == 359f) _rotation = 0f;
        }
    }
}
