namespace Lib.Cosmos.Animation
{
    using System;

    public interface ICameraActions
    {
        Action Orbit { get; }
        Action SnapReset { get; }
        Action SnapZoomIn { get; }
        Action ZoomIn { get; }
        Action MoveSide { get; }
    }
}