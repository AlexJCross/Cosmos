namespace Lib.Cosmos.Views
{
    /// <summary> Interaction logic for MasterDetailView.xaml </summary>
    public partial class MasterDetailView
    {
        public MasterDetailView()
        {
            this.InitializeComponent();

            this.Loaded += (sender, args) => this.ListBoxEpisodes.SelectedItem = this.ListBoxEpisodes.Items[0];
        }
    }
}
