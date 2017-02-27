namespace Lib.Cosmos.Scenes.Views
{
    using Animation;
    using Cosmos.ViewModels;
    using Utils;
    using Prism.Regions;
    using Extensions;
    using Infrastructure;
    using System.Collections.Generic;
    using System.Windows.Media.Media3D;

    public class BlackHoleViewController
    {
        private readonly BlackHoleView view;

        public BlackHoleViewController(BlackHoleView view)
        {
            this.view = view;
        }

        public IList<ISceneClip> CreateSceneClips()
        {
            var toggleBlackHole = new ToggleSceneClip("Black Hole", this.ToggleBlackHole);

            return new List<ISceneClip>
            {
                new SceneClip("Enter Black Hole", () =>
                {
                    toggleBlackHole.Value = true;
                    this.view.MyBlackHole.EnterGrow();
                }),
                toggleBlackHole
            };
        }

        private void ToggleBlackHole(bool isChecked)
        {
            this.ToggleVisual(this.view.MyBlackHole, isChecked);
        }

        private void ToggleVisual(Visual3D visual, bool isChecked)
        {
            if (isChecked)
            {
                this.view.MyViewPort.Children.InsertIfMissing(visual);
            }
            else
            {
                this.view.MyViewPort.Children.RemoveIfPresent(visual);
            }
        }
    }


    /// <summary> Interaction logic for BlackHoleView.xaml </summary>
    public partial class BlackHoleView : IRegionMemberLifetime
    {
        public BlackHoleView()
        {
            InitializeComponent();

            var blackHoleBuilder = new EmbeddedBlackHoleBuilder();
            this.MyBlackHole.Content = blackHoleBuilder.WithMass(2d).WithRingRadius(5).Build();

            this.Loaded += (s, e) =>
            {
                var vm = (ISceneAware)this.DataContext;
                var controller = new BlackHoleViewController(this);

                vm.CameraViewModel = this.MyViewPort.ToCameraActions().AsViewModel();
                vm.SceneClips = controller.CreateSceneClips();
            };
        }

        public bool KeepAlive
        {
            get { return false; }
        }

        ~BlackHoleView()
        {
            MemoryUtils.LogGc<BlackHoleView>();
        }
    }
}
