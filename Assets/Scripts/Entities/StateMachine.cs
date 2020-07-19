using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;
public class StateMachine
{

    State currentState;
    List<Transition> currentTransitions = new List<Transition>();

    Dictionary<Type, List<Transition>> transitions = new Dictionary<Type, List<Transition>>();
    List<Transition> anyTransitions = new List<Transition>();

    static List<Transition> EmptyTransitions = new List<Transition>(0);


    public void Tick()
    {
        var nextTransition = GetTransition();
        if (nextTransition != null)
            SetState(nextTransition.To);

        currentState.Tick();
    }


    public void SetState(State state)
    {
        if (state == currentState)
            return;

        currentState?.OnExit();
        currentState = state;

        transitions.TryGetValue(currentState.GetType(), out currentTransitions); //Get list of current state's transitions
        if (currentTransitions == null)
            currentTransitions = EmptyTransitions;

        currentState.OnEnter();
    }

    private class Transition
    {
        public Func<bool> Condition { get; }
        public State To { get; }

        public Transition(State to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }

    public void AddTransition(State from, State to, Func<bool> predicate)
    {
        if (transitions.TryGetValue(from.GetType(), out var t) == false)
        {
            t = new List<Transition>();
            transitions[from.GetType()] = t;
        }

        t.Add(new Transition(to, predicate));
    }

    public void AddAnyTransition(State state, Func<bool> predicate)
    {
        anyTransitions.Add(new Transition(state, predicate));
    }


    private Transition GetTransition() //Get highest priority transition
    {
        foreach (var transition in anyTransitions)
            if (transition.Condition())
                return transition;

        foreach (var transition in currentTransitions)
            if (transition.Condition())
                return transition;

        return null;
    }

}
