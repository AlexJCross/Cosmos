namespace Lib.Cosmos.Animation
{
    using System;
    using System.Windows;
    using System.Windows.Media.Animation;
    using System.Windows.Media.Media3D;

    public class LorentzAnimation : AnimationTimeline
    {
        public static DependencyProperty FromProperty = DependencyProperty.Register(
            nameof(From), typeof(double?), typeof(LorentzAnimation), new PropertyMetadata(0d));

        public static DependencyProperty ToProperty = DependencyProperty.Register(
            nameof(To), typeof(double?), typeof(LorentzAnimation), new PropertyMetadata(0d));

        public LorentzAnimation()
        {
        }

        public LorentzAnimation(double from, double to, Duration duration)
        {
            Duration = duration;
            From = from;
            To = to;
        }

        public double? From
        {
            set { SetValue(FromProperty, value); }
            get { return (double)GetValue(FromProperty); }
        }

        public override Type TargetPropertyType
        {
            get { return typeof(MatrixTransform3D); }
        }

        public double? To
        {
            set { SetValue(ToProperty, value); }
            get { return (double)GetValue(ToProperty); }
        }


        public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        {
            if (animationClock.CurrentProgress == null)
            {
                return null;
            }

            double progress = animationClock.CurrentProgress.Value;

            if (To.HasValue)
            {
                Matrix3D boostMatrix = Matrix3D.Identity;

                double beta = -progress * ((double)To - (double)From);
                double gamma = Math.Pow(1 - beta * beta, -0.5);

                boostMatrix.M11 = gamma;
                boostMatrix.M22 = gamma;
                boostMatrix.M12 = -beta * gamma;
                boostMatrix.M21 = -beta * gamma;

                return new MatrixTransform3D(boostMatrix);
            }

            throw new Exception("To matrix must be set");
        }


        protected override Freezable CreateInstanceCore()
        {
            return new LorentzAnimation();
        }
    }
}