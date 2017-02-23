namespace Lib.Cosmos.Scenes.Infrastructure
{
    using System;

    public class Scene : IScene
    {
        public Scene(int episode, int order, string name, string description, Type viewType)
        {
            this.Episode = episode;
            this.Order = order;
            this.Name = name;
            this.Description = description;
            this.ViewType = viewType;
        }

        public string Description { get; }
        public int Episode { get; }
        public string Name { get; }
        public int Order { get; }
        public Type ViewType { get; }
    }
}
