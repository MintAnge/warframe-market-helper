using System;
using System.Collections.Generic;

namespace Warframe_Helper.Data.Models;

public partial class ItemMonitoring
{
    public int ItemId { get; set; }

    public int BuyCost { get; set; }

    public int SellCost { get; set; }

    public virtual Item Item { get; set; } = null!;
}
