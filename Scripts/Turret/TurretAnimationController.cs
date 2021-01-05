using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAnimationController : MonoBehaviour
{
    // Billboard information
    // At what angle to switch to forward or backward sprites with .75 being directly 45
    [SerializeField]
    private float _frontCutoff = 0.75f;
    [SerializeField]
    private GameObject _player = default;

    // State sprites
    [SerializeField]
    private Sprite[] _active = default;
    [SerializeField]
    private Sprite[] _inactive = default;

    // Animation sprites
    [SerializeField]
    private Sprite[] _activate0 = default;
    [SerializeField]
    private Sprite[] _activate1 = default;
    [SerializeField]
    private Sprite[] _activate2 = default;
    [SerializeField]
    private Sprite[] _fire0 = default;

    // Emission textures
    [SerializeField]
    private List<Texture> _activeE = default;
    [SerializeField]
    private Texture[] _fireE = default;

    // Light
    [SerializeField]
    private GameObject _muzzleFlash = default;

    // Animation variables
    private float _activateSpeed = 0.05f;
    private int _activateLength = 3;
    private List<Sprite[]> _activateAnim = new List<Sprite[]>();
    private List<Sprite[]> _deactivateAnim = new List<Sprite[]>();
    private float _fireSpeed = 0.1f;
    private int _fireLength = 1;
    private List<Sprite[]> _fireAnim = new List<Sprite[]>();
    private List<Texture[]> _fireAnimE = new List<Texture[]>();

    // Other variables
    private bool _activated = false;
    private SpriteRenderer _spriteRenderer;
    private bool _playingAnimation = false;
    private int _lastIndex = -1;

    private void Awake()
    {
        _activateAnim.Add(_activate2);
        _activateAnim.Add(_activate1);
        _activateAnim.Add(_activate0);

        _deactivateAnim.Add(_activate0);
        _deactivateAnim.Add(_activate1);
        _deactivateAnim.Add(_activate2);

        _fireAnim.Add(_fire0);

        _fireAnimE.Add(_fireE);

        _spriteRenderer = GetComponent<SpriteRenderer>();

        _spriteRenderer.material.SetTexture("_EmissionMap", null);

        _muzzleFlash.SetActive(false);
    }

    private void Update()
    {
        // Don't set a state since an animation is playing
        if (_playingAnimation)
        {
            _lastIndex = -1;
            return;
        }

        int directionIndex = BillboardDirection.GetDirection8(transform, _player.transform, _frontCutoff);

        if (_activated)
        {
            if (directionIndex != _lastIndex)
            {
                PlayActiveState(directionIndex);
                _lastIndex = directionIndex;
            }
        }
        else
        {
            if (directionIndex != _lastIndex)
            {
                PlayInactiveState(directionIndex);
                _lastIndex = directionIndex;
            }
        }
    }

    /// <summary>
    /// Play the animation of the turret activating
    /// </summary>
    private void PlayActivationAnim()
    {
        _playingAnimation = true;
        StartCoroutine(PlayAnim(_activateAnim, _activateSpeed, 0, _activateLength));
    }

    /// <summary>
    /// Play the animation of the turret retracting
    /// </summary>
    private void PlayDeactivationAnim()
    {
        _playingAnimation = true;
        StartCoroutine(PlayAnim(_deactivateAnim, _activateSpeed, 0, _activateLength));
    }

    /// <summary>
    /// Play the firing animation
    /// </summary>
    private void PlayFireAnim()
    {
        _playingAnimation = true;
        StartCoroutine(PlayAnim(_fireAnim, _fireSpeed, 0, _fireLength));
        StartCoroutine(PlayAnimE(_fireAnimE, _fireSpeed, 0, _fireLength));
        StartCoroutine(muzzleFlashTimer(_fireSpeed * _fireLength));
    }

    /// <summary>
    /// Play an animation
    /// </summary>
    /// <param name="animation">A list of sprite arrays
    /// Each list item represents a frame of the animation
    /// Each array item represents that frame in a different direction out of eight</param>
    /// <param name="playSpeed">The length of one frame</param>
    /// <param name="index">The current frame index</param>
    /// <param name="animationLength">The total amount of frames in this animation</param>
    /// <returns></returns>
    private IEnumerator PlayAnim(List<Sprite[]> animation, float playSpeed, int index, int animationLength)
    {
        int direction = BillboardDirection.GetDirection8(transform, _player.transform, _frontCutoff);
        _spriteRenderer.sprite = animation[index][direction];

        yield return new WaitForSeconds(playSpeed);

        if (index < animationLength - 1)
        {
            StartCoroutine(PlayAnim(animation, playSpeed, index + 1, animationLength));
        }
        else
        {
            _playingAnimation = false;
        }
    }

    /// <summary>
    /// Play an animation of emission maps
    /// </summary>
    /// <param name="animation">A list of texture arrays
    /// Each list item represents a frame of the animation
    /// Each array item represents that frame in a different direction out of eight</param>
    /// <param name="playSpeed">The length of one frame</param>
    /// <param name="index">The current frame index</param>
    /// <param name="animationLength">The total amount of frames in this animation</param>
    /// <returns></returns>
    private IEnumerator PlayAnimE(List<Texture[]> animation, float playSpeed, int index, int animationLength)
    {
        int direction = BillboardDirection.GetDirection8(transform, _player.transform, _frontCutoff);
        _spriteRenderer.material.SetTexture("_EmissionMap", animation[index][direction]);

        yield return new WaitForSeconds(playSpeed);

        if (index < animationLength - 1)
        {
            StartCoroutine(PlayAnimE(animation, playSpeed, index + 1, animationLength));
        }
    }

    /// <summary>
    /// Activate the muzzle flash for the length of the firing animation
    /// </summary>
    /// <param name="playLength">The total time to keep the flash active</param>
    /// <returns></returns>
    private IEnumerator muzzleFlashTimer(float playLength)
    {
        _muzzleFlash.SetActive(true);

        yield return new WaitForSeconds(playLength);

        _muzzleFlash.SetActive(false);
    }

    /// <summary>
    /// The turret should play activated animations
    /// </summary>
    public void Activate()
    {
        if (!_activated)
        {
            _activated = true;
            PlayActivationAnim();
        }
    }

    /// <summary>
    /// The turret should play deactivated animations
    /// </summary>
    public void Deactivate()
    {
        if (_activated)
        {
            _activated = false;
            _spriteRenderer.material.SetTexture("_EmissionMap", null);
            PlayDeactivationAnim();
        }
    }

    /// <summary>
    /// Play the firing animation
    /// </summary>
    public void Fire()
    {
        PlayFireAnim();
    }

    /// <summary>
    /// Play the active idle animations
    /// </summary>
    /// <param name="direction">The current direction of the turret relative to the player</param>
    private void PlayActiveState(int direction)
    {
        _spriteRenderer.sprite = _active[direction];

        _spriteRenderer.material.SetTexture("_EmissionMap", _activeE[direction]);
        Color color = Color.white;
        _spriteRenderer.material.SetColor("_EmissionColor", color);
    }

    /// <summary>
    /// Play the animation of the turret idle and inactive
    /// </summary>
    /// <param name="index"></param>
    private void PlayInactiveState(int index)
    {
        _spriteRenderer.sprite = _inactive[index];
    }
}
