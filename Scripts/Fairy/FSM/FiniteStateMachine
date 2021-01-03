// Adapted from:
// Finite State Machines in Unity
// Table Flip Games
// 12/27/2020
// https://youtu.be/DO-6ikrN9jg

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour
{
    [SerializeField]
    private List<AbstractState> _validStates = null;

    private AbstractState _currentState;
    private Dictionary<FsmStateType, AbstractState> _fsmStates;

    public void Awake()
    {
        _currentState = null;
        _fsmStates = new Dictionary<FsmStateType, AbstractState>();

        Fairy fairy = this.GetComponent<Fairy>();

        foreach(AbstractState state in _validStates)
        {
            state.SetExecutingFSM(this);
            state.SetExecutingNPC(fairy);
            _fsmStates.Add(state.StateType, state);
        }
    }

    public void Start()
    {
        EnterState(FsmStateType.Idle);
    }

    public void Update()
    {
        if (_currentState != null)
        {
            _currentState.UpdateState();
        }
    }

    public void EnterState(AbstractState nextState)
    {
        if (nextState == null)
        {
            return;
        }
        else
        {
            if (_currentState != null)
            {
                _currentState.ExitState();
            }
            _currentState = nextState;
            _currentState.EnterState();
        }
    }

    public void EnterState(FsmStateType stateType)
    {
        if (_fsmStates.ContainsKey(stateType))
        {
            AbstractState nextState = _fsmStates[stateType];

            EnterState(nextState);
        }
    }
}
