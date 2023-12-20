using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace ConnectPoints.Gameplay.Managers
{
    public static class FilesManager
    {
        public static string LevelsPath => $"{Application.dataPath}/Levels/level_data.json";

        public static T GetFileContent<T>(string path)
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}