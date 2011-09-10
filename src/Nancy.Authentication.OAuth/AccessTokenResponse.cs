namespace Nancy.Authentication.OAuth
{
    public class AccessToken
    {
        public string Access_Token { get; set; }
        
        public string Token_type { get; set; }
        
        public int? Expires_In { get; set; }
        
        public string Refresh_Token { get; set; }

        public string State { get; set; }
    }
}