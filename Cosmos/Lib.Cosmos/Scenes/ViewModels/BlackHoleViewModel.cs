namespace Lib.Cosmos.Scenes.ViewModels
{
    using Cosmos.ViewModels;
    using Utils;

    public class BlackHoleViewModel : SceneAwareBase
    {
        public BlackHoleViewModel()
        {
            MemoryUtils.LogCtor<BlackHoleViewModel>();
        }

        ~BlackHoleViewModel()
        {
            MemoryUtils.LogGc<BlackHoleViewModel>();
        }
    }
}
