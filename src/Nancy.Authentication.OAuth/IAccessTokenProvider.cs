namespace Nancy.Authentication.OAuth
{
    public interface IAccessTokenGenerator
    {
        string Generate();
    }
}