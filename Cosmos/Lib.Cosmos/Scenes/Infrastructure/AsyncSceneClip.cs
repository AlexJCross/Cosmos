namespace Lib.Cosmos.Scenes.Infrastructure
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Windows;

    public class AsyncSceneClip : ISceneClip
    {
        public AsyncSceneClip(string text, Func<Task> clip)
        {
            this.Text = text;
            this.Command = new AsyncCommand(clip);
        }

        public ICommand Command { get; }

        public string Text { get; }
    }
}