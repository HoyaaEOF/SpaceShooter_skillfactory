using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{

    public class ResultPanelController : SingletonBase<ResultPanelController>
    {
        [SerializeField] private Text m_Kills;
        [SerializeField] private Text m_Score;
        [SerializeField] private Text m_Time;
        [SerializeField] private Text m_TimeBonus;

        [SerializeField] private Text m_Result;

        [SerializeField] private Text m_ButtonNextText;

        private bool m_Success;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void ShowResult(PlayerStatistics levelResult, bool success)
        {
            gameObject.SetActive(true);

            m_Success = success;

            m_Result.text = success ? "Win" : "Lose";
            m_ButtonNextText.text = success ? "Next level" : "Restart";

            m_Kills.text = "Kills: " + levelResult.NumKills.ToString();

            if (levelResult.Time < 50)
            {
                m_TimeBonus.text = "Time bonus: " + (levelResult.Scores / 3).ToString();
            }
            else
            {
                m_TimeBonus.text = "";
            }
            m_Score.text = "Score: " + levelResult.Scores.ToString();
            m_Time.text = "Time: " + levelResult.Time.ToString();

            Time.timeScale = 0;
        }

        public void OnButtonNextAction()
        {
            gameObject.SetActive(false);
            Time.timeScale = 1;

            if (m_Success) LevelSequenceController.Instance.AdvanceLevel();
            else LevelSequenceController.Instance.RestartLevel();
        }
    }
}