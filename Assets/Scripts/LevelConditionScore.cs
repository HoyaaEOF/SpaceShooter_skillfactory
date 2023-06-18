using UnityEngine;

namespace SpaceShooter
{

    public class LevelConditionScore : MonoBehaviour, ILevelCondition
    {
        [SerializeField] private int Score;

        private bool m_Reached;

        bool ILevelCondition.IsCompleted
        {
            get
            {
                if (Player.Instance != null && Player.Instance.ActiveShip != null)
                {
                    if (Player.Instance.score >= Score)
                    {
                        m_Reached = true;
                    }
                }

                return m_Reached;
            }
        }
    }
}