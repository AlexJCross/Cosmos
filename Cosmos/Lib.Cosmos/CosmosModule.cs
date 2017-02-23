namespace Lib.Cosmos
{
    using Views;
    using Microsoft.Practices.Unity;
    using Prism.Modularity;
    using Prism.Regions;
    using Windows;
    using Scenes.Infrastructure;
    using Scenes.Views;

    public class CosmosModule : IModule
    {
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer container;

        public CosmosModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }

        public void Initialize()
        {
            this.container.AddNewExtension<CosmosUnityExtension>();
            this.RegisterScenes();
            
            this.regionManager.RegisterViewWithRegion(RegionNames.MasterRegion, typeof(MasterDetailView));
        }

        private void RegisterScenes()
        {
            IScene scene1 = new Scene(1, 1, "Lorentz", "Description", typeof(LorentzView));
            IScene scene2 = new Scene(1, 2, "Black Hole", "Description",  typeof(BlackHoleView));

            this.container.RegisterInstance(scene1.Name, scene1);
            this.container.RegisterInstance(scene2.Name, scene2);
        }
    }
}
