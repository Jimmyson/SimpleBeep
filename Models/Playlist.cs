using System.Collections.Generic;

namespace SimpleBeep.Models
{
    public class Playlist : BaseEntity
    {
        public string Name { get; set; }
        public string Composer { get; set; }
        public List<Track> Tracks { get; set; }
    }
}