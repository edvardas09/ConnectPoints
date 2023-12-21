using ConnectPoints.Gameplay.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ConnectPoints.Gameplay.Level
{
    public class Point : MonoBehaviour
    {
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

            LeanTween.alphaCanvas(initialStateObject, 0f, 0.2f).setOnComplete(() =>
            {
                initialStateObject.gameObject.SetActive(false);
            });

            pressedStateObject.gameObject.SetActive(true);
            LeanTween.alphaCanvas(pressedStateObject, 1f, 0.2f);

            transform.SetAsFirstSibling();
        }
    }
}