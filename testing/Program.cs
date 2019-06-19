using System;
using System.Collections.Generic;
using STLHandler;

namespace testing
{
    class Program
    {
        static void Main(string[] args)
        {
            var facet1 = new Facet(new Vec3(0, 0, 0), new Vec3(1, 0, 0), new Vec3(0, 0, 1));
            var facet2 = new Facet(new Vec3(0, 0, 0), new Vec3(0, 1, 0), new Vec3(1, 0, 0));
            var facet3 = new Facet(new Vec3(0, 0, 0), new Vec3(0, 0, 1), new Vec3(0, 1, 0));
            var facet4 = new Facet(new Vec3(1, 0, 0), new Vec3(0, 1, 0), new Vec3(0, 0, 1));

            STLModel model = new STLModel("test");
            model.AddFacet(facet1);
            model.AddFacet(facet2);
            model.AddFacet(facet3);
            model.AddFacet(facet4);
            model.ComputeAllNormals();
            model.ToBinaryFile("test.stl");
        }
    }
}
