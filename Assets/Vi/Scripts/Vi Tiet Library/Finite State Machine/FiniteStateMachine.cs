using System.Collections.Generic;
using UnityEngine;

namespace FiniteStateMachine.StateInterface
{
    public delegate bool TransitionCondition();
    public delegate void StateAction();

    public interface IState
    {
        void Update(float deltaTime);
        void OnStateEnter();
        void OnStateExit();
    }

    public class Transition
    {
        public Transition(string destinationState, TransitionCondition condition)
        {
            DestinationState = destinationState;
            EvaluateCondition = condition;
        }

        public string DestinationState { get; }
        public TransitionCondition EvaluateCondition { get; }
    }

    public class EmptyState : IState
    {
        public void OnStateEnter()
        {
        }

        public void OnStateExit()
        {
        }

        public void Update(float deltaTime)
        {
        }
    }

    public class FSM
    {
        private IState activeState;
        private Dictionary<string, IState> states;
        private Dictionary<IState, List<Transition>> transitions;

        public FSM()
        {
            activeState = new EmptyState();
            states = new Dictionary<string, IState>();
            transitions = new Dictionary<IState, List<Transition>>();
        }

        public void AddState(string name, IState state)
        {
            if (states.Count == 0)
            {
                activeState = state;
                InitiateStateMachine(activeState);
            }

            states.Add(name, state);
        }

        public void AddTransition(string fromStateName, string toStateName, TransitionCondition condition)
        {
            IState fromState = new EmptyState();
            if (states[fromStateName] != fromState) fromState = states[fromStateName];
            Transition transition = new Transition(toStateName, condition);

            if (!states.ContainsKey(toStateName) && !states.TryGetValue(fromStateName, out fromState))
                return;

            if (!transitions.ContainsKey(fromState))
                transitions.Add(fromState, new List<Transition>());

            if (!transitions[fromState].Contains(transition))
                transitions[fromState].Add(transition);
        }

        public void Update(float deltaTime)
        {
            activeState.Update(deltaTime);

            if (transitions.Count > 0 && transitions.ContainsKey(activeState))
            {
                foreach (Transition transition in transitions[activeState])
                {
                    if (transition.EvaluateCondition())
                    {
                        TransitionTo(transition.DestinationState);
                    }
                }
            }
        }

        public void TransitionTo(string stateName)
        {
            activeState.OnStateExit();
            activeState = states[stateName];
            activeState.OnStateEnter();
        }

        public void InitiateStateMachine(IState state)
        {
            state.OnStateEnter();
        }
    }
}

namespace FiniteStateMachine.StatePolymorphism
{
    public delegate bool TransitionCondition();
    public delegate void StateAction();

    /// <summary>
    /// The class that all finite state machine states derived from
    /// </summary>
    public class State
    {
        virtual public void Update(float deltaTime)
        {
        }

        virtual public void OnStateEnter()
        {
        }

        virtual public void OnStateExit()
        {
        }
    }

    /// <summary>
    /// The class that holds the info of the state to be transitioned to and the condition for the transition to be triggered
    /// </summary>
    public class Transition
    {
        public Transition(string destinationState, TransitionCondition condition)
        {
            DestinationState = destinationState;
            EvaluateCondition = condition;
        }

        public string DestinationState { get; }
        public TransitionCondition EvaluateCondition { get; }
    }

    /// <summary>
    /// An empty FSM state
    /// </summary>
    public class EmptyState : State
    {
        override public void OnStateEnter()
        {
        }

        override public void OnStateExit()
        {
        }

        override public void Update(float deltaTime)
        {
        }
    }

    /// <summary>
    /// The finite state machine class that controls all of its states
    /// </summary>
    public class FSM
    {
        private State activeState;
        private Dictionary<string, State> states;
        private Dictionary<State, List<Transition>> transitions;

        /// <summary>
        /// Empty constructor
        /// </summary>
        public FSM()
        {
            activeState = new EmptyState();
            states = new Dictionary<string, State>();
            transitions = new Dictionary<State, List<Transition>>();
        }

