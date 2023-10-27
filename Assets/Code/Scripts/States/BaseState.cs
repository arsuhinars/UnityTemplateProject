using System;

namespace Game.States
{
    [Serializable]
    public abstract class BaseState
    {
        public event Action OnEntered;
        public event Action OnExited;

        public bool IsActive { get; private set; } = false;

        public void Enter()
        {
            IsActive = true;
            EnterHandler();
            OnEntered?.Invoke();
        }

        public void Exit()
        {
            IsActive = false;
            ExitHandler();
            OnExited?.Invoke();
        }

        public virtual void Update() { }

        public virtual void FixedUpdate() { }

        protected virtual void EnterHandler() { }

        protected virtual void ExitHandler() { }
    }
}
