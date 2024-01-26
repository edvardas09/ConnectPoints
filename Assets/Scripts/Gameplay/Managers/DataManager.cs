using UnityEngine;
using UnityEngine.SceneManagement;
using ConnectPoints.Gameplay.LevelSelecion;
using ConnectPoints.Managers;

namespace ConnectPoints.Gameplay.Managers
{
    public class DataManager : MonoSingleton<DataManager>
    {
        private static string LEVELS_PATH           => $"{Application.streamingAssetsPath}/level_data.json";
        private const string GAMEPLAY_SCENE_NAME    = "Gameplay";

        public int SelectedLevel => selectedLevel;
        public Levels Levels => levels;

        private int selectedLevel;
        private Levels levels;

        protected override void Awake()
        {
            base.Awake();

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

        public void SetSelectedLevel(int level)
        {
            selectedLevel = level;
        }

        public void LoadSelectedLevel()
        {
            SceneManager.LoadScene(GAMEPLAY_SCENE_NAME, LoadSceneMode.Single);
        }
    }
}