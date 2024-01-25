using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using ConnectPoints.Gameplay.Level;

namespace ConnectPoints.Gameplay.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public UnityAction<Point> PointPressed;

        private const string LEVEL_SELECTION_SCENE_NAME  = "LevelSelection";
        private const int POINT_POSITION_REF_SCALE    = 1000;

        [SerializeField] private Point pointPrefab;
        [SerializeField] private Transform pointsParent;
        [SerializeField] private GameObject levelCompletedObject;
        [SerializeField] private LineManager lineManager;
        [SerializeField] private float paddingFromSides = 1f;
        [SerializeField] private float levelEndAnimationDuration = 0.5f;
        [SerializeField] private float delayBeforeLoadingMenuScene = 1f;

        private List<PointData> pointDatas = new List<PointData>();

        private int selectedLevel;
        private int currentPointId;
        private float maxSize;

        public LevelManager()
        {
            currentPointId = 0;
        }

        private void Start()
        {
            if (GameManager.Instance == null)
            {
                SceneManager.LoadScene(LEVEL_SELECTION_SCENE_NAME);
                return;
            }

            LoadSelectedLevel();
        }

        private void OnEnable()
        {
            lineManager.OnLevelCompleted += OnLevelCompleted;
        }

        private void OnDisable()
        {
            lineManager.OnLevelCompleted -= OnLevelCompleted;
        }

        private void LoadSelectedLevel()
        {
            if (GameManager.Instance == null)
            {
                return;
            }

            Rect _screenRect = GetScreenWorldRect();
            maxSize = _screenRect.height > _screenRect.width ? _screenRect.width : _screenRect.height;
            maxSize -= paddingFromSides * 2;

            pointDatas.Clear();
            selectedLevel = GameManager.Instance.SelectedLevel;

            List<int> _selectedLevelPointPositions = GameManager.Instance.Levels.LevelDataList[selectedLevel].PointPositions;
            for (int i = 0; i < _selectedLevelPointPositions.Count; i++)
            {
                int _pointPosition = _selectedLevelPointPositions[i];

                if (i % 2 == 0)
                {
                    pointDatas.Add(new PointData
                    {
                        Id = pointDatas.Count,
                        PositionX = _pointPosition,
                        PositionY = 0
                    });
                    continue;
                }

                PointData _lastPointData = pointDatas[pointDatas.Count - 1];
                _lastPointData.PositionY = _pointPosition;

                SpawnPoint(_lastPointData);
            }

            UpdateParentPosition();
        }

        private void SpawnPoint(PointData lastPointData)
        {
            Point _point = Instantiate(pointPrefab, pointsParent);
            _point.transform.SetAsFirstSibling();

            Vector2 _position = new Vector2(
                ConvertPointPositionToScreenPosition(lastPointData.PositionX),
                ConvertPointPositionToScreenPosition(lastPointData.PositionY) * -1
                );

            _point.transform.position = _position;
            _point.Initialize(lastPointData);
            _point.OnPointClicked += OnPointClicked;
        }

        private float ConvertPointPositionToScreenPosition(int pointPosition)
        {
            return (float)pointPosition / POINT_POSITION_REF_SCALE * maxSize;
        }

        private Rect GetScreenWorldRect()
        {
            Camera camera = Camera.main;
            Vector3 bottomLeft = camera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
            Vector3 topRight = camera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));
            return (new Rect(bottomLeft.x, bottomLeft.y, topRight.x * 2f, topRight.y * 2f));
        }

        private void UpdateParentPosition()
        {
            float _halfMaxSize = maxSize / 2;
            Vector2 _topRight = new Vector2(-_halfMaxSize, _halfMaxSize);
            pointsParent.position = _topRight;
        }

        private void OnPointClicked(Point point)
        {
            if (!IsCorrectPointPressed(point))
            {
                return;
            }

            point.OnPointClicked -= OnPointClicked;
            point.PointPressed();

            if (!IsLastPoint(point))
            {
                return;
            }

            lineManager.AddLastPointToDrawLine();
        }

        private bool IsLastPoint(Point point)
        {
            return pointDatas.Count - 1 == point.PointData.Id;
        }

        private bool IsCorrectPointPressed(Point point)
        {
            bool _isCorrectPointPressed = point.PointData.Id == currentPointId;

            if (_isCorrectPointPressed)
            {
                currentPointId++;
                PointPressed?.Invoke(point);
            }

            return _isCorrectPointPressed;
        }

        private void OnLevelCompleted()
        {
            Vector3 _newPosition = levelCompletedObject.transform.position;
            _newPosition.y = ((RectTransform)pointsParent.transform).sizeDelta.y * -1;
            levelCompletedObject.transform.position = _newPosition;

            levelCompletedObject.SetActive(true);

            LeanTween.moveLocalY(levelCompletedObject, 0f, levelEndAnimationDuration)
                .setEase(LeanTweenType.easeInOutQuad)
                .setOnComplete(() =>
            {
                LeanTween.moveLocalY(levelCompletedObject, ((RectTransform)pointsParent.transform).sizeDelta.y * -2, levelEndAnimationDuration)
                .setEase(LeanTweenType.easeInOutQuad)
                .setDelay(delayBeforeLoadingMenuScene)
                .setOnComplete(() =>
                {
                    SceneManager.LoadScene(LEVEL_SELECTION_SCENE_NAME, LoadSceneMode.Single);
                });
            });
        }

    }
}