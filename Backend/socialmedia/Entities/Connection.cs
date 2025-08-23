namespace socialmedia.Entities
{
    public class Connection
    {
        public Connection() { }
        public Connection(string username, string connectionId)
        {
            Username = username;
            this.connectionId = connectionId;
        }
        public string connectionId { get; set; }
       public string Username { get; set; } 

    }
}
