using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Media.Playback;

namespace MusicPlayer
{
    public interface IPagePlaying
    {
        RunDelegate Run { get; set; }
        MusicModel ViewModel { get; }
        Task InitSource();
        void SetViewSource(List<MusicInfo> source);
        void SelectIndex(int index, bool movePlaylist, bool setPlaying);
        void MoveNext();
        void MoveBack();
        void ClearSearch();
        void InitSearch(List<MusicInfo> source, MediaPlaybackList list);
    }
}
