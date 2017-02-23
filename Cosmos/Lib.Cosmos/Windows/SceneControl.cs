using Prism.Regions;
using System.Windows.Controls;

namespace Lib.Cosmos.Windows
{

    public abstract class SceneControl : UserControl, IRegionMemberLifetime
    {
        public bool KeepAlive
        {
            get { return false; }
        }
    }
}