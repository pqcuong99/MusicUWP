using System;
using System.Collections.Generic;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml;

namespace MusicPlayer
{
    public class MusicModel : DependencyObject
    {
        public List<MusicInfo> Source { get; }
        public MediaPlaybackList MediaPlaybackList { get; }

        public MusicModel()
        {
            Source = new List<MusicInfo>();
            MediaPlaybackList = new MediaPlaybackList();
        }

        public MusicModel(List<MusicInfo> source, MediaPlaybackList list)
        {
            Source = source;
            MediaPlaybackList = list;
        }

        public void Load(ResponsiveTopZing.Song[] songs)
        {
            foreach (var song in songs)
            {
                MusicInfo music = new();
                music.Position = song.position;
                music.Duration = Utility.IntToSecond(song.duration);
                music.Id = song.id;
                music.Thumbnail = song.thumbnail.Replace("w94", "w512");
                music.Performer = song.performer;
                music.Title = song.name;
                music.Name = song.artists_names;
                Source.Add(music);
                MediaPlaybackList.Items.Add(new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri($"http://api.mp3.zing.vn/api/streaming/audio.co/{song.id}/320"))));
            }
        }
    }
}
