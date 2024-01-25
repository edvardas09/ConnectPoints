using ConnectPoints.Gameplay.Level;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ConnectPoints.Gameplay.Managers
{
    public class LineManager : MonoSingleton<LineManager>
    {
        public UnityAction OnLevelCompleted;

        [SerializeField] private Image linePrefab;
        [SerializeField] private Transform linesParent;
        [SerializeField] private Transform mainCanvas;

        private Point firstPoint;
        private Point lastPoint;
        private List<Point> pointsToDrawLineTo = new List<Point>();

        private void Start()
        {
            LevelManager.Instance.PointPressed += OnPointClicked;
        }

        private void OnDestroy()
        {
            LevelManager.Instance.PointPressed -= OnPointClicked;
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

            pointsToDrawLineTo.Add(point);

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

            var point = pointsToDrawLineTo[0];
            DrawLine(lastPoint, point);

            lastPoint = point;

            if (LevelManager.Instance.IsLastPoint(point))
            {
                pointsToDrawLineTo.Add(firstPoint);
            }
        }

        private void DrawLine(Point fromPoint, Point toPoint)
        {
            var line = Instantiate(linePrefab, linesParent);
            line.transform.position = fromPoint.GetPosition();

            var rotationToTarget = Vector2.SignedAngle(Vector2.up, toPoint.GetPosition() - fromPoint.GetPosition());
            line.rectTransform.rotation = Quaternion.Euler(0f, 0f, rotationToTarget);

            var lineSizeX = line.rectTransform.sizeDelta.x;
            var lineSizeY = Vector2.Distance(toPoint.GetPosition(), fromPoint.GetPosition()) / mainCanvas.localScale.x;
            var lineSize = new Vector2(lineSizeX, lineSizeY);
            LeanTween.size(line.rectTransform, lineSize, 0.5f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
            {
                if (firstPoint == toPoint)
                {
                    OnLevelCompleted?.Invoke();
                    return;
                }

                pointsToDrawLineTo.RemoveAt(0);
                DrawNewLine();
            });
        }
    }
}