using System;
using System.Collections.Generic;

namespace Tool_Menagement.Models;

public partial class Rejestracja
{
    public int IdPozycji { get; set; }

    public int IdZlecenia { get; set; }

    public int? Sztuk { get; set; }

    public DateOnly? DataWykonania { get; set; }

    public string? Wykonal { get; set; }

    public virtual Zlecenie IdZleceniaNavigation { get; set; } = null!;
}
