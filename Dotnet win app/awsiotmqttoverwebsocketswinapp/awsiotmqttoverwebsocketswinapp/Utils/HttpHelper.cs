using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awsiotmqttoverwebsocketswinapp.Utils
{
    public static class HttpHelper
    {
        // The Set of accepted and valid Url characters per RFC3986. Characters outside of this set will be encoded.
        const string ValidUrlCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

        public static string UrlEncode(string data, bool isPath = false)
        {

            var encoded = new StringBuilder(data.Length * 2);

            try
            { 
                string unreservedChars = String.Concat(ValidUrlCharacters, (isPath ? "/:" : ""));

                foreach (char symbol in System.Text.Encoding.UTF8.GetBytes(data))
                {
                    if (unreservedChars.IndexOf(symbol) != -1)
                        encoded.Append(symbol);
                    else
                        encoded.Append("%").Append(String.Format("{0:X2}", (int)symbol));
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);

            }
            return encoded.ToString();
        }
    }
}
