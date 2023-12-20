using ConnectPoints.Gameplay.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace ConnectPoints.UI.LevelSelecion
{
    public class LevelSelectionController : MonoBehaviour
    {
        [SerializeField] private ushort maxLevelsCount;

        [Header("UI references")]
        [SerializeField] private Button levelsLeftButton;
        [SerializeField] private Button levelsRightButton;

        [Header("Level option references")]
        [SerializeField] private LevelOption levelOptionPrefab;
        [SerializeField] private Transform levelOptionsContainer;

        private ushort levelsPage = 0;

        private void Start()
        {
            GameManager.Instance.OnLevelsLoaded += OnLevelsLoaded;

            levelsLeftButton.onClick.AddListener(() => ChangeLevelsPage(levelsPage--));
            levelsRightButton.onClick.AddListener(() => ChangeLevelsPage(levelsPage++));
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnLevelsLoaded -= OnLevelsLoaded;

            levelsLeftButton.onClick.RemoveAllListeners();
            levelsRightButton.onClick.RemoveAllListeners();
        }

        private void SpawnLevelOptions()
        {
            var levelDatas = GameManager.Instance.Levels.levels;

            for (int i = levelsPage * maxLevelsCount; i < levelDatas.Count; i++)
            {
                if (i >= maxLevelsCount)
                    break;

                var levelOption = Instantiate(levelOptionPrefab, levelOptionsContainer);
                levelOption.Setup((ushort)i);
            }
        }

        private void ChangeLevelsPage(int page)
        {
            levelsPage = (ushort)Mathf.Clamp(page, 0, GameManager.Instance.Levels.levels.Count / maxLevelsCount);
            SpawnLevelOptions();
            SetupLevelsButtons();
        }

        private void SetupLevelsButtons()
        {
            levelsLeftButton.gameObject.SetActive(levelsPage > 0);
            levelsRightButton.gameObject.SetActive(levelsPage < GameManager.Instance.Levels.levels.Count / maxLevelsCount);
        }

        private void OnLevelsLoaded()
        {
            ChangeLevelsPage(0);
        }
    }
}