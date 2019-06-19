using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace STLHandler
{
    public class STLModel
    {
        public STLModel(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public int FacetCount { get { return Facets.Count; } }

        public List<Facet> Facets { get; private set; } = new List<Facet>();

        public void AddFacet(Facet facet)
        {
            Facets.Add(facet);
        }

        public void AddFacets(IEnumerable<Facet> facets)
        {
            this.Facets.AddRange(facets);
        }

        public void AlignWithOrigin()
        {
            Translate(-MinX(), -MinY(), -MinZ());
        }

        public void ComputeAllNormals()
        {
            foreach (Facet facet in Facets)
            {
                facet.ComputeNormal();
            }
        }

        public STLModel DeepCopy()
        {
            var model = new STLModel(Name)
            {
                Facets = Facets.ConvertAll(facet => new Facet(facet))
            };
            return model;
        }

        public static STLModel FromAsciiFile(string filename)
        {
            using (var fs = File.OpenRead(filename))
            using (var sr = new StreamReader(fs))
            {
                string solidName = sr.ReadLine();
                var model = new STLModel(solidName.Split()[1]);
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().Trim();
                    if (line == "endsolid")
                    {
                        break;
                    }
                    string[] normal = line.Split();
                    sr.ReadLine();
                    line = sr.ReadLine().Trim();
                    string[] vertex1 = line.Split();
                    line = sr.ReadLine().Trim();
                    string[] vertex2 = line.Split();
                    line = sr.ReadLine().Trim();
                    string[] vertex3 = line.Split();
                    sr.ReadLine();
                    var facet = new Facet
                    {
                        Normal = new Vec3(float.Parse(normal[2]), float.Parse(normal[3]), float.Parse(normal[4])),
                        Vertex1 = new Vec3(float.Parse(vertex1[1]), float.Parse(vertex1[2]), float.Parse(vertex1[3])),
                        Vertex2 = new Vec3(float.Parse(vertex2[1]), float.Parse(vertex2[2]), float.Parse(vertex2[3])),
                        Vertex3 = new Vec3(float.Parse(vertex3[1]), float.Parse(vertex3[2]), float.Parse(vertex3[3]))
                    };
                    model.AddFacet(facet);
                    sr.ReadLine();
                }
                return model;
            }
        }

        public static STLModel FromBinaryFile(string filename)
        {
            var model = new STLModel("model");
            using (var fs = File.OpenRead(filename))
            using (var br = new BinaryReader(fs))
            {
                br.BaseStream.Seek(80, SeekOrigin.Begin);
                int facetCount = (int)br.ReadUInt32();
                for (int i = 0; i < facetCount; i++)
                {
                    var facet = new Facet
                    {
                        Normal = new Vec3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()),
                        Vertex1 = new Vec3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()),
                        Vertex2 = new Vec3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()),
                        Vertex3 = new Vec3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle())
                    };
                    model.AddFacet(facet);
                    br.BaseStream.Seek(2, SeekOrigin.Current);
                }
            }
            return model;
        }

        public float MaxX()
        {
            float maxX = Facets[0].MaxX();
            for (int i = 1; i < FacetCount; i++)
            {
                float crntMaxX = Facets[i].MaxX();
                if (crntMaxX > maxX)
                {
                    maxX = crntMaxX;
                }
            }
            return maxX;
        }

        public float MaxY()
        {
            float maxY = Facets[0].MaxY();
            for (int i = 1; i < FacetCount; i++)
            {
                float crntMaxY = Facets[i].MaxY();
                if (crntMaxY > maxY)
                {
                    maxY = crntMaxY;
                }
            }
            return maxY;
        }

        public float MaxZ()
        {
            float maxZ = Facets[0].MaxZ();
            for (int i = 1; i < FacetCount; i++)
            {
                float crntMaxZ = Facets[i].MaxZ();
                if (crntMaxZ > maxZ)
                {
                    maxZ = crntMaxZ;
                }
            }
            return maxZ;
        }

        public float MinX()
        {
            float minX = Facets[0].MinX();
            for (int i = 1; i < FacetCount; i++)
            {
                float crntMinX = Facets[i].MinX();
                if (crntMinX < minX)
                {
                    minX = crntMinX;
                }
            }
            return minX;
        }

        public float MinY()
        {
            float minY = Facets[0].MinY();
            for (int i = 1; i < FacetCount; i++)
            {
                float crntMinY = Facets[i].MinY();
                if (crntMinY < minY)
                {
                    minY = crntMinY;
                }
            }
            return minY;
        }

        public float MinZ()
        {
            float minZ = Facets[0].MinZ();
            for (int i = 1; i < FacetCount; i++)
            {
                float crntMinZ = Facets[i].MinZ();
                if (crntMinZ < minZ)
                {
                    minZ = crntMinZ;
                }
            }
            return minZ;
        }

        public void RemoveAllFacets()
        {
            Facets.Clear();
        }

        public void RemoveFacet(Facet facet)
        {
            Facets.Remove(facet);
        }

        public void RemoveFacetAt(int idx)
        {
            Facets.RemoveAt(idx);
        }

        public void RemoveFacets(int idx, int count)
        {
            Facets.RemoveRange(idx, count);
        }

        public void Rotate(float rotX, float rotY, float rotZ)
        {
            var rotMat = Mat3x3.RotationX(rotX).Dot(Mat3x3.RotationY(rotY)).Dot(Mat3x3.RotationZ(rotZ));
            foreach (Facet facet in Facets)
            {
                facet.Vertex1 = facet.Vertex1.Dot(rotMat);
                facet.Vertex2 = facet.Vertex2.Dot(rotMat);
                facet.Vertex3 = facet.Vertex3.Dot(rotMat);
            }
        }

        public void Rotate(Mat3x3 mat)
        {
            foreach (Facet facet in Facets)
            {
                facet.Vertex1 = facet.Vertex1.Dot(mat);
                facet.Vertex2 = facet.Vertex2.Dot(mat);
                facet.Vertex3 = facet.Vertex3.Dot(mat);
            }
        }

        public void ToAsciiFile(string filename)
        {
            using (var fs = File.Create(filename))
            using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine("solid " + Name);
                foreach (Facet facet in Facets)
                {
                    sw.WriteLine(facet.Normal.ToFormattedString("  facet normal"));
                    sw.WriteLine("    outer loop");
                    sw.WriteLine(facet.Vertex1.ToFormattedString("      vertex"));
                    sw.WriteLine(facet.Vertex2.ToFormattedString("      vertex"));
                    sw.WriteLine(facet.Vertex3.ToFormattedString("      vertex"));
                    sw.WriteLine("    endloop");
                    sw.WriteLine("  endfacet");
                }
                sw.WriteLine("endsolid");
            }
        }

        public void ToBinaryFile(string filename)
        {
            using (var fs = File.Create(filename))
            using (var bw = new BinaryWriter(fs))
            {
                var header = new byte[80];
                byte[] stl = Encoding.ASCII.GetBytes("STL file created with the STLHandler library");
                Buffer.BlockCopy(stl, 0, header, 0, stl.Length);
                bw.Write(header);
                bw.Write((uint)Facets.Count);
                const short attribCount = 0;
                foreach (Facet facet in Facets)
                {
                    bw.Write(facet.Normal.X);
                    bw.Write(facet.Normal.Y);
                    bw.Write(facet.Normal.Z);
                    bw.Write(facet.Vertex1.X);
                    bw.Write(facet.Vertex1.Y);
                    bw.Write(facet.Vertex1.Z);
                    bw.Write(facet.Vertex2.X);
                    bw.Write(facet.Vertex2.Y);
                    bw.Write(facet.Vertex2.Z);
                    bw.Write(facet.Vertex3.X);
                    bw.Write(facet.Vertex3.Y);
                    bw.Write(facet.Vertex3.Z);
                    bw.Write(attribCount);
                }
            }
        }

        public void Translate(float x, float y, float z)
        {
            foreach (Facet facet in Facets)
            {
                facet.Vertex1.X += x;
                facet.Vertex1.Y += y;
                facet.Vertex1.Z += z;
                facet.Vertex2.X += x;
                facet.Vertex2.Y += y;
                facet.Vertex2.Z += z;
                facet.Vertex3.X += x;
                facet.Vertex3.Y += y;
                facet.Vertex3.Z += z;
            }
        }

        public void Translate(Vec3 vec)
        {
            foreach (Facet facet in Facets)
            {
                facet.Vertex1 += vec;
                facet.Vertex2 += vec;
                facet.Vertex3 += vec;
            }
        }

        public List<Vec3> UniqueVertices()
        {
            var result = new HashSet<Vec3>();
            foreach (Facet facet in Facets)
            {
                result.Add(facet.Vertex1);
                result.Add(facet.Vertex2);
                result.Add(facet.Vertex3);
            }
            return result.ToList();
        }

    }
}
