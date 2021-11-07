namespace SocketChat.Domain.Providers
{
    public interface IAccessContextProvider
    {
        AccessContext Get();
    }

    public class AccessContext
    {
        public int UserId { get; private set; }
        public string UserEmail { get; private set; }

        public AccessContext(int userId, string userEmail)
        {
            UserId = userId;
            UserEmail = userEmail;
        }
    }
}