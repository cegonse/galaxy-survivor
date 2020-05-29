using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Player : MonoBehaviour
{
    public static Player instance;

    /****************************************/

    public GameObject _repairEffectPrefab;
    public GameObject _bombEffectPrefab;

    /****************************************/

    private Rigidbody2D _rigidbody;
    private float _movementDirection = -1f;
    private bool _inPlace = false;
    private Vector3 _originalPosition;
    private bool _paused = false;

    /****************************************/

    public void OnPause(bool pause)
    {
        _paused = pause;
    }

    public void OnBombSkill()
    {
        // Play effect
        Instantiate(_bombEffectPrefab, transform.position, Quaternion.identity);
    }

    public void OnRepairSkill()
    {
        // Play effect
        Instantiate(_repairEffectPrefab, transform);
    }

    public void OnSpeedSkill()
    {

    }

    public void OnTimeSkill()
    {

    }

    /****************************************/

    void Start()
    {
        if (instance == null) instance = this;

        _rigidbody = GetComponent<Rigidbody2D>();
        _originalPosition = transform.position;
        transform.position = transform.position + 4 * Vector3.down;

        transform.DOMove(_originalPosition, 0.5f).OnComplete(() =>
        {
            RotatePlayer();
            _inPlace = true;
        });
	}

    private void Update()
    {
        if (!_paused)
        {
            if (_inPlace &&
                Input.mousePosition.y > Screen.height * 0.25f &&
                Input.mousePosition.y < Screen.height * 0.75f &&
                Input.GetMouseButtonDown(0))
            {
                _movementDirection *= -1f;
                RotatePlayer();
            }

            // Handle collisions
            CheckCollisions();

            // Handle Movement
            _rigidbody.MovePosition(transform.position + Vector3.right * Constants.instance.PLAYER_MOVEMENT_SPEED * _movementDirection * Time.deltaTime);

            if (_movementDirection == -1f &&
                transform.position.x < -Constants.instance.PLAYER_MOVEMENT_LIMIT)
            {
                _movementDirection *= -1;
                RotatePlayer();
            }
            else if (_movementDirection == 1f &&
                     transform.position.x > Constants.instance.PLAYER_MOVEMENT_LIMIT)
            {
                _movementDirection *= -1;
                RotatePlayer();
            }
        }
    }

    private void CheckCollisions()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, Constants.instance.PLAYER_HITBOX_RADIUS, Vector2.right);

        if (hit.collider != null)
        {
            // Hit an asteroid
            if (hit.collider.CompareTag("Asteroid"))
            {
                hit.collider.GetComponent<Asteroid>().OnHit();
                Game.instance.OnAsteroidHit();
            }
            // Hit a star
            else if (hit.collider.CompareTag("Star"))
            {
                hit.collider.GetComponent<Asteroid>().OnStarHit();
                Game.instance.OnStar();
            }
        }
    }

    private void RotatePlayer()
    {
        if (_movementDirection == -1f)
        {
            transform.DORotateQuaternion(Quaternion.AngleAxis(Constants.instance.PLAYER_ROTATION_ANGLE, Vector3.forward), Constants.instance.PLAYER_ROTATION_TIME);
        }
        else
        {
            transform.DORotateQuaternion(Quaternion.AngleAxis(-Constants.instance.PLAYER_ROTATION_ANGLE, Vector3.forward), Constants.instance.PLAYER_ROTATION_TIME);
        }
    }
}
