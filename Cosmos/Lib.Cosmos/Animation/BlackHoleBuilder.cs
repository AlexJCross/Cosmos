namespace Lib.Cosmos.Animation
{
    using HelixToolkit.Wpf;
    using System;
    using System.Collections.Generic;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;

    public class EmbeddedBlackHoleBuilder
    {
        public int Theta { get; set; } = 4;
        public int NumRings { get; set; } = 12;
        public double Thickness { get; set; } = 0.15;
        public double RingRadius { get; set; } = 10;
        public double Mass { get; set; } = 1;


        public EmbeddedBlackHoleBuilder WithThickness(double thickness)
        {
            this.Thickness = thickness;
            return this;
        }

        public EmbeddedBlackHoleBuilder WithRingRadius(double ringRadius)
        {
            this.RingRadius = ringRadius;
            return this;
        }

        public EmbeddedBlackHoleBuilder WithMass(double mass)
        {
            this.Mass = mass;
            return this;
        }

        public EmbeddedBlackHoleBuilder WithTheta(int theta)
        {
            this.Theta = theta;
            return this;
        }

        public EmbeddedBlackHoleBuilder WithNumRings(int numRings)
        {
            this.NumRings = numRings;
            return this;
        }

        public Model3DGroup Build()
        {
            var group = new Model3DGroup();

            this.BuildAzimuthalTubes(group);
            this.BuildRadialTubes(group);

            return group;
        }

        private void BuildAzimuthalTubes(Model3DGroup group)
        {
            double startRadius = 2 * this.Mass;
            double radius = startRadius;

            for (int i = 0; i < this.NumRings; i++)
            {
                IEnumerable<Point3D> points = this.GenerateAzimuthalPoints(radius);

                var tubeVisual = new TubeVisual3D
                {
                    Fill = Brushes.DarkCyan,
                    Diameter = this.Thickness,
                    ThetaDiv = Theta,
                    Path = new Point3DCollection(points),
                    BackMaterial = null,
                    IsPathClosed = true
                };

                tubeVisual.Path.Freeze();
                tubeVisual.Model.Freeze();

                var model = new GeometryModel3D
                {
                    Geometry = tubeVisual.Model.Geometry,
                    Material = tubeVisual.Material,
                    BackMaterial = null,
                };

                model.Transform.Freeze();
                model.Geometry.Freeze();
                model.Material.Freeze();
                model.Freeze();

                group.Children.Add(model);

                radius += this.RingRadius;
            }
        }

        private void BuildRadialTubes(Model3DGroup group)
        {
            IEnumerable<Point3D> points = GenerateRadialPoints();

            var tubeVisual = new TubeVisual3D
            {
                Fill = Brushes.DarkCyan,
                Diameter = this.Thickness,
                ThetaDiv = Theta,
                IsPathClosed = false,
                Path = new Point3DCollection(points),
                BackMaterial = null
            };

            tubeVisual.Path.Freeze();
            tubeVisual.Model.Freeze();

            var numLongitude = 24;

            for (int i = 0; i < numLongitude; i++)
            {
                var model = tubeVisual.Model;

                double angle = (360 / numLongitude) * i;
                var axis = new Vector3D(0, 0, 1);
                Rotation3D rotation = new AxisAngleRotation3D(axis, angle);
                Transform3D transform = new RotateTransform3D(rotation);
                var rotatedModel = new GeometryModel3D
                {
                    Geometry = tubeVisual.Model.Geometry,
                    Material = tubeVisual.Material,
                    BackMaterial = null,
                    Transform = transform
                };

                rotatedModel.Transform.Freeze();
                rotatedModel.Geometry.Freeze();
                rotatedModel.Material.Freeze();
                rotatedModel.Freeze();

                group.Children.Add(rotatedModel);
            }
        }

        private IEnumerable<Point3D> GenerateRadialPoints()
        {
            const int EventHorizonDivisions = 64;
            const int Divisions = 2;

            double startRadius = 2 * this.Mass;
            double radius = startRadius;
            double increment = this.RingRadius / EventHorizonDivisions;

            for (int i = 0; i < EventHorizonDivisions; i++)
            {
                double x = radius;
                double y = 0;
                double z = this.GetZValue(radius);

                yield return new Point3D(x, y, z);

                radius += increment;
            }

            for (int i = 0; i < Divisions * (this.NumRings - 1); i++)
            {
                double x = radius;
                double y = 0;
                double z = this.GetZValue(radius);

                yield return new Point3D(x, y, z);

                radius += this.RingRadius / Divisions;
            }
        }

        private IEnumerable<Point3D> GenerateAzimuthalPoints(double radius)
        {
            const int Points = 100;

            double z = this.GetZValue(radius);

            for (int i = 0; i <= Points; i++)
            {
                var pt = new Point3D(
                    radius * Math.Cos(i * Math.PI * 2 / Points),
                    radius * Math.Sin(i * Math.PI * 2 / Points),
                    z);

                yield return pt;
            }
        }

        private double GetZValue(double radius)
        {
            return 2 * Math.Sqrt(2 * this.Mass * (radius - (2 * this.Mass))) - 5;
        }
    }
}
