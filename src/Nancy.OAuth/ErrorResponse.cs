namespace Nancy.OAuth
{
    public class ErrorResponse
    {
        public string Error { get; set; }

        public string Error_Description { get; set; }

        public string State? { get; set; }
    }
}