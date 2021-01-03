using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ChargeAnimationController : MonoBehaviour
{
    // State sprites
    [SerializeField]
    private Sprite[] _charging = default;
    [SerializeField]
    private Sprite[] _charged = default;
    [SerializeField]
    private Sprite[] _uncharged = default;

    // Emission textures
    [SerializeField]
    private Texture[] _chargingE = default;
    [SerializeField]
    private Texture[] _chargedE = default;

    // Light
    [SerializeField]
    private GameObject _glow = default;

    // Animation variables
    private float _chargingSpeed = 0.09f;
    private int _chargingLength = 5;
    private float _chargedSpeed = 0.05f;
    private int _chargedLength = 2;

    // Components
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.material.SetTexture("_EmissionMap", null);

        _glow.SetActive(false);
    }

    /// <summary>
    /// Starts the animation of the glowing orb projectile
    /// </summary>
    public void PlayChargingAnimation()
    {
        StartCoroutine(PlayChargingAnimation(_chargingSpeed, 0, _chargingLength));
        _glow.SetActive(true);
    }

    /// <summary>
    /// Ends any current animations and returns to blank sprites
    /// </summary>
    public void StopChargeAnimation()
    {
        StopAllCoroutines();
        _spriteRenderer.material.SetTexture("_EmissionMap", null);
        _spriteRenderer.sprite = _uncharged[0];
        _glow.SetActive(false);
    }

    /// <summary>
    /// Animation of glowing orb growing larger
    /// Ends after orb has grown full size
    /// </summary>
    /// <param name="playSpeed">The length of a single frame</param>
    /// <param name="index">Current frame of animation</param>
    /// <param name="animationLength">Total frames in the animation</param>
    /// <returns></returns>
    private IEnumerator PlayChargingAnimation(float playSpeed, int index, int animationLength)
    {
        _spriteRenderer.sprite = _charging[index];
        _spriteRenderer.material.SetTexture("_EmissionMap", _chargingE[index]);

        yield return new WaitForSeconds(playSpeed);

        if (index < animationLength - 1)
        {
            StartCoroutine(PlayChargingAnimation(playSpeed, index + 1, animationLength));
        }
        else
        {
            StartCoroutine(PlayChargedAnimation(playSpeed, 0, 2));
        }
    }

    /// <summary>
    /// Animation of the fully charged orb
    /// Animation will continue to play until the coroutine is stopped externally
    /// </summary>
    /// <param name="playSpeed">The length of a single frame</param>
    /// <param name="index">Current frame of animation</param>
    /// <param name="animationLength">Total frames in the animation</param>
    /// <returns></returns>
    private IEnumerator PlayChargedAnimation(float playSpeed, int index, int animationLength)
    {
        _spriteRenderer.sprite = _charged[index];
        _spriteRenderer.material.SetTexture("_EmissionMap", _chargedE[index]);

        yield return new WaitForSeconds(playSpeed);

        if (index < animationLength - 1)
        {
            StartCoroutine(PlayChargedAnimation(playSpeed, index + 1, animationLength));
        }
        else
        {
            StartCoroutine(PlayChargedAnimation(playSpeed, 0, animationLength));
        }
    }
}
