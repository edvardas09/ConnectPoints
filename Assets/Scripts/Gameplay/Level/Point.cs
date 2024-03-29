using UnityEngine;
using TMPro;
using System;

namespace ConnectPoints.Gameplay.Level
{
    public class Point : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private TextMeshPro idText;
        [SerializeField] private Sprite pressedSprite;
        [SerializeField] private BoxCollider2D boxCollider2D;

        [Header("Animation")]
        [SerializeField] private float onHoverScaleIncrease = 0.1f;
        [SerializeField] private float scaleAnimationDuration = 0.2f;

        public Action<Point> PointClicked;

        private const int PRESSED_POINT_Z_POSITION = 500;

        public PointData PointData => pointData;

        private PointData pointData;
        private bool isPressed;
        private Vector2 defaultScale;

        public void Initialize(PointData _pointData)
        {
            this.pointData = _pointData;
            defaultScale = gameObject.transform.localScale;

            idText.text = $"{_pointData.Id + 1}";

            transform.position = new Vector3(transform.position.x, transform.position.y, _pointData.Id);
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void OnMouseExit()
        {
            if (isPressed)
            {
                return;
            }

            LeanTween.cancel(gameObject);
            LeanTween.scale(gameObject, defaultScale, scaleAnimationDuration);
        }

        public void OnMouseEnter()
        {
            if (isPressed)
            {
                return;
            }

            LeanTween.cancel(gameObject);
            LeanTween.scale(gameObject, new Vector2(defaultScale.x + onHoverScaleIncrease, defaultScale.y + onHoverScaleIncrease), scaleAnimationDuration);
        }

        public void OnMouseDown()
        {
            PointClicked?.Invoke(this);
        }

        public void PointPressed()
        {
            isPressed = true;

            LeanTween.cancel(gameObject);
            LeanTween.scale(gameObject, defaultScale, scaleAnimationDuration);

            spriteRenderer.sprite = pressedSprite;
            idText.gameObject.SetActive(false);
            boxCollider2D.enabled = false;
            transform.position = new Vector3(transform.position.x, transform.position.y, PRESSED_POINT_Z_POSITION);
        }
    }
}