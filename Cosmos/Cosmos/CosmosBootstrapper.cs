using Prism.Unity;
using Microsoft.Practices.Unity;
using System;
using System.Windows;
using Prism.Modularity;
using Lib.Cosmos;

namespace Cosmos
{
    public class CosmosBootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return this.Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            App.Current.MainWindow = (Window)this.Shell;
            App.Current.MainWindow.Show();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return base.CreateModuleCatalog();
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            this.AddModule<CosmosModule>();
        }

        private void AddModule<TModule>()
        {
            Type moduleType = typeof(TModule);

            this.ModuleCatalog.AddModule(
              new ModuleInfo
              {
                  ModuleName = moduleType.Name,
                  ModuleType = moduleType.AssemblyQualifiedName,
              });
        }
    }
}