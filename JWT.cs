using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JwtHandler
{

    [System.Serializable]
    public class JwtPayload
    {
        public string id;
        public string email;
        public List<string> role;
        public string avatar;
        public string names;
        public int iat;
        public int exp;
    }

    public static class Jwt
    {
        public static string Decode(string token)
        {
            string[] tokenParts = token.Split('.');
            string payload = tokenParts[1];
            string decodedPayload = Base64Decode(payload);
            return decodedPayload;
        }

        public static string Base64Decode(string base64EncodedData)
        {
            string base64EncodedDataWithPadding = base64EncodedData;
            int mod4 = base64EncodedDataWithPadding.Length % 4;
            if (mod4 > 0)
            {
                base64EncodedDataWithPadding += new string('=', 4 - mod4);
            }
            byte[] base64EncodedBytes = System.Convert.FromBase64String(base64EncodedDataWithPadding);
            string decodedPayload = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            return decodedPayload;
        }

        public static JwtPayload GetPayload(string token)
        {
            string decodedPayload = Decode(token);
            JwtPayload payload = JsonUtility.FromJson<JwtPayload>(decodedPayload);
            return payload;
        }
    }
}
