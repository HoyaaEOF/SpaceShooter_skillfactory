using UnityEngine;

namespace SpaceShooter
{
    public class PlayerStatistics : MonoBehaviour
    {
        public int NumKills;
        public float Scores;
        public float Time;

        public void Reset()
        {
            NumKills = 0;
            Scores = 0;
            Time = 0;
        }
    }
}