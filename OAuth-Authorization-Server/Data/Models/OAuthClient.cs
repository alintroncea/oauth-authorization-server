namespace OAuth_Authorization_Server.Models
{
    using System;
    using System.Collections.Generic;

    public class OAuthClient
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AppName { get; set; }
        public string Website { get; set; }
        public string FallbackUri { get; set; }
        public string CurrentState { get; set; }
        public ICollection<OAuthScope> OAuthScopes { get; set; }
    }

    // Using Guid for clarification and Demo purpose only
}
