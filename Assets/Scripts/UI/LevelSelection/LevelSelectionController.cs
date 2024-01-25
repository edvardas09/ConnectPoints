using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ConnectPoints.Gameplay.LevelSelecion;
using ConnectPoints.Gameplay.Managers;

namespace ConnectPoints.UI.LevelSelecion
{
    public class LevelSelectionController : MonoBehaviour
    {
        [SerializeField] private int maxLevelsCount;

        [Header("UI references")]
        [SerializeField] private Button levelsLeftButton;
        [SerializeField] private Button levelsRightButton;

        [Header("Level option references")]
        [SerializeField] private LevelOption levelOptionPrefab;
        [SerializeField] private Transform levelOptionsContainer;

        private int levelsPage = 0;

        private List<LevelOption> spawnedLevelOptions = new List<LevelOption>();
        private List<LevelData> levelDataList => GameManager.Instance.Levels.LevelDataList;

        private void OnEnable()
        {
            levelsLeftButton.onClick.AddListener(OnLevelsLeftButtonClicked);
            levelsRightButton.onClick.AddListener(OnLevelsRightButtonClicked);

            SetLevelsPage(0);
        }

        private void OnDisable()
        {
            levelsLeftButton.onClick.RemoveListener(OnLevelsLeftButtonClicked);
            levelsRightButton.onClick.RemoveListener(OnLevelsRightButtonClicked);
        }

        private void SpawnLevelOptions()
        {
            DespawnLevelOptions();

            spawnedLevelOptions.Clear();

            int _startIndex = levelsPage * maxLevelsCount;
            int _endIndex = Mathf.Min((levelsPage + 1) * maxLevelsCount, levelDataList.Count);

            for (int i = _startIndex; i < _endIndex; i++)
            {
                LevelOption _levelOption = Instantiate(levelOptionPrefab, levelOptionsContainer);
                spawnedLevelOptions.Add(_levelOption);
                _levelOption.Setup(i);
            }
        }

        private void DespawnLevelOptions()
        {
            foreach (LevelOption _levelOption in spawnedLevelOptions)
            {
                Destroy(_levelOption.gameObject);
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
            levelsPage = Mathf.Clamp(page, 0, levelDataList.Count / maxLevelsCount);
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