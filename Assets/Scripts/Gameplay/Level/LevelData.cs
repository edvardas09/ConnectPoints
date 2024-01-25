using System.Collections.Generic;
using Newtonsoft.Json;

namespace ConnectPoints.Gameplay.LevelSelecion
{
    public class LevelData
    {
        [JsonProperty("level_data")] public List<int> PointPositions;
    }
}