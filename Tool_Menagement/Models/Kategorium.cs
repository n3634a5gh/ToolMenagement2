using System;
using System.Collections.Generic;

namespace Tool_Menagement.Models;

public partial class Kategorium
{
    public int IdKategorii { get; set; }

    public string Opis { get; set; } = null!;

    public int ToolPolicy { get; set; }

    public virtual ICollection<KategoriaDetail> KategoriaDetails { get; set; } = new List<KategoriaDetail>();

    public virtual ICollection<Narzedzie> Narzedzies { get; set; } = new List<Narzedzie>();
}
