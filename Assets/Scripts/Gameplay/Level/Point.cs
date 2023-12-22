using ConnectPoints.Gameplay.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ConnectPoints.Gameplay.Level
{
    public class Point : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private const float onHoverScale = 1.2f;

        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI idText;
        [SerializeField] private CanvasGroup initialStateObject;
        [SerializeField] private CanvasGroup pressedStateObject;

        public PointData PointData => pointData;

        private PointData pointData;

        public void Initialize(PointData pointData)
        {
            this.pointData = pointData;

            idText.text = $"{pointData.Id + 1}";
            button.onClick.AddListener(OnButtonClick);
        }

        public Vector3 GetPosition()
        {
            return initialStateObject.transform.position;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!initialStateObject.isActiveAndEnabled)
            {
                return;
            }

            LeanTween.cancel(initialStateObject.gameObject);
            LeanTween.scale(initialStateObject.gameObject, Vector3.one, 0.2f);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!initialStateObject.isActiveAndEnabled)
            {
                return;
            }

            LeanTween.cancel(initialStateObject.gameObject);
            LeanTween.scale(initialStateObject.gameObject, new Vector3(onHoverScale, onHoverScale), 0.2f);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            if (!LevelManager.Instance.IsCorrectPointPressed(this))
            {
                return;
            }

            button.onClick.RemoveListener(OnButtonClick);

            LeanTween.cancel(initialStateObject.gameObject);
            LeanTween.scale(initialStateObject.gameObject, Vector3.one, 0.2f);

            pressedStateObject.gameObject.SetActive(true);
            LeanTween.alphaCanvas(pressedStateObject, 1f, 0.2f).setOnComplete(() =>
            {
                initialStateObject.gameObject.SetActive(false);
                initialStateObject.alpha = 0f;
            });

            transform.SetAsFirstSibling();
        }
    }
}