using ConnectPoints.Gameplay.Level;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ConnectPoints.Gameplay.Managers
{
    public class LevelManager : MonoSingleton<LevelManager>
    {
        public UnityAction<Point> PointPressed;

        public const string LevelSelectionSceneName = "LevelSelection";
        public const ushort PointPositionRefScale = 1000;

        [SerializeField] private Point pointPrefab;
        [SerializeField] private Transform pointsParent;

        private ushort selectedLevel;
        private List<PointData> pointDatas = new();
        private List<Point> spawnedPoints = new();

        private ushort currentPointId = 0;

        private Vector2 pointsParentTransformSize;

        private void Start()
        {
            if (GameManager.Instance == null)
            {
                SceneManager.LoadScene(LevelSelectionSceneName);
                return;
            }

            pointsParentTransformSize = ((RectTransform)pointsParent.transform).sizeDelta;

            LoadSelectedLevel();
        }

        public bool IsCorrectPointPressed(Point point)
        {
            var isCorrectPointPressed = point.PointData.Id == currentPointId;

            if (isCorrectPointPressed)
            {
                currentPointId++;
                PointPressed?.Invoke(point);
            }

            return isCorrectPointPressed;
        }

        public bool IsLastPoint(Point point)
        {
            return pointDatas.Count - 1 == point.PointData.Id;
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
                        Id = (ushort)pointDatas.Count,
                        PositionX = pointPosition,
                        PositionY = 0
                    });
                    continue;
                }

                var lastPointData = pointDatas[^1];
                lastPointData.PositionY = pointPosition;

                SpawnPoint(lastPointData);
            }
        }

        private void SpawnPoint(PointData lastPointData)
        {
            var point = Instantiate(pointPrefab, pointsParent);
            spawnedPoints.Add(point);
            point.transform.SetAsFirstSibling();

            var position = new Vector2(
                ConvertPointPositionToScreenPosition(lastPointData.PositionX),
                ConvertPointPositionToScreenPosition(lastPointData.PositionY) * -1
                );

            var rectTransform = (RectTransform)point.transform;
            rectTransform.anchoredPosition = position;
            point.Initialize(lastPointData);
        }

        private float ConvertPointPositionToScreenPosition(ushort pointPosition)
        {
            return (float)pointPosition / PointPositionRefScale * (pointsParentTransformSize.x - ((RectTransform)pointPrefab.transform).sizeDelta.x);
        }
    }
}