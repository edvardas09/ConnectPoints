using ConnectPoints.Gameplay.LevelSelecion;
using UnityEngine.Events;

namespace ConnectPoints.Gameplay.Managers
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public UnityAction OnLevelsLoaded;

        public ushort SelectedLevel => selectedLevel;
        public Levels Levels => levels;

        private ushort selectedLevel;
        private Levels levels;

        private void Start()
        {
            levels = FilesManager.GetFileContent<Levels>(FilesManager.LevelsPath);
            OnLevelsLoaded?.Invoke();
        }

        public void SetSelectedLevel(ushort level)
        {
            selectedLevel = level;
        }
    }
}