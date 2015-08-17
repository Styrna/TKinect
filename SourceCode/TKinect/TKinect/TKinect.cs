using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Kinect;
using TKinect.Data;
using TKinect.Data.ColorData;
using TKinect.Data.DepthData;
using TKinect.Data.SkeletonData;
using TKinect.FrameManagement;

namespace TKinect
{
    public class TKinect
    {
        //Out events
        public event EventHandler<TColorFrame> ColorFrameReady;
        public event EventHandler<TDepthFrame> DepthFrameReady;
        public event EventHandler<TSkeletonFrame> SkeletonFrameReady;

        public event EventHandler<string> ReplayEnded;

        //In handlers
        public void SensorColorFrameHandler(object sender, ColorImageFrameReadyEventArgs e)
        {
            if (_isPlaying) return;
            if (!ColorFrameActive) return;

            var sensorFrame = e.OpenColorImageFrame();
            if (sensorFrame != null)
            {
                var frame = new TColorFrame(sensorFrame);
                ColorFrameReady(this, frame);
            }
        }
        public void TColorFrameHandler(object senderm, TColorFrame frame)
        {
            if (!ColorFrameActive) return;
            ColorFrameReady(this, frame);
        }
        public void SensorDepthFrameHandler(object sender, DepthImageFrameReadyEventArgs e)
        {
            if (_isPlaying) return;
            if (!DepthFrameActive) return;

            var sensorFrame = e.OpenDepthImageFrame();
            if (sensorFrame != null)
            {
                var frame = new TDepthFrame(sensorFrame);
                DepthFrameReady(this, frame);
            }
        }
        public void TDepthFrameHandler(object senderm, TDepthFrame frame)
        {
            if (!DepthFrameActive) return;
            DepthFrameReady(this, frame);
        }
        public void SensorSkeletonFrameHandler(object sender, SkeletonFrameReadyEventArgs e)
        {
            if (_isPlaying) return;
            if (!SkeletonFrameActive) return;

            var sensorFrame = e.OpenSkeletonFrame();
            if (sensorFrame != null)
            {
                var frame = new TSkeletonFrame(sensorFrame);
                SkeletonFrameReady(this, frame);
            }
        }
        public void TSkeletonFrameHandler(object senderm, TSkeletonFrame frame)
        {
            if (!SkeletonFrameActive) return;
            SkeletonFrameReady(this, frame);
        }

        //On/Off frames
        public bool ColorFrameActive;
        public bool DepthFrameActive;
        public bool SkeletonFrameActive;

        //On/Off replay
        private bool _isPlaying;

        public TKinect()
        {
            _isPlaying = false;
            ColorFrameActive = true;
            DepthFrameActive = true;
            SkeletonFrameActive = true;

            InitializeHosts();
            InitializeClients();

            InitializePlayers();
            
            InitializeRecorders();
            InitializeReplayer();
        }

        #region Hosts

        //Host
        public FrameHost<TColorFrame> ColorFrameHost;
        public FrameHost<TDepthFrame> DepthFrameHost;
        public FrameHost<TSkeletonFrame> SkeletonFrameHost;

        public void InitializeHosts()
        {
            //InitializeHosts
            ColorFrameHost = new FrameHost<TColorFrame>();
            DepthFrameHost = new FrameHost<TDepthFrame>();
            SkeletonFrameHost = new FrameHost<TSkeletonFrame>();

            ColorFrameReady += ColorFrameHost.SendFrame;
            DepthFrameReady += DepthFrameHost.SendFrame;
            SkeletonFrameReady += SkeletonFrameHost.SendFrame;
        }

        #endregion

        #region Clinets

        //Client
        public FrameClient<TColorFrame> ColorFrameClient;
        public FrameClient<TDepthFrame> DepthFrameClient;
        public FrameClient<TSkeletonFrame> SkeletonFrameClient;

        public void InitializeClients()
        {
            //InitializeClients
            ColorFrameClient = new FrameClient<TColorFrame>();
            DepthFrameClient = new FrameClient<TDepthFrame>();
            SkeletonFrameClient = new FrameClient<TSkeletonFrame>();

            ColorFrameClient.FrameReady += TColorFrameHandler;
            DepthFrameClient.FrameReady += TDepthFrameHandler;
            SkeletonFrameClient.FrameReady += TSkeletonFrameHandler;
        }

        #endregion

        #region Players

        //Player
        public FramePlayer<TColorFrame> ColorPlayer;
        public FramePlayer<TDepthFrame> DepthPlayer;
        public FramePlayer<TSkeletonFrame> SkeletonPlayer;


        public void InitializePlayers()
        {
            ColorPlayer = new FramePlayer<TColorFrame>();
            DepthPlayer = new FramePlayer<TDepthFrame>();
            SkeletonPlayer = new FramePlayer<TSkeletonFrame>();

            ColorPlayer.FrameReady += TColorFrameHandler;
            DepthPlayer.FrameReady += TDepthFrameHandler;
            SkeletonPlayer.FrameReady += TSkeletonFrameHandler;
        }

        #endregion

        #region Recorders

        //Recorders
        public FrameRecorder FrameRecorder;

        public void InitializeRecorders()
        {
            FrameRecorder = new FrameRecorder();

            ColorFrameReady += FrameRecorder.RecordHandler;
            DepthFrameReady += FrameRecorder.RecordHandler;
            SkeletonFrameReady += FrameRecorder.RecordHandler;
        }

        #endregion

        #region Replays
        public FrameReplayer FrameReplayer;

        public void InitializeReplayer()
        {
            FrameReplayer = new FrameReplayer();

            FrameReplayer.ColorFrameReady += TColorFrameHandler;
            FrameReplayer.DepthFrameReady += TDepthFrameHandler;
            FrameReplayer.SkeletonFrameReady += TSkeletonFrameHandler;

            FrameReplayer.ReplayEnded += ReplayEnded;
        }

        #endregion

        #region Controll

        public void RecordStart(Stream stream)
        {
            FrameRecorder.Start(stream);
        }

        public Stream RecordStop()
        {
            return FrameRecorder.Stop();
        }

        public void ReplayStart(Stream stream)
        {
            _isPlaying = true;

            FrameReplayer.Play(stream);
        }

        public void ReplayStop()
        {
            _isPlaying = false;

        }

        public void ReplaySave(Stream stream)
        {
            
        }

        public void ReplayLoad(Stream stream)
        {
            
        }

        #endregion

    }
}
