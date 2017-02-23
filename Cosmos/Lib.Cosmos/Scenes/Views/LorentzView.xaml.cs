namespace Lib.Cosmos.Scenes.Views
{
    using Prism.Regions;

    /// <summary> Interaction logic for LorentzView.xaml </summary>
    public partial class LorentzView : IRegionMemberLifetime
    {
        public LorentzView()
        {
            InitializeComponent();
        }

        public bool KeepAlive
        {
            get { return false; }
        }
    }
}
