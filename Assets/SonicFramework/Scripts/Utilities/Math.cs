using Unity.Mathematics;
using UnityEngine;

namespace SonicFramework
{
    public static class Math
    {
        public static void SplitPlanarVector(Vector3 Vector, Vector3 Normal, out Vector3 Planar, out Vector3 Vertical)
        {
            Planar = Vector3.ProjectOnPlane(Vector, Normal);
            Vertical = Vector - Planar;
        }
        
        public static bool IsApproximate(Vector3 A, Vector3 B, float DeadZone)
        {
            Vector3 Difference = A - B;
            return Difference.magnitude <= DeadZone;
        }

        public static float ConvertAngle(float a)
        {
            if (a > 180) return a - 360;

            return a;
        }

        public static Vector3 ConvertFloat3ToVector3(float3 a)
        {
            return new Vector3(a.x, a.y, a.z);
        }

        public static float Sign(float a, float threshold = 0.1f)
        {
            if (a > 0) return 1;
            if (a < 0) return -1;
            if (Mathf.Abs(a) < threshold) return 0;

            return 0;
        }
        
        public static bool LaunchAngle(float speed, float distance, float yOffset, float gravity, out float angle0, out float angle1)
        {
            angle0 = angle1 = 0;

            float speedSquared = speed * speed;

            float operandA = Mathf.Pow(speed, 4);
            float operandB = gravity * (gravity * (distance * distance) + (2 * yOffset * speedSquared));

            // Target is not in range
            if (operandB > operandA)
                return false;

            float root = Mathf.Sqrt(operandA - operandB);

            angle0 = Mathf.Atan((speedSquared + root) / (gravity * distance));
            angle1 = Mathf.Atan((speedSquared - root) / (gravity * distance));

            return true;
        }

        /// <summary>
        /// Calculates the initial launch speed required to hit a target at distance with elevation yOffset.
        /// </summary>
        /// <param name="distance">Planar distance from origin to the target</param>
        /// <param name="yOffset">Elevation of the origin with respect to the target</param>
        /// <param name="gravity">Downward acceleration in m/s^2</param>
        /// <param name="angle">Initial launch angle in radians</param>
        /// <returns>Initial launch speed</returns>
        public static float LaunchSpeed(float distance, float yOffset, float gravity, float angle)
        {
            float speed = (distance * Mathf.Sqrt(gravity) * Mathf.Sqrt(1 / Mathf.Cos(angle))) / Mathf.Sqrt(2 * distance * Mathf.Sin(angle) + 2 * yOffset * Mathf.Cos(angle));

            return speed;
        }

        /// <summary>
        /// Calculates how long a projectile will stay in the air before reaching its target
        /// </summary>
        /// <param name="speed">Initial speed of the projectile</param>
        /// <param name="angle">Initial launch angle in radians</param>
        /// <param name="yOffset">Elevation of the target with respect to the initial fire position</param>
        /// <param name="gravity">Downward acceleration in m/s^2</param>
        /// <returns></returns>
        public static float TimeOfFlight(float speed, float angle, float yOffset, float gravity)
        {
            float ySpeed = speed * Mathf.Sin(angle);

            float time = (ySpeed + Mathf.Sqrt((ySpeed * ySpeed) + 2 * gravity * yOffset)) / gravity;

            return time;
        }

        /// <summary>
        /// Samples a series of points along a projectile arc
        /// </summary>
        /// <param name="iterations">Number of points to sample</param>
        /// <param name="speed">Initial speed of the projectile</param>
        /// <param name="distance">Distance the projectile will travel along the horizontal axis</param>
        /// <param name="gravity">Downward acceleration in m/s^2</param>
        /// <param name="angle">Initial launch angle in radians</param>
        /// <returns>Array of sampled points with the length of the supplied iterations</returns>
        public static Vector2[] ProjectileArcPoints(int iterations, float speed, float distance, float gravity, float angle)
        {
            float iterationSize = distance / iterations;

            float radians = angle;

            Vector2[] points = new Vector2[iterations + 1];

            for (int i = 0; i <= iterations; i++)
            {
                float x = iterationSize * i;
                float t = x / (speed * Mathf.Cos(radians));
                float y = -0.5f * gravity * (t * t) + speed * Mathf.Sin(radians) * t;

                Vector2 p = new Vector2(x, y);

                points[i] = p;
            }

            return points;
        }
    }
}