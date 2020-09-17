#region 組件 Microsoft.Kinect.Face, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// C:\Program Files\Microsoft SDKs\Kinect\v2.0_1409\Redist\Face\x64\Microsoft.Kinect.Face.dll
#endregion

using System;
using System.ComponentModel;

namespace Microsoft.Kinect.Face
{
    //
    // 摘要:
    //     Represents a face frame reader.
    public sealed class FaceFrameReader : INotifyPropertyChanged, IDisposable
    {
        //
        // 摘要:
        //     Gets the face frame source.
        public FaceFrameSource FaceFrameSource { get; }
        //
        // 摘要:
        //     Gets or sets a boolean that indicates if the reader is paused.
        public bool IsPaused { get; set; }

        //
        // 摘要:
        //     Occurs when a new frame is ready.
        public event EventHandler<FaceFrameArrivedEventArgs> FrameArrived;
        //
        // 摘要:
        //     Occurs when a property of the FaceFrameReader changes.
        public event PropertyChangedEventHandler PropertyChanged;

        //
        // 摘要:
        //     Gets the most recent frame. It may return null if no frame is available.
        public FaceFrame AcquireLatestFrame();
        public sealed override void Dispose();
        protected void Dispose(bool A_0);
    }
}