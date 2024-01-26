using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace ConnectPoints.UI.LevelSelecion
{
    public class LevelOption : MonoBehaviour
    {
        public UnityAction<int> LevelOptionClicked;

        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Button button;

        private int level;

        private void OnEnable()
        {
            button.onClick.AddListener(OnLevelSelected);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(OnLevelSelected);
        }

        public void Setup(int level)
        {
            this.level = level;
            levelText.text = $"{level + 1}";
        }

        private void OnLevelSelected()
        {
            LevelOptionClicked?.Invoke(level);
        }
    }
}