using UnityEngine;

namespace UnityBasis.Data
{
    public struct SphrericalVector
    {
        public readonly float R;
        public readonly float Teta;
        public readonly float Fi;

        public SphrericalVector(float r, float teta, float fi)
        {
            R = r;
            Teta = teta;
            Fi = fi;
        }

        public override string ToString()
        {
            return $"r: {R}; teta: {Teta}; fi: {Fi}";
        }

        public Vector3 ToDecard()
        {
            float tetaRad = Teta * Mathf.Deg2Rad;
            float fiRad = Fi * Mathf.Deg2Rad;

            return new Vector3(
                x: R * Mathf.Sin(tetaRad) * Mathf.Cos(fiRad),
                y: - R * Mathf.Cos(tetaRad),
                z: R * Mathf.Sin(tetaRad) * Mathf.Sin(fiRad));
        }

        public static SphrericalVector FromDecard(Vector3 decardVector)
        {
            return new SphrericalVector(
                r: decardVector.magnitude,
                teta: Mathf.Atan(Mathf.Sqrt(Mathf.Pow(decardVector.x, 2) + Mathf.Pow(decardVector.z, 2)) / decardVector.y),
                fi: Mathf.Atan(decardVector.z / decardVector.x)
            );
        }

        public static SphrericalVector operator +(SphrericalVector vector1, SphrericalVector vector2)
        {
            return new SphrericalVector(
                r: vector1.R + vector2.R,
                teta: vector1.Teta + vector2.Teta,
                fi: vector1.Fi + vector2.Fi
            );
        }
    }
}