using System;

namespace STLHandler
{
    public class Facet
    {
        public Vec3 Normal { get; set; }
        public Vec3 Vertex1 { get; set; }
        public Vec3 Vertex2 { get; set; }
        public Vec3 Vertex3 { get; set; }

        public Facet()
        {

        }

        public Facet(Vec3 v1, Vec3 v2, Vec3 v3)
        {
            Vertex1 = v1;
            Vertex2 = v2;
            Vertex3 = v3;
        }

        public Facet(Facet other)
        {
            Normal = other.Normal;
            Vertex1 = other.Vertex1;
            Vertex2 = other.Vertex2;
            Vertex3 = other.Vertex3;
        }

        public void ComputeNormal()
        {
            Normal = Vec3.Normal(Vertex1, Vertex2, Vertex3);
        }

        public float MaxX()
        {
            return Math.Max(Vertex1.X, Math.Max(Vertex2.X, Vertex3.X));
        }

        public float MaxY()
        {
            return Math.Max(Vertex1.Y, Math.Max(Vertex2.Y, Vertex3.Y));
        }

        public float MaxZ()
        {
            return Math.Max(Vertex1.Z, Math.Max(Vertex2.Z, Vertex3.Z));
        }

        public float MinX()
        {
            return Math.Min(Vertex1.X, Math.Min(Vertex2.X, Vertex3.X));
        }

        public float MinY()
        {
            return Math.Min(Vertex1.Y, Math.Min(Vertex2.Y, Vertex3.Y));
        }

        public float MinZ()
        {
            return Math.Min(Vertex1.Z, Math.Min(Vertex2.Z, Vertex3.Z));
        }

        public override string ToString()
        {
            return string.Format("[Normal:  {0}\n Vertex1: {1} \n Vertex2: {2} \n Vertex3: {3}]",
                Normal, Vertex1, Vertex2, Vertex3);
        }
    }
}
