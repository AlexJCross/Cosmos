namespace Lib.Cosmos.Scenes.Views
{
    using ViewControllers;

    public class TestView : SceneViewBase
    {
        protected override ISceneViewController GetController()
        {
            return new TestViewController();
        }
    }
}