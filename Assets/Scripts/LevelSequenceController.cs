using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceShooter
{
    public class LevelSequenceController : SingletonBase<LevelSequenceController>
    {

        public static string MainMenuSceneNickname = "main_menu";

        public Episode CurrentEpisode { get; private set; }

        public int CurrentLevel { get; private set; }

        public bool LastLevelResult { get; private set; }

        public PlayerStatistics LevelStatistics { get; private set; }

        public static SpaceShip PlayerShip { get; set; }

        public void StartEpisode(Episode e)
        {
            CurrentEpisode = e;
            CurrentLevel = 0;

            //sbrosit staty pered nachalom episoda

            LevelStatistics = new PlayerStatistics();
            LevelStatistics.Reset();

            SceneManager.LoadScene(e.Levels[CurrentLevel]);
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(CurrentEpisode.Levels[CurrentLevel]);
        }

        public void FinishCurrentLevel(bool success)
        {
            LastLevelResult = success;
            CalculateLevelStatistic();
            ResultPanelController.Instance.ShowResult(LevelStatistics,LastLevelResult);
        }

        public void AdvanceLevel()
        {
            LevelStatistics.Reset();

            CurrentLevel++;

            if (CurrentEpisode.Levels.Length <= CurrentLevel)
            {
                SceneManager.LoadScene(MainMenuSceneNickname);
            }

            else
            {
                SceneManager.LoadScene(CurrentEpisode.Levels[CurrentLevel]);
            }
        }

        private void CalculateLevelStatistic()
        {
            LevelStatistics.Scores = Player.Instance.score;
            LevelStatistics.NumKills = Player.Instance.numKills;
            LevelStatistics.Time = (int)LevelController.Instance.LevelTime;

            if (LevelStatistics.Time < 50)
            {
                LevelStatistics.Scores *= 1.5f;
            }

            CalculateTotalStatistic();
        }

        public int TotalKills { get; private set; }
        public float TotalScore { get; private set; }
        public int TotalTime { get; private set; }

        private void CalculateTotalStatistic()
        {
            TotalKills += LevelStatistics.NumKills;
            TotalScore += LevelStatistics.Scores;
            TotalTime += (int)LevelStatistics.Time;
        }
    }
}