using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyAnimationController : MonoBehaviour
{
    // Billboard information
    // At what angle to switch to forward or backward sprites with .75 being directly 45
    [SerializeField]
    private float _frontCutoff = 0.9f;
    [SerializeField]
    private GameObject _player = default;

    // State sprites
    [SerializeField]
    private Sprite[] _idle = default;

    // Components
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        int directionIndex = BillboardDirection.GetDirection8(transform, _player.transform, _frontCutoff);
        PlayAnimation(directionIndex);
    }

    /// <summary>
    /// Change enemy sprite
    /// </summary>
    /// <param name="direction">The direction the enemy is facing relative to the player</param>
    private void PlayAnimation(int direction)
    {
        _spriteRenderer.sprite = _idle[direction];
    }
}
