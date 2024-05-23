using System.Collections.Generic;
using SonicFramework.PlayerStates;
using UnityEngine;
using UnityEngine.Splines;

namespace SonicFramework
{
    public class PlayerModel : PlayerComponent
    {
        [Header("Body Parts References")] 
        public Transform foot;
        
        [Header("Shader")]
        public Renderer modelRenderer;
        private readonly List<Material> materials = new List<Material>();
        
        [Header("Model Offset")]
        [SerializeField] private Transform root;
        [SerializeField] private Vector3 offset;
        [SerializeField] private float rollingOffset;
        [SerializeField] private float standOffset;
        public SplineContainer spline;
        public CapsuleCollider col { get; private set; }

        private void Start()
        {
            col = GetComponent<CapsuleCollider>();

            for (int i = 0; i < modelRenderer.materials.Length; i++)
            {
                materials.Add(modelRenderer.materials[i]);
            }
        }

        private void LateUpdate()
        {
            if (player.fsm.CurrentState is not StateGoal)
            {
                offset.y = player.fsm.CurrentState is StateRoll or StateSpinDashCharge ? rollingOffset : standOffset;
                root.localPosition += offset;
            }
            else
            {
                root.localPosition = Vector3.zero;
            }
        }
    }
}