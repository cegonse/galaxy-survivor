using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BurstAnimationController : MonoBehaviour
{
    public const float ANIMATION_TIME = 0.1f;

    public Sprite[] _upperFrames;
    public Sprite[] _bottomFrames;

    public SpriteRenderer _upper;
    public SpriteRenderer _bottom;

    /*******************************************/

    private float _timer = 0f;
    private bool _running = false;
    private int _bottomFrame = 0;
    private int _upperFrame = 0;

    /*******************************************/

    public void StartAnimation()
    {
        if (!_running)
        {
            _timer = 0f;
            _running = true;

            _upper.enabled = false;
            _bottom.enabled = true;

            _upper.color = Color.white;
            _bottom.color = Color.white;

            _bottomFrame = 0;
            _upperFrame = 0;
            _upper.sprite = _upperFrames[0];
            _bottom.sprite = _bottomFrames[0];

            StartCoroutine(AnimationCallback());
        }
    }

    /*******************************************/

	void Start ()
    {
        _upper.enabled = false;
        _bottom.enabled = false;
	}

	private IEnumerator AnimationCallback()
    {
        while (_bottomFrame < _bottomFrames.Length - 1)
        {
            yield return new WaitForSeconds(ANIMATION_TIME);

            _bottom.sprite = _bottomFrames[++_bottomFrame];
            
            if (_upperFrame < _upperFrames.Length - 1 && _bottomFrame > 5)
            {
                _upper.sprite = _upperFrames[++_upperFrame];
            }

            if (_bottomFrame == 5)
            {
                _upper.enabled = true;
                _upper.color = Color.white;
            }
        }

        _bottom.DOFade(0f, 0.2f);
        _upper.DOFade(0f, 0.2f);
        _running = false;
    }
}
