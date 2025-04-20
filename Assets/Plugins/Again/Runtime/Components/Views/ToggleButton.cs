using Again.Runtime.Components.Interfaces;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Again.Runtime.Components.Views
{
    public class ToggleButton : MonoBehaviour, IToggleButton
    {
        public Color onColor = Color.white;
        public Color offColor = new(0.8f, 0.8f, 0.8f, 0.5f);
        private Button _button;
        private Image _image;
        private bool _isOn;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _image = GetComponent<Image>();
        }

        public void Toggle(bool isOn)
        {
            _isOn = isOn;
            _image.color = isOn ? onColor : offColor;
        }

        public void SetOnClick(UnityAction<bool> onClick)
        {
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => onClick(!_isOn));
        }
    }
}