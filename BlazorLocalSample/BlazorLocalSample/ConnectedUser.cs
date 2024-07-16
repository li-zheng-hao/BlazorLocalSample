namespace BlazorLocalSample
{
    public class ConnectedUser
    {
        public string Name { get; set; } = Guid.NewGuid().ToString();
    }
    public class ConnectedUserList
    {
        public List<ConnectedUser> Users { get; set; } = new();

        public void Add(ConnectedUser user)
        {
            Users.Add(user);
        }

        public void Remove(ConnectedUser user)
        {
            Users.Remove(user);
        }
    }
}
