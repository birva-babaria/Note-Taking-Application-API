using System;
using System.Collections.Generic;

namespace NoteAPPLibrary;

public partial class Trash
{
    public int Id { get; set; }

    public int NoteId { get; set; }

    public DateTime Deleteddate { get; set; }

    public virtual Note Note { get; set; } = null!;
}
