using TMPro;
using UnityEngine;

namespace Again.Runtime.Components.Views
{
    public class LogBlock : MonoBehaviour
    {
        public TMP_Text contentText;
        public TMP_Text nameText;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void SetColor(Color color)
        {
            nameText.color = color;
            contentText.color = color;
        }

        public void SetName(string speakerName)
        {
            nameText.text = speakerName;
        }

        public void SetContent(string content)
        {
            contentText.text = content;
        } // ReSharper disable Unity.PerformanceAnalysis
    }
}