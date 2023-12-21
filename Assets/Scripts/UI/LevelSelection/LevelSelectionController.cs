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
        private List<LevelData> levelDatas => GameManager.Instance.Levels.levels;

        private void Start()
        {
            levelsLeftButton.onClick.AddListener(() => ChangeLevelsPage(levelsPage - 1));
            levelsRightButton.onClick.AddListener(() => ChangeLevelsPage(levelsPage + 1));

            ChangeLevelsPage(0);
        }

        private void OnDestroy()
        {
            levelsLeftButton.onClick.RemoveAllListeners();
            levelsRightButton.onClick.RemoveAllListeners();
        }

        private void SpawnLevelOptions()
        {
            foreach (var levelOption in spawnedLevelOptions)
            {
                Destroy(levelOption.gameObject);
            }

            spawnedLevelOptions.Clear();

            for (int i = levelsPage * maxLevelsCount; i < levelDatas.Count; i++)
            {
                if (i >= (levelsPage + 1) * maxLevelsCount)
                    break;

                var levelOption = Instantiate(levelOptionPrefab, levelOptionsContainer);
                spawnedLevelOptions.Add(levelOption);
                levelOption.Setup((ushort)i);
            }
        }

        private void ChangeLevelsPage(int page)
        {
            levelsPage = (ushort)Mathf.Clamp(page, 0, levelDatas.Count / maxLevelsCount);
            SpawnLevelOptions();
            SetupLevelsButtons();
        }

        private void SetupLevelsButtons()
        {
            levelsLeftButton.gameObject.SetActive(levelsPage > 0);
            levelsRightButton.gameObject.SetActive(levelsPage < levelDatas.Count / maxLevelsCount);
        }
    }
}