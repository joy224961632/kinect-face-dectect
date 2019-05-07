using LightBuzz.Vitruvius;
using Microsoft.Kinect;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;using System.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Media.Media3D;
using System.Threading;
//using Microsoft.Kinect.Face;

namespace KinectFloor
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    { double cache;
        double text;
        private KinectSensor _sensor = null;
        private DepthFrameReader _depthReader = null;
        private BodyFrameReader _bodyReader = null;
        //color
        ColorFrameReader cfr;
        WriteableBitmap wbmp = new WriteableBitmap(512, 424, 96.0, 96.0, PixelFormats.Bgr32, null);
        BitmapSource bmpSource;

        byte[] colorData = new byte[512 * 424];
        ColorImageFormat format;
        private Body _body = null;
        private Floor _floor = null;

        /// <summary>
        /// Font size of face property text 
        /// </summary>
        private const double DrawTextFontSize = 30;
        private double T = 0;
    

        /// <summary>
        /// Text layout offset in X axis
        /// </summary>
        private const float TextLayoutOffsetX = -0.1f;

        /// <summary>
        /// Text layout offset in Y axis
        /// </summary>
        private const float TextLayoutOffsetY = -0.15f;


        /// <summary>
        /// Text layout for the no face tracked message
        /// </summary>
        private Point textLayoutFaceNotTracked = new Point(10.0, 10.0);

        /// <summary>
        /// Drawing group for body rendering output
        /// </summary>
        private DrawingGroup drawingGroup;

        /// <summary>
        /// Drawing image that we will display
        /// </summary>
        private DrawingImage imageSource;

        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensor kinectSensor = null;

        /// <summary>
        /// Coordinate mapper to map one type of point to another
        /// </summary>
        private CoordinateMapper coordinateMapper = null;

        /// <summary>
        /// Reader for body frames
        /// </summary>
        private BodyFrameReader bodyFrameReader = null;

        /// <summary>
        /// Array to store bodies
        /// </summary>
        private Body[] bodies = null;

        /// <summary>
        /// Number of bodies tracked
        /// </summary>
        private int bodyCount;

    
        private int displayWidth;

        /// <summary>
        /// Height of display (color space)
        /// </summary>
        private int displayHeight;

        /// <summary>
        /// Display rectangle
        /// </summary>
        private Rect displayRect;

        /// <summary>
        /// List of brushes for each face tracked
        /// </summary>
        private List<Brush> faceBrush;

        /// <summary>
        /// Current status text to display
        /// </summary>
        private string statusText = null;
        


        public MainWindow()
        {
            InitializeComponent();


            _sensor = KinectSensor.GetDefault();
            var fd = _sensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Bgra);
            uint frameSize = fd.BytesPerPixel * fd.LengthInPixels;
            colorData = new byte[frameSize];
            format = ColorImageFormat.Bgra;

            // one sensor is currently supported
            this.kinectSensor = KinectSensor.GetDefault();

            // get the coordinate mapper
            this.coordinateMapper = this.kinectSensor.CoordinateMapper;

            // get the color frame details
            FrameDescription frameDescription = this.kinectSensor.ColorFrameSource.FrameDescription;

            // set the display specifics
            this.displayWidth = frameDescription.Width;
            this.displayHeight = frameDescription.Height;
            this.displayRect = new Rect(0.0, 0.0, this.displayWidth, this.displayHeight);

            // open the reader for the body frames
            this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();

            // wire handler for body frame arrival
            //this.bodyFrameReader.FrameArrived += this.Reader_BodyFrameArrived;

            // set the maximum number of bodies that would be tracked by Kinect
            this.bodyCount = this.kinectSensor.BodyFrameSource.BodyCount;

            // allocate storage to store body objects
            this.bodies = new Body[this.bodyCount];
            /*
            // specify the required face frame results
            FaceFrameFeatures faceFrameFeatures =
                FaceFrameFeatures.BoundingBoxInColorSpace
                | FaceFrameFeatures.PointsInColorSpace
                | FaceFrameFeatures.RotationOrientation
                | FaceFrameFeatures.FaceEngagement
                | FaceFrameFeatures.Glasses
                | FaceFrameFeatures.Happy
                | FaceFrameFeatures.LeftEyeClosed
                | FaceFrameFeatures.RightEyeClosed
                | FaceFrameFeatures.LookingAway
                | FaceFrameFeatures.MouthMoved
                | FaceFrameFeatures.MouthOpen;
                
            // create a face frame source + reader to track each face in the FOV
            this.faceFrameSources = new FaceFrameSource[this.bodyCount];
            this.faceFrameReaders = new FaceFrameReader[this.bodyCount];
            for (int i = 0; i < this.bodyCount; i++)
            {
                // create the face frame source with the required face frame features and an initial tracking Id of 0
                this.faceFrameSources[i] = new FaceFrameSource(this.kinectSensor, 0, faceFrameFeatures);

                // open the corresponding reader
                this.faceFrameReaders[i] = this.faceFrameSources[i].OpenReader();
            }
            
            // allocate storage to store face frame results for each face in the FOV
            this.faceFrameResults = new FaceFrameResult[this.bodyCount];
            */
            // populate face result colors - one for each face index
            this.faceBrush = new List<Brush>()
            {
                Brushes.White,
                Brushes.Orange,
                Brushes.Green,
                Brushes.Red,
                Brushes.LightBlue,
                Brushes.Yellow
            };

            // set IsAvailableChanged event notifier
            this.kinectSensor.IsAvailableChanged += this.Sensor_IsAvailableChanged;

            // open the sensor
            this.kinectSensor.Open();

            // set the status text
            this.StatusText = this.kinectSensor.IsAvailable ? Properties.Resources.RunningStatusText
                                                            : Properties.Resources.NoSensorStatusText;

            // Create the drawing group we'll use for drawing
            this.drawingGroup = new DrawingGroup();

            // Create an image source that we can use in our image control
            this.imageSource = new DrawingImage(this.drawingGroup);

            // use the window object as the view model in this simple example
            this.DataContext = this;




            if (_sensor != null)
            {
                _sensor.Open();

                _depthReader = _sensor.DepthFrameSource.OpenReader();
                _depthReader.FrameArrived += DepthReader_FrameArrived;
                _bodyReader = _sensor.BodyFrameSource.OpenReader();
                _bodyReader.FrameArrived += BodyReader_FrameArrived;
                cfr = _sensor.ColorFrameSource.OpenReader();
                cfr.FrameArrived += cfr_FrameArrived;
            }



        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            if (this.kinectSensor != null)
            {
                // on failure, set the status text
                this.StatusText = this.kinectSensor.IsAvailable ? Properties.Resources.RunningStatusText
                                                                : Properties.Resources.SensorNotAvailableStatusText;
            }
        }
        public string StatusText
        {
            get
            {
                return this.statusText;
            }

            set
            {
                if (this.statusText != value)
                {
                    this.statusText = value;

                    // notify any bound elements that the text has changed
                    if (this.PropertyChanged != null)
                    {
                        this.PropertyChanged(this, new PropertyChangedEventArgs("StatusText"));
                    }
                }
            }
        }
        
    private bool GetFaceTextPositionInColorSpace(int faceIndex, out Point faceTextLayout)
        {
            faceTextLayout = new Point();
            bool isLayoutValid = false;

            Body body = this.bodies[faceIndex];
            if (body.IsTracked)
            {
                var headJoint = body.Joints[JointType.Head].Position;

                CameraSpacePoint textPoint = new CameraSpacePoint()
                {
                    X = headJoint.X + TextLayoutOffsetX,
                    Y = headJoint.Y + TextLayoutOffsetY,
                    Z = headJoint.Z
                };

                ColorSpacePoint textPointInColor = this.coordinateMapper.MapCameraPointToColorSpace(textPoint);

                faceTextLayout.X = textPointInColor.X;
                faceTextLayout.Y = textPointInColor.Y;
                isLayoutValid = true;
            }

            return isLayoutValid;
        }
        
        int a = 0;
        int b = 0;
        private void BodyReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            using (BodyFrame frame = e.FrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    _floor = frame.Floor();
                    _body = frame.Body();

                    if (_floor != null && _body != null)
                    {
                        
                        CameraSpacePoint wrist3D = _body.Joints[JointType.Head].Position;
                        Point wrist2D = wrist3D.ToPoint();
                        double distance = _floor.DistanceFrom(wrist3D);
                        text = Convert.ToDouble(distance.ToString("N2"));

                        // 調整誤差值
                        text += 0.18;
                        if (T == 0)
                        {
                            cache = text;
                        }
                        int floorY = _floor.FloorY((int)wrist2D.X, (ushort)(wrist3D.Z * 1000));
                        Cache.Text = Convert.ToString(cache);
                        TblDistance.Text = Convert.ToString(text);
                        Canvas.SetLeft(ImgHand, wrist2D.X - ImgHand.Width / 2.0);
                        Canvas.SetTop(ImgHand, wrist2D.Y - ImgHand.Height / 2.0);
                        Canvas.SetLeft(ImgFloor, wrist2D.X - ImgFloor.Width / 2.0);
                        Canvas.SetTop(ImgFloor, floorY - ImgFloor.Height / 2.0);
                        
                        if (cache > 1.6)
                        {
                            if (T == 0) {
                                a += 1;
                                T = 1;
                                distance = 0;
                            }
                        }
                        if (cache < 1.6 &&cache >1.3&& T == 0 && text !=0)
                        {
                            T += 1;
                            b += 1;
                            textBlock2.Text = Convert.ToString(b);
                        }
                        if (cache - 0.15 > text)
                        {
                            //wrist3D = ;
                            T = 0;
                            cache = 0;
                            text = 0;
                            distance = 0;
                            Thread.Sleep(2000);
                            //_body = null;
                            //_floor = null;
                        }
                        int cc = 0;
                        if(T == 1)
                        {
                            if (cc ==0)
                            {
                                textBlock1.Text = Convert.ToString(a);

                            }

                        }
                        //                    }
                    }
                }
            }
        }

        private void DepthReader_FrameArrived(object sender, DepthFrameArrivedEventArgs e)
        {
            using (DepthFrame frame = e.FrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    Camera.Source = frame.Bitmap();
                }
            }
        }

        private void cfr_FrameArrived(object sender, ColorFrameArrivedEventArgs e)

        {

            if (e.FrameReference == null) return;



            using (ColorFrame cf = e.FrameReference.AcquireFrame())

            {
                if (cf == null) return;

                cf.CopyConvertedFrameDataToArray(colorData, format);

                var fd = cf.FrameDescription;
                // Creating BitmapSource

                var bytesPerPixel = (PixelFormats.Bgr32.BitsPerPixel) / 8;

                var stride = bytesPerPixel * cf.FrameDescription.Width;



                bmpSource = BitmapSource.Create(fd.Width, fd.Height, 96.0, 96.0, PixelFormats.Bgr32, null, colorData, stride);
                // WritableBitmap to show on UI
                wbmp = new WriteableBitmap(bmpSource);
                image.Source = wbmp;
              //  image1.Source = People.png
            }

        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (_depthReader != null)
            {
                _depthReader.Dispose();
            }

            if (_bodyReader != null)
            {
                _bodyReader.Dispose();
            }

            if (_sensor != null && _sensor.IsOpen)
            {
                _sensor.Close();
            }
        }










        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.wbmp != null)

            {

                // create a png bitmap encoder which knows how to save a .png file

                BitmapEncoder encoder = new PngBitmapEncoder();



                // create frame from the writable bitmap and add to encoder

                encoder.Frames.Add(BitmapFrame.Create(this.wbmp));



                string myPhotos = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);



                string path = Path.Combine(myPhotos, "KinectScreenshot-Color-" + ".png");





                // FileStream is IDisposable

                using (FileStream fs = new FileStream(path, FileMode.Create))

                {

                    encoder.Save(fs);

                }

            }
        }



    }
}
