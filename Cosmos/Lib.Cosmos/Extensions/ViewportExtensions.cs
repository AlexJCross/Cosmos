namespace Lib.Cosmos.Extensions
{
    using HelixToolkit.Wpf;
    using Animation;
    using System;
    using System.Windows.Media.Media3D;
    using System.Windows.Media.Animation;
    using System.Windows;

    public static class ViewportExtensions
    {
        public static HelixViewport3D AddDefaultLights(this HelixViewport3D viewport)
        {
            Visual3D lightAbove = new SunLight
            {
                Ambient = 0.9,
                Altitude = 90,
                Brightness = 0.5,
                ShowLights = false
            };

            Visual3D lightBelow = new SunLight
            {
                Ambient = 0.9,
                Altitude = -90,
                Brightness = 0.5,
                ShowLights = false
            };

            viewport.Children.Add(lightAbove);
            viewport.Children.Add(lightBelow);

            return viewport;
        }

        public static CameraActions ToCameraActions(this HelixViewport3D viewport)
        {
            return new CameraActions(
                () => SnapReset(viewport),
                () => ZoomIn(viewport),
                () => SnapZoomIn(viewport),
                () => Orbit(viewport),
                () => MoveSide(viewport));
        }

        private static void MoveSide(HelixViewport3D viewport)
        {
            const double side = 52;

            viewport.SetView(
                new Point3D(side, side, 80),
                new Vector3D(-side, -side, -80),
                new Vector3D(0, 0, 1),
                1000);
        }

        private static void SnapReset(HelixViewport3D viewport)
        {
            viewport.Camera.Transform = null;
            viewport.ResetCamera();
        }

        private static void Orbit(HelixViewport3D viewport)
        {
            var center = new Point3D(0, 0, 0);
            double angle = 0;
            var axis = new Vector3D(0, 0, 1);
            var rotation = new AxisAngleRotation3D(axis, angle);
            viewport.Camera.Transform = new RotateTransform3D(rotation, center);

            var a1 = new DoubleAnimation(0, 360, new Duration(TimeSpan.FromMilliseconds(4000)))
            {
                AccelerationRatio = 0.3,
                DecelerationRatio = 0.5,
                FillBehavior = FillBehavior.Stop,
                AutoReverse = true
            };

            a1.Completed += (s, a) =>
            {
                viewport.SetView(
                    new Point3D(0, 0, 120),
                    new Vector3D(0, 0, -120),
                    new Vector3D(-0.5, -0.5, 0), // This is really important
                    1000);
            };

            rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, a1);
        }

        private static void ZoomIn(HelixViewport3D viewport)
        {
            var position = viewport.Camera.Position;
            var newPosition = new Point3D(position.X, position.Y, position.Z - 20);
            viewport.SetView(newPosition, viewport.Camera.LookDirection, viewport.Camera.UpDirection, 2000);
        }

        private static void SnapZoomIn(HelixViewport3D viewport)
        {
            var position = viewport.Camera.Position;
            var newPosition = new Point3D(position.X, position.Y, position.Z - 10);
            viewport.Camera.Position = newPosition;
        }
    }
}
