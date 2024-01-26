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
        [SerializeField] private GridLayoutGroup levelOptionsContainer;

        private int levelsPage = 0;

        private List<LevelOption> spawnedLevelOptions;
        private List<LevelData> levelDataList;

        public LevelSelectionController()
        {
            spawnedLevelOptions = new List<LevelOption>();
        }

        private void Start()
        {
            levelDataList = DataManager.Instance.Levels.LevelDataList;
            SetupLayoutProperties();
            SetLevelsPage(0);
        }

        private void OnEnable()
        {
            levelsLeftButton.onClick.AddListener(OnLevelsLeftButtonClicked);
            levelsRightButton.onClick.AddListener(OnLevelsRightButtonClicked);
        }

        private void OnDisable()
        {
            levelsLeftButton.onClick.RemoveListener(OnLevelsLeftButtonClicked);
            levelsRightButton.onClick.RemoveListener(OnLevelsRightButtonClicked);
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

        private void SpawnLevelOptions()
        {
            DespawnLevelOptions();

            spawnedLevelOptions.Clear();

            int _startIndex = levelsPage * maxLevelsCount;
            int _endIndex = Mathf.Min((levelsPage + 1) * maxLevelsCount, levelDataList.Count);

            for (int i = _startIndex; i < _endIndex; i++)
            {
                LevelOption _levelOption = Instantiate(levelOptionPrefab, levelOptionsContainer.transform);
                spawnedLevelOptions.Add(_levelOption);
                _levelOption.Setup(i);
            }
        }

        private void SetupLevelsButtons()
        {
            levelsLeftButton.gameObject.SetActive(levelsPage > 0);
            levelsRightButton.gameObject.SetActive(levelsPage < levelDataList.Count - maxLevelsCount * (levelsPage + 1));
        }

        private void SetupLayoutProperties()
        {
            RectTransform _levelsContainerRectTransform = (RectTransform)levelOptionsContainer.transform;

            float _levelsContainerHeight = _levelsContainerRectTransform.rect.size.y - levelOptionsContainer.padding.vertical;
            float _levelsContainerWidth = _levelsContainerRectTransform.rect.size.x - levelOptionsContainer.padding.horizontal;
            Vector2 _cellSize = levelOptionsContainer.cellSize;
            Vector2 _spacing = levelOptionsContainer.spacing;

            int _maxAmountOfCellsHorizontally = Mathf.FloorToInt((_levelsContainerWidth - _cellSize.x) / (_cellSize.x + _spacing.x)) + 1;
            int _maxAmountOfCellsVertically = Mathf.FloorToInt((_levelsContainerHeight - _cellSize.y) / (_cellSize.y + _spacing.y)) + 1;

            float _spaceForSpacingHorizontally = _levelsContainerWidth - (_maxAmountOfCellsHorizontally * _cellSize.x);
            float _newSpacingX = _spaceForSpacingHorizontally / (_maxAmountOfCellsHorizontally - 1);

            float _spaceForSpacingVertically = _levelsContainerHeight - (_maxAmountOfCellsVertically * _cellSize.y);
            float _newSpacingY = _spaceForSpacingVertically / (_maxAmountOfCellsVertically - 1);

            levelOptionsContainer.spacing = new Vector2(_newSpacingX, _newSpacingY);

            maxLevelsCount = _maxAmountOfCellsHorizontally * _maxAmountOfCellsVertically;
        }

    }
}