
public class OrdersResponse
{
    public Payload payload { get; set; }

    public class Payload
    {
        public Order[] orders { get; set; }
    }
}

public class Order
{
    public string order_type { get; set; }
    public int quantity { get; set; }
    public int platinum { get; set; }
    public User user { get; set; }
    public string platform { get; set; }
    public DateTime creation_date { get; set; }
    public DateTime last_update { get; set; }
    public bool visible { get; set; }
    public string id { get; set; }
    public int mod_rank { get; set; }
    public string region { get; set; }

    public string ToString()
    {
        string s = $"Platinum: {platinum} \n User name: {user.ingame_name} \n User status: {user.status}";
        return s;
    }
}

public class User
{
    public int reputation { get; set; }
    public string locale { get; set; }
    public string avatar { get; set; }
    public DateTime last_seen { get; set; }
    public string ingame_name { get; set; }
    public string id { get; set; }
    public string region { get; set; }
    public string status { get; set; }
}



