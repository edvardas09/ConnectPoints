using ConnectPoints.Gameplay.LevelSelecion;
using ConnectPoints.Gameplay.Managers;
using System.Collections.Generic;
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

        private List<LevelOption> spawnedLevelOptions = new();
        private List<LevelData> levelDataList => GameManager.Instance.Levels.levels;

        private void Start()
        {
            levelsLeftButton.onClick.AddListener(OnLevelsLeftButtonClicked);
            levelsRightButton.onClick.AddListener(OnLevelsRightButtonClicked);

            SetLevelsPage(0);
        }

        private void OnDestroy()
        {
            levelsLeftButton.onClick.RemoveListener(OnLevelsLeftButtonClicked);
            levelsRightButton.onClick.RemoveListener(OnLevelsRightButtonClicked);
        }

        private void SpawnLevelOptions()
        {
            DespawnLevelOptions();

            spawnedLevelOptions.Clear();

            var startIndex = levelsPage * maxLevelsCount;
            var endIndex = Mathf.Min((levelsPage + 1) * maxLevelsCount, levelDataList.Count);

            for (int i = startIndex; i < endIndex; i++)
            {
                var levelOption = Instantiate(levelOptionPrefab, levelOptionsContainer);
                spawnedLevelOptions.Add(levelOption);
                levelOption.Setup((ushort)i);
            }
        }

        private void DespawnLevelOptions()
        {
            foreach (var levelOption in spawnedLevelOptions)
            {
                Destroy(levelOption.gameObject);
            }
        }

        private void OnLevelsRightButtonClicked()
        {
            SetLevelsPage(levelsPage + 1);
        }

        private void OnLevelsLeftButtonClicked()
        {
            SetLevelsPage(levelsPage - 1);
        }

        private void SetLevelsPage(int page)
        {
            levelsPage = (ushort)Mathf.Clamp(page, 0, levelDataList.Count / maxLevelsCount);
            SpawnLevelOptions();
            SetupLevelsButtons();
        }

        private void SetupLevelsButtons()
        {
            levelsLeftButton.gameObject.SetActive(levelsPage > 0);
            levelsRightButton.gameObject.SetActive(levelsPage < levelDataList.Count / maxLevelsCount);
        }
    }
}