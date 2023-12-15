using System;
using System.Collections.Generic;

namespace NoteAPPLibrary;

public partial class Note
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Data { get; set; } = null!;

    public DateTime Createddate { get; set; }

    public DateTime Lastmodifieddate { get; set; }

    public bool Trashstatus { get; set; }
    public string UserName { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Trash> Trashes { get; set; } = new List<Trash>();
}
