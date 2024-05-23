using UnityEngine;
using UnityEngine.Serialization;

namespace SonicFramework
{
    [CreateAssetMenu(fileName = "Character", menuName = "Framework/Movement Config")]
    public class PlayerMovementConfig : ScriptableObject
    {
        [Header("Base")]
        public float topSpeed = 45;
        public float maxSpeed = 100;
        public float gravity = -45;
        public float stepOffset = 0.3f;
        public Vector3 vectorGravity => new Vector3(0, gravity, 0);
        public float jumpForce = 0.185f;
        public AnimationCurve turnSpeedOverSpeed;
        public AnimationCurve accelOverSpeed;
        public float turnRate = 8;
        public float turnSpeed = 8;
        public float turnSmoothing = 9;
        [SerializeField, Range(0, 1)] public float turnDecel;
        public AnimationCurve turnDecelOverSpeed;
        public float accelRate = 8;
        [Range(0, 1)] public float groundHandlingAmount = 1f;
        [Range(0, 1)] public float airHandlingAmount = 0.5f;
        public float minDecelRate = 8;
        public float maxDecelRate = 25;
        public float fullStopThreshold = 3f;
        
        [Header("Slopes")]
        public float minSlopeAngle = 15;
        public int stickSpeed = 5;
        public float slopeMultiplier = 1.25f;
        public float inLoopSpeedThreshold = 15;
        public float speedOverShoot = 0.1f;
        public AnimationCurve slopeForceOverSpeed;
        public AnimationCurve loopForceOverSpeed;

        [Header("Ground Check")]
        public float groundCheckDistance = 1.1f;
        public LayerMask groundMask;
        public LayerMask dashPanelMask;
        public AnimationCurve groundCheckOverSpeed;
        
        [Header("Wall Check")]
        public float wallCheckDistance = 1.2f;
        
        [Header("Drop Dash")]
        public float dropDashPower = 0.6f;
        public float maxDropDashPower = 1.8f;
        
        [Header("Jump Dash")] 
        public float jumpDashForce = 2f;
        [Min(0)] public float velocityFadeOutTime = 0.4f;
        public float jumpDashMaxTimer = 65f;

        [Header("Roll")]
        public float rollSteering;
        public float rollMultiplier;
        
        [Header("Homing Attack")]
        public float homingSpeed;
        public float homingFindRadius = 15f;
        public float homingFindDistance = 10f;
        public float homingKnockback;
        [Range(0, 1)] public float homingVelocityKeep;
        public float homingMaxTime;
        public LayerMask homingMask;

        [Header("Spin Dash")]
        public SpinDashType spinDashType = SpinDashType.Manual;
        public float spinDashMaxChargeTime = 3;
        public float spinDashChargeMultiplier = 0.5f;
        public float spinDashChargeRate = 1f;

        [Header("LSD")]
        public float lsdSpeed = 35f;
        public float lsdFindFirstRadius = 1f;
        public float lsdFindFirstDistance = 1f;
        public float lsdFindRadius = 1f;
        public float lsdFindDistance = 1f;
        public int lsdMinRings = 3;
        public int lsdMaxRings = 15;
        public LayerMask ringMask;

        [Header("Damage")] 
        public float knockBackUpwardsForce = 0.3f;
        public float knockBackBackwardsForce = 0.25f;
    }
}
