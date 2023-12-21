using Newtonsoft.Json;
using System.Collections.Generic;

namespace ConnectPoints.Gameplay.LevelSelecion
{
    public class Levels
    {
        [JsonProperty("levels")] public List<LevelData> levels;
    }
}