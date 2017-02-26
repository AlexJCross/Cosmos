using System.Diagnostics;
namespace Lib.Cosmos.Views
{
    using Utils;
    using System.Windows.Controls;

    /// <summary> Interaction logic for CameraControlView.xaml </summary>
    public partial class CameraControlView : UserControl
    {
        public CameraControlView()
        {
            InitializeComponent();
        }

        ~CameraControlView()
        {
            MemoryUtils.LogGc<CameraControlView>();
        }
    }
}
