using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardShadow : MonoBehaviour
{
    public GameObject _parentCharacter;

    public string _filepath;

    SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Sprite current = Resources.Load<Sprite>(_filepath + _parentCharacter.GetComponent<SpriteRenderer>().sprite.name);
        _spriteRenderer.sprite = current;
    }

    private void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
