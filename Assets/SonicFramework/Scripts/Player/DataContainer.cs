using SonicFramework.Events;

namespace SonicFramework
{
    public class DataContainer
    {
        private int ringCount;
        private int score;
        private int endScore;
        private float time;

        public int RingCount
        {
            get => ringCount;
            set
            {
                ringCount = value;
                GlobalSpawnablesEvents.OnRingCollected?.Invoke(ringCount);
                
                if (ringCount > 0)
                {
                    if (ringCount % 50 == 0)
                    {
                        GlobalSpawnablesEvents.OnOneHundredRingsCollected?.Invoke();
                    }
                }
            }
        }

        public int Score
        {
            get => score;
            set
            {
                score = value;
                GlobalSpawnablesEvents.OnScoreChanged?.Invoke(score);
            }
        }
        
        public int EndScore
        {
            get => endScore;
            set
            {
                endScore = value;
            }
        }

        public float Time
        {
            get => time;
            set
            {
                time = value;
            }
        }
    }
}