using System;
using System.Collections.Generic;
using Godot;

public class EnvFiniteStateMachine
{
    public enum State
    {
        NOT_INITIALIZED,
        MAIN_MENU,
        GAMING,
        GAME_OVER,
        PAUSED,
        SCORES,
        EXIT
    }

    public enum Event
    {
        ON_NEW_GAME,
        ON_PAUSE,
        ON_RESUME
    }

    public class SpecificMachineStateListener
    {
        public State State { get; set; }
        public Action Fn { get; set; }
    }

    public class GeneralMachineStateListener
    {
        public Action<State> Fn { get; set; }
    }

    private State CurrentState = State.NOT_INITIALIZED;
    private readonly List<SpecificMachineStateListener> SpecificMachineStateListeners = new();
    private readonly List<GeneralMachineStateListener> GeneralMachineStateListeners = new();

    private void NotifySpecificChange(State state)
    {
        foreach (var listener in SpecificMachineStateListeners)
        {
            if (listener.State == state)
            {
                listener.Fn.Invoke();
            }
        }
    }

    private void NotifyGeneralChange(State state)
    {
        foreach (var listener in GeneralMachineStateListeners)
        {
            listener.Fn.Invoke(state);
        }
    }

    private void SetState(State state)
    {
        CurrentState = state;
        NotifySpecificChange(state);
        NotifyGeneralChange(state);
    }

    public State GetState()
    {
        return CurrentState;
    }

    public GeneralMachineStateListener AddStateListener(Action<State> fn, bool call = false)
    {
        var listener = new GeneralMachineStateListener
        {
            Fn = fn
        };

        if (call)
        {
            listener.Fn(CurrentState);
        }

        GeneralMachineStateListeners.Add(listener);

        return listener;
    }

    public void RemoveListener(GeneralMachineStateListener listener)
    {
        GeneralMachineStateListeners.Remove(listener);
    }

    public SpecificMachineStateListener AddStateListener(State state, Action fn)
    {
        var listener = new SpecificMachineStateListener
        {
            State = state,
            Fn = fn
        };

        SpecificMachineStateListeners.Add(listener);

        return listener;
    }

    public void RemoveListener(SpecificMachineStateListener listener)
    {
        SpecificMachineStateListeners.Remove(listener);
    }

    public void OnNotInitialized()
    {
        SetState(State.NOT_INITIALIZED);
    }

    public void OnNewGame()
    {
        SetState(State.GAMING);
    }

    public void OnPause()
    {
        SetState(State.PAUSED);
    }

    public void OnResume()
    {
        SetState(State.GAMING);
    }

    public void OnGameOver()
    {
        SetState(State.GAME_OVER);
    }

    public void OnMainMenu()
    {
        SetState(State.MAIN_MENU);
    }

    public void OnScores()
    {
        SetState(State.SCORES);
    }

    public void OnExit()
    {
        SetState(State.EXIT);
    }
}