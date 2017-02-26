namespace Lib.Cosmos.Extensions
{
    using Animation;
    using System.Windows.Media.Media3D;
    using ViewModels;

    public static class CameraActionsExtensions
    {
        public static ICameraControlViewModel AsViewModel(this CameraActions cameraActions)
        {
            return new CameraControlViewModel(cameraActions);
        }
    }

    public static class Visual3DCollectionExtensions
    {
        public static void RemoveIfPresent(this Visual3DCollection collection, Visual3D visual)
        {
            if (collection.Contains(visual))
            {
                collection.Remove(visual);
            }
        }

        public static void InsertIfMissing(this Visual3DCollection collection, Visual3D visual, int index = 0)
        {
            if (collection.Contains(visual))
            {
                return;
            }

            collection.Insert(index, visual);
        }

        public static void AddIfMissing(this Visual3DCollection collection, Visual3D visual)
        {
            if (collection.Contains(visual))
            {
                return;
            }

            collection.Add(visual);
        }
    }
}