namespace Lib.Cosmos.Scenes.Views
{
    using ViewControllers;

    public class VectorSpaceView : SceneViewBase
    {
        protected override ISceneViewController GetController()
        {
            return new VectorSpaceViewController(this);
        }
    }
}