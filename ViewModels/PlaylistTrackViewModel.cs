using System.Collections.Generic;

namespace SimpleBeep.ViewModels
{
    public class PlaylistTrackViewModel : PlaylistViewModel
    {
        public IList<TrackViewModel> Tracks { get; set; }
    }
}