        /// <summary>
        /// Add state to the finite state machine
        /// </summary>
        /// <param name="name"> Name of the state </param>
        /// <param name="state"> The class that inherited from State class </param>
        public void AddState(string name, State state)
        {
            if (states.Count == 0)
            {
                activeState = state;
                InitiateStateMachine(activeState);
            }

            states.Add(name, state);
        }

        /// <summary>
        /// Add transition from one state to another
        /// </summary>
        /// <param name="fromStateName"> Name of the state that is transitioned from </param>
        /// <param name="toStateName"> Name of the state to transition to </param>
        /// <param name="condition"> Bolean delegate </param>
        public void AddTransition(string fromStateName, string toStateName, TransitionCondition condition)
        {
            State fromState = new EmptyState();

            // if contains FromState, get value and assign to FromState
            if (states.ContainsKey(fromStateName))
            {
                // if FromState is not empty, get value from state dictionary and assign to FromState
                if (states[fromStateName] != fromState)
                {
                    fromState = states[fromStateName];
                }
            }
            // otherwise, exit
            else
            {
                LogError("Could not add transition. [From State] " + fromStateName + " state name, does not exist.");
                return;
            }

            // if doesn't contain ToState, exit
            if (!states.ContainsKey(toStateName))
            {
                LogError("Could not add transition. [To State]" + toStateName + " state name, does not exist.");
                return;
            }

            // if have not already instantiated the transitions list of FromState
            if (!transitions.ContainsKey(fromState))
            {
                // add FromState and its transitions list to dictionary
                transitions.Add(fromState, new List<Transition>());
            }
            //otherwise, if have not already have a transition from FromState to ToState
            else
            {
                // create a transition to ToState with condition
                Transition transition = new Transition(toStateName, condition);

                if (!transitions[fromState].Contains(transition))
                {
                    // add the transition to the transitions list
                    transitions[fromState].Add(transition);
                }
            }
        }

        /// <summary>
        /// Add transition to state from any state
        /// </summary>
        /// <param name="toStateName"> Target state name </param>
        /// <param name="condition"> Boolean delegate </param>
        public void AddAnyStateTransition(string toStateName, TransitionCondition condition)
        {
            foreach (var item in states)
            {
                if (item.Key != toStateName)
                    AddTransition(item.Key, toStateName, condition);
            }
        }

        /// <summary>
        /// Execute the active state actions and check transition conditions
        /// </summary>
        /// <param name="deltaTime"> Delta time </param>
        public void Update(float deltaTime)
        {
            activeState.Update(deltaTime);

            CheckTransitions();
        }

        /// <summary>
        /// Check if any transition condition is met this frame
        /// </summary>
        private void CheckTransitions()
        {
            if (transitions.Count > 0 && transitions.ContainsKey(activeState))
            {
                foreach (Transition transition in transitions[activeState])
                {
                    if (transition.EvaluateCondition())
                    {
                        TransitionTo(transition.DestinationState);
                    }
                }
            }
        }

        /// <summary>
        /// Change to new state
        /// </summary>
        /// <param name="stateName"> Name of target state </param>
        private void TransitionTo(string stateName)
        {
            activeState.OnStateExit();
            activeState = states[stateName];
            activeState.OnStateEnter();
        }

        /// <summary>
        /// Initialize OnStateEnter parameters
        /// </summary>
        /// <param name="state"> State to be initialized </param>
        private void InitiateStateMachine(State state)
        {
            state.OnStateEnter();
        }

        /// <summary>
        /// Debug.Log wrapper
        /// </summary>
        /// <param name="message"> Message to be displayed </param>
        private void Log(string message)
        {
            Debug.Log("[" + this + "]: " + message);
        }

        /// <summary>
        /// Debug.LogWarning wrapper
        /// </summary>
        /// <param name="message"> Message to be displayed </param>
        private void LogWarning(string message)
        {
            Debug.LogWarning("[" + this + "]: " + message);
        }

        /// <summary>
        /// Debug.LogError wrapper
        /// </summary>
        /// <param name="message"> Message to be displayed </param>
        private void LogError(string message)
        {
            Debug.LogError("[" + this + "]: " + message);
        }
    }
}