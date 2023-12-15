using NoteAPPLibrary;

namespace NoteTakingAppAPI.DTOs
{
    public class Comments
    {
        public int Id { get; set; }

        public int NoteId { get; set; }

        public string Content { get; set; } = null!;

        public DateTime Posteddate { get; set; }

        public string UserName { get; set; } = null!;
    }
}
