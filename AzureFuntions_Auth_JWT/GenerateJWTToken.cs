using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFuntions_Auth_JWT
{
    /// <summary>
    ///     Service class for performing authentication.
    /// </summary>
    public class GenerateJWTToken
    {
        private readonly IJwtAlgorithm _algorithm;
        private readonly IJsonSerializer _serializer;
        private readonly IBase64UrlEncoder _base64Encoder;
        private readonly IJwtEncoder _jwtEncoder;
        public GenerateJWTToken()
        {
            // JWT specific initialization.
            _algorithm = new HMACSHA256Algorithm();
            _serializer = new JsonNetSerializer();
            _base64Encoder = new JwtBase64UrlEncoder();
            _jwtEncoder = new JwtEncoder(_algorithm, _serializer, _base64Encoder);
        }
        public string IssuingJWT(string user)
        {
            Dictionary<string, object> claims = new Dictionary<string, object>
        {
            // JSON representation of the user Reference with ID and display name
            { "username", user },
 
            // TODO: Add other claims here as necessary; maybe from a user database
            { "role", "admin"}
        };

            string token = _jwtEncoder.Encode(claims, "Your Secret Securtity key string"); // Put this key in config
            return token;
        }
    }
}
