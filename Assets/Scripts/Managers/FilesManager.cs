using System.IO;
using Newtonsoft.Json;

namespace ConnectPoints.Managers
{
    public static class FilesManager
    {
        public static T GetFileContent<T>(string path)
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}