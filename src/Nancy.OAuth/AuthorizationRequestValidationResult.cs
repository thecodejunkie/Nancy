namespace Nancy.OAuth
{
    public class ValidationResult
    {
        public ValidationResult(ErrorType errorType)
        {
            this.ErrorType = errorType;
        }

        public bool IsValid
        {
            get { return this.ErrorType == ErrorType.None; }
        }

        public ErrorType ErrorType { get; private set; }

        public static implicit operator ValidationResult(ErrorType errorType)
        {
            return new ValidationResult(errorType);
        }
    }
}