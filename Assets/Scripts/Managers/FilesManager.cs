using Newtonsoft.Json;
using System.IO;

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