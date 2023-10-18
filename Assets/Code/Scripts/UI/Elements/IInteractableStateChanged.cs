using System;

namespace Game.UI.Elements
{
    public interface IInteractableStateChanged
    {
        public event Action<bool> OnInteractableStateChanged;
    }
}
