using Unity.Burst;
using UnityEngine;
using UnityEngine.Jobs;

namespace SonicFramework.Jobs
{
    [BurstCompile]
    public struct RingRotateJob : IJobParallelForTransform
    {
        private readonly float _rotationSpeed;
        public float dt;

        public RingRotateJob(float rotationSpeed, float deltaTime)
        {
            _rotationSpeed = rotationSpeed;
            dt = deltaTime;
        }

        public void Execute(int index, TransformAccess transform)
        {
            transform.rotation *= Quaternion.AngleAxis(_rotationSpeed * dt, Vector3.up);
        }
    }
}