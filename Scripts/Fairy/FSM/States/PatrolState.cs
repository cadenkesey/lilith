// Adapted from:
// Finite State Machines in Unity
// Table Flip Games
// 12/27/2020
// https://youtu.be/DO-6ikrN9jg

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "PatrolState", menuName = "Unity-FSM/States/Patrol", order = 2)]

public class PatrolState : AbstractState
{
    [SerializeField]
    private Waypoint[] _patrolPoints;

    private NavMeshPath _pathToNextPoint;
    private Vector3[] _pathCorners;
    private int _cornerIndex;
    private int _patrolPointIndex;

    public override void OnEnable()
    {
        base.OnEnable();
        StateType = FsmStateType.Patrol;
        _patrolPointIndex = -1;
    }

    public override bool EnterState()
    {
        EnteredState = false;

        if (base.EnterState())
        {
            _patrolPoints = FairyCharacter.PatrolPoints;

            if (_patrolPoints == null || _patrolPoints.Length == 0)
            {
                Debug.LogError("Unable to enter Patrol State: Failed to get Patrol Points");
                EnteredState = false;
            }
            else
            {
                if (_patrolPointIndex < 0)
                {
                    _patrolPointIndex = UnityEngine.Random.Range(0, _patrolPoints.Length);
                }
                else
                {
                    _patrolPointIndex = (_patrolPointIndex + 1) % _patrolPoints.Length;
                }

                SetDestination(_patrolPoints[_patrolPointIndex].getLocation());

                EnteredState = true;
            }
        }

        return EnteredState;
    }

    public override void UpdateState()
    {
        if (EnteredState)
        {
            if (FairyCharacter.PlayerNearby())
            {
                Fsm.EnterState(FsmStateType.Chase);
            }

            if (_cornerIndex <= _pathCorners.Length - 1)
            {
                // Get the next movement vector, and if true then the corner will be reached
                if (FairyOperations.GetNextPosition(_pathCorners[_cornerIndex], FairyCharacter.GroundCheck.transform.position, FairyCharacter.MovementSpeed, out Vector3 nextMovement))
                {
                    // Prepare to move to next corner on next update
                    if (_cornerIndex <= _pathCorners.Length - 1)
                    {
                        _cornerIndex += 1;
                    }
                }

                // Move towards the new position and look towards it too
                FairyCharacter.transform.position += nextMovement;

                Vector3 nextLook = new Vector3(nextMovement.x, 0, nextMovement.z);
                if (!nextLook.Equals(new Vector3(0, 0, 0)))
                {
                    FairyCharacter.transform.rotation = Quaternion.LookRotation(nextLook);
                }
            }

            // Check if the enemy should stop
            if (_cornerIndex >= _pathCorners.Length)
            {
                Fsm.EnterState(FsmStateType.Idle);
            }
        }
    }

    public override bool ExitState()
    {
        base.ExitState();
        return true;
    }

    /// <summary>
    /// Called when the enemy updates its Navigation Mesh Path
    /// </summary>
    /// <param name="destination">The new target for the enemy</param>
    private void SetDestination(Vector3 destination)
    {
        _pathToNextPoint = new NavMeshPath();
        Vector3 height = new Vector3(0, FairyCharacter.transform.position.y - FairyCharacter.GroundCheck.transform.position.y, 0);
        NavMesh.CalculatePath(FairyCharacter.transform.position - height, destination, NavMesh.AllAreas, _pathToNextPoint);

        _pathCorners = FairyOperations.CheckArrayForDuplicates(_pathToNextPoint.corners, FairyCharacter.GroundCheck.transform.position).ToArray();

        _cornerIndex = 0;
    }
}