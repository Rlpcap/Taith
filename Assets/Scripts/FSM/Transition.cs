using System;

namespace MyFSM
{
	public class Transition<T>
    {
		public T Input { get { return input; } }
		public State<T> TargetState { get { return targetState;  } }

		T input;
		State<T> targetState;

		public Transition(T input, State<T> targetState)
        {
			this.input = input;
			this.targetState = targetState;
		}
	}
}