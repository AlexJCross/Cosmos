namespace Lib.Cosmos.Visual3Ds
{
    using System.Windows;
    using System.Windows.Media.Media3D;
    using HelixToolkit.Wpf;

    public enum LabelPosition
    {
        None,
        Left,
        Right
    }

    public class LabelledArrowVisual3D : ArrowVisual3D
    {
        public LabelPosition LabelPosition
        {
            get { return (LabelPosition)this.GetValue(LabelPositionProperty); }
            set { this.SetValue(LabelPositionProperty, value); }
        }

        public static readonly DependencyProperty LabelPositionProperty =
            DependencyProperty.Register(
                nameof(LabelPosition),
                typeof(LabelPosition),
                typeof(LabelledArrowVisual3D),
                new UIPropertyMetadata(LabelPosition.None, GeometryChanged));

        public Point3D TextPoint
        {
            get { return (Point3D)this.GetValue(TextPointProperty); }
            private set { this.SetValue(TextPointProperty, value); }
        }

        public static readonly DependencyProperty TextPointProperty =
            DependencyProperty.Register(nameof(TextPoint), typeof(Point3D), typeof(LabelledArrowVisual3D), new PropertyMetadata(default(Point3D)));

        protected override MeshGeometry3D Tessellate()
        {
            if (this.LabelPosition != LabelPosition.None)
            {
                // bug this only works for vectors in the plane...
                Vector3D up = new Vector3D(0, 0, 1);
                Vector3D arrowDirection = this.Point2 - this.Point1;

                Vector3D perp = this.LabelPosition == LabelPosition.Right
                    ? Vector3D.CrossProduct(arrowDirection, up)
                    : Vector3D.CrossProduct(up, arrowDirection);

                perp.Normalize();

                this.TextPoint = this.Point1 + (0.7 * arrowDirection) + (4 * perp);
            }

            return base.Tessellate();
        }
    }
}