using ConnectPoints.Gameplay.LevelSelecion;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ConnectPoints.Gameplay.Managers
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public const string GameplaySceneName = "Gameplay";

        public ushort SelectedLevel => selectedLevel;
        public Levels Levels => levels;

        private ushort selectedLevel;
        private Levels levels;

        protected override void Awake()
        {
            base.Awake();

            levels = FilesManager.GetFileContent<Levels>(FilesManager.LevelsPath);
        }

        public void SetSelectedLevel(ushort level)
        {
            selectedLevel = level;
        }

        public void LoadSelectedLevel()
        {
            SceneManager.LoadScene(GameplaySceneName, LoadSceneMode.Single);
        }
    }
}