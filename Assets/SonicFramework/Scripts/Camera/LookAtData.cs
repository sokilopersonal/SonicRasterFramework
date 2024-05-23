using System;
using UnityEngine;

namespace SonicFramework
{
    [Serializable]
    public class LookAtData : PanData
    {
        public Transform positionTarget;
        public Transform lookTarget;
    }

    public class PanData
    {
        public float time;
        public float positionSpeed;
        public float lookSpeed;
    }
}
