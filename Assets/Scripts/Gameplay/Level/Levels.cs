using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectPoints.Gameplay.LevelSelecion
{
    public class Levels : MonoBehaviour
    {
        [JsonProperty("levels")] public List<LevelData> levels;
    }
}