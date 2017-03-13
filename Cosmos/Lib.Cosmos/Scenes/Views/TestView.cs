using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace Lib.Cosmos.Scenes.Views
{
    using ViewControllers;

    public class VectorSpaceView : SceneViewBase
    {
        protected override ISceneViewController GetController()
        {
            return new VectorSpaceViewController(this);
        }

        protected override void CompositionTargetRendering(object sender, EventArgs e)
        {
            var matrix = this.MyViewPort.Viewport.GetTotalTransform();
            foreach (FrameworkElement element in this.overlay.Children)
            {
                var position = Overlay.GetPosition3D(element);
                var position2D = matrix.Transform(position);
                Canvas.SetLeft(element, position2D.X - element.ActualWidth / 2);
                Canvas.SetTop(element, position2D.Y - element.ActualHeight / 2);
            }
        }
    }

    /// <summary> The overlay. </summary>
    public class Overlay : DependencyObject
    {
        /// <summary>
        /// The position 3 d property.
        /// </summary>
        public static readonly DependencyProperty Position3DProperty = DependencyProperty.RegisterAttached(
            "Position3D", typeof(Point3D), typeof(Overlay));

        /// <summary> The get position 3 d. </summary>
        /// <param name="obj"> The obj. </param>
        /// <returns> </returns>
        public static Point3D GetPosition3D(DependencyObject obj)
        {
            return (Point3D)obj.GetValue(Position3DProperty);
        }

        /// <summary> The set position 3 d. </summary>
        /// <param name="obj"> The obj. </param>
        /// <param name="value"> The value. </param>
        public static void SetPosition3D(DependencyObject obj, Point3D value)
        {
            obj.SetValue(Position3DProperty, value);
        }
    }
}