using System.Collections.Generic;
using Newtonsoft.Json;

namespace ConnectPoints.Gameplay.LevelSelecion
{
    public class Levels
    {
        [JsonProperty("levels")] public List<LevelData> LevelDataList;
    }
}