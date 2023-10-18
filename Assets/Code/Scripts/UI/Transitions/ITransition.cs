using System;

namespace Game.UI.Transitions
{
    public interface ITransition
    {
        public bool IsShowed { get; }

        public void Show(Action completeHandler=null);

        public void Hide(Action completeHandler=null);

        public void Complete();
    }
}
