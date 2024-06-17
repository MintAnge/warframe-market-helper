
public class ItemResponse
{ 
    public Payload payload { get; set; }

    public class Payload
    {
        public Item item { get; set; }
    }   

}


public class Item
{
    public string id { get; set; }
    public ItemFull[] items_in_set { get; set; }
}

public class ItemFull
{
    public int trading_tax { get; set; }
    public string id { get; set; }
    public object sub_icon { get; set; }
    public string url_name { get; set; }
    public string[] tags { get; set; }
    public string rarity { get; set; }
    public string thumb { get; set; }
    public string icon { get; set; }
    public int mod_max_rank { get; set; }
    public string icon_format { get; set; }
    public ItemDescription en { get; set; }
    public ItemDescription ru { get; set; }
    public ItemDescription ko { get; set; }
    public ItemDescription fr { get; set; }
    public ItemDescription sv { get; set; }
    public ItemDescription de { get; set; }
    public ItemDescription zhhant { get; set; }
    public ItemDescription zhhans { get; set; }
    public ItemDescription pt { get; set; }
    public ItemDescription es { get; set; }
    public ItemDescription pl { get; set; }
    public ItemDescription cs { get; set; }
    public ItemDescription uk { get; set; }
}

public class ItemDescription
{
    public string item_name { get; set; }
    public string description { get; set; }
    public string wiki_link { get; set; }
    public string thumb { get; set; }
    public string icon { get; set; }
    public object[] drop { get; set; }
}
