namespace Nancy.OAuth
{
    public class AuthorizationRequestValidationResult
    {
        public AuthorizationRequestValidationResult(AuthorizationErrorType errorType)
        {
            this.ErrorType = errorType;
        }

        public bool IsValid
        {
            get { return this.ErrorType == AuthorizationErrorType.None; }
        }

        public AuthorizationErrorType ErrorType { get; set; }
    }
}