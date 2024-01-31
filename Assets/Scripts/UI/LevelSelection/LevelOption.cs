using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace ConnectPoints.UI.LevelSelecion
{
    public class LevelOption : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Button button;

        public Action<int> LevelOptionClicked;

        private int level;

        private void OnEnable()
        {
            button.onClick.AddListener(OnLevelSelected);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(OnLevelSelected);
        }

        public void Setup(int _level)
        {
            this.level = _level;
            levelText.text = $"{_level + 1}";
        }

        private void OnLevelSelected()
        {
            LevelOptionClicked?.Invoke(level);
        }
    }
}