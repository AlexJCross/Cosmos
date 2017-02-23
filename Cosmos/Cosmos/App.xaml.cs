using System.Windows;
namespace Cosmos
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var bootstrapper = new CosmosBootstrapper();
            bootstrapper.Run();
        }
    }
}
