namespace Lib.Cosmos.Scenes.ViewModels
{
    using Cosmos.ViewModels;
    using Utils;

    public class LorentzViewModel : SceneAwareBase
    {
        public LorentzViewModel()
        {
            MemoryUtils.LogCtor<LorentzViewModel>();
        }

        ~LorentzViewModel()
        {
            MemoryUtils.LogGc<LorentzViewModel>();
        }
    }
}
