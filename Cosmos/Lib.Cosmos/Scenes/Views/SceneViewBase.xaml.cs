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
            this.MyViewPort.AddDefaultLights();

            this.Loaded += (s, e) =>
            {
                var vm = this.DataContext as ISceneAware;

                if (vm != null)
                {
                    var controller = this.GetController();

                    vm.CameraViewModel = this.MyViewPort.ToCameraActions().AsViewModel();
                    vm.SceneClips = controller.CreateSceneClips();
                }
            };
        }

        public bool KeepAlive => false;

        protected abstract ISceneViewController GetController();
    }

}
