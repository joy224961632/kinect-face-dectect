#region 組件 Microsoft.Kinect.Face, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// C:\Program Files\Microsoft SDKs\Kinect\v2.0_1409\Redist\Face\x64\Microsoft.Kinect.Face.dll
#endregion

using System;

namespace Microsoft.Kinect.Face
{
    //
    // 摘要:
    //     Flags that indicate the face frame features that are present.
    [Flags]
    public enum FaceFrameFeatures
    {
        //
        // 摘要:
        //     No face frame features will be computed
        None = 0,
        //
        // 摘要:
        //     The bounding box will be computed in infrared space
        BoundingBoxInInfraredSpace = 1,
        //
        // 摘要:
        //     The face alignment points will be computed in infrared space
        PointsInInfraredSpace = 2,
        //
        // 摘要:
        //     The bounding box will be computed in color space
        BoundingBoxInColorSpace = 4,
        //
        // 摘要:
        //     The face alignment points will be computed in color space
        PointsInColorSpace = 8,
        //
        // 摘要:
        //     The face rotation and orientation will be computed
        RotationOrientation = 16,
        //
        // 摘要:
        //     Enable deteciotn of the user's happy facial expression
        Happy = 32,
        //
        // 摘要:
        //     Enable detection of whether or not the user's right eye is closed
        RightEyeClosed = 64,
        //
        // 摘要:
        //     Enable detection of whether or not the user's left eye is closed
        LeftEyeClosed = 128,
        //
        // 摘要:
        //     Enable detection of whether or not the user's mouth is open
        MouthOpen = 256,
        //
        // 摘要:
        //     Enable detection of whether or not the user's mouth has moved since the previous
        //     frame
        MouthMoved = 512,
        //
        // 摘要:
        //     Enable detection of whether or not the user is looking at the sensor
        LookingAway = 1024,
        //
        // 摘要:
        //     Enable detection of whether or not the user is wearing glasses
        Glasses = 2048,
        //
        // 摘要:
        //     Enable detection of whether or not the user is engaged with the content they
        //     are viewing
        FaceEngagement = 4096
    }
}