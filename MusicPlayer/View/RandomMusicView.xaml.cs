using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Media.Playback;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


namespace MusicPlayer
{
    public sealed partial class RandomMusicView : Page, IPagePlaying
    {
        MusicModel Model { get; }
        MusicModel SearchModel { get; set; }
        public MusicModel ViewModel => SearchModel != null ? SearchModel : Model;
        public RunDelegate Run { get; set; }
        readonly IPlayingMusicView PlayingMusicView;

        public RandomMusicView(IPlayingMusicView playingView)
        {
            PlayingMusicView = playingView;
            Model = new MusicModel();
            Model.MediaPlaybackList.CurrentItemChanged += OnCurrentChanged;
            InitializeComponent();
            ViewMain.DataContext = ViewModel;
            ViewMain.Tapped += OnGridTapped;
        }

        public void MoveBack()
        {
            if (ViewMain.SelectedItem is MusicInfo musicInfo)
            {
                var index = ViewModel.Source.IndexOf(musicInfo);
                if (index > 0)
                {
                    var music = ViewModel.Source[index - 1];
                    ViewMain.SelectedItem = music;
                    ViewMain.ScrollIntoView(ViewMain.SelectedItem);
                    PlayingMusicView.SetPlayingView(music.Thumbnail, music.Title, music.Name);
                }
            }
        }

        public void MoveNext()
        {
            if (ViewMain.SelectedItem is MusicInfo musicInfo)
            {
                var index = ViewModel.Source.IndexOf(musicInfo);
                if (index < ViewModel.Source.Count - 1)
                {
                    var music = ViewModel.Source[index + 1];
                    ViewMain.SelectedItem = music;
                    ViewMain.ScrollIntoView(ViewMain.SelectedItem);
                    PlayingMusicView.SetPlayingView(music.Thumbnail, music.Title, music.Name);
                }
            }
        }

        public void SelectIndex(int index, bool movePlaylist, bool setPlaying)
        {
            if (index < 0 || index >= ViewModel.Source.Count) return;
            var music = ViewModel.Source[index];
            SetSelected(music);
            if (setPlaying)
            {
                _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    PlayingMusicView.SetPlayingView(music.Thumbnail, music.Title, music.Name);
                });
            }
            if (movePlaylist) ViewModel.MediaPlaybackList.MoveTo(Convert.ToUInt32(index));
        }

        public void SetViewSource(List<MusicInfo> source)
        {
            ViewMain.ItemsSource = source;
        }

        public void ClearSearch()
        {
            SearchModel = null;
            SetViewSource(ViewModel.Source);
        }

        public void InitSearch(List<MusicInfo> source, MediaPlaybackList list)
        {
            SearchModel = new MusicModel(source, list);
            SearchModel.MediaPlaybackList.CurrentItemChanged += OnCurrentChanged;
            SetViewSource(ViewModel.Source);
        }

        public async Task InitSource()
        {
            var response = await Http.Instance.GetAsync<ResponsiveTopZing.Rootobject>("https://mp3.zing.vn/xhr/chart-realtime?songId=0&videoId=0&albumId=0&chart=song&time=-1", "93e37968-e61c-16ac-2f6b-8b34932f8159", "no-cache", 60000);
            if (response == null) return;
            ViewModel.Load(response.data.song);
            SetViewSource(ViewModel.Source);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Run = (RunDelegate)e.Parameter;
        }

        private void OnCurrentChanged(MediaPlaybackList sender, CurrentMediaPlaybackItemChangedEventArgs args)
        {
            if (args.Reason == MediaPlaybackItemChangedReason.EndOfStream)
            {
                var index = ViewModel.MediaPlaybackList.Items.IndexOf(args.NewItem);
                SelectIndex(index, false, true);
            }
        }

        private void OnGridTapped(object sender, TappedRoutedEventArgs e)
        {
            if (ViewMain.SelectedItem is not MusicInfo music) return;
            PlayingMusicView.SetPlayingView(music.Thumbnail, music.Title, music.Name);
            PlayingMusicView.StartRotation();
            Run(music);
        }

        private async void SetSelected(object item)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                ViewMain.SelectedItem = item;
                ViewMain.ScrollIntoView(item);
            });
        }
    }
}
