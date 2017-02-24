namespace Lib.Cosmos.Animation
{
    using HelixToolkit.Wpf;
    using System;
    using System.Collections.Generic;
    using System.Windows.Media.Media3D;

    public class PolarCoordinateBuilder
    {
        public TubeVisual3D TubeVisual { get; set; }

        public int Theta { get; set; } = 4;

        public double Thickness { get; set; } = 0.15;

        public PolarCoordinateBuilder WithTubeVisual(TubeVisual3D tubeVisual)
        {
            this.TubeVisual = tubeVisual;
            return this;
        }

        public PolarCoordinateBuilder WithThickness(double thickness)
        {
            this.Thickness = thickness;
            return this;
        }

        public PolarCoordinateBuilder WithTheta(int theta)
        {
            this.Theta = theta;
            return this;
        }

        public TubeVisual3D Build()
        {
            this.TubeVisual.Diameter = this.Thickness;

            IEnumerable<Point3D> points = GeneratePolarPoints();
            this.TubeVisual.Path = new Point3DCollection(points);
            this.TubeVisual.ThetaDiv = Theta;
            this.TubeVisual.IsPathClosed = false;
            this.TubeVisual.BackMaterial = null;
            this.TubeVisual.Path.Freeze();
            this.TubeVisual.Model.Freeze();

            return this.TubeVisual;
        }

        private static IEnumerable<Point3D> GeneratePolarPoints()
        {
            const int Points = 60;
            const int BaseRadius = 10;

            for (int j = 1; j < 20; j++)
            {
                var numPoints = Points + (2 * j);// * j;

                for (int i = 0; i <= numPoints; i++)
                {
                    var pt = new Point3D(
                        j * BaseRadius * Math.Cos(i * Math.PI * 2 / numPoints),
                        j * BaseRadius * Math.Sin(i * Math.PI * 2 / numPoints),
                        0);

                    yield return pt;
                }
            }

            const int Radius = 200;

            for (int i = 0; i < 180; i += 15)
            {
                var pt1 = new Point3D(Radius * Math.Cos(i * Math.PI / 180), Radius * Math.Sin(i * Math.PI / 180), 0);
                var pt2 = new Point3D(Radius * Math.Cos((i * Math.PI / 180) + Math.PI), Radius * Math.Sin((i * Math.PI / 180) + Math.PI), 0);

                if (i % 2 == 0)
                {
                    yield return pt2;
                    yield return pt1;
                }
                else
                {
                    yield return pt1;
                    yield return pt2;
                }

            }
        }
    }
}