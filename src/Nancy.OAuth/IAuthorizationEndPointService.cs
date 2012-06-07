namespace Nancy.OAuth
{
    using System;

    /// <summary>
    /// This interface might be split up into smaller pieces later on, but for spiking purposes it's all left in
    /// this single definition.
    /// </summary>
    public interface IAuthorizationEndPointService
    {
        /// <summary>
        /// Generate a token. Should this be responsible for storing it as well? I am kinda tempted to say that it
        /// should be responsible for storing it as well, to make the code generation automic. However, what happens
        /// if an error happens, after the code's been generated (and stored)? Would leave a dead code in the
        /// storage.
        /// 
        /// However, the specification states "The authorization code MUST expire shortly after it is issued to 
        /// mitigate the risk of leaks.  A maximum authorization code lifetime of 10 minutes is RECOMMENDED." so
        /// there should probably be a TTL configuration passed in here to. It would then be up to the storage
        /// mechanism to make sure the code is made invalid/cleaned up once the TTL expired. The strategy would
        /// probably vary depending on the implementation, so imposing a strict schema for doing this might not
        /// be a wise thing to do.
        /// </summary>
        string GenerateAuthorizationToken(NancyContext context);

        /// <summary>
        /// Returns the name of the view that should be used to ask the user to approve the application. It also
        /// returns the view model that should be used when rendering the view. Sending in the AuthorizationRequest
        /// lets the user add things like the Scope information to their view model. For instance you could also use
        /// the client id to fetch information about the application that is requesting approval.
        /// </summary>
        Tuple<string, object> GetAuthorizationView(AuthorizationRequest request, NancyContext context);

        /// <summary>
        /// Thing you are going to want to validate are the client_id, redirect_url (is it valid? does it match the
        /// one that's registered for the application with client_id) and so on
        /// </summary>
        ValidationResult ValidateRequest(AuthorizationRequest request, NancyContext context);
    }
}