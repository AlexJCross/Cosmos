namespace Lib.Cosmos.Scenes.Infrastructure
{
    using Prism.Mvvm;
    using System;

    public class ToggleSceneClip : BindableBase, ISceneClip
    {
        private readonly Action<bool> action;
        private readonly string falseText;
        private readonly string trueText;

        private string text;
        private bool value = true;

        public ToggleSceneClip(string trueText, string falseText, Action<bool> action)
        {
            this.trueText = trueText;
            this.falseText = falseText;
            this.action = action;
            this.Value = false;
        }

        public ToggleSceneClip(string text, Action<bool> action)
            : this(text, text, action)
        {
        }

        public string Text
        {
            get { return this.text; }
            set { this.SetProperty(ref this.text, value); }
        }

        public bool Value
        {
            get
            {
                return this.value;
            }

            set
            {
                bool hasChanged = this.SetProperty(ref this.value, value);
                this.Text = value ? this.trueText : this.falseText;

                if (hasChanged)
                {
                    this.action(value);
                }
            }
        }
    }
}