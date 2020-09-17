#region 組件 Microsoft.Kinect.Face, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// C:\Program Files\Microsoft SDKs\Kinect\v2.0_1409\Redist\Face\x64\Microsoft.Kinect.Face.dll
#endregion

using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

namespace Microsoft.Kinect.Face
{
    //
    // 摘要:
    //     Represents a face frame result.
    public sealed class FaceFrameResult
    {
        ~FaceFrameResult();

        //
        // 摘要:
        //     Gets the face properties.
        public IReadOnlyDictionary<FaceProperty, DetectionResult> FaceProperties { get; }
        //
        // 摘要:
        //     Gets the face points in infrared space.
        public IReadOnlyDictionary<FacePointType, PointF> FacePointsInInfraredSpace { get; }
        //
        // 摘要:
        //     Gets the face points in color space.
        public IReadOnlyDictionary<FacePointType, PointF> FacePointsInColorSpace { get; }
        //
        // 摘要:
        //     Gets the face bounding box in infrared space.
        public RectI FaceBoundingBoxInInfraredSpace { get; }
        //
        // 摘要:
        //     Gets the face bounding box in color space.
        public RectI FaceBoundingBoxInColorSpace { get; }
        //
        // 摘要:
        //     Gets the quaternion representing the face rotation.
        public Vector4 FaceRotationQuaternion { get; }
        //
        // 摘要:
        //     Gets the tracking ID of the face frame result.
        public ulong TrackingId { get; }
        //
        // 摘要:
        //     Gets the face frame features.
        public FaceFrameFeatures FaceFrameFeatures { get; }
        //
        // 摘要:
        //     Gets the timestamp of the face frame result.
        public TimeSpan RelativeTime { get; }

        [HandleProcessCorruptedStateExceptions]
        protected void Dispose(bool A_0);
    }
}