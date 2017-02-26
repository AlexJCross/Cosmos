namespace Lib.Cosmos.Animation
{
    using System;

    public class CameraActions : ICameraActions
    {
        public CameraActions(Action snapReset, Action zoomIn, Action snapZoomIn, Action orbit, Action moveSide)
        {
            this.SnapReset = snapReset;
            this.ZoomIn = zoomIn;
            this.SnapZoomIn = snapZoomIn;
            this.Orbit = orbit;
            this.MoveSide = moveSide;
        }

        public Action SnapReset { get; }
        public Action SnapZoomIn { get; }
        public Action ZoomIn { get; }
        public Action Orbit { get; }
        public Action MoveSide { get; }
    }
}