namespace Lib.Cosmos.Animation
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media.Animation;
    using System.Windows.Media.Media3D;

    public static class AnimationUtil
    {
        private const double Speed = 1000;

        public static Task<TVisual> EnterGrow<TVisual>(this TVisual modelVisual)  where TVisual : ModelVisual3D
        {
            var scaleTransform = new ScaleTransform3D();

            // TODO consider transform group in case this overrides an existing transform :S
            modelVisual.Transform = scaleTransform;

            var growAnimation = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(Speed)))
            {
                AccelerationRatio = 0,
                DecelerationRatio = 0.5,
                FillBehavior = FillBehavior.Stop,
                AutoReverse = false
            };

            var tcs = new TaskCompletionSource<TVisual>();

            growAnimation.Completed += (s, e) =>
            {
                tcs.TrySetResult(modelVisual);
            };

            scaleTransform.BeginAnimation(ScaleTransform3D.ScaleXProperty, growAnimation);
            scaleTransform.BeginAnimation(ScaleTransform3D.ScaleYProperty, growAnimation);
            scaleTransform.BeginAnimation(ScaleTransform3D.ScaleZProperty, growAnimation);

            return tcs.Task;
        }

        public static Task<TVisual> ExitGrow<TVisual>(this TVisual modelVisual) where TVisual : ModelVisual3D
        {
            var scaleTransform = new ScaleTransform3D();

            // TODO consider transform group in case this overrides an existing transform :S
            modelVisual.Transform = scaleTransform;

            var shrinkAnimation = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromMilliseconds(Speed)))
            {
                AccelerationRatio = 0,
                DecelerationRatio = 0.5,
                FillBehavior = FillBehavior.Stop,
                AutoReverse = false
            };

            var tcs = new TaskCompletionSource<TVisual>();

            shrinkAnimation.Completed += (s, e) =>
            {
                tcs.TrySetResult(modelVisual);
            };

            scaleTransform.BeginAnimation(ScaleTransform3D.ScaleXProperty, shrinkAnimation);
            scaleTransform.BeginAnimation(ScaleTransform3D.ScaleYProperty, shrinkAnimation);
            scaleTransform.BeginAnimation(ScaleTransform3D.ScaleZProperty, shrinkAnimation);

            return tcs.Task;
        }
    }
}