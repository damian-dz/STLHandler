using System;

namespace STLHandler
{
    public class Vec3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        /// <summary>
        /// Constructs a zero-initialized 3-D vector.
        /// </summary>
        public Vec3()
        {
            X = 0f;
            Y = 0f;
            Z = 0f;
        }

        public Vec3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vec3 Cross(Vec3 other)
        {
            float x = Y * other.Z - Z * other.Y;
            float y = Z * other.X - X * other.Z;
            float z = X * other.Y - Y * other.X;
            return new Vec3(x, y, z);
        }

        public Vec3 Dot(Mat3x3 mat)
        {
            float x = X * mat.XX + Y * mat.YX + Z * mat.ZX;
            float y = X * mat.XY + Y * mat.YY + Z * mat.ZY;
            float z = X * mat.XZ + Y * mat.YZ + Z * mat.ZZ;
            return new Vec3(x, y, z);
        }

        public float Dot(Vec3 other)
        {
            return X * other.X + Y * other.Y + Z * other.Z;
        }
        public float Magnitude()
        {
            return MathF.Sqrt(X * X + Y * Y + Z * Z);
        }

        public static Vec3 Normal(Vec3 a, Vec3 b, Vec3 c)
        {
            Vec3 x = b - a;
            Vec3 y = c - a;
            return x.Cross(y).Unit();
        }

        public Vec3 Unit()
        {
            return this / Magnitude();
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", X, Y, Z);
        }

        public string ToFormattedString(string name)
        {
            return string.Format("{0} {1} {2} {3}", name, (float)X, (float)Y, (float)Z);
        }

        public static bool operator ==(Vec3 a, Vec3 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Vec3 a, Vec3 b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var other = (Vec3)obj;
            return (X == other.X && Y == other.Y && Z == other.Z);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        public static Vec3 operator +(Vec3 a, Vec3 b)
        {
            return new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vec3 operator +(Vec3 a, float val)
        {
            return new Vec3(a.X + val, a.Y + val, a.Z + val);
        }

        public static Vec3 operator -(Vec3 a, Vec3 b)
        {
            return new Vec3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vec3 operator -(Vec3 a, float val)
        {
            return new Vec3(a.X - val, a.Y - val, a.Z - val);
        }

        public static Vec3 operator *(Vec3 a, Vec3 b)
        {
            return new Vec3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        }
        public static Vec3 operator *(Vec3 a, float val)
        {
            return new Vec3(a.X * val, a.Y * val, a.Z * val);
        }
        public static Vec3 operator /(Vec3 a, Vec3 b)
        {
            return new Vec3(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
        }
        public static Vec3 operator /(Vec3 a, float val)
        {
            return new Vec3(a.X / val, a.Y / val, a.Z / val);
        }
    }
}
