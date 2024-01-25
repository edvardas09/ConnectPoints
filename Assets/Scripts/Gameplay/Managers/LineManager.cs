using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ConnectPoints.Gameplay.Level;

namespace ConnectPoints.Gameplay.Managers
{
    public class LineManager : MonoBehaviour
    {
        public UnityAction OnLevelCompleted;

        [SerializeField] private SpriteRenderer linePrefab;
        [SerializeField] private Transform linesParent;
        [SerializeField] private Transform mainCanvas;
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private float lineDrawDuration = 0.5f;

        private Point firstPoint;
        private Point lastPoint;
        private Queue<Point> pointsToDrawLineTo;

        public LineManager()
        {
            pointsToDrawLineTo = new Queue<Point>();
        }

        private void OnEnable()
        {
            levelManager.PointPressed += OnPointClicked;
        }

        private void OnDisable()
        {
            levelManager.PointPressed -= OnPointClicked;
        }

        public void AddLastPointToDrawLine()
        {
            pointsToDrawLineTo.Enqueue(firstPoint);
        }

        private void OnPointClicked(Point point)
        {
            if (point == null)
            {
                return;
            }

            if (firstPoint == null)
            {
                firstPoint = point;
                lastPoint = point;
                return;
            }

            pointsToDrawLineTo.Enqueue(point);

            if (pointsToDrawLineTo.Count == 1)
            {
                DrawNewLine();
            }
        }

        private void DrawNewLine()
        {
            if (pointsToDrawLineTo.Count == 0)
            {
                return;
            }

            Point _point = pointsToDrawLineTo.Dequeue();
            DrawLine(lastPoint, _point);

            lastPoint = _point;
        }

        private void DrawLine(Point fromPoint, Point toPoint)
        {
            SpriteRenderer _line = Instantiate(linePrefab, linesParent);
            _line.transform.position = fromPoint.GetPosition();

            float _rotationToTarget = Vector2.SignedAngle(Vector2.up, toPoint.GetPosition() - fromPoint.GetPosition());
            _line.transform.rotation = Quaternion.Euler(0f, 0f, _rotationToTarget);

            float _lineSizeX = _line.size.x;
            float _lineSizeY = Vector2.Distance(toPoint.GetPosition(), fromPoint.GetPosition());
            Vector2 _lineSize = new Vector2(_lineSizeX, _lineSizeY);
            LeanTween.value(_line.gameObject, 0, _lineSizeY, lineDrawDuration).setEase(LeanTweenType.easeInOutQuad).setOnUpdate(newSizeY =>
            {
                Vector2 _newSize = new Vector2(_line.size.x, newSizeY);
                _line.size = _newSize;
            }).setOnComplete(() =>
            {
                if (firstPoint == toPoint)
                {
                    OnLevelCompleted?.Invoke();
                    return;
                }

                DrawNewLine();
            });
        }
    }
}