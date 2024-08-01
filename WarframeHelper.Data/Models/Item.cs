using System;
using System.Collections.Generic;

namespace MintAnge.WarframeHelper.Data.Models;

public partial class Item
{
    public int Id { get; set; }

    public string UrlName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Alert> Alerts { get; set; } = new List<Alert>();

    public virtual ItemMonitoring? ItemMonitoring { get; set; }
}
