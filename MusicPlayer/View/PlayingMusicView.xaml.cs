using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MusicPlayer
{
    public sealed partial class PlayingMusicView : Page, IPlayingMusicView
    {
        bool running = true;
        double rotation = 0d;
        public PlayingMusicView()
        {
            InitializeComponent();
        }

        public async void StartRotation()
        {
            running = true;
            while (running)
            {
                if (rotation == 360) rotation = 0;
                rotationTransform.Angle = rotation;
                await Task.Delay(50);
                rotation += 0.2;
            }
        }

        public void StopRotation()
        {
            running = false;
        }

        public void SetImage(ImageSource value)
        {
            imgThum.Source = value;
        }

        public void SetTitle(string value)
        {
            txtName.Text = value;
        }

        public void SetSubTitle(string value)
        {
            txtNameCasi.Text = value;
        }

        public async void SetPlayingView(string source, string title, string subtitle)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.High, () => {
                SetImage(new BitmapImage(new Uri(source)));
                SetTitle(title);
                SetSubTitle(subtitle);
            });
        }
    }
}
