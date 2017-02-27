namespace Lib.Cosmos.ViewModels
{
    using Prism.Regions;
    using Prism.Mvvm;
    using Scenes.Views;
    using System.Collections.Generic;
    using Scenes.Infrastructure;

    public interface ISceneAware : INavigationAware
    {
        ICameraControlViewModel CameraViewModel { get; set; }

        IList<ISceneClip> SceneClips { get; set; }
    }

    public abstract class SceneAwareBase : BindableBase, ISceneAware
    {
        private ICameraControlViewModel cameraViewModel;
        private IList<ISceneClip> sceneClips = new SceneClip[0];

        public ICameraControlViewModel CameraViewModel
        {
            get { return this.cameraViewModel; }
            set { this.SetProperty(ref this.cameraViewModel, value); }
        }

        public IList<ISceneClip> SceneClips
        {
            get { return this.sceneClips; }
            set { this.SetProperty(ref this.sceneClips, value); }
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
            this.CameraViewModel = null;
            this.SceneClips= new SceneClip[0];
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
        }
    }
}