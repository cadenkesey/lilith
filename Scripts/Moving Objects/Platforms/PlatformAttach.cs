using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAttach : MonoBehaviour
{
    private Transform _parentPlatform;

    private void Awake()
    {
        _parentPlatform = this.transform.parent;
    }

    /// <summary>
    /// Triggers if someone steps onto the platform
    /// </summary>
    /// <param name="other">The object on the platform</param>
    private void OnTriggerEnter(Collider other)
    {
        bool platformIsAutomatic = _parentPlatform.GetComponent<Platform>().Automatic;

        if (!platformIsAutomatic)
        {
            _parentPlatform.GetComponent<Platform>().SetOccupants(1);
            _parentPlatform.GetComponent<Platform>().PlayerTrigger();
        }

        other.transform.parent = this.transform.parent;
    }

    /// <summary>
    /// Someone has left the platform
    /// </summary>
    /// <param name="other">The object leaving the platform</param>
    private void OnTriggerExit(Collider other)
    {
        bool platformIsAutomatic = _parentPlatform.GetComponent<Platform>().Automatic;

        if (!platformIsAutomatic)
        {
            _parentPlatform.GetComponent<Platform>().SetOccupants(-1);
        }

        other.transform.parent = null;
    }
}
