using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float _speed = 3f;
    private bool _paused = false;

    /****************************************/

    public void OnPause(bool pause)
    {
        _paused = pause;
    }

    /****************************************/

    private void Update()
    {
        if (!_paused)
        {
            transform.position += Vector3.down * Time.deltaTime * _speed;

            if (transform.position.y <= -Constants.instance.BACKGROUND_POSITION_LIMIT)
            {
                transform.position = Vector3.up * Constants.instance.BACKGROUND_POSITION_LIMIT;
            }
        }
    }
}
