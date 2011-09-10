namespace Nancy.Authentication.OAuth
{
    using System.Collections.Generic;

    public interface IAuthorizationCodeRequestScopeValidator
    {
        bool Validate(IEnumerable<string> scopes);
    }

    public class DefaultAuthorizationCodeRequestScopeValidator : IAuthorizationCodeRequestScopeValidator
    {
        public bool Validate(IEnumerable<string> scopes)
        {
            return true;
        }
    }
}