using System;

namespace MyFSM
{
	public class EventFSM<T> : IUpdate, IFixedUpdate, ILateUpdate
    {
		public State<T> Current { get { return current; } }
		private State<T> current;

		public EventFSM(State<T> initial)
        {
			current = initial;
			current.Enter(default(T));
		}

		public void SendInput(T input)
        {
			State<T> newState;

			if (current.CheckInput(input, out newState))
            {
				current.Exit(input);
				current = newState;
				current.Enter(input);
			}
		}

		public void OnUpdate()
		{
            current.OnUpdate();
        }

        public void OnFixedUpdate()
		{
			current.OnFixedUpdate();
		}

		public void OnLateUpdate()
		{
			current.OnLateUpdate();
		}
	}
}