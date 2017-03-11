namespace Lib.Cosmos.Scenes.Views
{
    using Animation;
    using Prism.Regions;
    using System;
    using System.Collections.Generic;
    using System.Windows.Media.Media3D;
    using Cosmos.ViewModels;
    using Extensions;
    using ViewControllers;

    /// <summary> Interaction logic for LorentzView.xaml </summary>
    public partial class LorentzView : IRegionMemberLifetime
    {
        public LorentzView()
        {
            this.InitializeComponent();
            this.MyViewPort.AddDefaultLights();

            var builder = new PolarCoordinateBuilder();
            builder.WithTubeVisual(this.MyTube).Build();

            var sphereBuilder = new SphericalCoordinateBuilder();

            this.MySphericals.Content =
                sphereBuilder.WithShowLatitudes(true)
                             .WithShowLongitudes(true)
                             .WithRadius(20)
                             .Build();

            this.Loaded += (s, e) =>
            {
                var vm = (ISceneAware)this.DataContext;

                vm.CameraViewModel = this.MyViewPort.ToCameraActions().AsViewModel();
                vm.SceneClips = this.SceneViewController.CreateSceneClips();
            };
        }

        public ISceneViewController SceneViewController => new LorentzViewController(this);

        public bool KeepAlive => false;

        private static IEnumerable<Point3D> GeneratePolarLines()
        {
            const int Points = 60;
            const int BaseRadius = 10;

            for (int j = 1; j < 20; j++)
            {
                var numPoints = Points + (2 * j);// * j;

                for (int i = 0; i <= numPoints; i++)
                {
                    var pt = new Point3D(
                        j * BaseRadius * Math.Cos(i * Math.PI * 2 / numPoints),
                        j * BaseRadius * Math.Sin(i * Math.PI * 2 / numPoints),
                        0);

                    yield return pt;

                    if (i > 0 && i < numPoints)
                    {
                        yield return pt;
                    }
                }
            }

            const int Radius = 200;

            for (int i = 0; i < 180; i += 15)
            {
                var pt1 = new Point3D(Radius * Math.Cos(i * Math.PI / 180), Radius * Math.Sin(i * Math.PI / 180), 0);
                var pt2 = new Point3D(Radius * Math.Cos((i * Math.PI / 180) + Math.PI), Radius * Math.Sin((i * Math.PI / 180) + Math.PI), 0);
                yield return pt1;
                yield return pt2;
                yield return pt1;
            }
        }
    }
}
