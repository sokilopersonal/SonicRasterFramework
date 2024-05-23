using System;
using FMOD.Studio;
using FMODUnity;
using NaughtyAttributes;
using SonicFramework.Events;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace SonicFramework
{
    [Serializable]
    public struct StageData
    {
        [Label("Score for S Rank")] public float SRankThreshold;
        [Label("Score for A Rank")] public float ARankThreshold;
        [Label("Score for B Rank")] public float BRankThreshold;
        [Label("Score for C Rank")] public float CRankThreshold;
        [Label("Score for D Rank")] public float DRankThreshold;
        [Label("Score for E Rank")] public float ERankThreshold;
        [Label("Max Seconds"), Tooltip("Max stage time in seconds (will affect time bonus)")] public int maxStageTime;
        public float baseTimeBonus;
        public float maxTimeBonus;
        
        [ReadOnly] public float timeInMinutes;
    }
    
    public class Stage : MonoBehaviour
    {
        [Header("Data")]
        public DataContainer dataContainer;
        public StageData stageData;
        
        [Header("Music")]
        [SerializeField] private EventReference stageMusicReference;

        private EventInstance instance;
        private bool running;

        private void Awake()
        {
            dataContainer = new DataContainer();
            running = true;

            var time = stageData.maxStageTime * 60;
        }

        private void Start()
        {
            instance = RuntimeManager.CreateInstance(stageMusicReference);
            instance.start();
        }

        private void OnEnable()
        {
            GlobalSpawnablesEvents.OnGoal += OnGoal;
        }

        private void OnDisable()
        {
            GlobalSpawnablesEvents.OnGoal -= OnGoal;
        }

        private void OnGoal()
        {
            running = false;
            
            instance.stop();
        }

        private void Update()
        {
            if (running) dataContainer.Time += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Debug.Log(GetTimeBonus());
            }
        }

        [Button("Add 30 seconds")]
        public void AddSeconds()
        {
            stageData.maxStageTime += 30;
            stageData.timeInMinutes = stageData.maxStageTime / 60;
        }

        [Button("Add One Minute")]
        public void AddMinute()
        {
            stageData.maxStageTime += 60;
            stageData.timeInMinutes = stageData.maxStageTime / 60;
        }
        
        [Button("Remove 30 seconds")]
        public void RemoveSeconds()
        {
            stageData.maxStageTime -= 30;
            stageData.maxStageTime = (int)Mathf.Clamp(stageData.maxStageTime, 0, Mathf.Infinity);
            
            stageData.timeInMinutes = stageData.maxStageTime / 60;
        }

        [Button("Remove One Minute")]
        public void RemoveMinute()
        {
            stageData.maxStageTime -= 60;
            stageData.maxStageTime = (int)Mathf.Clamp(stageData.maxStageTime, 0, Mathf.Infinity);
            
            stageData.timeInMinutes = stageData.maxStageTime / 60;
        }

        private void OnDestroy()
        {
            instance.stop();
        }

        private void OnValidate()
        {
            stageData.timeInMinutes = stageData.maxStageTime / 60;
        }

        public Rank GetRank()
        {
            var score = dataContainer.EndScore;

            // return score <= stageData.ERankThreshold ? Rank.E :
            //     score >= stageData.DRankThreshold && score <= stageData.CRankThreshold ? Rank.D :
            //     score >= stageData.CRankThreshold && score <= stageData.BRankThreshold ? Rank.C :
            //     score >= stageData.BRankThreshold && score <= stageData.ARankThreshold ? Rank.B :
            //     score >= stageData.ARankThreshold && score <= stageData.SRankThreshold ? Rank.A :
            //     score >= stageData.SRankThreshold ? Rank.S : Rank.E;
            
            if (score >= stageData.SRankThreshold)
            {
                return Rank.S;
            }

            if (score >= stageData.ARankThreshold)
            {
                return Rank.A;
            }

            if (score >= stageData.BRankThreshold)
            {
                return Rank.B;
            }

            if (score >= stageData.CRankThreshold)
            {
                return Rank.C;
            }

            if (score >= stageData.DRankThreshold)
            {
                return Rank.D;
            }

            return Rank.E;
        }

        public int GetTimeBonus()
        {
            // float playerTimeSeconds = dataContainer.Time;
            // float maxTimeSeconds = stageData.maxStageTime * 60;
            //
            // float bonus = stageData.baseTimeBonus * (1 - Mathf.Clamp(playerTimeSeconds / maxTimeSeconds, 0f, 1f));
            // bonus = Mathf.Clamp(bonus, 1000, stageData.maxTimeBonus);
            //
            // return Mathf.FloorToInt(bonus);
            
            return Mathf.FloorToInt(Mathf.InverseLerp(1000, stageData.maxTimeBonus, stageData.maxStageTime / dataContainer.Time));
        }

        public int GetRingBonus()
        {
            int result = dataContainer.RingCount;

            return result * 10;
        }
    }
}