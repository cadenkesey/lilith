// Adapted from:
// Finite State Machines in Unity
// Table Flip Games
// 12/27/2020
// https://youtu.be/DO-6ikrN9jg

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "ChaseState", menuName = "Unity-FSM/States/Chase", order = 3)]

public class ChaseState : AbstractState
{
    private NavMeshPath _pathToTarget;
    Vector3[] _pathCorners;
    private int _cornerIndex;
    private float _timeBetweenDestinationUpdates;
    private float _timeSinceLastUpdate;

    public override void OnEnable()
    {
        base.OnEnable();
        StateType = FsmStateType.Chase;
    }

    public override bool EnterState()
    {
        EnteredState = false;

        if (base.EnterState())
        {
            SetDestination(FairyCharacter.Player.transform.position);

            EnteredState = true;
        }

        return EnteredState;
    }

    public override void UpdateState()
    {
        if (EnteredState)
        {
            _timeSinceLastUpdate += Time.deltaTime;

            if (_cornerIndex <= _pathCorners.Length-1)
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

                // Check that the enemy is not moving too close to the player
                if (FairyOperations.CheckNextPosition(nextMovement, FairyCharacter.transform.position, FairyCharacter.Player.transform.position, 2.0f))
                {
                    // Get a new point to move to
                    if (FairyOperations.RandomPoint(FairyCharacter.Player.transform.position, 10.0f, out Vector3 point))
                    {
                        SetDestination(point);
                    }
                }
                // Move towards the new position and look towards it too
                else
                {
                    FairyCharacter.transform.position += nextMovement;
                    FairyCharacter.transform.rotation = Quaternion.LookRotation(new Vector3(nextMovement.x, 0, nextMovement.z));
                }
            }

            // Check if the enemy should update its path
            if ((_cornerIndex >= _pathCorners.Length) || (_timeSinceLastUpdate >= _timeBetweenDestinationUpdates))
            {
                SetDestination(FairyCharacter.Player.transform.position);
            }
        }
    }

    public override bool ExitState()
    {
        base.ExitState();
        return true;
    }

    /// <summary>
    /// Called when the enemy updates its pathfinding
    /// Sets a random amount of time before the enemy should update its destination again
    /// </summary>
    /// <param name="destination">The new target for the enemy to chase</param>
    private void SetDestination(Vector3 destination)
    {
        if (!FairyCharacter.FiredLastTurn && FairyOperations.chanceToFire(FairyCharacter.transform, FairyCharacter.Player.transform))
        {
            Fsm.EnterState(FsmStateType.Attack);
        }
        else
        {
            _timeSinceLastUpdate = 0.0f;
            _timeBetweenDestinationUpdates = Random.Range(0.5f, 2.0f);

            _pathToTarget = new NavMeshPath();
            Vector3 height = new Vector3(0, FairyCharacter.transform.position.y - FairyCharacter.GroundCheck.transform.position.y, 0);
            NavMesh.CalculatePath(FairyCharacter.transform.position - height, destination, NavMesh.AllAreas, _pathToTarget);

            _pathCorners = FairyOperations.CheckArrayForDuplicates(_pathToTarget.corners, FairyCharacter.GroundCheck.transform.position).ToArray();

            _cornerIndex = 0;

            FairyCharacter.FiredLastTurn = false;
        }
    }
}