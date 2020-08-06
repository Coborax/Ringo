using Caliburn.Micro;
using LibVLCSharp.Shared;
using LibVLCSharp.WPF;
using Microsoft.Win32;
using Ringo.Helpers;
using Ringo.Models;
using SubtitlesParser.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Ringo.ViewModels
{
    public class ShellViewModel : Caliburn.Micro.Screen
    {
        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;
        private SubtitleHelper _subHelper;

        private IWindowManager _windowManager;
        private AboutViewModel _aboutVM;

        private ObservableCollection<Subtitle> _subtitleItems;
        private Subtitle _selectedSub;

        private int _seekerMax = 0;
        private int _seekerVal = 0;

        //private bool _seeking = false;
        private bool _internalSubChange = false;

        public ShellViewModel(SubtitleHelper subHelper, IWindowManager windowManager, AboutViewModel aboutVM)
        {
            LibVLCSharp.Shared.Core.Initialize();
            _libVLC = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVLC);
            _mediaPlayer.TimeChanged += MediaPlayerTimeChanged;
            _mediaPlayer.LengthChanged += MediaPlayerLengthChanged;

            _windowManager = windowManager;
            _aboutVM = aboutVM;

            _subHelper = subHelper;
        }

        private void MediaPlayerLengthChanged(object sender, MediaPlayerLengthChangedEventArgs e)
        {
            SeekerMax = (int)e.Length;
        }

        private void MediaPlayerTimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            SeekerVal = (int)e.Time;

            Subtitle sub = _subHelper.GetSubAtTime(SeekerVal);
            if (sub != null)
            {
                _internalSubChange = true;
                SelectedSub = sub;
            }
        }

        public ObservableCollection<Subtitle> SubtitleItems
        {
            get { return _subtitleItems; }
            set 
            {
                _subtitleItems = value;
                NotifyOfPropertyChange(() => SubtitleItems);
            }
        }

        public Subtitle SelectedSub
        {
            get { return _selectedSub; }
            set
            {
                _selectedSub = value;
                NotifyOfPropertyChange(() => SelectedSub);

                if (!_internalSubChange)
                    ThreadPool.QueueUserWorkItem(_ => _mediaPlayer.Time = _selectedSub.StartTime);

                _internalSubChange = false;

            }
        }

        public int SeekerMax
        {
            get { return _seekerMax; }
            set
            {
                _seekerMax = value;
                NotifyOfPropertyChange(() => SeekerMax);
            }
        }

        public int SeekerVal
        {
            get { return _seekerVal; }
            set
            {
                _seekerVal = value;
                NotifyOfPropertyChange(() => SeekerVal);
            }
        }

        public void About()
        {
            _windowManager.ShowDialog(_aboutVM);
        }

        public void PlayBTN()
        {
            if (!_mediaPlayer.IsPlaying)
                _mediaPlayer.Play();
        }

        public void PauseBTN()
        {
            if (_mediaPlayer.IsPlaying)
                _mediaPlayer.Pause();
        }


        public void VideoViewLoaded(VideoView videoView)
        {
            videoView.MediaPlayer = _mediaPlayer;
        }

        public void OpenVideo()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == true)
            {
                Uri fileUri = new Uri(dialog.FileName);
                _mediaPlayer.Media = new Media(_libVLC, fileUri);

                
                //Check for .srt subs
                string subPath = Path.ChangeExtension(fileUri.LocalPath, ".srt");
                if (File.Exists(subPath))
                    _subHelper.LoadSubtitles(subPath);

                //Check for .ass subs
                subPath = Path.ChangeExtension(fileUri.LocalPath, ".ass");
                if (File.Exists(subPath))
                    _subHelper.LoadSubtitles(subPath);

                _mediaPlayer.Play();
                SubtitleItems = new ObservableCollection<Subtitle>(_subHelper.Subtitles);
            }
        }

        public void CopySub(Subtitle sub)
        {
            Clipboard.SetText(sub.Line);
        }

        public void SubChanged(ListBox list)
        {
            list.ScrollIntoView(list.SelectedItem);
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            _mediaPlayer.Dispose();
        }
    }
}
