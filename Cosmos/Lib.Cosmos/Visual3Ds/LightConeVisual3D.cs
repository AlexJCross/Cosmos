namespace Lib.Cosmos.Visual3Ds
{
    using System.Windows;
    using System.Windows.Media.Media3D;
    using HelixToolkit.Wpf;

    public class LightConeVisual3D : MeshElement3D
    {
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register(
                nameof(X),
                typeof(Vector3D),
                typeof(LightConeVisual3D),
                new PropertyMetadata(new Vector3D(1, 0, 0)));

        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register(
                nameof(Time),
                typeof(Vector3D),
                typeof(LightConeVisual3D),
                new PropertyMetadata(new Vector3D(0, 1, 0)));

        public static readonly DependencyProperty HeightProperty =
            DependencyProperty.Register(nameof(Height), typeof(double), typeof(LightConeVisual3D), new PropertyMetadata(0.01));

        public static readonly DependencyProperty LengthProperty =
            DependencyProperty.Register(nameof(Length), typeof(double), typeof(LightConeVisual3D), new PropertyMetadata(200d));
        
        public static readonly DependencyProperty CenterProperty =
            DependencyProperty.Register(nameof(Center), typeof(Point3D), typeof(LightConeVisual3D), new PropertyMetadata(new Point3D()));

        public static readonly DependencyProperty WidthProperty =
            DependencyProperty.Register(nameof(Width), typeof(double), typeof(LightConeVisual3D), new PropertyMetadata(0.5));

        public double Width
        {
            get { return (double)this.GetValue(WidthProperty); }
            set { this.SetValue(WidthProperty, value); }
        }
        
        public Point3D Center
        {
            get { return (Point3D)this.GetValue(CenterProperty); }
            set { this.SetValue(CenterProperty, value); }
        }

        public Vector3D Time
        {
            get { return (Vector3D)this.GetValue(TimeProperty); }
            set { this.SetValue(TimeProperty, value); }
        }

        public double Height
        {
            get { return (double)this.GetValue(HeightProperty); }
            set { this.SetValue(HeightProperty, value); }
        }

        public double Length
        {
            get { return (double)this.GetValue(LengthProperty); }
            set { this.SetValue(LengthProperty, value); }
        }

        public Vector3D X
        {
            get { return (Vector3D)this.GetValue(XProperty); }
            set { this.SetValue(XProperty, value); }
        }

        protected override MeshGeometry3D Tessellate()
        {
            var builder = new MeshBuilder(true, true);

            var normal = Vector3D.CrossProduct(this.X, this.Time);

            var matrix = new Matrix3D();
            var quaternion = new Quaternion(normal, 45);
            matrix.Rotate(quaternion);

            var x1 = matrix.Transform(this.X);
            var x2 = matrix.Transform(this.Time);

            builder.AddBox(this.Center, x1, x2, this.Length, this.Width, this.Height);
            builder.AddBox(this.Center, x2, x1, this.Length, this.Width, this.Height);

            var mesh = builder.ToMesh();

            return mesh;
        }
    }
}