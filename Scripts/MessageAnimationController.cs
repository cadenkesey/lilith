using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageAnimationController : MonoBehaviour
{
    [SerializeField]
    private GameObject _player = default;
    [SerializeField]
    private float _openSpeed = 0f;
    [SerializeField]
    private Texture _mainMessage = default;
    [SerializeField]
    private Texture[] _openCloseAnimation = default;

    private GameObject _message;
    private GameObject _messageSwitch;
    private Renderer _renderer;
    private int _index = 0;

    private void Awake()
    {
        _message = gameObject;
        _messageSwitch = transform.parent.gameObject;
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        // Look at player reversed since otherwise back is invisible
        transform.LookAt(2 * transform.position - _player.transform.position);

        RaycastHit playerHit;
        if (!Physics.Raycast(transform.position, _player.transform.position - transform.position, out playerHit, 2))
        {
            _messageSwitch.GetComponent<MessageSwitch>().Use();
        }
    }

    private void OnEnable()
    {
        _renderer.material.mainTexture = _openCloseAnimation[_index];
        _index++;
        StartCoroutine(OpenScreen(_openSpeed));
    }

    /// <summary>
    /// Play animation of the screen opening
    /// </summary>
    /// <param name="waitTime">The length of one frame of animation</param>
    /// <returns></returns>
    private IEnumerator OpenScreen(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _renderer.material.mainTexture = _openCloseAnimation[_index];
        if (_index < _openCloseAnimation.Length - 1)
        {
            _index++;
            StartCoroutine(OpenScreen(_openSpeed));
        }
        else
        {
            StartCoroutine(SetMessage(_openSpeed));
        }
    }

    /// <summary>
    /// Set the message texture to the main message
    /// </summary>
    /// <param name="waitTime">The length of one frame of the opening animation</param>
    /// <returns></returns>
    private IEnumerator SetMessage(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _renderer.material.mainTexture = _mainMessage;
    }
    
    private void OnDisable()
    {
        _index = 0;
    }

    /// <summary>
    /// Player is closing the message
    /// </summary>
    public void PlayerClose()
    {
        StopAllCoroutines();
        StartCoroutine(CloseScreen(_openSpeed));
    }

    /// <summary>
    /// Play the animation of the screen closing
    /// </summary>
    /// <param name="waitTime">The length of one frame of animation</param>
    /// <returns></returns>
    private IEnumerator CloseScreen(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _renderer.material.mainTexture = _openCloseAnimation[_index];
        if (_index > 1)
        {
            _index--;
            StartCoroutine(CloseScreen(_openSpeed));
        }
        else
        {
            _messageSwitch.GetComponent<MessageSwitch>().SetCanOpen(true);
            _message.SetActive(false);
        }
    }
}
