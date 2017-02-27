namespace Lib.Cosmos.Extensions
{
    using Animation;
    using ViewModels;

    public static class CameraActionsExtensions
    {
        public static ICameraControlViewModel AsViewModel(this CameraActions cameraActions)
        {
            return new CameraControlViewModel(cameraActions);
        }
    }
}