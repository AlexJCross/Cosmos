namespace Lib.Cosmos.Visual3Ds
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media.Media3D;
    using HelixToolkit.Wpf;

    /// <summary> A visual element that shows a 3D rectangle defined by origin, normal, length and width. </summary>
    public class ParallelogramVisual3D : MeshElement3D
    {
        public static readonly DependencyProperty Point1Property =
            DependencyProperty.Register(
                nameof(Point1),
                typeof(Point3D),
                typeof(ParallelogramVisual3D),
                new UIPropertyMetadata(default(Point3D), GeometryChanged));

        public static readonly DependencyProperty Point2Property =
            DependencyProperty.Register(
                nameof(Point2),
                typeof(Point3D),
                typeof(ParallelogramVisual3D),
                new UIPropertyMetadata(default(Point3D), GeometryChanged));

        public static readonly DependencyProperty ResultantPoint3DProperty =
            DependencyProperty.Register(
                nameof(ResultantPoint3D),
                typeof(Point3D),
                typeof(ParallelogramVisual3D),
                new PropertyMetadata(default(Point3D), null, CoerceValueCallback));

        private static object CoerceValueCallback(DependencyObject d, object basevalue)
        {
            var me = (ParallelogramVisual3D) d;

            var vec1 = me.Point1 - me.Origin;
            var vec2 = me.Point2 - me.Origin;

            return me.Origin + vec1 + vec2;
        }

        public Point3D ResultantPoint3D
        {
            get { return (Point3D)this.GetValue(ResultantPoint3DProperty); }
            set { this.SetValue(ResultantPoint3DProperty, value); }
        }

        /// <summary> Identifies the <see cref="Origin" /> dependency property. </summary>
        public static readonly DependencyProperty OriginProperty = DependencyProperty.Register(
            nameof(Origin),
            typeof(Point3D),
            typeof(ParallelogramVisual3D),
            new PropertyMetadata(default(Point3D), GeometryChanged));


        public Point3D Point1
        {
            get { return (Point3D) this.GetValue(Point1Property); }
            set { this.SetValue(Point1Property, value); }
        }

        public Point3D Point2
        {
            get { return (Point3D) this.GetValue(Point2Property); }
            set { this.SetValue(Point2Property, value); }
        }

        /// <summary>
        ///     Gets or sets the center point of the plane.
        /// </summary>
        /// <value>The origin.</value>
        public Point3D Origin
        {
            get { return (Point3D) this.GetValue(OriginProperty); }

            set { this.SetValue(OriginProperty, value); }
        }

        /// <summary> Do the tessellation and return the <see cref="MeshGeometry3D" />. </summary>
        /// <returns>A triangular mesh geometry.</returns>
        protected override MeshGeometry3D Tessellate()
        {
            this.CoerceValue(ResultantPoint3DProperty);

            Point3D[] pts =
            {
                this.Origin,
                this.Point1,
                this.Point2,
                this.ResultantPoint3D
            };

            var builder = new MeshBuilder(false, false);

            IList<int> indices = new[]
            {
                0, 1, 2,
                2, 1, 3
            };

            builder.Append(pts, indices);

            return builder.ToMesh();
        }
    }
}