namespace Lib.Cosmos.ViewModels
{
    using Scenes.Infrastructure;
    using Prism.Mvvm;
    using Prism.Regions;
    using System;
    using System.Linq;
    using Windows;

    class MasterDetailViewModel : BindableBase
    {
        private readonly IRegionManager regionMangager;
        private IScene scene;
        private IScene[] scenes;

        public MasterDetailViewModel(IRegionManager regionMangager, IScene[] scenes)
        {
            this.scenes = scenes;
            this.regionMangager = regionMangager;
        }

        public IScene[] Scenes
        {
            get { return this.scenes; }
            set { this.SetProperty(ref this.scenes, value); }
        }

        public IScene Scene
        {
            get { return this.scene; }
            set
            {
                if (value == null)
                {
                    return;
                }

                this.SetProperty(ref this.scene, value);
                this.Navigate();
            }
        }

        public void Navigate()
        {
            var region = this.regionMangager.Regions[RegionNames.DetailRegion];

            // Make sure the view is not already registered
            if (region.Views.Count() > 1)
            {
                throw new AccessViolationException("View already registered. This suggests a memory leak has occured.");
            }

            this.regionMangager.RegisterViewWithRegion(RegionNames.DetailRegion, this.scene.ViewType);
            region.RequestNavigate(scene.ViewType.Name, OnNavigation);

        }

        private void OnNavigation(NavigationResult result)
        {
            if (result.Error != null)
            {
                throw result.Error;
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }


}
