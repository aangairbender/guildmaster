using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Gameplay.Terrain
{
    public class Placement : MonoBehaviour
    {
        public float radius = 1;
        public Vector2 regionSize = Vector2.one;
        public int rejectionSamples = 30;
        public float displayRadius = 1;

        List<Vector2> points;

        private void OnValidate()
        {
            points = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSamples);
        }

        private void OnDrawGizmos()
        {
            if (points == null) return;

            Gizmos.DrawWireCube(regionSize / 2, regionSize);
            foreach (var point in points)
            {
                Gizmos.DrawSphere(point, displayRadius);
            }
        }
    }
}