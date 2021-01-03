// Adapted from:
// Finite State Machines in Unity
// Table Flip Games
// 12/27/2020
// https://youtu.be/DO-6ikrN9jg

using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public enum ExecutionState
{
    None,
    Active,
    Completed,
    Terminated,
};

public enum FsmStateType
{
    Idle,
    Patrol,
    Chase,
    Attack,
};

public abstract class AbstractState : ScriptableObject
{
    private Fairy _fairyCharacter;
    private FiniteStateMachine _fsm;
    private bool _enteredState;

    public bool EnteredState
    {
        get
        {
            return _enteredState;
        }
        set
        {
            _enteredState = value;
        }
    }

    public Fairy FairyCharacter
    {
        get
        {
            return _fairyCharacter;
        }
        protected set
        {
            _fairyCharacter = value;
        }
    }

    public FiniteStateMachine Fsm
    {
        get
        {
            return _fsm;
        }
        protected set
        {
            _fsm = value;
        }
    }

    public ExecutionState ExecutionState { get; protected set; }
    public FsmStateType StateType { get; protected set; }

    public virtual void OnEnable()
    {
        ExecutionState = ExecutionState.None;
    }

    public virtual bool EnterState()
    {
        ExecutionState = ExecutionState.Active;

        // Does the executing agent exist?
        bool successfullyEntered = (_fairyCharacter != null);

        return successfullyEntered;
    }

    public abstract void UpdateState();

    public virtual bool ExitState()
    {
        ExecutionState = ExecutionState.Completed;
        return true;
    }

    public virtual void SetExecutingFSM(FiniteStateMachine fsm)
    {
        if (fsm != null)
        {
            _fsm = fsm;
        }
    }

    public virtual void SetExecutingNPC(Fairy fairy)
    {
        if (fairy != null)
        {
            _fairyCharacter = fairy;
        }
    }
}
