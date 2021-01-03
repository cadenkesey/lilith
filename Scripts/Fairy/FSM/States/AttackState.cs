// Adapted from:
// Finite State Machines in Unity
// Table Flip Games
// 12/27/2020
// https://youtu.be/DO-6ikrN9jg

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackState", menuName = "Unity-FSM/States/Attack", order = 4)]

public class AttackState : AbstractState
{
    private float _firingRate = 0.5f;
    private float _fireTimer = 0.0f;

    public override void OnEnable()
    {
        base.OnEnable();
        StateType = FsmStateType.Attack;
    }

    public override bool EnterState()
    {
        EnteredState = false;

        if (base.EnterState())
        {
            _fireTimer = 0.0f;

            FairyCharacter.ChargeAnimationControl.PlayChargingAnimation();

            EnteredState = true;
        }

        return EnteredState;
    }

    public override void UpdateState()
    {
        if (EnteredState)
        {
            if (FairyCharacter.PlayerVisible())
            {
                _fireTimer += Time.deltaTime;

                FairyCharacter.transform.rotation = FairyOperations.TurnTowardPlayer(FairyCharacter.transform, FairyCharacter.Player.transform);

                if ((_fireTimer >= _firingRate) && (FairyCharacter.Fire()))
                {
                    FairyCharacter.ChargeAnimationControl.StopChargeAnimation();
                    Fsm.EnterState(FsmStateType.Chase);
                }
            }
            else
            {
                FairyCharacter.ChargeAnimationControl.StopChargeAnimation();
                Fsm.EnterState(FsmStateType.Chase);
            }
        }
    }

    public override bool ExitState()
    {
        base.ExitState();
        return true;
    }
}
