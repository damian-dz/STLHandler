using System;
using System.Collections.Generic;
using System.Text;

namespace STLHandler
{
    public class Mat3x3
    {
        private float[,] data;

        public float XX { get { return data[0, 0]; } set { data[0, 0] = value; } }
        public float XY { get { return data[0, 1]; } set { data[0, 1] = value; } }
        public float XZ { get { return data[0, 2]; } set { data[0, 2] = value; } }
        public float YX { get { return data[1, 0]; } set { data[1, 0] = value; } }
        public float YY { get { return data[1, 1]; } set { data[1, 1] = value; } }
        public float YZ { get { return data[1, 2]; } set { data[1, 2] = value; } }
        public float ZX { get { return data[2, 0]; } set { data[2, 0] = value; } }
        public float ZY { get { return data[2, 1]; } set { data[2, 1] = value; } }
        public float ZZ { get { return data[2, 2]; } set { data[2, 2] = value; } }

        public Mat3x3()
        {
            data = new float[3, 3];
        }

        public Mat3x3 Dot(Mat3x3 other)
        {
            var result = new Mat3x3();
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    float sum = 0f;
                    for (int z = 0; z < 3; z++)
                    {
                        sum += data[y, z] * other.data[z, x];
                    }
                    result.data[y, x] = sum;
                }
            }
            return result;
        }

        public static Mat3x3 RotationX(float angle)
        {
            var result = new Mat3x3();
            result.data[0, 0] = 1;
            result.data[1, 1] = MathF.Cos(angle);
            result.data[1, 2] = -MathF.Sin(angle);
            result.data[2, 1] = MathF.Sin(angle);
            result.data[2, 2] = MathF.Cos(angle);
            return result;
        }

        public static Mat3x3 RotationY(float angle)
        {
            var result = new Mat3x3();
            result.data[0, 0] = MathF.Cos(angle);
            result.data[0, 2] = MathF.Sin(angle);
            result.data[1, 1] = 1;
            result.data[2, 0] = -MathF.Sin(angle);
            result.data[2, 2] = MathF.Cos(angle);
            return result;
        }

        public static Mat3x3 RotationZ(float angle)
        {
            var result = new Mat3x3();
            result.data[0, 0] = MathF.Cos(angle);
            result.data[0, 1] = -MathF.Sin(angle);
            result.data[1, 0] = MathF.Sin(angle);
            result.data[1, 1] = MathF.Cos(angle);
            result.data[2, 2] = 1;
            return result;
        }

        public override string ToString()
        {
            string output = null;
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    output += data[y, x] + " ";
                }
                output += Environment.NewLine;
            }
            return output;
        }

        public float this[int y, int x]
        {
            get { return data[y, x]; }
            set { data[y, x] = value; }
        }

        public static implicit operator Mat3x3(float[,] data)
        {
            if (data.GetLength(0) != 3 || data.GetLength(1) != 3)
            {
                throw new ArgumentException("Wrong array dimensions.");
            }
            var result = new Mat3x3
            {
                data = data
            };
            return result;
        }
    }
}
