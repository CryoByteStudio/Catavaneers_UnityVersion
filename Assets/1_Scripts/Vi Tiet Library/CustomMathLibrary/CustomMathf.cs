using UnityEngine;
using CustomMathLibrary.Interpolation.Easing;

namespace CustomMathLibrary
{
    public static class CustomMathf
    {
        public static float CalculateLerpValue(float lerpValue, Type easingType, bool isZeroToOne)
        {
            switch (easingType)
            {
                case Type.Linear:
                    lerpValue = Linear.InOut(lerpValue);
                    break;
                case Type.Quadratic:
                    lerpValue = Quadratic.InOut(lerpValue);
                    break;
                case Type.Cubic:
                    lerpValue = Cubic.InOut(lerpValue);
                    break;
                case Type.Quartic:
                    lerpValue = Quartic.InOut(lerpValue);
                    break;
                case Type.Quintic:
                    lerpValue = Quintic.InOut(lerpValue);
                    break;
                case Type.Sinusoidal:
                    lerpValue = Sinusoidal.InOut(lerpValue);
                    break;
                case Type.Exponential:
                    lerpValue = Exponential.InOut(lerpValue);
                    break;
                case Type.Circular:
                    lerpValue = Circular.InOut(lerpValue);
                    break;
                case Type.Elastic:
                    lerpValue = Elastic.InOut(lerpValue);
                    break;
                case Type.Back:
                    lerpValue = Back.InOut(lerpValue);
                    break;
                case Type.Bounce:
                    lerpValue = Bounce.InOut(lerpValue);
                    break;
                default:
                    return -1f;
            }

            return lerpValue;
        }

        public static float CalculateLerpValueClamp01(float lerpValue, Type easingType, bool isZeroToOne)
        {
            switch (easingType)
            {
                case Type.Linear:
                    lerpValue = Linear.InOut(lerpValue);
                    break;
                case Type.Quadratic:
                    lerpValue = Quadratic.InOut(lerpValue);
                    break;
                case Type.Cubic:
                    lerpValue = Cubic.InOut(lerpValue);
                    break;
                case Type.Quartic:
                    lerpValue = Quartic.InOut(lerpValue);
                    break;
                case Type.Quintic:
                    lerpValue = Quintic.InOut(lerpValue);
                    break;
                case Type.Sinusoidal:
                    lerpValue = Sinusoidal.InOut(lerpValue);
                    break;
                case Type.Exponential:
                    lerpValue = Exponential.InOut(lerpValue);
                    break;
                case Type.Circular:
                    lerpValue = Circular.InOut(lerpValue);
                    break;
                case Type.Elastic:
                    lerpValue = Elastic.InOut(lerpValue);
                    break;
                case Type.Back:
                    lerpValue = Back.InOut(lerpValue);
                    break;
                case Type.Bounce:
                    lerpValue = Bounce.InOut(lerpValue);
                    break;
                default:
                    return -1f;
            }

            lerpValue = ClampMinMax(0f, 1f, lerpValue);

            return lerpValue;
        }

        public static float ClampMinMax(float min, float max, float value)
        {
            if (value > max) value = max;
            else if (value < min) value = min;

            return value;
        }

        /// <summary>
        /// Returns a random point with 0 coord value on specified axis within a radius
        /// </summary>
        /// <param name="radius"> Limit radius that the random generated position will be in </param>
        /// <param name="axis"></param>
        public static Vector3 RandomPointInCirclePerpendicularToAxis(float radius, Axis axis)
        {
            Vector2 randomPosIn2DCircle = Random.insideUnitCircle * radius;

            switch (axis)
            {
                case Axis.X:
                    return new Vector3(0, randomPosIn2DCircle.y, randomPosIn2DCircle.x);
                case Axis.Y:
                    return new Vector3(randomPosIn2DCircle.x, 0, randomPosIn2DCircle.y);
                case Axis.Z:
                    return new Vector3(randomPosIn2DCircle.x, randomPosIn2DCircle.y, 0);
            }

            return Vector3.zero;
        }

        /// <summary>
        /// Used in for loop to get evenly spacing position with length of 1 around the specified axis
        /// </summary>
        /// <param name="maxIndex"> The max index of the for loop </param>
        /// <param name="currentIndex"> The current index of the for loop </param>
        public static Vector3 GetEvenlySpacingPositionAroundAxis(int maxIndex, int currentIndex, Axis axis)
        {
            Vector3 position = Vector3.zero;

            switch (axis)
            {
                case Axis.X:
                    position.z = Mathf.Cos(360f / maxIndex * currentIndex * Mathf.Deg2Rad);
                    position.y = Mathf.Sin(360f / maxIndex * currentIndex * Mathf.Deg2Rad);
                    break;
                case Axis.Y:
                    position.x = Mathf.Cos(360f / maxIndex * currentIndex * Mathf.Deg2Rad);
                    position.z = Mathf.Sin(360f / maxIndex * currentIndex * Mathf.Deg2Rad);
                    break;
                case Axis.Z:
                    position.x = Mathf.Cos(360f / maxIndex * currentIndex * Mathf.Deg2Rad);
                    position.y = Mathf.Sin(360f / maxIndex * currentIndex * Mathf.Deg2Rad);
                    break;
            }

            return position;
        }
        
        public static int GetNextLoopIndex(int currentIndex, int maxIndex)
        {
            return ((currentIndex + 1) % maxIndex + maxIndex) % maxIndex;
        }

        public static int GetPreviousLoopIndex(int currentIndex, int maxIndex)
        {
            return ((currentIndex - 1) % maxIndex + maxIndex) % maxIndex;
        }

        public static int GetLoopIndex(int currentIndex, int maxIndex)
        {
            return (currentIndex % maxIndex + maxIndex) % maxIndex;
        }

        public static int GetClampedLoopIndex(int currentIndex, int minIndex, int maxIndex)
        {
            int index = (currentIndex % maxIndex + maxIndex) % maxIndex;
            index = index >= minIndex ? index : minIndex;
            return index;
        }
    }
}