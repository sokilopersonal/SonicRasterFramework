using System;
using UnityEngine;

namespace SonicFramework.Events
{
    public static class GlobalSpawnablesEvents
    {
        // Ring
        public static Action<int> OnRingCollected;
        public static Action<GameObject> OnRingWorldCollected;
        public static Action OnOneHundredRingsCollected;
        
        // Score
        public static Action<int> OnScoreChanged;
        
        // Goal Ring
        public static Action OnGoal;
        public static Action OnStageResult;
        
        // Hint Mark
        public static Action<string> OnHintTriggered;
    }
}