using HandyControl.Data;
using LibVLCSharp.Shared;
using Ringo.Core;
using Ringo.Core.Helpers;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Ringo.Windows.Views
{
    public partial class ShellView : HandyControl.Controls.Window
    {
        /*LibVLC _libVLC;
        MediaPlayer _mediaPlayer;
        SubtitleHelper _subHelper;*/

        public ShellView()
        {
            InitializeComponent();

            /*_libVLC = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVLC);
            _subHelper = new SubtitleHelper();

            VideoView.Loaded += (sender, e) => VideoView.MediaPlayer = _mediaPlayer;*/
        }

        /*private void PlayBTN_Click(object sender, RoutedEventArgs e)
        {
            if (!VideoView.MediaPlayer.IsPlaying)
            {
                _subHelper.LoadSubtitles(new Uri("E:\\Entertainment\\Anime-Serier\\Yuru Yuri\\Yuru Yuri S1E3： ウチくる!？ ･･･いくいくっ!.srt"));
                VideoView.MediaPlayer.Play(new Media(_libVLC, new Uri("E:\\Entertainment\\Anime-Serier\\Yuru Yuri\\Yuru Yuri S1E3： ウチくる!？ ･･･いくいくっ!.mkv")));

                SubList.ItemsSource = _subHelper.SubtitleItems;
            }
        }
        
        private void PauseBTN_Click(object sender, RoutedEventArgs e)
        {
            if (VideoView.MediaPlayer.IsPlaying)
            {
                VideoView.MediaPlayer.Pause();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            VideoView.Dispose();
        }*/

        
    }
}
