using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAnimationController : MonoBehaviour
{
    [SerializeField]
    private float _playSpeed = 0f;
    [SerializeField]
    private Sprite[] _particleAnimation = default;
    [SerializeField]
    private Texture[] _particleEmission = default;

    private SpriteRenderer _renderer;
    private int _index = 0;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();

        _renderer.material.SetTexture("_EmissionMap", null);
    }

    private void Start()
    {
        _renderer.sprite = _particleAnimation[_index];
        _renderer.material.SetTexture("_EmissionMap", _particleEmission[_index]);
        _index++;
        StartCoroutine(UpdateAnimation(_playSpeed));
    }

    /// <summary>
    /// Move to the next frame of animation
    /// </summary>
    /// <param name="waitTime">The length of time of one animation frame</param>
    /// <returns></returns>
    private IEnumerator UpdateAnimation(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _renderer.sprite = _particleAnimation[_index];
        _renderer.material.SetTexture("_EmissionMap", _particleEmission[_index]);
        if (_index < _particleAnimation.Length - 1)
        {
            _index++;
            StartCoroutine(UpdateAnimation(_playSpeed));
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
