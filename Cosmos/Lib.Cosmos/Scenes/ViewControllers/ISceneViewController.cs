using System.Collections.Generic;
using Lib.Cosmos.Scenes.Infrastructure;

namespace Lib.Cosmos.Scenes.ViewControllers
{
    public interface ISceneViewController
    {
        IList<ISceneClip> CreateSceneClips();
    }
}