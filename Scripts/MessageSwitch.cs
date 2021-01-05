using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSwitch : MonoBehaviour
{

    [SerializeField]
    private Texture _textureOn = default;
    [SerializeField]
    private Texture _textureOff = default;
    [SerializeField]
    private GameObject _message = default;

    private Renderer _render;

    private bool _state = false;
    private bool _canOpen = true;

    private void Start()
    {
        _render = GetComponent<Renderer>();
    }

    /// <summary>
    /// The player is opening or closing the message
    /// </summary>
    public void Use()
    {
        if (_state == false)
        {
            if (_canOpen)
            {
                On();
                _state = true;
            }
        }
        else
        {
            Off();
            _state = false;
        }
    }

    /// <summary>
    /// Turn on the message
    /// </summary>
    private void On()
    {
        _canOpen = false;
        _render.material.mainTexture = _textureOn;
        _message.SetActive(true);
    }

    /// <summary>
    /// Turn off the message
    /// </summary>
    private void Off()
    {
        _render.material.mainTexture = _textureOff;
        _message.GetComponent<MessageAnimationController>().PlayerClose();
    }

    public void SetCanOpen(bool newCanOpen)
    {
        _canOpen = newCanOpen;
    }
}
