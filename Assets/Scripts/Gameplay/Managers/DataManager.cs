using UnityEngine;
using ConnectPoints.Gameplay.LevelSelecion;
using ConnectPoints.Managers;

namespace ConnectPoints.Gameplay.Managers
{
    public class DataManager : Singleton<DataManager>
    {
        private static string LEVELS_PATH => $"{Application.streamingAssetsPath}/level_data.json";

        public int SelectedLevel => selectedLevel;
        public Levels Levels => levels;

        private int selectedLevel = 0;
        private Levels levels = new Levels();

        public DataManager()
        {
            LoadLevels();
        }

        protected void LoadLevels()
        {
            levels = FilesManager.GetFileContent<Levels>(LEVELS_PATH);
            for (int i = 0; i < levels.LevelDataList.Count; i++)
            {
                LevelData levelData = levels.LevelDataList[i];
                if (levelData.PointPositions.Count % 2 == 0)
                {
                    continue;
                }

                Debug.LogWarning($"Y coordinate is missing in level NR.{i+1}");
            }
        }

        public void SetSelectedLevel(int _level)
        {
            selectedLevel = _level;
        }

    }
}