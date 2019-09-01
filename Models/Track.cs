namespace SimpleBeep.Models
{
    public class Track : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Playlist Playlist { get; set; }
    }
}