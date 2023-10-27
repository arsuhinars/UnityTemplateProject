namespace Game.States
{
    public class StateMachine
    {
        public BaseState ActiveState
        {
            get => m_activeState;
            set
            {
                m_activeState?.Exit();
                value?.Enter();
                m_activeState = value;
            }
        }

        private BaseState m_activeState = null;

        public void Update()
        {
            ActiveState?.Update();
        }

        public void FixedUpdate()
        {
            ActiveState?.FixedUpdate();
        }
    }
}
