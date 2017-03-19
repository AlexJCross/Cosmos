namespace Lib.Cosmos.Scenes.Infrastructure
{
    using System;
    using System.Windows.Input;
    using Prism.Commands;

    public class SceneClip : ISceneClip
    {
        public SceneClip(string text, Action clip)
        {
            this.Text = text;
            this.Command = new DelegateCommand(clip);
        }

        public ICommand Command { get; }

        public string Text { get; }
    }
}