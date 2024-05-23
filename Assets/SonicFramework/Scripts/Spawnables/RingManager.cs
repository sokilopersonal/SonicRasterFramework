using System;
using System.Collections.Generic;
using SonicFramework.Jobs;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using Zenject;

namespace SonicFramework
{
    public class RingManager : IInitializable, ITickable, IDisposable
    {
        public List<Transform> rings = new();
        
        private TransformAccessArray transformAccessArray;
        private RingRotateJob job; 
        private JobHandle jobHandle;
        private readonly float rotationSpeed;

        public RingManager(float rotationSpeed) => this.rotationSpeed = rotationSpeed;

        public void Initialize()
        {
            transformAccessArray = new TransformAccessArray(2000);
            job = new RingRotateJob(rotationSpeed, Time.deltaTime);
        }

        public void Tick()
        {
            job.dt = Time.deltaTime;
            
            jobHandle = job.Schedule(transformAccessArray);
            jobHandle.Complete();
        }

        public void AddRing(Transform ring)
        {
            rings.Add(ring);
            transformAccessArray.Add(ring);
        }

        public void Dispose()
        {
            transformAccessArray.Dispose();
        }
    }
}