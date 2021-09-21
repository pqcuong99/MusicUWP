using Windows.UI.Core;
using Windows.UI.Xaml.Media;

namespace MusicPlayer
{
    public interface IPlayingMusicView
    {
        void SetImage(ImageSource value);
        void SetTitle(string value);
        void SetSubTitle(string value);
        void StartRotation();
        void StopRotation();
        void SetPlayingView(string source, string title, string subtitle);
        CoreDispatcher Dispatcher { get; }
    }
}
