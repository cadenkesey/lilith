using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeChecker : MonoBehaviour
{
    [SerializeField]
    private List<string> _enemyTags = default;

    private List<GameObject> _targets = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        bool validTarget = false;

        for (int i = 0; i < _enemyTags.Count; i++)
        {
            if (other.CompareTag(_enemyTags[i]))
            {
                validTarget = true;
                break;
            }
        }

        if (validTarget)
        {
            _targets.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < _targets.Count; i++)
        {
            if (other.gameObject == _targets[i])
            {
                _targets.Remove(other.gameObject);
                return;
            }
        }
    }

    public int GetNumberOfTargets()
    {
        return _targets.Count;
    }

    public List<GameObject> GetValidTargets()
    {
        return _targets;
    }

    public GameObject GetTarget()
    {
        return _targets[0];
    }

    public List<string> GetTags()
    {
        return _enemyTags;
    }
}
