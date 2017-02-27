namespace Lib.Cosmos.Views
{
    using Scenes.Infrastructure;
    using System.Collections.Generic;
    using System.Windows;

    /// <summary> Interaction logic for SceneClipsView.xaml </summary>
    public partial class SceneClipsControl
    {
        public IEnumerable<ISceneClip> SceneClips
        {
            get { return (IEnumerable<ISceneClip>)GetValue(SceneClipsProperty); }
            set { SetValue(SceneClipsProperty, value); }
        }

        public static readonly DependencyProperty SceneClipsProperty =
            DependencyProperty.Register(nameof(SceneClips), typeof(IEnumerable<ISceneClip>), typeof(SceneClipsControl), new PropertyMetadata(null));

        public SceneClipsControl()
        {
            InitializeComponent();
        }
    }
}
