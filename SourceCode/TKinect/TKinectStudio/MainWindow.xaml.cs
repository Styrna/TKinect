using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Kinect;
using Microsoft.Win32;
using TKinect;
using TKinect.Data;
using Telerik.Windows.Controls;
using MahApps.Metro.Controls;
using Telerik.Windows.Controls.GridView;
using Path = System.IO.Path;

namespace TKinectStudio 
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            StyleManager.ApplicationTheme = new MetroTheme();
            InitializeComponent();
        }

        //public readonly string RecordingDir = @"C:\Users\piotrsty\Desktop\TKinectDir";
        public readonly string RecordingDir = @"C:\Users\Styrna\Desktop\PassedData";
        public readonly string TempRecordingDir = System.IO.Path.GetTempPath();
        public readonly string TempRecordingName = "KinectRecording.xed";

        public ObservableCollection<Recording> Recordings { get; set; }
        public ObservableCollection<Recording> Playlist { get; set; }
        public ObservableCollection<RecordingSuit> RecordingSuits { get; set; }

        private SynchronizationContext _guiContext; 

        //INIT
        private void WindowLoaded(object sender, EventArgs e)
        {
            _guiContext = SynchronizationContext.Current;

            var recordingDir = new DirectoryInfo(RecordingDir);

            if (!recordingDir.Exists)
                recordingDir.Create();

            Playlist = new ObservableCollection<Recording>();
            Recordings = new ObservableCollection<Recording>();
            RecordingSuits = new ObservableCollection<RecordingSuit>();

            Playlist.Add(new Recording(TempRecordingName, TempRecordingDir));

            foreach (var file in recordingDir.GetFiles())
                Recordings.Add(new Recording(file.Name,file.DirectoryName));

            RecordingsGrid.ItemsSource = Recordings;
            RecordingsGrid.MouseDoubleClick += RecordingDoubleClick;

            PlayListGrid.ItemsSource = Playlist;
            PlayListGrid.MouseDoubleClick += PlaylistDoubleClick;

            RecordingsSuitsGrid.ItemsSource = RecordingSuits;
            RecordingsSuitsGrid.MouseDoubleClick += RecordingSuitDoubleClick; 


            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    Utils.Sensor = potentialSensor;
                    break;
                }
            }

            Utils.InitKinectHost();
            //Utils.InitKinectClient();

            if (null != Utils.Sensor)
                Utils.InitKinectSensor(Utils.Sensor);

            Utils.TKinect.FrameReplayer.ReplayEnded += PlayRecording;
        }

        //CLEANUP
        private void WindowClosing(object sender, EventArgs e)
        {
            if (null != Utils.Sensor)
            {
                Utils.Sensor.Stop();
            }
        }

        private void ButtonRecordChanged(object sender, RoutedEventArgs e)
        {
            var toggle = sender as ToggleButton;
            if (toggle.IsChecked.Value)
            {
                var fileStream = new FileStream(Path.Combine(TempRecordingDir, TempRecordingName), FileMode.Create);
                Utils.TKinect.RecordStart(fileStream); 
            }
            else
            {
                var stream = Utils.TKinect.RecordStop();
                stream.Close();
                Playlist.Clear();
                Playlist.Add(new Recording(TempRecordingName,TempRecordingDir));
            }
        }
        
        private void ButtonReplay(object sender, RoutedEventArgs e)
        {
            if (!Playlist.Any())
            {
                this.ShowMessageAsync("No Recording", string.Format("Playlist empty"));
                return;
            }
            
            PlayRecording(this,"start");
        }

        private void ButtonSave(object sender, RoutedEventArgs e)
        {
            if (!Playlist.Any())
            {
                this.ShowMessageAsync("No Recording", string.Format("Playlist empty"));
                return;
            }

            if (!File.Exists(Playlist.First().GetFullPath()))
            {
                this.ShowMessageAsync("No Recording", string.Format("Recording file {0} could not be found", Playlist.First().GetFullPath()));
                return;
            }

            var fileDialog = new SaveFileDialog();
            fileDialog.InitialDirectory = RecordingDir;
            fileDialog.Filter = "TKinectRecording (*.xed)|*.xed|All Files (*.*)|*.*";
            fileDialog.FilterIndex = 1;

            bool? userClickedOk = fileDialog.ShowDialog();

            if (userClickedOk == true)
            {
                File.Copy(Path.Combine(TempRecordingDir, TempRecordingName), fileDialog.FileName);

                var fileInfo = new FileInfo(fileDialog.FileName);
                if(RecordingDir == fileInfo.DirectoryName)
                    Recordings.Add(new Recording(fileInfo.Name,fileInfo.DirectoryName));
            }
        }

        private void ButtonLoad(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = RecordingDir;
            fileDialog.Filter = "TKinectRecording (*.xed)|*.xed|All Files (*.*)|*.*";
            fileDialog.FilterIndex = 1;

            bool? userClickedOk = fileDialog.ShowDialog();

            if (userClickedOk == true)
            {
                var fileInfo = new FileInfo(fileDialog.FileName);
                Playlist.Add(new Recording(fileInfo.Name,fileInfo.DirectoryName));
            }
        }

        private void PlayRecording(object sender, string message)
        {
            Dispatcher.Invoke(() =>
                                  {
                                      if (message != "start")
                                          Playlist.Remove(Playlist.First());

                                      if (!Playlist.Any())
                                          return;

                                      var recording = Playlist.First();

                                      if (!File.Exists(recording.GetFullPath()))
                                      {
                                          this.ShowMessageAsync("No Recording", string.Format("Recording file {0} could not be found", recording.GetFullPath()));
                                          Playlist.Remove(recording);
                                          PlayRecording(this, "next");
                                      }
                                      else
                                      {
                                          var fileStream = new FileStream(recording.GetFullPath(), FileMode.Open);
                                          Utils.TKinect.ReplayStart(fileStream);
                                      }
                                  });
        }

        private void RecordingDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var originalSender = e.OriginalSource as FrameworkElement;
            if (originalSender == null) return;

            var row = originalSender.ParentOfType<GridViewRow>();
            if (row == null) return;

            var recording = row.DataContext as Recording;
            if(recording != null)
                Playlist.Add(new Recording(recording.Name, recording.Path));
        }

        private void PlaylistDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var originalSender = e.OriginalSource as FrameworkElement;
            if (originalSender == null) return;

            var row = originalSender.ParentOfType<GridViewRow>();
            if (row == null) return;

            var recording = row.DataContext as Recording;
            if (recording != null)
                Playlist.Remove(recording);
        }

        private void RecordingSuitDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var originalSender = e.OriginalSource as FrameworkElement;
            if (originalSender == null) return;

            var row = originalSender.ParentOfType<GridViewRow>();
            if (row == null) return;

            var recordingSuit = row.DataContext as RecordingSuit;
            if (recordingSuit != null)
            {
                Playlist.Clear();
                foreach(var recording in recordingSuit.Recordings)
                    Playlist.Add(new Recording(recording.Name,recording.Path));
            }
                
        }

        private void ButtonRecordingSuit(object sender, RoutedEventArgs e)
        {
            var recordingSuit = new RecordingSuit(Guid.NewGuid().ToString());

            foreach(var recording in Playlist)
                recordingSuit.Recordings.Add(recording);

            Playlist.Clear();

            RecordingSuits.Add(recordingSuit);
        }
    }
}
