namespace NoteTakingAppAPI.DTOs
{
    public class Notes
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Data { get; set; } = null!;

        public DateTime Createddate { get; set; }

        public DateTime Lastmodifieddate { get; set; }

        public bool Trashstatus { get; set; }
        public string UserName { get; set; } = null!;
        public DateTime? Deleteddate { get; set; }
    }
}
