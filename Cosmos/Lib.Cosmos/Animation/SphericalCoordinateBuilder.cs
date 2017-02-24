namespace Lib.Cosmos.Animation
{
    using HelixToolkit.Wpf;
    using System;
    using System.Collections.Generic;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;

    public class SphericalCoordinateBuilder
    {
        public int Theta { get; set; } = 4;

        public double Thickness { get; set; } = 0.15;

        public double Radius { get; set; } = 10;
        public bool ShowLatitutes { get; set; } = true;
        public bool ShowLongitudes { get; set; } = true;


        public SphericalCoordinateBuilder WithThickness(double thickness)
        {
            this.Thickness = thickness;
            return this;
        }

        public SphericalCoordinateBuilder WithShowLatitudes(bool showLatitudes)
        {
            this.ShowLatitutes = showLatitudes;
            return this;
        }

        public SphericalCoordinateBuilder WithShowLongitudes(bool showLongitudes)
        {
            this.ShowLongitudes = showLongitudes;
            return this;
        }

        public SphericalCoordinateBuilder WithRadius(double radius)
        {
            this.Radius = radius;
            return this;
        }

        public SphericalCoordinateBuilder WithTheta(int theta)
        {
            this.Theta = theta;
            return this;
        }

        public Model3DGroup Build()
        {
            var group = new Model3DGroup();


            if (this.ShowLatitutes)
            {
                this.BuildLatitudeTubes(group);
            }

            if (this.ShowLongitudes)
            {
                this.BuildLongitudinalTubes(group);
            }

            return group;
        }

        private void BuildLatitudeTubes(Model3DGroup group)
        {
            const int NumLatitudeBands = 12;
            double thetaIncrement = Math.PI / NumLatitudeBands;

            Vector3D scale = new Vector3D(1, 1, -1);
            var reflection = new ScaleTransform3D(scale);
            reflection.Freeze();

            for (int i = 1; i <= NumLatitudeBands / 2; i++)
            {
                IEnumerable<Point3D> points = GenerateLatitudePoints(thetaIncrement * i);

                var tubeVisual = new TubeVisual3D
                {
                    Fill = Brushes.DarkCyan,
                    Diameter = this.Thickness,
                    ThetaDiv = Theta,
                    Path = new Point3DCollection(points),
                    BackMaterial = null
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

                var reflectedModel = new GeometryModel3D
                {
                    Geometry = tubeVisual.Model.Geometry,
                    Material = tubeVisual.Material,
                    BackMaterial = null,
                    Transform = reflection
                };

                group.Children.Add(model);

                if (i != NumLatitudeBands / 2 || NumLatitudeBands % 2 == 1)
                {
                    group.Children.Add(reflectedModel);
                }
            }
        }

        private void BuildLongitudinalTubes(Model3DGroup group)
        {
            IEnumerable<Point3D> points = GenerateLongitudePoints();

            var tubeVisual = new TubeVisual3D
            {
                Fill = Brushes.DarkCyan,
                Diameter = this.Thickness,
                ThetaDiv = Theta,
                IsPathClosed = true,
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

        private IEnumerable<Point3D> GenerateLongitudePoints()
        {
            const int Points = 60;

            for (int i = 0; i <= Points; i++)
            {
                var pt = new Point3D(
                    this.Radius * Math.Cos(i * Math.PI * 2 / Points),
                    0,
                    this.Radius * Math.Sin(i * Math.PI * 2 / Points));

                yield return pt;
            }
        }

        private IEnumerable<Point3D> GenerateLatitudePoints(double theta)
        {
            const int Points = 60;

            double radius = Math.Sin(theta) * this.Radius;
            double z = Math.Cos(theta) * this.Radius;

            for (int i = 0; i <= Points; i++)
            {
                var pt = new Point3D(
                    radius * Math.Cos(i * Math.PI * 2 / Points),
                    radius * Math.Sin(i * Math.PI * 2 / Points),
                    z);

                yield return pt;
            }
        }
    }
}