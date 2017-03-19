namespace Lib.Cosmos.Scenes.ViewControllers
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media.Animation;
    using System.Windows.Media.Media3D;
    using HelixToolkit.Wpf;

    public static class ArrowMoveExtensions
    {
        public static Task<ArrowVisual3D> PointTo(this ArrowVisual3D arrow, Point3D point)
        {
            return arrow.PointTo(point, TimeSpan.FromSeconds(1));
        }

        public static Task<ArrowVisual3D> PointTo(this ArrowVisual3D arrow, Point3D point, TimeSpan timeSpan)
        {
            var startPoint = arrow.Point2;

            var pointAnimation = new Point3DAnimation(startPoint, point, new Duration(timeSpan))
            {
                AccelerationRatio = 0.3,
                DecelerationRatio = 0.5,
                FillBehavior = FillBehavior.HoldEnd,
                AutoReverse = false
            };

            var tcs = new TaskCompletionSource<ArrowVisual3D>();
            pointAnimation.Completed += (s, e) => tcs.SetResult(arrow);
            arrow.BeginAnimation(ArrowVisual3D.Point2Property, pointAnimation);

            return tcs.Task;
        }
    }
}