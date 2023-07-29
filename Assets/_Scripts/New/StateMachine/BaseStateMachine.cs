using System;
using UnityEngine;

public abstract class BaseStateMachine : MonoBehaviour
{
    private BaseState _currentState = null;

    private void Update()
    {
        _currentState?.StateTick(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        _currentState?.StateFixedTick(Time.fixedDeltaTime);
    }

    public void ChangeState(BaseState newState)
    {
        if (newState == _currentState) return;
        _currentState?.StateExit();
        _currentState = newState;
        _currentState.StateEnter();
    }
}