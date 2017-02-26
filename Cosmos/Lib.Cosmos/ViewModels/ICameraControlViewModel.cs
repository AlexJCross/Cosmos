namespace Lib.Cosmos.ViewModels
{
    using System.Windows.Input;

    public interface ICameraControlViewModel
    {
        ICommand OrbitCommand { get; }
        ICommand SnapResetCommand { get; }
        ICommand SnapZoomInCommand { get; }
        ICommand ZoomInCommand { get; }
        ICommand MoveSideCommand { get; }
    }
}