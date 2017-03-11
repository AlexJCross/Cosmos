namespace Lib.Cosmos.Scenes.Views
{
    using Utils;
    using ViewControllers;

    public class BlackHoleView : SceneViewBase
    {
        ~BlackHoleView()
        {
            MemoryUtils.LogGc<BlackHoleView>();
        }

        protected override ISceneViewController GetController()
        {
            return new BlackHoleViewController(this);
        }
    }
}
