#region 組件 Microsoft.Kinect.Face, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// C:\Program Files\Microsoft SDKs\Kinect\v2.0_1409\Redist\Face\x64\Microsoft.Kinect.Face.dll
#endregion

using System;
using System.ComponentModel;
using System.Runtime.ExceptionServices;

namespace Microsoft.Kinect.Face
{
    //
    // 摘要:
    //     Represents a face frame source.
    public sealed class FaceFrameSource : INotifyPropertyChanged, IDisposable
    {
        public FaceFrameSource(KinectSensor sensor, ulong initialTrackingId, FaceFrameFeatures initialFaceFrameFeatures);

        ~FaceFrameSource();

        //
        // 摘要:
        //     Gets or sets the tracking id.
        public ulong TrackingId { get; set; }
        //
        // 摘要:
        //     Gets the Kinect sensor associated with the face frame source.
        public KinectSensor KinectSensor { get; }
        //
        // 摘要:
        //     Gets a boolean that indicates if the tracking id is valid.
        public bool IsTrackingIdValid { get; }
        //
        // 摘要:
        //     Gets the current activity status of this source.
        public bool IsActive { get; }
        //
        // 摘要:
        //     Gets the flags that indicate which face frame features data are present in this
        //     face frame.
        public FaceFrameFeatures FaceFrameFeatures { get; set; }
        public MultiSourceFrame CurrentFrame { get; }

        //
        // 摘要:
        //     Occurs when the tracking ID for the face frame source is lost.
        public event EventHandler<TrackingIdLostEventArgs> TrackingIdLost;
        //
        // 摘要:
        //     Occurs when a property of the FaceFrameSource changes.
        public event PropertyChangedEventHandler PropertyChanged;

        public sealed override void Dispose();
        //
        // 摘要:
        //     Opens a new stream reader. This reader must be disposed.
        public FaceFrameReader OpenReader();
        [HandleProcessCorruptedStateExceptions]
        protected void Dispose(bool A_0);
    }
}