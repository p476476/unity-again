using UnityEngine.Events;

namespace Again.Runtime.Components.Interfaces
{
    public interface IToggleButton
    {
        public void Toggle(bool isOn);
        public void SetOnClick(UnityAction<bool> onClick);
    }
}