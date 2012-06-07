namespace Nancy.Demo.OAuth
{
    using Nancy.OAuth;
    using Security;

    public class OAuthLogin : IOAuthLogin
    {
        public IUserIdentity GetUser(string token)
        {
            return new DemoUserIdentity { UserName = "admin " };
        }
    }
}