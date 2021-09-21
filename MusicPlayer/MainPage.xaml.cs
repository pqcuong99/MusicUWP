using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace MusicPlayer
{
    public sealed partial class MainPage : Page
    {
        public RunDelegate Run { get; set; }

        //2 trang demo
        IPagePlaying TopZingView;
        IPagePlaying RandomMusicView;
        IPlayingMusicView PlayingMusicView;

        //Lưu lại các giá trị hiện tại
        readonly MediaPlayer CurrentMediaPlayer;
        MediaPlaybackList CurrentPlayback;

        bool isTopZingView => frNext.Content is TopZingView || (TopZingView != null && TopZingView.ViewModel.MediaPlaybackList == CurrentPlayback);
        bool isRandomView => frNext.Content is RandomMusicView || (RandomMusicView != null && RandomMusicView.ViewModel.MediaPlaybackList == CurrentPlayback);

        public MainPage()
        {
            InitializeComponent();
            CurrentMediaPlayer = new MediaPlayer();
            PlayingMusicView = new PlayingMusicView();
            NavigationCacheMode = NavigationCacheMode.Disabled;

            CurrentMediaPlayer.CommandManager.IsEnabled = true;
            CurrentMediaPlayer.CommandManager.NextReceived += OnNext;
            CurrentMediaPlayer.CommandManager.PreviousReceived += OnBack;
            CurrentMediaPlayer.CommandManager.PauseReceived += OnPause;
            CurrentMediaPlayer.CommandManager.PlayReceived += OnPlay;
            musicControl.SetMediaPlayer(CurrentMediaPlayer);
            NavViewSearchBox.QuerySubmitted += OnSearch;
            SetAccess();
            Loading += OnLoading;
        }

        //Khi load page thì tải top view
        private void OnLoading(FrameworkElement sender, object args)
        {
            NavView.IsPaneOpen = false;
            CurrentMediaPlayer.Pause();
            TopZingView ??= new TopZingView(PlayingMusicView);
            frNext.Content = TopZingView;
            InitPlayingPage(TopZingView);
            SetAccess();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            NavView.ItemInvoked += OnNavigation;
            Run = OpenPlayingView;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        //Chọn bài để nghe
        private void OpenPlayingView(object musicInfo)
        {
            CurrentMediaPlayer.Pause();
            if (musicInfo is MusicInfo info)
            {
                if (isTopZingView)
                    TopZingView.ViewModel.MediaPlaybackList.MoveTo(Convert.ToUInt32(TopZingView.ViewModel.Source.IndexOf(info)));
                if (isRandomView)
                    RandomMusicView.ViewModel.MediaPlaybackList.MoveTo(Convert.ToUInt32(RandomMusicView.ViewModel.Source.IndexOf(info)));
                CurrentMediaPlayer.Play();
                frNext.Content = PlayingMusicView;
            }
        }

        //Tìm kiếm
        private void OnSearch(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            CurrentPlayback = new MediaPlaybackList();
            if (isTopZingView) SetPlayListWhenSearch(TopZingView, args.QueryText.ToLower());
            else if (isRandomView) SetPlayListWhenSearch(RandomMusicView, args.QueryText.ToLower());
        }

        //Back
        private void OnBack(MediaPlaybackCommandManager sender, MediaPlaybackCommandManagerPreviousReceivedEventArgs args)
        {
            if (isTopZingView) TopZingView.MoveBack();
            else if (isRandomView) RandomMusicView.MoveBack();
        }

        //Next
        private void OnNext(MediaPlaybackCommandManager sender, MediaPlaybackCommandManagerNextReceivedEventArgs args)
        {
            if (isTopZingView) TopZingView.MoveNext();
            else if (isRandomView) RandomMusicView.MoveNext();
        }

        void OnPlay(MediaPlaybackCommandManager sender, MediaPlaybackCommandManagerPlayReceivedEventArgs args)
        {
            PlayingMusicView.StartRotation();
        }

        void OnPause(MediaPlaybackCommandManager sender, MediaPlaybackCommandManagerPauseReceivedEventArgs args)
        {
            PlayingMusicView.StopRotation();
        }

        //Handle khi chọn menu
        private void OnNavigation(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var beforeView = frNext.Content;
            if (args.InvokedItem.ToString().Equals("Top Zing"))
            {
                if (beforeView == TopZingView)
                    return;

                if (TopZingView != null && CurrentPlayback == TopZingView.ViewModel.MediaPlaybackList)
                {
                    frNext.Content = TopZingView;
                    return;
                }

                CurrentMediaPlayer.Pause();
                TopZingView ??= new TopZingView(PlayingMusicView);
                frNext.Content = TopZingView;
                InitPlayingPage(TopZingView);
                SetAccess();
            }
            else if (args.InvokedItem.ToString().Equals("Random music"))
            {
                if (beforeView == RandomMusicView)
                    return;

                if (RandomMusicView != null && CurrentPlayback == RandomMusicView.ViewModel.MediaPlaybackList)
                {
                    frNext.Content = RandomMusicView;
                    return;
                }

                CurrentMediaPlayer.Pause();
                RandomMusicView ??= new RandomMusicView(PlayingMusicView);
                frNext.Content = RandomMusicView;
                InitPlayingPage(RandomMusicView);
                SetAccess();
            }
            else if (args.InvokedItem.ToString().Equals("Playing music"))
            {
                if (frNext.Content == TopZingView)
                {
                    var index = TopZingView.ViewModel.MediaPlaybackList.Items.IndexOf(TopZingView.ViewModel.MediaPlaybackList.CurrentItem);
                    if (index >= 0 && index < TopZingView.ViewModel.MediaPlaybackList.Items.Count)
                    {
                        var current = TopZingView.ViewModel.Source[index];
                        PlayingMusicView.SetPlayingView(current.Thumbnail, current.Title, current.Name);
                    }
                }
                frNext.Content = PlayingMusicView;
            }
        }


        //Khởi tạo các giá trị của trang
        async void InitPlayingPage(IPagePlaying page)
        {
            page.Run = OpenPlayingView;
            await page.InitSource();
            page.SelectIndex(0, false, true);
            page.ClearSearch();
            CurrentMediaPlayer.Source = CurrentPlayback = page.ViewModel.MediaPlaybackList;
        }


        //Gán giá trị cho trang sau khi search
        void SetPlayListWhenSearch(IPagePlaying pagePlaying, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                pagePlaying.ClearSearch();
                pagePlaying.SelectIndex(0, false, true);
                CurrentMediaPlayer.Source = CurrentPlayback = pagePlaying.ViewModel.MediaPlaybackList;
                return;
            }

            var likeSource = pagePlaying.ViewModel.Source.Where(x => x.Name.ToLower().Contains(text) || x.Title.ToLower().Contains(text));
            if (likeSource.Count() == 0)
            {
                Utility.ShowToast("Không tìm thấy kết quả");
                return;
            }

            CurrentPlayback = new MediaPlaybackList();
            foreach (var music in likeSource)
            {
                CurrentPlayback.Items.Add(new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri($"http://api.mp3.zing.vn/api/streaming/audio.co/{music.Id}/320"))));
            }
            pagePlaying.InitSearch(new List<MusicInfo>(likeSource), CurrentPlayback);
            pagePlaying.SelectIndex(0, false, true);
            CurrentMediaPlayer.Source = pagePlaying.ViewModel.MediaPlaybackList;
            NavView.IsPaneOpen = false;
        }

        //Tiện ích để set enable thôi chứ không có gì cả.
        void SetAccess()
        {
            CurrentMediaPlayer.SystemMediaTransportControls.IsPlayEnabled = true;
            CurrentMediaPlayer.SystemMediaTransportControls.IsPauseEnabled = true;
            CurrentMediaPlayer.SystemMediaTransportControls.IsNextEnabled = true;
            CurrentMediaPlayer.SystemMediaTransportControls.IsPreviousEnabled = true;
            CurrentMediaPlayer.SystemMediaTransportControls.IsRewindEnabled = true;
        }

    }
}
