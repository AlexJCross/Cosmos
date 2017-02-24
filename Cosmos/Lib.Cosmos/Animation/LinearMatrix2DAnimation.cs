namespace Lib.Cosmos.Animation
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Animation;

    public class LinearMatrix2DAnimation : AnimationTimeline
    {
        public static DependencyProperty FromProperty = DependencyProperty.Register(
            nameof(From), typeof(Matrix?), typeof(LinearMatrix2DAnimation), new PropertyMetadata(null));

        public static DependencyProperty ToProperty = DependencyProperty.Register(
            nameof(To), typeof(Matrix?), typeof(LinearMatrix2DAnimation), new PropertyMetadata(null));

        public LinearMatrix2DAnimation()
        {
        }

        public LinearMatrix2DAnimation(Matrix from, Matrix to, Duration duration)
        {
            Duration = duration;
            From = from;
            To = to;
        }

        public Matrix? To
        {
            set { SetValue(ToProperty, value); }
            get { return (Matrix)GetValue(ToProperty); }
        }

        public Matrix? From
        {
            set { SetValue(FromProperty, value); }
            get { return (Matrix)GetValue(FromProperty); }
        }

        public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        {
            if (animationClock.CurrentProgress == null)
            {
                return null;
            }

            double progress = animationClock.CurrentProgress.Value;
            Matrix from = From ?? (Matrix)defaultOriginValue;

            if (To.HasValue)
            {
                Matrix to = To.Value;

                double m11 = ((to.M11 - from.M11) * progress) + from.M11;
                double m22 = ((to.M22 - from.M22) * progress) + from.M22;
                double offsetX = ((to.OffsetX - from.OffsetX) * progress) + from.OffsetX;
                double offsetY = ((to.OffsetY - from.OffsetY) * progress) + from.OffsetY;

                Matrix newMatrix = new Matrix(m11, 0, 0, m22, offsetX, offsetY);

                return newMatrix;
            }

            return Matrix.Identity;
        }

        protected override Freezable CreateInstanceCore()
        {
            return new LinearMatrix2DAnimation();
        }

        public override Type TargetPropertyType
        {
            get { return typeof(Matrix); }
        }
    }
}