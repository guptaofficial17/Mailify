using Leaf.xNet;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Mailify
{
public static class Crypto
    {
        public static byte[] HMACSHA1(byte[] input, byte[] key)
        {
            using (HMACSHA1 hmac = new HMACSHA1(key))
            {
                return hmac.ComputeHash(input);
            }
        }

        public static string ToHex(this byte[] bytes)
        {
            var sb = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                sb.Append(b.ToString("X2"));
            return sb.ToString();
        }
    }
    class Functions
    {
        public static string Hmac(string baseString, Hash type, string key, bool inputBase64 = false, bool keyBase64 = false, bool outputBase64 = false)
        {
            byte[] rawInput = inputBase64 ? Convert.FromBase64String(baseString) : Encoding.UTF8.GetBytes(baseString);
            byte[] rawKey = keyBase64 ? Convert.FromBase64String(key) : Encoding.UTF8.GetBytes(key);
            byte[] signature;

            switch (type)
            {
                case Hash.SHA1:
                    signature = Crypto.HMACSHA1(rawInput, rawKey);
                    break;
                default:
                    throw new NotSupportedException("Unsupported algorithm");
            }
            return outputBase64 ? Convert.ToBase64String(signature) : signature.ToHex();
        }
        public static string RandomString(int length, Random random)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string Resolve(string str)
        {
            Random r = new Random();
            string char1 = str.Replace("?h", "?").Replace("?l", "?").Replace("?u", "?").Replace("?d", "?").Replace("?m", "?").Replace("?i", "?").Replace("?s", "?");
            char[] ch = char1.ToCharArray();
            string result = "";
            foreach (var letter in ch)
            {
                if (letter.ToString() == "?")
                {
                    string sletter = RandomString(1, r);
                    result += sletter;
                }
                else
                {
                    result += letter.ToString();
                }
            }
            return result;
        }
        public static string RandomString(string Randomize)
        {
            string After = "";

            // Lists
            string HexList = "123456789abcdef";
            string LowerList = "abcdefghijklmnopqrstuvwxyz";
            string UpperList = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string DigitList = "1234567890";
            string SymbolList = "!@#$%^&*()_+";
            string UpperDigitList = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            string UpperLowerDigitList = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

            Random ran = new Random();

            for (int i = 0; i < Randomize.Length - 1; i++)
            {
                // HexaDecimal Random
                if ((Randomize[i].ToString() + Randomize[i + 1].ToString()).Equals("?h"))
                {
                    After += HexList[ran.Next(0, HexList.Length)];
                }
                // LowerCase Random
                else if ((Randomize[i].ToString() + Randomize[i + 1].ToString()).Equals("?l"))
                {
                    After += LowerList[ran.Next(0, LowerList.Length)];
                }
                // UpperCase Random
                else if ((Randomize[i].ToString() + Randomize[i + 1].ToString()).Equals("?u"))
                {
                    After += UpperList[ran.Next(0, UpperList.Length)];
                }
                // Digit Random
                else if ((Randomize[i].ToString() + Randomize[i + 1].ToString()).Equals("?d"))
                {
                    After += DigitList[ran.Next(0, DigitList.Length)];
                }
                // Upper Digit Random
                else if ((Randomize[i].ToString() + Randomize[i + 1].ToString()).Equals("?m"))
                {
                    After += UpperDigitList[ran.Next(0, UpperDigitList.Length)];
                }
                // Upper Lower Digit Random
                else if ((Randomize[i].ToString() + Randomize[i + 1].ToString()).Equals("?i"))
                {
                    After += UpperLowerDigitList[ran.Next(0, UpperLowerDigitList.Length)];
                }
                else if ((Randomize[i].ToString() + Randomize[i + 1].ToString()).Equals("?s"))
                {
                    After += SymbolList[ran.Next(0, SymbolList.Length)];
                }
                // Dash Separators
                else if ((Randomize[i].ToString().Contains("-")))
                {
                    After += "-";
                }
                // Incase there's a static number/letter here, we keep it the same
                else if (Randomize[i - 1].ToString().Equals("-") && !Randomize[i].ToString().Equals("?"))
                {
                    After += Randomize[i].ToString();
                }

            }

            return After;
        }
        public static string Base64Decode(string Base)
        {
            byte[] data = Convert.FromBase64String(Base);
            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }
        public static string Base64Encode(string Base)
        {
            var plain = Encoding.UTF8.GetBytes(Base);
            return Convert.ToBase64String(plain);
        }
        public static string sha256(string randomString)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
        public static string SolveRecaptcha(Captcha captcha)
        {
            while (true)
            {
                using (HttpRequest req = new HttpRequest())
                {
                    var res1 = req.Get("http://2captcha.com/in.php?key="+captcha.apiKey+"&method=userrecaptcha&googlekey="+captcha.siteKey+"&pageurl="+captcha.siteURL+"");
                    string text1 = res1.ToString();
                    if (text1.Contains("OK"))
                    {
                        string taskId = text1.Replace("OK|", "");
                        while (true)
                        {
                            Thread.Sleep(5000);
                            var res2 = req.Get("http://2captcha.com/res.php?key="+captcha.apiKey+"&action=get&id="+taskId+"");
                            string text2 = res2.ToString();
                            if (text2.Contains("OK"))
                            {
                                var token = text2.Replace("OK|", "");
                                return token;
                            }
                            else if ((!text2.Contains("CAPCHA_NOT_READY")) && !text2.Contains("OK"))
                            {
                                //Export.SaveData(text2, "errors_res");
                                return null;
                            }
                        }
                    }
                    else
                    {
                       
                        return null;
                    }
                }
            }
        }
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString().ToLower();
            }
        }
        public static IEnumerable<string> JSON(string input, string field, bool recursive = false, bool useJToken = false)
        {
            var list = new List<string>();

            if (useJToken)
            {
                if (recursive)
                {
                    if (input.Trim().StartsWith("["))
                    {
                        JArray json = JArray.Parse(input);
                        var jsonlist = json.SelectTokens(field, false);
                        foreach (var j in jsonlist)
                            list.Add(j.ToString());
                    }
                    else
                    {
                        JObject json = JObject.Parse(input);
                        var jsonlist = json.SelectTokens(field, false);
                        foreach (var j in jsonlist)
                            list.Add(j.ToString());
                    }
                }
                else
                {
                    if (input.Trim().StartsWith("["))
                    {
                        JArray json = JArray.Parse(input);
                        list.Add(json.SelectToken(field, false).ToString());
                    }
                    else
                    {
                        JObject json = JObject.Parse(input);
                        list.Add(json.SelectToken(field, false).ToString());
                    }
                }
            }
            else
            {
                var jsonlist = new List<KeyValuePair<string, string>>();
                parseJSON("", input, jsonlist);
                foreach (var j in jsonlist)
                    if (j.Key == field)
                        list.Add(j.Value);

                if (!recursive && list.Count > 1) list = new List<string>() { list.First() };
            }

            return list;
        }
        private static void parseJSON(string A, string B, List<KeyValuePair<string, string>> jsonlist)
        {
            jsonlist.Add(new KeyValuePair<string, string>(A, B));

            if (B.StartsWith("["))
            {
                JArray arr = null;
                try { arr = JArray.Parse(B); } catch { return; }

                foreach (var i in arr.Children())
                    parseJSON("", i.ToString(), jsonlist);
            }

            if (B.Contains("{"))
            {
                JObject obj = null;
                try { obj = JObject.Parse(B); } catch { return; }

                foreach (var o in obj)
                    parseJSON(o.Key, o.Value.ToString(), jsonlist);
            }
        }
        private static string BuildLRPattern(string ls, string rs)
        {
            var left = string.IsNullOrEmpty(ls) ? "^" : Regex.Escape(ls); // Empty LEFT = start of the line
            var right = string.IsNullOrEmpty(rs) ? "$" : Regex.Escape(rs); // Empty RIGHT = end of the line
            return "(?<=" + left + ").+?(?=" + right + ")";
        }
        public static IEnumerable<string> LR(string input, string left, string right, bool recursive = false, bool useRegex = false)
        {
            // No L and R = return full input
            if (left == string.Empty && right == string.Empty)
            {
                return new string[] { input };
            }

            // L or R not present and not empty = return nothing
            else if (((left != string.Empty && !input.Contains(left)) || (right != string.Empty && !input.Contains(right))))
            {
                return new string[] { };
            }

            var partial = input;
            var pFrom = 0;
            var pTo = 0;
            var list = new List<string>();

            if (recursive)
            {
                if (useRegex)
                {
                    try
                    {
                        var pattern = BuildLRPattern(left, right);
                        MatchCollection mc = Regex.Matches(partial, pattern);
                        foreach (Match m in mc)
                            list.Add(m.Value);
                    }
                    catch { }
                }
                else
                {
                    try
                    {
                        while (left == string.Empty || (partial.Contains(left)) && (right == string.Empty || partial.Contains(right)))
                        {
                            // Search for left delimiter and Calculate offset
                            pFrom = left == string.Empty ? 0 : partial.IndexOf(left) + left.Length;
                            // Move right of offset
                            partial = partial.Substring(pFrom);
                            // Search for right delimiter and Calculate length to parse
                            pTo = right == string.Empty ? (partial.Length - 1) : partial.IndexOf(right);
                            // Parse it
                            var parsed = partial.Substring(0, pTo);
                            list.Add(parsed);
                            // Move right of parsed + right
                            partial = partial.Substring(parsed.Length + right.Length);
                        }
                    }
                    catch { }
                }
            }

            // Non-recursive
            else
            {
                if (useRegex)
                {
                    var pattern = BuildLRPattern(left, right);
                    MatchCollection mc = Regex.Matches(partial, pattern);
                    if (mc.Count > 0) list.Add(mc[0].Value);
                }
                else
                {
                    try
                    {
                        pFrom = left == string.Empty ? 0 : partial.IndexOf(left) + left.Length;
                        partial = partial.Substring(pFrom);
                        pTo = right == string.Empty ? partial.Length : partial.IndexOf(right);
                        list.Add(partial.Substring(0, pTo));
                    }
                    catch { }
                }
            }

            return list;
        }
    }
    public class Captcha
    {
        public string apiKey { get; set; }
        public string siteKey { get; set; }
        public string siteURL { get; set; }
    }
}
public enum Hash
{
    SHA1,
    SHA256,
    SHA512
}

