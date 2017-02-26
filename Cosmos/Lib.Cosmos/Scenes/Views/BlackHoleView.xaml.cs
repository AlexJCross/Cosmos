namespace Lib.Cosmos.Scenes.Views
{
    using Animation;
    using Cosmos.ViewModels;
    using Utils;
    using Prism.Regions;
    using Extensions;

    /// <summary>
    /// Interaction logic for BlackHoleView.xaml
    /// </summary>
    public partial class BlackHoleView : IRegionMemberLifetime
    {
        public BlackHoleView()
        {
            InitializeComponent();

            var vm = (ISceneAware)this.DataContext;
            vm.CameraViewModel = this.MyViewPort.ToCameraActions().AsViewModel();

            var blackHoleBuilder = new EmbeddedBlackHoleBuilder();
            this.MyBlackHole.Content = blackHoleBuilder.WithMass(2d).WithRingRadius(5).Build();
        }

        public bool KeepAlive
        {
            get { return false; }
        }

        ~BlackHoleView()
        {
            MemoryUtils.LogGc<BlackHoleView>();
        }

        private void EnterBlackHole(object sender, System.Windows.RoutedEventArgs e)
        {
        }
    }
}
