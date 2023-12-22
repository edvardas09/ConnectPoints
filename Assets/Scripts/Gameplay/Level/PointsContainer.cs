using ConnectPoints.Gameplay.Managers;
using UnityEngine;

namespace ConnectPoints.Gameplay.Level
{
    public class PointsContainer : MonoBehaviour
    {
        [SerializeField] private RectTransform mainCanvas;
        [SerializeField] private float paddingFromScreenBorder = 100f;

        //TODO: onResolutionChange reinitialize proportions
        private void Start()
        {
            var width = mainCanvas.rect.width;
            var height = mainCanvas.rect.height;            
            var referenceSize = width < height ? width : height;

            var rectTransform = (RectTransform)transform;
            rectTransform.sizeDelta = new Vector2(referenceSize - paddingFromScreenBorder, referenceSize - paddingFromScreenBorder);

            LevelManager.Instance.LoadSelectedLevel();
        }
    }
}