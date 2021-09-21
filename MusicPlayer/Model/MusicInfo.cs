using Windows.Media.Playback;
using Windows.UI.Xaml;

namespace MusicPlayer
{
    public class MusicInfo : DependencyObject
    {
        public static readonly DependencyProperty IDProperty = DependencyProperty.Register("ID", typeof(string), typeof(MusicInfo), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(string), typeof(MusicInfo), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(MusicInfo), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty CodeProperty = DependencyProperty.Register("Code", typeof(string), typeof(MusicInfo), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty PlaylistIDProperty = DependencyProperty.Register("PlaylistID", typeof(string), typeof(MusicInfo), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty ArtistsNamesProperty = DependencyProperty.Register("ArtistsNames", typeof(string), typeof(MusicInfo), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty PerformerProperty = DependencyProperty.Register("Performer", typeof(string), typeof(MusicInfo), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register("Type", typeof(string), typeof(MusicInfo), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty LinkProperty = DependencyProperty.Register("Link", typeof(string), typeof(MusicInfo), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty LyricProperty = DependencyProperty.Register("Lyric", typeof(string), typeof(MusicInfo), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty ThumbnailProperty = DependencyProperty.Register("Thumbnail", typeof(string), typeof(MusicInfo), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register("Duration", typeof(int), typeof(MusicInfo), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty ThumbnailRankProperty = DependencyProperty.Register("ThumbnailRank", typeof(string), typeof(MusicInfo), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(string), typeof(MusicInfo), new PropertyMetadata(-1));
        //public static readonly DependencyProperty PlayBackProperty = DependencyProperty.Register("PlayBack", typeof(MediaPlaybackItem), typeof(MusicInfo), new PropertyMetadata(null));


        public string Id { get => GetValue(IDProperty) as string; set => SetValue(IDProperty, value); }
        public string Name { get => GetValue(NameProperty) as string; set => SetValue(NameProperty, value); }
        public string Title { get => GetValue(TitleProperty) as string; set => SetValue(TitleProperty, value); }
        public string Code { get => GetValue(CodeProperty) as string; set => SetValue(CodeProperty, value); }
        public string PlaylistID { get => GetValue(PlaylistIDProperty) as string; set => SetValue(PlaylistIDProperty, value); }
        public string ArtistsNames { get => GetValue(ArtistsNamesProperty) as string; set => SetValue(ArtistsNamesProperty, value); }
        public string Performer { get => GetValue(PerformerProperty) as string; set => SetValue(PerformerProperty, value); }
        public string Type { get => GetValue(TypeProperty) as string; set => SetValue(TypeProperty, value); }
        public string Link { get => GetValue(LinkProperty) as string; set => SetValue(LinkProperty, value); }
        public string Lyric { get => GetValue(LyricProperty) as string; set => SetValue(LyricProperty, value); }
        public string Thumbnail { get => GetValue(ThumbnailProperty) as string; set => SetValue(ThumbnailProperty, value); }
        public string Duration { get => GetValue(DurationProperty) as string; set => SetValue(DurationProperty, value); }
        public string ThumbnailRank { get => GetValue(ThumbnailRankProperty) as string; set => SetValue(ThumbnailRankProperty, value); }
        public int Position { get => (int)GetValue(PositionProperty); set => SetValue(PositionProperty, value); }
        //public MediaPlaybackItem PlayBack { get => (MediaPlaybackItem)GetValue(PlayBackProperty); set => SetValue(PlayBackProperty, value); }
    }
}
