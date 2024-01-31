using System.Collections.Generic;
using UnityEngine;
using ConnectPoints.Gameplay.Level;
using System;

namespace ConnectPoints.Gameplay.Managers
{
    public class LineManager : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer linePrefab;
        [SerializeField] private Transform linesParent;
        [SerializeField] private Transform mainCanvas;
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private float lineDrawDuration = 0.5f;

        public Action LevelCompleted;

        private Point firstPoint;
        private Point lastPoint;
        private readonly Queue<Point> pointsToDrawLineTo = new Queue<Point>();

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

        private void OnPointClicked(Point _point)
        {
            if (_point == null)
            {
                return;
            }

            if (firstPoint == null)
            {
                firstPoint = _point;
                lastPoint = _point;
                return;
            }

            pointsToDrawLineTo.Enqueue(_point);

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

            Point _point = pointsToDrawLineTo.Peek();
            DrawLine(lastPoint, _point);

            lastPoint = _point;
        }

        private void DrawLine(Point _fromPoint, Point _toPoint)
        {
            SpriteRenderer _line = Instantiate(linePrefab, linesParent);
            _line.transform.position = _fromPoint.GetPosition();

            float _rotationToTarget = Vector2.SignedAngle(Vector2.up, _toPoint.GetPosition() - _fromPoint.GetPosition());
            _line.transform.rotation = Quaternion.Euler(0f, 0f, _rotationToTarget);

            float _lineSizeX = _line.size.x;
            float _lineSizeY = Vector2.Distance(_toPoint.GetPosition(), _fromPoint.GetPosition());
            LeanTween.value(_line.gameObject, 0, _lineSizeY, lineDrawDuration)
                .setEase(LeanTweenType.easeInOutQuad)
                .setOnUpdate(_newSizeY =>
                {
                    Vector2 _newSize = new Vector2(_line.size.x, _newSizeY);
                    _line.size = _newSize;
                })
                .setOnComplete(DrawNextLine);

            return;

            void DrawNextLine()
            {
                pointsToDrawLineTo.Dequeue();

                if (firstPoint == _toPoint)
                {
                    LevelCompleted?.Invoke();
                    return;
                }

                DrawNewLine();
            }
        }
    }
}