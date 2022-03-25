using Abstraction_Layer;

using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;

namespace User_Service
{
    public class JWTGenerator : ITokenGenerator
    {
        private readonly string secret;
        private readonly double validityTime;

        public JWTGenerator(string secret, double validityTime)
        {
            this.secret = secret ?? throw new ArgumentNullException(nameof(secret));
            this.validityTime = validityTime;
        }

        /// <summary>
        /// Create a Json Web Token with the given payload
        /// </summary>
        /// <param name="payload">string-object dictionary of the values a token should be generated for</param>
        /// <param name="parameters">No extra parameters are used</param>
        /// <returns>Json Web Token</returns>
        /// <exception cref="ArgumentException">An argument was</exception>
        public string Create(object payload, params object[] parameters)
        {
            if (payload.GetType() != typeof(Dictionary<string, object>))
            {
                throw new ArgumentException("No valid string-object dictonary was supplied");
            }

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            IDateTimeProvider provider = new UtcDateTimeProvider();
            ((Dictionary<string, object>) payload).Add("exp", UnixEpoch.GetSecondsSince(provider.GetNow()) + validityTime);

            string token = encoder.Encode(payload, secret);
            return token;
        }

        /// <summary>
        /// Decode and possibly verify a given JWT
        /// </summary>
        /// <param name="token">A JWT that will be decoded</param>
        /// <param name="parameters">First parameters defines wether the token should be verified. Default is true</param>
        /// <returns>A string-object dictionary representing the JSON</returns>
        /// <exception cref="ArgumentNullException">A given argument was not valid</exception>
        /// <exception cref="TokenExpiredException">The given JWT has expired</exception>
        /// <exception cref="SignatureVerificationException">The given JWT has not been signed with the right signature</exception>
        public object Decode(string token, params object[] parameters)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (parameters.Length == 0 || parameters[0] is not bool)
            {
                parameters = new object[] { true };
            }

            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

                return decoder.DecodeToObject<Dictionary<string, object>>(token, secret, verify: (bool)parameters[0]);  
            }
            catch (TokenExpiredException)
            {
                throw new TokenExpiredException("Token has expired");
            }
            catch (SignatureVerificationException)
            {
                throw new SignatureVerificationException("Token has invalid signature");
            }
        }
    }
}
