using System;
using System.Collections.Generic;

namespace Warframe_Helper.Data.Models;

public partial class Alert
{
    public int Id { get; set; }

    public long TgId { get; set; }

    public int ItemId { get; set; }

    public int Cost { get; set; }

    public virtual Item Item { get; set; } = null!;
}
