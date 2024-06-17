public class AllItemsResponse
{
    public Payload payload { get; set; }

    public class Payload
    {
        public ItemShort[] items { get; set; }
    }
}



public class ItemShort
{
    public string id { get; set; }
    public string thumb { get; set; }
    public string item_name { get; set; }
    public string url_name { get; set; }
    public bool vaulted { get; set; }
}
