namespace Lib.Cosmos.Animation
{
    using System;
    using System.Windows;
    using System.Windows.Media.Animation;
    using System.Windows.Media.Media3D;

    public static class AnimationUtil
    {
        private const double Speed = 1000;

        public static void EnterGrow(this ModelVisual3D modelVisual)
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

            scaleTransform.BeginAnimation(ScaleTransform3D.ScaleXProperty, growAnimation);
            scaleTransform.BeginAnimation(ScaleTransform3D.ScaleYProperty, growAnimation);
            scaleTransform.BeginAnimation(ScaleTransform3D.ScaleZProperty, growAnimation);
        }

        public static void ExitShrink(this ModelVisual3D modelVisual, Action onExit)
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

            shrinkAnimation.Completed += (s, e) => onExit?.Invoke();

            scaleTransform.BeginAnimation(ScaleTransform3D.ScaleXProperty, shrinkAnimation);
            scaleTransform.BeginAnimation(ScaleTransform3D.ScaleYProperty, shrinkAnimation);
            scaleTransform.BeginAnimation(ScaleTransform3D.ScaleZProperty, shrinkAnimation);
        }
    }
}