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

    public sealed partial class TopZingView : Page, IPagePlaying
    {
        MusicModel Model { get; }
        MusicModel SearchModel { get; set; }
        public MusicModel ViewModel => SearchModel != null ? SearchModel : Model;
        public RunDelegate Run { get; set; }

        readonly IPlayingMusicView PlayingMusicView;

        public TopZingView(IPlayingMusicView playingView)
        {
            PlayingMusicView = playingView;
            Model = new MusicModel();
            Model.MediaPlaybackList.CurrentItemChanged += OnCurrentChanged;
            InitializeComponent();
            ViewMain.DataContext = ViewModel;
            ViewMain.Tapped += OnGridTapped;
        }

        //Hàm này dùng để back lại bài trước
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

        //Hàm này dùng để next bài kế tiếp
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


        //Hàm này dùng để gán selected index mặc định cho view. Khi mở app thì chọn luôn dòng đầu
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

        //Hàm này dùng để gán lại data source cho view
        public void SetViewSource(List<MusicInfo> source)
        {
            ViewMain.ItemsSource = source;
        }

        //Hàm này loại bỏ search, khi gõ rỗng
        public void ClearSearch()
        {
            SearchModel = null;
            SetViewSource(ViewModel.Source);
        }

        //Khởi tạo lại view khi search
        public void InitSearch(List<MusicInfo> source, MediaPlaybackList list)
        {
            SearchModel = new MusicModel(source, list);
            SetViewSource(ViewModel.Source);
            SearchModel.MediaPlaybackList.CurrentItemChanged += OnCurrentChanged;
        }

        //Load top bài hát, nhớ đổi lại link cho phù hợp mục đích của mỗi trang
        public async Task InitSource()
        {
            var response = await Http.Instance.GetAsync<ResponsiveTopZing.Rootobject>("https://mp3.zing.vn/xhr/chart-realtime?songId=0&videoId=0&albumId=0&chart=song&time=-1", "93e37968-e61c-16ac-2f6b-8b34932f8159", "no-cache", 60000);
            if (response == null) return;
            ViewModel.Load(response.data.song);
            SetViewSource(ViewModel.Source);
        }

        //hàm này có sẵn
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Run = (RunDelegate)e.Parameter;
        }

        //hàm này bắt sự kiện khi hết bài hát và chuyển bài
        private void OnCurrentChanged(MediaPlaybackList sender, CurrentMediaPlaybackItemChangedEventArgs args)
        {
            if (args.Reason == MediaPlaybackItemChangedReason.EndOfStream)
            {
                var index = ViewModel.MediaPlaybackList.Items.IndexOf(args.NewItem);
                SelectIndex(index, false, true);
            }
        }
        
        //hàm này dùng để bắt sự kiện khi người dùng chọn bài hát trên danh sách
        private void OnGridTapped(object sender, TappedRoutedEventArgs e)
        {
            if (ViewMain.SelectedItem is not MusicInfo music) return;
            PlayingMusicView.SetPlayingView(music.Thumbnail, music.Title, music.Name);
            PlayingMusicView.StartRotation();
            Run(music);
        }

        //hàm này dùng để gán bài hát được chọn.
        private async void SetSelected(object item)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                ViewMain.SelectedItem = item;
                ViewMain.ScrollIntoView(item);
            });
        }
    }
}
