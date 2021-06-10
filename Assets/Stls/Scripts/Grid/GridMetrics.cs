using UnityEngine;

namespace Stls
{
    public static class GridMetrics
    {
        //    public const float OuterRadius = 1f;

        //    public const float InnerRadius = OuterRadius * 0.866025404f;

        public const float InnerRadius = 0.8f;
        public const float OuterRadius = InnerRadius / 0.866025404f;


        public const float Angle = 60f;

        public const float StartAngle = 30f;

        // Used for visiblity check - distance test.
        public const float RadiusError = OuterRadius * 0.001f;


        public static readonly Vector3[] Corners = {
            new Vector3(0,0, OuterRadius),
            new Vector3(InnerRadius, 0, OuterRadius * 0.5f),
            new Vector3(InnerRadius, 0, OuterRadius * -0.5f),
            new Vector3(0,0, -OuterRadius),
            new Vector3(-InnerRadius, 0, OuterRadius * -0.5f),
            new Vector3(-InnerRadius, 0, OuterRadius * 0.5f)
        };
    }
}