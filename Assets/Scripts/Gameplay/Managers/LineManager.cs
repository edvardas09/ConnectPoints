using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ConnectPoints.Gameplay.Managers
{
    public class LineManager : MonoSingleton<LineManager>
    {
        [SerializeField] private Image linePrefab;
        [SerializeField] private Transform linesParent;

        private List<Image> lines = new();


        
    }
}