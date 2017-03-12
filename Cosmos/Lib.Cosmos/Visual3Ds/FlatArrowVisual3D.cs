namespace Lib.Cosmos.Visual3Ds
{
    using HelixToolkit.Wpf;
    using System.Windows;
    using System.Windows.Media.Media3D;

    /// <summary> A visual element that shows an arrow. </summary>
    public class FlatArrowVisual3D : MeshElement3D
    {
        /// <summary> Identifies the <see cref="Width"/> dependency property. </summary>
        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register(
            nameof(Width), typeof(double), typeof(FlatArrowVisual3D), new UIPropertyMetadata(0.05, GeometryChanged));

        /// <summary> Identifies the <see cref="Height"/> dependency property. </summary>
        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register(
            nameof(Height), typeof(double), typeof(FlatArrowVisual3D), new UIPropertyMetadata(0.01, GeometryChanged));

        /// <summary> Identifies the <see cref="HeadLength"/> dependency property. </summary>
        public static readonly DependencyProperty HeadLengthProperty = DependencyProperty.Register(
            nameof(HeadLength), typeof(double), typeof(FlatArrowVisual3D), new UIPropertyMetadata(0.1, GeometryChanged));

        /// <summary> Identifies the <see cref="Point1"/> dependency property. </summary>
        public static readonly DependencyProperty Point1Property = DependencyProperty.Register(
            nameof(Point1),
            typeof(Point3D),
            typeof(FlatArrowVisual3D),
            new UIPropertyMetadata(new Point3D(0, 0, 0), GeometryChanged));

        /// <summary> Identifies the <see cref="Point2"/> dependency property. </summary>
        public static readonly DependencyProperty Point2Property = DependencyProperty.Register(
            nameof(Point2),
            typeof(Point3D),
            typeof(FlatArrowVisual3D),
            new UIPropertyMetadata(new Point3D(0, 5, 0), GeometryChanged));

        /// <summary> Gets or sets the height. </summary>
        /// <value>The height.</value>
        public double Height
        {
            get { return (double)this.GetValue(HeightProperty); }
            set { this.SetValue(HeightProperty, value); }
        }

        /// <summary> Gets or sets the width. </summary>
        /// <value>The width.</value>
        public double Width
        {
            get { return (double)this.GetValue(WidthProperty); }
            set { this.SetValue(WidthProperty, value); }
        }

        /// <summary> Gets or sets the direction. </summary>
        /// <value>The direction.</value>
        public Vector3D Direction
        {
            get { return this.Point2 - this.Point1; }
            set { this.Point2 = this.Point1 + value; }
        }

        /// <summary>
        /// Gets or sets the length of the head (relative to the width of the vector).
        /// </summary>
        /// <value>The length of the head relative to the diameter.</value>
        public double HeadLength
        {
            get { return (double)this.GetValue(HeadLengthProperty); }
            set { this.SetValue(HeadLengthProperty, value); }
        }

        /// <summary> Gets or sets the origin. </summary>
        /// <value>The origin.</value>
        public Point3D Origin
        {
            get { return this.Point1; }
            set { this.Point1 = value; }
        }

        /// <summary> Gets or sets the start point of the arrow. </summary>
        /// <value>The start point.</value>
        public Point3D Point1
        {
            get { return (Point3D)this.GetValue(Point1Property); }
            set { this.SetValue(Point1Property, value); }
        }

        /// <summary> Gets or sets the end point of the arrow. </summary>
        /// <value>The end point.</value>
        public Point3D Point2
        {
            get { return (Point3D)this.GetValue(Point2Property); }
            set { this.SetValue(Point2Property, value); }
        }

        /// <summary>
        /// Do the tessellation and return the <see cref="MeshGeometry3D"/>.
        /// </summary>
        /// <returns>A triangular mesh geometry.</returns>
        protected override MeshGeometry3D Tessellate()
        {
            if (this.Width <= 0)
            {
                return null;
            }

            var dir = this.Direction;
            var length = dir.Length;
            var headLength = this.HeadLength;
            var width = this.Width;
            var height = this.Height;

            var bodyLength = length - headLength;
            var halfWidth = width * 0.5;
            var halfHeight = height * 0.5;

            Point3D[] corners = {
                // Body
                new Point3D(         0, -halfWidth, -halfHeight),
                new Point3D(bodyLength, -halfWidth, -halfHeight),
                new Point3D(         0, +halfWidth, -halfHeight),
                new Point3D(bodyLength, +halfWidth, -halfHeight),
                new Point3D(         0, -halfWidth, +halfHeight),
                new Point3D(bodyLength, -halfWidth, +halfHeight),
                new Point3D(         0, +halfWidth, +halfHeight),
                new Point3D(bodyLength, +halfWidth, +halfHeight),

                // Head
                new Point3D(length - headLength,   -width, -height / 2),
                new Point3D(             length,        0, -height / 2),
                new Point3D(length - headLength,    width, -height / 2),
                new Point3D(length - headLength,   -width, +height / 2),
                new Point3D(             length,        0, +height / 2),
                new Point3D(length - headLength,    width, +height / 2)
            };

            int[] indices = {
                // Body
                2, 3, 1,
                2, 1, 0,
                6, 5, 7,
                6, 4, 5,
                6, 2, 0,
                2, 0, 4,
                2, 7, 3,
                2, 6, 7,
                0, 1, 5,
                0, 5, 4,

                // Bottom
                8,10,9,
                // back
                8,11,10,
                10,11,13,
                // Right
                9,12,8,
                12,11,8,
                // Left
                9,10,12,
                10,13,12,
                // Top
                11,12,13,
            };

            var x = new Vector3D(1, 0, 0);

            var v = Vector3D.CrossProduct(x, dir);
            var angle = Vector3D.AngleBetween(x, dir);

            var matrix = Matrix3D.Identity;

            if (angle > 0)
            {
                var quaternion5 = new Quaternion(v, angle);
                matrix.Rotate(quaternion5);
            }

            matrix.Translate(this.Point1 - default(Point3D));
            matrix.Transform(corners);

            var builder = new MeshBuilder(false, false);
            builder.Append(corners, indices);
            return builder.ToMesh(true);
        }
    }
}
