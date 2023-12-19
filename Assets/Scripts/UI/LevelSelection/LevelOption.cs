using TMPro;
using UnityEngine;

namespace ConnectPoints.UI.LevelSelecion
{
    public class LevelOption : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelText;

        private ushort level;

        public void Setup(ushort level)
        {
            this.level = level;
            levelText.text = level.ToString();
        }
    }
}