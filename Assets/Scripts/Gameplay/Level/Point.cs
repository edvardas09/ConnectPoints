using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Events;

namespace ConnectPoints.Gameplay.Level
{
    public class Point : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public UnityAction<Point> OnPointClicked;

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private TextMeshProUGUI idText;
        [SerializeField] private Sprite pressedSprite;

        [Header("Animation")]
        [SerializeField] private float onHoverScaleIncrease = 0.1f;
        [SerializeField] private float scaleAnimationDuration = 0.2f;

        public PointData PointData => pointData;

        private PointData pointData;
        private bool isPressed;
        private Vector2 defaultScale;

        public void Initialize(PointData pointData)
        {
            this.pointData = pointData;
            defaultScale = gameObject.transform.localScale;

            idText.text = $"{pointData.Id + 1}";
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isPressed)
            {
                return;
            }

            LeanTween.cancel(gameObject);
            LeanTween.scale(gameObject, defaultScale, scaleAnimationDuration);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isPressed)
            {
                return;
            }

            LeanTween.cancel(gameObject);
            LeanTween.scale(gameObject, new Vector2(defaultScale.x + onHoverScaleIncrease, defaultScale.y + onHoverScaleIncrease), scaleAnimationDuration);
        }

        public void OnPointerClick(PointerEventData _)
        {
            OnPointClicked?.Invoke(this);
        }

        public void PointPressed()
        {
            isPressed = true;

            LeanTween.cancel(gameObject);
            LeanTween.scale(gameObject, defaultScale, scaleAnimationDuration);

            spriteRenderer.sprite = pressedSprite;
        }
    }
}