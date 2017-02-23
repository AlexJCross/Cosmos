namespace Lib.Cosmos
{
    using Microsoft.Practices.Unity;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

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
