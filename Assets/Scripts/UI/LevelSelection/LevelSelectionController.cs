using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ConnectPoints.Gameplay.LevelSelecion;
using ConnectPoints.Gameplay.Managers;
using ConnectPoints.Enums;

namespace ConnectPoints.UI.LevelSelecion
{
    public class LevelSelectionController : MonoBehaviour
    {
        [Header("Level option references")]
        [SerializeField] private LevelOption levelOptionPrefab;
        [SerializeField] private GridLayoutGroup levelOptionsContainer;

        private List<LevelData> levelDataList = new List<LevelData>();

        private void Start()
        {
            levelDataList = DataManager.Instance.Levels.LevelDataList;
            SpawnLevelOptions();
        }

        private void SpawnLevelOptions()
        {
            for (int i = 0; i < levelDataList.Count; i++)
            {
                LevelOption _levelOption = Instantiate(levelOptionPrefab, levelOptionsContainer.transform);
                _levelOption.Setup(i);
                _levelOption.LevelOptionClicked += OnLevelSelected;
            }
        }

        private void OnLevelSelected(int _level)
        {
            DataManager.Instance.SetSelectedLevel(_level);
            SceneManager.LoadScene(SceneName.Gameplay.ToString(), LoadSceneMode.Single);
        }

    }
}