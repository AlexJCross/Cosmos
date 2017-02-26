namespace Lib.Cosmos.ViewModels
{
    using System.Windows.Input;
    using Prism.Commands;
    using Animation;
    using Utils;

    public class CameraControlViewModel : ICameraControlViewModel
    {
        private readonly ICameraActions cameraActions;

        public CameraControlViewModel(ICameraActions cameraActions)
        {
            this.cameraActions = cameraActions;

            this.ZoomInCommand = new DelegateCommand(this.cameraActions.ZoomIn);
            this.SnapZoomInCommand = new DelegateCommand(this.cameraActions.SnapZoomIn);
            this.OrbitCommand = new DelegateCommand(this.cameraActions.Orbit);
            this.SnapResetCommand = new DelegateCommand(this.cameraActions.SnapReset);
            this.MoveSideCommand = new DelegateCommand(this.cameraActions.MoveSide);
        }

        ~CameraControlViewModel()
        {
            MemoryUtils.LogGc<CameraControlViewModel>();
        }

        public ICommand OrbitCommand { get; }
        public ICommand SnapResetCommand { get; }
        public ICommand SnapZoomInCommand { get; }
        public ICommand ZoomInCommand { get; }
        public ICommand MoveSideCommand { get; }
    }
}