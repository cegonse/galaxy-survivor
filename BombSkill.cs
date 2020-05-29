using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BombSkill : MonoBehaviour
{
    private float _circleRadius = 0.64f;
    private RaycastHit2D[] _hit;

    /****************************************/

    void Start()
    {
        DOTween.To(() => _circleRadius, x => _circleRadius = x, 3f, 0.8f);
        _hit = new RaycastHit2D[16];
        StartCoroutine(OnKill());
    }

    private IEnumerator OnKill()
    {
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }

    void Update()
    {
        int hits = Physics2D.CircleCastNonAlloc(transform.position, _circleRadius, Vector2.right, _hit);

        if (hits > 0)
        {
            for (int i = 0; i < hits; ++i)
            {
                if (_hit[i].collider.CompareTag("Asteroid"))
                {
                    _hit[i].collider.GetComponent<Asteroid>().OnHit();
                }
            }
        }
	}
}
