namespace Lib.Cosmos.Scenes.Views
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using HelixToolkit.Wpf;
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
}