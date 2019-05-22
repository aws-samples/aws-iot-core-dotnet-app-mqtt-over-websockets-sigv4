using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace AwsIOTMqttOverWebsockets.Utils
{
    public static class Sigv4util
    {
        public const string ISO8601BasicFormat = "yyyyMMddTHHmmssZ";
        public const string DateStringFormat = "yyyyMMdd";
        public const string EmptyBodySha256 = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";
        public static HashAlgorithm CanonicalRequestHashAlgorithm = HashAlgorithm.Create("SHA-256");
        // the name of the keyed hash algorithm used in signing
        public const string HmacSha256 = "HMACSHA256";
        public const string XAmzSignature = "X-Amz-Signature";




        private static byte[] HmacSHA256(String data, byte[] key)
        {
            String algorithm = "HmacSHA256";
            KeyedHashAlgorithm keyHashAlgorithm = KeyedHashAlgorithm.Create(algorithm);
            keyHashAlgorithm.Key = key;

            return keyHashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(data));
        }

        private static byte[] ComputeKeyedHash(string algorithm, byte[] key, byte[] data)
        {
            var kha = KeyedHashAlgorithm.Create(algorithm);
            kha.Key = key;
            return kha.ComputeHash(data);
        }

        public static string ToHexString(byte[] data, bool lowerCase)
        {
            StringBuilder stringBuilder = new StringBuilder();

            try
            {
                for (var i = 0; i < data.Length; i++)
                {
                    stringBuilder.Append(data[i].ToString(lowerCase ? "x2" : "X2"));
                }

            }
            catch (Exception ex)
            {
                Logger.LogDebug(ex.Message);
            }

            return stringBuilder.ToString();
        }

        private static byte[] getSignatureKey(String key, String dateStamp, String regionName, String serviceName)
        {
            byte[] kSecret = Encoding.UTF8.GetBytes(("AWS4" + key).ToCharArray());
            byte[] kDate = HmacSHA256(dateStamp, kSecret);
            byte[] kRegion = HmacSHA256(regionName, kDate);
            byte[] kService = HmacSHA256(serviceName, kRegion);
            byte[] kSigning = HmacSHA256("aws4_request", kService);

            return kSigning;
        }

        public static string getSignedurl(string host, string region, string accessKey, string secretKey)
        {
            string requestUrl = "";
            try
            {

                DateTime requestDateTime = DateTime.UtcNow;
                string datetime = requestDateTime.ToString(ISO8601BasicFormat, CultureInfo.InvariantCulture);
                var date = requestDateTime.ToString(DateStringFormat, CultureInfo.InvariantCulture);

                string method = ConfigHelper.ReadSetting("method");

                string protocol = ConfigHelper.ReadSetting("protocol");

                string uri = ConfigHelper.ReadSetting("uri");

                string service = ConfigHelper.ReadSetting("service");

                string algorithm = ConfigHelper.ReadSetting("algorithm");

                string credentialScope = date + "/" + region + "/" + service + "/" + "aws4_request";
                string canonicalQuerystring = "X-Amz-Algorithm=" + algorithm;
                canonicalQuerystring += "&X-Amz-Credential=" + HttpHelper.UrlEncode(accessKey + '/' + credentialScope);

                canonicalQuerystring += "&X-Amz-Date=" + datetime;
                canonicalQuerystring += "&X-Amz-Expires=86400";
                canonicalQuerystring += "&X-Amz-SignedHeaders=host";

                string canonicalHeaders = "host:" + host + "\n";

                var canonicalRequest = method + "\n" + uri + "\n" + canonicalQuerystring + "\n" + canonicalHeaders + "\n" + "host" + "\n" + EmptyBodySha256;

                byte[] hashValueCanonicalRequest = CanonicalRequestHashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(canonicalRequest));



                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < hashValueCanonicalRequest.Length; i++)
                {
                    builder.Append(hashValueCanonicalRequest[i].ToString("x2"));
                }

                string byteString = builder.ToString();

                var stringToSign = algorithm + "\n" + datetime + "\n" + credentialScope + "\n" + byteString;
                // compute the signing key
                KeyedHashAlgorithm keyedHashAlgorithm = KeyedHashAlgorithm.Create(HmacSha256);

                keyedHashAlgorithm.Key = getSignatureKey(secretKey, date, region, service);

                var signingKey = keyedHashAlgorithm.Key;

                var signature = ComputeKeyedHash(HmacSha256, signingKey, Encoding.UTF8.GetBytes(stringToSign));
                var signatureString = ToHexString(signature, true);

                canonicalQuerystring += "&X-Amz-Signature=" + signatureString;

                requestUrl = protocol + "://" + host + uri + "?" + canonicalQuerystring;


            }

            catch (Exception ex)
            {
                Logger.LogError(ex.Message);

            }


            return requestUrl;
        }
    }
}
