using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AsteroidParticle : MonoBehaviour
{
    public Sprite[] _sprites;

    private Vector3 _direction;
    private float _time = 0f;

    /****************************************/

    void Start()
    {
        _direction = Random.insideUnitCircle;
        GetComponent<SpriteRenderer>().sprite = _sprites[Random.Range(0, _sprites.Length)];
        StartCoroutine(OnTimeOver());
	}

    private IEnumerator OnTimeOver()
    {
        yield return new WaitForSeconds(Constants.instance.ASTEROID_PARTICLE_TIME);
        GetComponent<SpriteRenderer>().DOFade(0f, Constants.instance.ASTEROID_PARTICLE_FADE_TIME).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

    private void Update()
    {
        transform.position += _direction * Constants.instance.ASTEROID_PARTICLE_SPEED * Time.deltaTime;
    }
}
