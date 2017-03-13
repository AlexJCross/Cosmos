using System;
using System.Windows.Media;

namespace Lib.Cosmos.Scenes.Views
{
    using Cosmos.ViewModels;
    using Prism.Regions;
    using Extensions;
    using ViewControllers;

    /// <summary> Interaction logic for SceneViewBase.xaml </summary>
    public abstract partial class SceneViewBase : IRegionMemberLifetime
    {
        protected SceneViewBase()
        {
            this.InitializeComponent();

            CompositionTarget.Rendering += this.CompositionTargetRendering;

            Console.WriteLine(this.MyLight);

            this.Loaded += (s, e) =>
            {
                this.MyViewPort.AddDefaultLights();
                var vm = this.DataContext as ISceneAware;

                if (vm != null)
                {
                    var controller = this.GetController();

                    vm.CameraViewModel = this.MyViewPort.ToCameraActions().AsViewModel();
                    vm.SceneClips = controller.CreateSceneClips();
                }
            };
        }

        protected virtual void CompositionTargetRendering(object sender, EventArgs e)
        {
        }

        public bool KeepAlive => false;

        protected abstract ISceneViewController GetController();
    }

}
