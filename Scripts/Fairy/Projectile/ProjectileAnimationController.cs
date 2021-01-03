using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAnimationController : MonoBehaviour
{
    // State sprites
    [SerializeField]
    private Sprite[] _fly0 = default;
    [SerializeField]
    private Sprite[] _fly1 = default;

    // Emission textures
    [SerializeField]
    private Texture[] _flyE0 = default;
    [SerializeField]
    private Texture[] _flyE1 = default;

    // Light
    [SerializeField]
    private GameObject _muzzleFlash = default;

    // Animations
    private List<Sprite[]> _flyAnim = new List<Sprite[]>();
    private List<Texture[]> _flyAnimE = new List<Texture[]>();
    private float _frameSpeed = 0.05f;
    private GameObject _player = default;
    // At what angle to switch to forward or backward sprites with .75 being directly 45
    private float _frontCutoff = 0.60f;

    // Components
    private SpriteRenderer _spriteRenderer;

    public GameObject Player
    {
        get
        {
            return _player;
        }
        set
        {
            _player = value;
        }
    }

    private void Awake()
    {
        _flyAnim.Add(_fly0);
        _flyAnim.Add(_fly1);

        _flyAnimE.Add(_flyE0);
        _flyAnimE.Add(_flyE1);

        _spriteRenderer = GetComponent<SpriteRenderer>();

        _spriteRenderer.material.SetTexture("_EmissionMap", null);

        _muzzleFlash.SetActive(false);
    }

    /// <summary>
    /// Starts the coroutine to cycle between animation frames
    /// </summary>
    public void PlayFlyAnim()
    {
        _muzzleFlash.SetActive(true);
        StartCoroutine(PlayAnimation(_frameSpeed, 0));
    }

    /// <summary>
    /// Cycles between two animation frames
    /// </summary>
    /// <param name="playSpeed">The length of one frame</param>
    /// <param name="index">The current frame of animation</param>
    /// <returns></returns>
    private IEnumerator PlayAnimation(float playSpeed, int index)
    {
        int direction = BillboardDirection.GetDirection5(transform, _player.transform, _frontCutoff);
        _spriteRenderer.sprite = _flyAnim[index][direction];
        _spriteRenderer.material.SetTexture("_EmissionMap", _flyAnimE[index][direction]);

        yield return new WaitForSeconds(playSpeed);

        if (index < 1)
        {
            StartCoroutine(PlayAnimation(playSpeed, index + 1));
        }
        else
        {
            StartCoroutine(PlayAnimation(playSpeed, 0));
        }
    }
}
