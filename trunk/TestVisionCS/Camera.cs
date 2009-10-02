//*****************************************************************************************
//  File:       TouchlessMgr.cs
//  Project:    TouchlessLib
//  Author(s):  Michael Wasserman (Michael.Wasserman@microsoft.com)
//              Gary Caldwell (gacald@microsoft.com)
//              John Conwell
//
//  Defines a class representing a camera
//*****************************************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;
using WebCamLib;

namespace TouchlessLib
{
    /// <summary>
    /// Represents a camera in use by the Touchless system
    /// </summary>
    public class Camera : IDisposable
    {
        #region Public Interface

        /// <summary>
        /// Gets the DirectShow device index of the camera
        /// </summary>
        public int Index
        {
            get { return _index; }
            set { }
        }

        /// <summary>
        /// Defines the frames per second limit that is in place, -1 means no limit
        /// </summary>
        public int FpsLimit
        {
            get
            {
                return _fpslimit;
            }
            set
            {
                if (value <= 0)
                {
                    _fpslimit = -1;
                    _timeBetweenFrames = 1;
                }
                else
                {
                    _fpslimit = (value > 1000)? 1000 : value;
                    _timeBetweenFrames = (uint)(1000.0 / (double)_fpslimit);
                }
            }
        }

        /// <summary>
        /// Defines the width of the image captured
        /// </summary>
        public int CaptureWidth
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        /// Defines the height of the image captured
        /// </summary>
        public int CaptureHeight
        {
            get { return _height; }
            set { _height = value; }
        }

        /// <summary>
        /// Command for rotating and flipping incoming images
        /// </summary>
        public RotateFlipType RotateFlip
        {
            get { return _rotateFlip; }
            set
            {
                // Swap height/width when rotating by 90 or 270
                if ((int)_rotateFlip % 2 != (int)value % 2)
                {
                    int temp = _width;
                    _width = _height;
                    _height = temp;
                }
                _rotateFlip = value;
            }
        }

        /// <summary>
        /// Shows a camera specific properties dialog
        /// </summary>
        /// <param name="handle">Native window handle of the dialog parent</param>
        public void ShowPropertiesDialog(IntPtr handle)
        {
            Camera._CameraMethods.DisplayCameraPropertiesDialog(_index, handle);
        }

        /// <summary>
        /// Returns the last image acquired from the camera
        /// </summary>
        /// <returns>A bitmap of the last image acquired from the camera</returns>
        /// <example>
        /// The following is a code snipet that shows acquiring the the current image from the camera. 
        /// Selecting a camera is omitted from this sample.
        /// <code lang="cs">
        ///     TouchlessMgr touch = new TouchlessMgr();
        /// 
        ///     // Code to select the camera omitted
        ///     ...
        /// 
        ///     Bitmap b = touch.CurrentCamera.GetCurrentImage();
        /// </code>
        /// </example>
        public Bitmap GetCurrentImage()
        {
            Bitmap b = null;
            lock (_bitmapLock)
            {
                if (_bitmap == null)
                {
                    return null;
                }

                b = (Bitmap)_bitmap.Clone();
            }

            return b;
        }

        /// <summary>
        /// Event fired when an image from the camera is captured
        /// </summary>
        /// <example>
        /// The following is a code snippet that shows how to attach to the <c>OnImageCaptured</c> event to process images captured by the current camera.
        /// <code lang="cs">
        ///     TouchlessMgr touch = new TouchlessMgr();
        /// 
        ///     // Code to select the camera omitted
        ///     ...
        /// 
        ///     touch.CurrentCamera.OnImageCaptured += new EventHandler&lt;CameraEventArgs&gt;(Camera_OnImageCaptured);
        ///     
        ///     ...
        /// 
        ///     void Camera_OnImageCaptured(object sender, CameraEventArgs args)
        ///     {
        ///         Camera c = (Camera)sender;
        /// 
        ///         // Do something with args.Image
        ///     }
        /// </code>
        /// </example>
        public event EventHandler<CameraEventArgs> OnImageCaptured;

        /// <summary>
        /// Returns the camera name as the ToString implementation
        /// </summary>
        /// <returns>The name of the camera</returns>
        public override string ToString()
        {
            return _name;
        }

        #endregion Public Interface

        #region Internal Implementation

        internal Camera(int index, string name)
        {
            _index = index;
            _name = name;
        }

        internal void ImageCaptured(Bitmap bitmap)
        {
            _cameraTimer.Stop();

            try
            {
                // Always save the bitmap
                lock (_bitmapLock)
                {
                    _bitmap = bitmap;
                }

                // Only invoke the callback if it's set and we satisfy the fps limit
                if (OnImageCaptured != null && _cameraTimer.ElapsedMilliseconds >= _timeBetweenFrames)
                {
                    // The minimum _timeBetweenFrames is 1, so we can't divide by 0...
                    float cameraFps = 1000f / _cameraTimer.ElapsedMilliseconds;
                    OnImageCaptured.Invoke(this, new CameraEventArgs(bitmap, cameraFps));
                    _cameraTimer.Reset();
                }
            }
            finally
            {
                // Always start the timer
                _cameraTimer.Start();
            }
        }

        internal static CameraMethods _CameraMethods = null;

        readonly private int _index;
        readonly private string _name;
        private Bitmap _bitmap;
        private int _width = 320;
        private int _height = 240;
        private int _fpslimit = -1;
        private uint _timeBetweenFrames = 1;
        private RotateFlipType _rotateFlip = RotateFlipType.RotateNoneFlipNone;
        readonly private Stopwatch _cameraTimer = new Stopwatch();
        readonly private object _bitmapLock = new object();
        #endregion

        #region IDisposable Members

        /// <summary>
        /// Cleanup function for the camera
        /// </summary>
        public void Dispose() {}

        #endregion
    }

    /// <summary>
    /// Camera specific EventArgs that provides the Image being captured
    /// </summary>
    public class CameraEventArgs : EventArgs
    {
        /// <summary>
        /// Current Camera Image
        /// </summary>
        public Bitmap Image
        {
            get { return image; }
        }

        /// <summary>
        /// The actual frames per second delivered to the OnImageCaptured callback
        /// </summary>
        public float CameraFps
        {
            get { return cameraFps; }
        }

        #region Internal Implementation

        internal CameraEventArgs(Bitmap image, float cameraFps)
        {
            this.image = image;
            this.cameraFps = cameraFps;
        }

        private Bitmap image;
        private float cameraFps;

        #endregion
    }
}
