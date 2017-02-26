namespace Lib.Cosmos.ViewModels
{
    using Scenes.Infrastructure;
    using Prism.Mvvm;
    using Prism.Regions;
    using System;
    using System.Linq;
    using Windows;
    using System.Diagnostics;

    class MasterDetailViewModel : BindableBase
    {
        private readonly IRegionManager regionMangager;
        private IScene scene;
        private IScene[] scenes;
        private WeakReference weakReference;

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

        #region Methods

        private void Navigate()
        {
            this.ValidateMemoryClearance();

            this.regionMangager.RegisterViewWithRegion(RegionNames.DetailRegion, this.scene.ViewType);
            this.regionMangager.RequestNavigate(RegionNames.DetailRegion, scene.ViewType.Name);
        }

        private void ValidateMemoryClearance()
        {
            // Reclaim memory while navigating to minimise garbage collection while rendering.
            // Commenting these lines out should cause the app to error on switching view.
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            var region = this.regionMangager.Regions[RegionNames.DetailRegion];

            // Make sure that, at most, one view is registered
            if (region.Views.Count() > 1)
            {
                throw new AccessViolationException("Memory leak detected. Region manager is holding a reference to the views.");
            }

            // We should have a weak reference to the old view. Check that it is dead.
            if (this.weakReference != null && this.weakReference.IsAlive)
            {
                Debug.Print("Memory not reclaimed");
                throw new AccessViolationException("Memory leak detected. Previous view still held in memory.");
            }

            // Now store a reference to the new view for future checks.
            new Action(() =>
            {
                if (region.Views.Any())
                {
                    this.weakReference = new WeakReference(region.Views.Single(), true);
                }
            })();
        }

        #endregion
    }
}
