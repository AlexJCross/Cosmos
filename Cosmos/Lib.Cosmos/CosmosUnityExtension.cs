namespace Lib.Cosmos
{
    using Microsoft.Practices.Unity;

    public class CosmosUnityExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            /*
            this.Container.RegisterTypes(
                typeof(CosmosUnityExtension).Assembly.GetTypes(),
                WithMappings.FromMatchingInterface,
                WithName.Default,
                WithLifetime.ContainerControlled);
             */
        }
    }
}
