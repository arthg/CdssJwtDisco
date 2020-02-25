namespace ClickSWITCH.CDSS.Api.Middleware
{
    public sealed class RequestJwt
    {
        /// <summary>
        /// Identifies the key which the client has determined to be available to the user performing the request (on the client).
        /// In CDSS the key will be regenerated from the resource name based on the key's construction rules, and verified to match this value. 
        /// The client is not expected to use the resource name to generate the key, but rather to use the identity of the individual 
        /// (and their role) making the request to determine the key.
        /// </summary>
        public string Key { get; set; } = default!;

        /// <summary>
        /// Uniquely identifies an individual who is performing an action within the source client.
        ///
        /// Identifies the subject of the JWT.
        /// </summary>
        public string Sub { get; set; } = default!;

        /// <summary>
        /// The application operation to be performed.
        /// </summary>
        public string Op { get; set; } = default!;

        /// <summary>
        /// The timestamp describing when the JWT was issued, allowing the detection of replay attacks.
        ///
        /// Wikipedia: Identifies the time at which the JWT was issued. The value must be a NumericDate.
        /// The timestamp age must be less than the service definition MaxRequestAgeInSeconds
        /// </summary>
        /*
         * https://tools.ietf.org/html/rfc7519: NumericDate: number of seconds from 1970-01-01T00:00:00Z UTC until the specified UTC date/time        
         */
        public long Iat { get; set; }

        /// <summary>
        /// HMAC generated with the SHA256 hash function, using the client's name as the key, and the signed_payload string as the message.     
        /// </summary>
        public string Sig { get; set; } = default!;
    }
}