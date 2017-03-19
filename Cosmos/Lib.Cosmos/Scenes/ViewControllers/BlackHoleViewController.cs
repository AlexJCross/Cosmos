namespace Lib.Cosmos.Scenes.ViewControllers
{
    using System.Collections.Generic;
    using System.Windows.Media.Media3D;
    using Animation;
    using Extensions;
    using Infrastructure;
    using Views;

    public class BlackHoleViewController : ISceneViewController
    {
        private readonly BlackHoleView view;
        private readonly ModelVisual3D blackHole;

        public BlackHoleViewController(BlackHoleView view)
        {
            this.view = view;

            var blackHoleBuilder = new EmbeddedBlackHoleBuilder();
            this.blackHole = blackHoleBuilder.WithMass(2d).WithRingRadius(5).Build();
            this.view.MyViewPort.Children.InsertIfMissing(this.blackHole);
        }

        public IList<ISceneClip> CreateSceneClips()
        {
            var toggleBlackHole = new ToggleSceneClip("Black Hole", this.ToggleBlackHole);

            return new List<ISceneClip>
            {
                new AsyncSceneClip("Enter Black Hole", () =>
                {
                    toggleBlackHole.Value = true;
                    return this.blackHole.EnterGrow();
                }),
                toggleBlackHole
            };
        }

        private void ToggleBlackHole(bool isChecked)
        {
            this.ToggleVisual(this.blackHole, isChecked);
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
}