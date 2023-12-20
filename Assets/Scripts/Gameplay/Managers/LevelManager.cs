using ConnectPoints.Gameplay.Level;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ConnectPoints.Gameplay.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public const string LevelSelectionSceneName = "LevelSelection";

        private ushort selectedLevel;
        private List<PointData> pointDatas = new();

        private void Start()
        {
            if (GameManager.Instance == null)
            {
                SceneManager.LoadScene(LevelSelectionSceneName);
                return;
            }

            LoadSelectedLevel();
        }

        private void LoadSelectedLevel()
        {
            pointDatas.Clear();
            selectedLevel = GameManager.Instance.SelectedLevel;

            for (int i = 0; i < GameManager.Instance.Levels.levels[selectedLevel].pointPositions.Count; i++)
            {
                ushort pointPosition = GameManager.Instance.Levels.levels[selectedLevel].pointPositions[i];

                if (i % 2 == 0)
                {
                    pointDatas.Add(new PointData
                    {
                        PositionX = pointPosition,
                        PositionY = 0
                    });
                    continue;
                }

                var lastPointData = pointDatas[^1];
                lastPointData.PositionY = pointPosition;
            }
        }
    }
}