using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace SpaceShooter
{
    public class TotalStatisticController : SingletonBase<TotalStatisticController>
    {
        [SerializeField] private Text m_TotalKills;
        [SerializeField] private Text m_TotalScore;
        [SerializeField] private Text m_TotalTime;

        public void ShowTotalStatistic()
        {
            m_TotalKills.text = "Kills: " + LevelSequenceController.Instance.TotalKills.ToString();
            m_TotalScore.text = "Score: " + LevelSequenceController.Instance.TotalScore.ToString();
            m_TotalTime.text = "Time: " + LevelSequenceController.Instance.TotalTime.ToString();
        }
    }
}