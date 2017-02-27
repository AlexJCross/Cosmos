
namespace Lib.Cosmos.Scenes.Infrastructure
{
    using Prism.Commands;
    using System;
    using System.Windows.Input;

    public class SceneClip : ISceneClip
    {
        public SceneClip(string text, Action clip)
        {
            this.Text = text;
            this.Command = new DelegateCommand(clip);
        }

        public string Text { get; }

        public ICommand Command { get; }
    }
}