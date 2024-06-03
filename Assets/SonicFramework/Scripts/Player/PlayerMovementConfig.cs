using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace SonicFramework
{
    [CreateAssetMenu(fileName = "Character", menuName = "Framework/Movement Config")]
    public class PlayerMovementConfig : ScriptableObject
    {
        [Foldout("Base")] public float topSpeed = 45;
        [Foldout("Base")] public float maxSpeed = 100;
        [Foldout("Base")] public float gravity = -45;
        [Foldout("Base")] public float jumpForce = 0.185f;
        [Foldout("Base")] public AnimationCurve turnSpeedOverSpeed;
        [Foldout("Base")] public AnimationCurve accelOverSpeed;
        [Foldout("Base")] public float turnRate = 8;
        [Foldout("Base")] public float turnSpeed = 8;
        [Foldout("Base")] public float turnSmoothing = 9;
        [Foldout("Base")] [SerializeField, Range(0, 1)] public float turnDecel;
        [Foldout("Base")] public AnimationCurve turnDecelOverSpeed;
        [Foldout("Base")] public float accelRate = 8;
        [Foldout("Base")] [Range(0, 1)] public float groundHandlingAmount = 1f;
        [Foldout("Base")] [Range(0, 1)] public float airHandlingAmount = 0.5f;
        [Foldout("Base")] public float minDecelRate = 8;
        [Foldout("Base")] public float maxDecelRate = 25;
        [Foldout("Base")] public float fullStopThreshold = 3f;
        [Foldout("Base"), Range(0, 1)] public float brakeThreshold;
        [Foldout("Slopes")] public float minSlopeAngle = 15;
        [Foldout("Slopes")] public int stickSpeed = 5;
        [Foldout("Slopes")] public float slopeMultiplier = 1.25f;
        [Foldout("Slopes")] public float inLoopSpeedThreshold = 15;
        [Foldout("Slopes")] public float speedOverShoot = 0.1f;
        [Foldout("Slopes")] public AnimationCurve slopeForceOverSpeed;
        [Foldout("Slopes")] public AnimationCurve loopForceOverSpeed;
        [Foldout("Ground Check")] public float groundCheckDistance = 1.1f;
        [Foldout("Ground Check")] public LayerMask groundMask;
        [Foldout("Ground Check")] public LayerMask dashPanelMask;
        [Foldout("Ground Check")] public AnimationCurve groundCheckOverSpeed;
        [Foldout("Drop Dash")] public float dropDashPower = 0.6f;
        [Foldout("Drop Dash")] public float maxDropDashPower = 1.8f;
        [Foldout("Jump Dash")] public float jumpDashForce = 2f;
        [Foldout("Jump Dash")] public float jumpDashMaxTimer = 65f;
        [Foldout("Roll")] public float rollSteering;
        [Foldout("Roll")] public float rollMultiplier;
        [Foldout("Homing Attack")] public float homingSpeed;
        [Foldout("Homing Attack")] public float homingFindRadius = 15f;
        [Foldout("Homing Attack")] public float homingFindDistance = 10f;
        [Foldout("Homing Attack")] public float homingKnockback;
        [Foldout("Homing Attack")] [Range(0, 1)] public float homingVelocityKeep;
        [Foldout("Homing Attack")] public float homingMaxTime;
        [Foldout("Homing Attack")] public LayerMask homingMask;
        [Foldout("Spin Dash")] public SpinDashType spinDashType = SpinDashType.Manual;
        [Foldout("Spin Dash")] public float spinDashMaxChargeTime = 3;
        [Foldout("Spin Dash")] public float spinDashChargeMultiplier = 0.5f;
        [Foldout("Spin Dash")] public float spinDashChargeRate = 1f;
        [Foldout("Light Speed Dash")] public float lsdSpeed = 35f;
        [Foldout("Light Speed Dash")] public float lsdFindFirstRadius = 1f;
        [Foldout("Light Speed Dash")] public float lsdFindFirstDistance = 1f;
        [Foldout("Light Speed Dash")] public int lsdMaxRings = 15;
        [Foldout("Light Speed Dash")] public LayerMask ringMask;
        [Foldout("Damage")] public float knockBackUpwardsForce = 0.3f;
        [Foldout("Damage")] public float knockBackBackwardsForce = 0.25f;
    }
}
