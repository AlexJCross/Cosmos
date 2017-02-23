namespace Lib.Cosmos.Scenes.Views
{
    using Prism.Regions;

    /// <summary>
    /// Interaction logic for BlackHoleView.xaml
    /// </summary>
    public partial class BlackHoleView : IRegionMemberLifetime
    {
        public BlackHoleView()
        {
            InitializeComponent();
        }

        public bool KeepAlive
        {
            get { return false; }
        }
    }
}
