namespace Lib.Cosmos.Scenes.ViewControllers
{
    using System.Collections.Generic;
    using Infrastructure;

    public interface ISceneViewController
    {
        IList<ISceneClip> CreateSceneClips();
    }
}