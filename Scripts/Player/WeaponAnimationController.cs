using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationController : MonoBehaviour
{
    // State sprites
    [SerializeField]
    private Sprite _fire0 = default;
    [SerializeField]
    private Sprite _fire1 = default;

    // Emission textures
    [SerializeField]
    private Texture _fireE0 = default;
    [SerializeField]
    private Texture _fireE1 = default;

    // Light
    [SerializeField]
    private GameObject _muzzleFlash = default;

    // Animation variables
    private List<Sprite> _fireAnim = new List<Sprite>();
    private List<Texture> _fireAnimE = new List<Texture>();

    // Components
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _fireAnim.Add(_fire0);
        _fireAnim.Add(_fire1);

        _fireAnimE.Add(_fireE0);
        _fireAnimE.Add(_fireE1);

        _spriteRenderer = GetComponent<SpriteRenderer>();

        _spriteRenderer.material.SetTexture("_EmissionMap", null);

        _muzzleFlash.SetActive(false);

        PlayIdleAnim();
    }

    /// <summary>
    /// Play the weapon firing animation
    /// </summary>
    public void PlayFireAnim()
    {
        _muzzleFlash.SetActive(true);
        _spriteRenderer.sprite = _fireAnim[0];
        _spriteRenderer.material.SetTexture("_EmissionMap", _fireAnimE[0]);
    }

    /// <summary>
    /// Play the weapon idle animation
    /// </summary>
    public void PlayIdleAnim()
    {
        _muzzleFlash.SetActive(false);
        _spriteRenderer.sprite = _fireAnim[1];
        _spriteRenderer.material.SetTexture("_EmissionMap", _fireAnimE[1]);
    }
}
