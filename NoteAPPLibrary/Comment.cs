using System;
using System.Collections.Generic;

namespace NoteAPPLibrary;

public partial class Comment
{
    public int Id { get; set; }

    public int NoteId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime Posteddate { get; set; }

    public virtual Note Note { get; set; } = null!;
    public string UserName { get; set; } = null!;
}
