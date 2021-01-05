using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSwitch : MonoBehaviour
{
    [SerializeField]
    private Texture _textureOn = default;
    [SerializeField]
    private Texture _textureOff = default;
    [SerializeField]
    private Platform _platform = default;

    private Renderer _renderer;

    private bool _state = false;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    /// <summary>
    /// Player is using the switch to move the platform
    /// </summary>
    public void Use()
    {
        if (_state == false)
        {
            On();
            _state = true;
        }
        else
        {
            Off();
            _state = false;
        }
    }

    /// <summary>
    /// Platform has been toggled on
    /// </summary>
    private void On()
    {
        _renderer.material.mainTexture = _textureOn;
        _platform.SwitchTrigger();
    }

    /// <summary>
    /// Platform has been toggled off
    /// </summary>
    private void Off()
    {
        _renderer.material.mainTexture = _textureOff;
        _platform.SwitchTrigger();
    }
}
