namespace Lib.Cosmos.Scenes.Infrastructure
{
    using System;

    public interface IScene
    {
        int Episode { get; }
        int Order { get; }
        string Name { get; }
        Type ViewType { get; }
        string Description { get; }
    }
}