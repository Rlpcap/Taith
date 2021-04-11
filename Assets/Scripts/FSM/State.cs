using System;
using System.Collections.Generic;

namespace MyFSM
{
	public class State<T> : IUpdate, IFixedUpdate, ILateUpdate
    {
		public string Name { get { return _stateName; } }

		public event Action<T> FsmEnter = delegate {};
		public event Action FsmUpdate = delegate {};
        public event Action FsmLateUpdate = delegate {};
        public event Action FsmFixedUpdate = delegate {};
		public event Action<T> FsmExit = delegate {};

		private string _stateName;
		private Dictionary<T, Transition<T>> transitions;

		public State(string name)
        {
			_stateName = name;
		}

		public State<T> Configure(Dictionary<T, Transition<T>> transitions)
        {
			this.transitions = transitions;
			return this;
		}

        public void AddTransition(T input, State<T> target)
        {
            transitions.Add(input, new Transition<T>(input, target));
        }


        public bool CheckInput(T input, out State<T> next)
        {
			if(transitions.ContainsKey(input)) 
			{
				var transition = transitions[input];
				next = transition.TargetState;
				return true;
			}

			next = this;
			return false;
		}

		public void Enter(T input)
        {
			FsmEnter(input);
		}

		public void OnUpdate()
		{
            FsmUpdate();
        }

        public void OnFixedUpdate()
		{
            FsmFixedUpdate();
        }

        public void OnLateUpdate()
		{
            FsmLateUpdate();
        }

        public void Exit(T input)
        {
			FsmExit(input);
		}
	}
}