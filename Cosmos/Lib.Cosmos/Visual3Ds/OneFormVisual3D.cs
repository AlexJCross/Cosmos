namespace Lib.Cosmos.Visual3Ds
{
    using System.Windows;
    using System.Windows.Media.Media3D;
    using HelixToolkit.Wpf;

    /// <summary> A visual element that shows an arrow. </summary>
    public class OneFormVisual3D : MeshElement3D
    {
        private const double UnitSeparation = 10;

        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register(
            nameof(Width),
            typeof(double),
            typeof(OneFormVisual3D),
            new UIPropertyMetadata(4d, GeometryChanged));

        public static readonly DependencyProperty NumBarsProperty =
            DependencyProperty.Register(nameof(NumBars),
                typeof(int),
                typeof(OneFormVisual3D),
                new PropertyMetadata(6));

        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register(
            nameof(Height),
            typeof(double),
            typeof(OneFormVisual3D),
            new UIPropertyMetadata(0.01, GeometryChanged));

        public static readonly DependencyProperty LengthProperty =
            DependencyProperty.Register(
                nameof(Length),
                typeof(double),
                typeof(OneFormVisual3D),
                new UIPropertyMetadata(10d, GeometryChanged));

        public static readonly DependencyProperty OriginProperty =
            DependencyProperty.Register(
                nameof(Origin),
                typeof(Point3D),
                typeof(OneFormVisual3D),
                new UIPropertyMetadata(default(Point3D), GeometryChanged));

        public static readonly DependencyProperty MagnitudeProperty =
            DependencyProperty.Register(
                nameof(Magnitude),
                typeof(double),
                typeof(OneFormVisual3D),
                new UIPropertyMetadata(1d, GeometryChanged));

        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register(
                nameof(Direction),
                typeof(Vector3D),
                typeof(OneFormVisual3D),
                new UIPropertyMetadata(new Vector3D(1, 0, 0), GeometryChanged));

        public static readonly DependencyProperty NormalProperty =
            DependencyProperty.Register(nameof(Normal),
                typeof(Vector3D),
                typeof(OneFormVisual3D),
                new UIPropertyMetadata(new Vector3D(0, 0, 1), GeometryChanged));

        public int NumBars
        {
            get { return (int) this.GetValue(NumBarsProperty); }
            set { this.SetValue(NumBarsProperty, value); }
        }

        public Vector3D Direction
        {
            get { return (Vector3D) this.GetValue(DirectionProperty); }
            set { this.SetValue(DirectionProperty, value); }
        }

        public Vector3D Normal
        {
            get { return (Vector3D) this.GetValue(NormalProperty); }
            set { this.SetValue(NormalProperty, value); }
        }

        public double Length
        {
            get { return (double) this.GetValue(LengthProperty); }
            set { this.SetValue(LengthProperty, value); }
        }

        public double Magnitude
        {
            get { return (double) this.GetValue(MagnitudeProperty); }
            set { this.SetValue(MagnitudeProperty, value); }
        }

        public Point3D Origin
        {
            get { return (Point3D) this.GetValue(OriginProperty); }
            set { this.SetValue(OriginProperty, value); }
        }

        /// <summary> Gets or sets the height. </summary>
        /// <value>The height.</value>
        public double Height
        {
            get { return (double) this.GetValue(HeightProperty); }
            set { this.SetValue(HeightProperty, value); }
        }

        /// <summary> Gets or sets the width. </summary>
        /// <value>The width.</value>
        public double Width
        {
            get { return (double) this.GetValue(WidthProperty); }
            set { this.SetValue(WidthProperty, value); }
        }

        /// <summary>
        ///     Do the tessellation and return the <see cref="MeshGeometry3D" />.
        /// </summary>
        /// <returns>A triangular mesh geometry.</returns>
        protected override MeshGeometry3D Tessellate()
        {
            if (this.Width <= 0)
            {
                return null;
            }

            var unitDirection = this.Direction;
            unitDirection.Normalize();
            var displacement = unitDirection * (UnitSeparation / this.Magnitude);

            var yDirection = Vector3D.CrossProduct(this.Direction, this.Normal);
            yDirection.Normalize();

            var num = this.NumBars;

            var builder = new MeshBuilder(false, false);
            const double XLength = 0.7;

            var scaledWidth = this.Width * UnitSeparation;

            Point3D[] points = this.GetArrowHeadPoints(displacement, yDirection);
            Vector3D widthOffset = new Vector3D((this.Width / 2 - 1) * UnitSeparation, 0, 0);


            if (num % 2 == 1)
            {

                builder.AddBox(this.Origin, this.Direction, yDirection, XLength, scaledWidth, this.Height);

                this.AddArrowHead(builder, points, widthOffset);
                this.AddArrowHead(builder, points, -widthOffset);

                for (var i = 1; i < (num + 1) / 2; i++)
                {
                    Vector3D increment = i * displacement;

                    builder.AddBox(this.Origin + increment, this.Direction, yDirection, XLength, scaledWidth, this.Height);
                    builder.AddBox(this.Origin - increment, this.Direction, yDirection, XLength, scaledWidth, this.Height);

                    Vector3D offset1 = widthOffset + increment;
                    Vector3D offset2 = widthOffset - increment;

                    this.AddArrowHead(builder, points,  offset1);
                    this.AddArrowHead(builder, points, -offset1);
                    this.AddArrowHead(builder, points,  offset2);
                    this.AddArrowHead(builder, points, -offset2);
                }
            }
            else
            {
                for (var i = 1; i <= num / 2; i++)
                {
                    Vector3D increment = (i - 0.5) * displacement;

                    builder.AddBox(this.Origin + increment, this.Direction, yDirection, XLength, scaledWidth, this.Height);
                    builder.AddBox(this.Origin - increment, this.Direction, yDirection, XLength, scaledWidth, this.Height);

                    Vector3D offset1 = widthOffset + increment;
                    Vector3D offset2 = widthOffset - increment;

                    this.AddArrowHead(builder, points, offset1);
                    this.AddArrowHead(builder, points, -offset1);
                    this.AddArrowHead(builder, points, offset2);
                    this.AddArrowHead(builder, points, -offset2);
                }
            }



            
            
            return builder.ToMesh(true);
        }

        private static readonly int[] ArrowHeadIndices = GetArrowHeadIndices();

        private static int[] GetArrowHeadIndices()
        {
            int[] indices =
            {
                // Bottom
                0, 2, 1,
                // back
                // 0, 3, 2,
                // 2, 3, 5,
                // Right
                1, 4, 0,
                4, 3, 0,
                // Left
                1, 2, 4,
                2, 5, 4,
                // Top
                3, 4, 5
            };

            return indices;
        }

        private Point3D[] GetArrowHeadPoints(Vector3D displacement, Vector3D yDirection)
        {
            Vector3D heightDisplacement = this.Height / 2 * this.Normal;
            Vector3D lengthDisplacement = displacement / 3;
            double width = 1.5;

            Point3D[] corners =
            {
                // Head
                this.Origin + (width * yDirection) - heightDisplacement,
                this.Origin + lengthDisplacement - heightDisplacement,
                this.Origin - (width * yDirection) - heightDisplacement,

                this.Origin + (width * yDirection) + heightDisplacement,
                this.Origin + lengthDisplacement + heightDisplacement,
                this.Origin - (width * yDirection) + heightDisplacement,
            };

            return corners;
        }

        private void AddArrowHead(MeshBuilder builder, Point3D[] points, Vector3D offset)
        {
            var transformedPoints = new Point3D[points.Length];

            for (int i = 0; i < points.Length; i++)
            {
                transformedPoints[i] = points[i] + offset;
            }

            builder.Append(transformedPoints, ArrowHeadIndices);
        }
    }
}