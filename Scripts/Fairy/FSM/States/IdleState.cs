// Adapted from:
// Finite State Machines in Unity
// Table Flip Games
// 12/27/2020
// https://youtu.be/DO-6ikrN9jg

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IdleState", menuName = "Unity-FSM/States/Idle", order = 1)]

public class IdleState : AbstractState
{
    [SerializeField]
    private float _idleDuration = 3f;

    private float _totalTime;

    public override void OnEnable()
    {
        base.OnEnable();
        StateType = FsmStateType.Idle;
    }

    public override bool EnterState()
    {
        EnteredState = base.EnterState();

        if (EnteredState)
        {
            _totalTime = 0f;
        }

        return EnteredState;
    }

    public override void UpdateState()
    {
        if (EnteredState)
        {
            _totalTime += Time.deltaTime;

            if (FairyCharacter.PlayerNearby())
            {
                Fsm.EnterState(FsmStateType.Chase);
            }

            if (_totalTime >= _idleDuration)
            {
                Fsm.EnterState(FsmStateType.Patrol);
            }
        }
    }

    public override bool ExitState()
    {
        base.ExitState();
        return true;
    }
}
