using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Net;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Linq;
using Microsoft.Extensions.Options;
using System.Data;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;

/// <summary>
/// Represents Leemo common project layer
/// </summary>
namespace TPSS.Common
{

    /// <summary>
    /// All the commom functions will be declared here for the application.
    /// </summary>
    public static class CommonFunction
    {

        public static string GenerateJSONWebToken(Guid userId,string userEmail, string userRole, string issuer, string key, string expiryTime , string UserRespJson , string userLoationId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(Constants.JwtTokenClaimType_UserId, Convert.ToString(userId)),
                new Claim(Constants.JwtTokenClaimType_UserEmail, userEmail),
                new Claim(Constants.JwtTokenClaimType_UserRole, userRole),
                new Claim(Constants.JwtTokenClaimType_UserRespJson, UserRespJson),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(Constants.JwtTokenClaimType_UserLocationID, userLoationId)
               
            };

            DateTime expirationTime = DateTime.Now.AddDays(Convert.ToDouble(expiryTime));
            var token = new JwtSecurityToken(issuer,
              issuer,
              claims,
              expires: expirationTime,
              signingCredentials: credentials);

            return string.Concat(new JwtSecurityTokenHandler().WriteToken(token), ",", Convert.ToString(expirationTime));
        }

        public static string HashPassword(string password, byte[] salt = null, bool needsOnlyHash = false)
        {
            if (salt == null || salt.Length != 16)
            {
                // generate a 128-bit salt using a secure PRNG
                salt = new byte[128 / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            if (needsOnlyHash) return hashed;
            // password will be concatenated with salt using ':'
            return $"{hashed}:{Convert.ToBase64String(salt)}";
        }

        public static bool VerifyPassword(string hashedPasswordWithSalt, string passwordToCheck)
        {
            // retrieve both salt and password from 'hashedPasswordWithSalt'
            var passwordAndHash = hashedPasswordWithSalt.Split(':');
            if (passwordAndHash == null || passwordAndHash.Length != 2)
                return false;
            var salt = Convert.FromBase64String(passwordAndHash[1]);
            if (salt == null)
                return false;
            // hash the given password
            var hashOfpasswordToCheck = HashPassword(passwordToCheck, salt, true);
            // compare both hashes
            if (String.Compare(passwordAndHash[0], hashOfpasswordToCheck) == 0)
            {
                return true;
            }
            return false;
        }

        public static string CreateRandomPassword(int length, string validCharacters)
        {
            Random random = new Random();
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validCharacters[random.Next(0, validCharacters.Length)];
            }
            return new string(chars);
        }

        public static string GetIpAddress()
        {
            string hostname = Dns.GetHostName();
            string MyIpAddress = Dns.GetHostEntry(hostname).AddressList[1].ToString();
            return MyIpAddress;
        }

        public static string returnJsontoString(object value)
        {
            var resultjson = JsonConvert.SerializeObject(value);
            string result = resultjson.ToString();
            return result;
        }

        public static void sendEmail(string from, string to, string bodyMsg, string subject,string password, string host, int port, bool enableSsl, bool isBodyHmtl, bool useDefaultCredentials, string alias)
        {
            try
            {
                using (SmtpClient client = new SmtpClient(host))
                {
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(from, alias);
                    mailMessage.BodyEncoding = Encoding.UTF8;
                    mailMessage.To.Add(to);
                    mailMessage.Body = bodyMsg;
                    mailMessage.Subject = subject;
                    mailMessage.IsBodyHtml = isBodyHmtl;
                    client.Port = port;
                    client.EnableSsl = enableSsl;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = useDefaultCredentials;
                    client.Credentials = new NetworkCredential(from, password);
                    client.Send(mailMessage);
                }
            }
            catch (Exception ex) 
            {

            }
        }

        public static string EncodeData(string any)
        {
            try
            {
                byte[] encData_byte = new byte[any.Length];
                encData_byte = Encoding.UTF8.GetBytes(any);
                string encodedData = Convert.ToBase64String(encData_byte);
                string result = randomString(20) + Constants.RandomString.RandomString1 + encodedData + Constants.RandomString.RandomString2 + randomString(25);
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string DecodeData(string encodedData)
        {
            string str1 = encodedData.Split(Constants.RandomString.RandomString1)[1];
            string str2 = str1.Split(Constants.RandomString.RandomString2)[0];
            UTF8Encoding encoder = new UTF8Encoding();
            Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(str2);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }

        public static string randomString(int length)
        {
            Random random = new Random();
            string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            string result = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            return result;
        }

        public static string RegexPatternCheckforDomainNamebyHostingkeyword(string domainname)
        {

            string domainNameStartValue = "";
            string[] domainnamepattern = Regex.Split(domainname, @"\d+");
            if (domainnamepattern != null)
            {

                domainNameStartValue = domainnamepattern[0].Trim();

            }

            return domainNameStartValue;

        }
        //DomainName Validation
        public static bool RegexPatternCheckforDomain(string domainname)
        {

            // Create a pattern for a word   
            string pattern = @"^[a-zA-Z][a-zA-Z0-9]*$";
            // Create a Regex  
            Regex rg = new Regex(pattern);

            // Long string  
            string domainnamePatterntoMatch = domainname;
            // Get all matches  
            MatchCollection domainnamePattern = rg.Matches(domainnamePatterntoMatch);
            // Print all matched authors  
            if (domainnamePattern.Count > 0)
            {
                return true;
            }

            return false;

        }

       //ValidateGuid
        public static bool isvalidGuid(string Guid)
        {

            // Create a pattern for a word   
            string pattern = @"^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$";
            // Create a Regex  
            Regex rg = new Regex(pattern);

            // Long string  
            string GuidPatterntoMatch = Guid;
            // Get all matches  
            MatchCollection GuidPattern = rg.Matches(GuidPatterntoMatch);
            // Print all matched authors  
            if (GuidPattern.Count > 0)
            {
                return true;
            }

            return false;

        }

        //ComapanyName to suggested domainName
        public static string CompanyNametoSuggestedDomainName(string CompanyName)
        {

           
            var SuggesteddomainName = "";
            // Create a pattern for a word   
            var pattern = CompanyName.Trim().Split(" ");

            foreach (var item in pattern) {

                if (item != "")
                {
                    SuggesteddomainName+= item.Substring(0, 1).ToUpper();
                }
            }
            while (SuggesteddomainName.Length<3) {

                Random random = new Random();
                var randomNumber = random.Next(0, 3);
                SuggesteddomainName += SuggesteddomainName  + randomNumber;


            }

           

            return SuggesteddomainName;

        }

        public static string GetMacAddress()
        {
            string addr = "";
            foreach (NetworkInterface n in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (n.OperationalStatus == OperationalStatus.Up)
                {
                    addr += n.GetPhysicalAddress().ToString();
                    break;
                }
            }
            return addr;
        }
        #region Validation Functions

        //we use EmailValidator from FluentValidation. So let's keep them sync - https://github.com/JeremySkinner/FluentValidation/blob/master/src/FluentValidation/Validators/EmailValidator.cs
        private const string EMAIL_EXPRESSION = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-||_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+([a-z]+|\d|-|\.{0,1}|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])?([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$";

        private static readonly Regex _emailRegex;

        /// <summary>
        /// Verifies that a string is in valid e-mail format
        /// </summary>
        /// <param name="email">Email to verify</param>
        /// <returns>true if the string is a valid e-mail address and false if it's not</returns>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            email = email.Trim();

            return _emailRegex.IsMatch(email);
        }


        /// <summary>
        /// Verifies that string is an valid IP-Address
        /// </summary>
        /// <param name="ipAddress">IPAddress to verify</param>
        /// <returns>true if the string is a valid IpAddress and false if it's not</returns>
        public static bool IsValidIpAddress(string ipAddress)
        {
            return IPAddress.TryParse(ipAddress, out var _);
        }

        /// <summary>
        /// Ensures that a string only contains numeric values
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Input string with only numeric values, empty string if input is null/empty</returns>
        public static string EnsureNumericOnly(string str)
        {
            return string.IsNullOrEmpty(str) ? string.Empty : new string(str.Where(char.IsDigit).ToArray());
        }

        /// <summary>
        /// Ensure that a string is not null
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Result</returns>
        public static string EnsureNotNull(string str)
        {
            return str ?? string.Empty;
        }


        #endregion

        #region Date Time Functions

        public static string IsTimeValid(string time)
        {
            string returnValidTIme = string.Empty;

            string ReturnVal = string.Empty;
            string Hourstt = string.Empty;
            string Replacestring = string.Empty;
            string hours = string.Empty;
            string minture = string.Empty;

            #region Set and Get Time

            if (!string.IsNullOrEmpty(time))
            {
                if (!time.StartsWith("0"))
                {
                    //time = "0" + time;
                    time = time;
                }

                if (time.ToLower().Contains("am") || time.ToLower().Contains("pm"))
                {
                    if (time.Contains(":"))
                    {
                        if (time.ToLower().Contains("am"))
                        {
                            ReturnVal = time.ToLower().Trim();
                            if (ReturnVal.Contains(" am"))
                            {
                                ReturnVal = ReturnVal.Replace(" am", "");
                            }
                            else
                            {
                                ReturnVal = ReturnVal.Replace("am", "");
                            }
                            Hourstt = ReturnVal.Substring(0, ReturnVal.IndexOf(":"));
                            int convertehours = 0;
                            if (Convert.ToInt32(Hourstt) > 11)
                            {
                                convertehours = Convert.ToInt32(0);
                                Replacestring = convertehours.ToString() + "0" + ReturnVal.Substring(ReturnVal.IndexOf(":"));
                                ReturnVal = Replacestring;
                            }
                        }
                        else
                        {
                            ReturnVal = time.ToLower().Trim();
                            if (ReturnVal.Contains(" pm"))
                            {
                                ReturnVal = ReturnVal.Replace(" pm", "");
                            }
                            else
                            {
                                ReturnVal = ReturnVal.Replace("pm", "");
                            }

                            Hourstt = ReturnVal.Substring(0, ReturnVal.IndexOf(":"));
                            int convertehours = 0;
                            if (Convert.ToInt32(Hourstt) > 11)
                            {
                                convertehours = Convert.ToInt32(Hourstt);
                            }
                            else
                            {
                                convertehours = Convert.ToInt32(Hourstt) + 12;
                            }
                            Replacestring = convertehours.ToString() + ReturnVal.Substring(ReturnVal.IndexOf(":"));
                            ReturnVal = Replacestring;
                        }
                    }
                    else
                    {
                        if (time.Length == 4)
                        {
                            ReturnVal = time.ToLower().Trim();
                            hours = ReturnVal.Substring(0, 2);
                            minture = ReturnVal.Substring(2, 2);
                            Replacestring = hours + ":" + minture;
                            ReturnVal = Replacestring;
                        }
                    }
                }
                else
                {
                    if (time.Contains(":"))
                    {
                        ReturnVal = time.ToLower().Trim();
                        Hourstt = ReturnVal.Substring(0, ReturnVal.IndexOf(":"));
                        if (Hourstt.Length == 1)
                            Replacestring = "0" + Hourstt.ToString() + ReturnVal.Substring(ReturnVal.IndexOf(":"));
                        else
                            Replacestring = Hourstt.ToString() + ReturnVal.Substring(ReturnVal.IndexOf(":"));
                        ReturnVal = Replacestring;

                    }
                    else
                    {
                        if (time.Length == 4)
                        {
                            ReturnVal = time.ToLower().Trim();
                            hours = ReturnVal.Substring(0, 2);
                            minture = ReturnVal.Substring(2, 2);
                            Replacestring = hours + ":" + minture;
                            ReturnVal = Replacestring;
                        }
                    }
                }
            }

            #endregion

            if (!string.IsNullOrEmpty(ReturnVal))
            {
                try
                {
                    returnValidTIme = ReturnVal.ToString();
                }
                catch { }
            }

            return returnValidTIme;
        }

        public static TimeSpan? GetValidTime(string time)
        {
            TimeSpan? Returnvalues = null;

            string ReturnVal = string.Empty;
            string Hourstt = string.Empty;
            string Replacestring = string.Empty;
            string hours = string.Empty;
            string minture = string.Empty;

            #region Set and Get Time

            if (time.ToLower().Contains("am") || time.ToLower().Contains("pm"))
            {
                if (time.Contains(":"))
                {
                    if (time.ToLower().Contains("am"))
                    {
                        ReturnVal = time.ToLower().Trim();
                        if (ReturnVal.Contains(" am"))
                        {
                            ReturnVal = ReturnVal.Replace(" am", "");
                        }
                        else
                        {
                            ReturnVal = ReturnVal.Replace("am", "");
                        }
                        Hourstt = ReturnVal.Substring(0, ReturnVal.IndexOf(":"));
                        int convertehours = 0;
                        if (Convert.ToInt32(Hourstt) > 11)
                        {
                            convertehours = Convert.ToInt32(0);
                            Replacestring = convertehours.ToString() + ReturnVal.Substring(ReturnVal.IndexOf(":"));
                            ReturnVal = Replacestring;
                        }
                    }
                    else
                    {
                        ReturnVal = time.ToLower().Trim();
                        if (ReturnVal.Contains(" pm"))
                        {
                            ReturnVal = ReturnVal.Replace(" pm", "");
                        }
                        else
                        {
                            ReturnVal = ReturnVal.Replace("pm", "");
                        }

                        Hourstt = ReturnVal.Substring(0, ReturnVal.IndexOf(":"));
                        int convertehours = 0;
                        if (Convert.ToInt32(Hourstt) > 11)
                        {
                            convertehours = Convert.ToInt32(Hourstt);
                        }
                        else
                        {
                            convertehours = Convert.ToInt32(Hourstt) + 12;
                        }
                        Replacestring = convertehours.ToString() + ReturnVal.Substring(ReturnVal.IndexOf(":"));
                        ReturnVal = Replacestring;
                    }
                }
                else
                {
                    if (time.Length == 4)
                    {
                        ReturnVal = time.ToLower().Trim();
                        hours = ReturnVal.Substring(0, 2);
                        minture = ReturnVal.Substring(2, 2);
                        Replacestring = hours + ":" + minture;
                        ReturnVal = Replacestring;
                    }
                }
            }
            else
            {
                if (time.Contains(":"))
                {
                    ReturnVal = time.ToLower().Trim();
                    Hourstt = ReturnVal.Substring(0, ReturnVal.IndexOf(":"));
                    if (Hourstt.Length == 1)
                        Replacestring = "0" + Hourstt.ToString() + ReturnVal.Substring(ReturnVal.IndexOf(":"));
                    else
                        Replacestring = Hourstt.ToString() + ReturnVal.Substring(ReturnVal.IndexOf(":"));
                    ReturnVal = Replacestring;

                }
                else
                {
                    if (time.Length == 4)
                    {
                        ReturnVal = time.ToLower().Trim();
                        hours = ReturnVal.Substring(0, 2);
                        minture = ReturnVal.Substring(2, 2);
                        Replacestring = hours + ":" + minture;
                        ReturnVal = Replacestring;
                    }
                }
            }
            #endregion

            if (!string.IsNullOrEmpty(ReturnVal))
            {
                try
                {
                    Returnvalues = TimeSpan.Parse(ReturnVal);
                }
                catch { }
            }

            return Returnvalues;
        }

        public static TimeSpan? ConvertStringToTime(string time)
        {
            DateTime dt = DateTime.Parse(time);
            TimeSpan? ts = new TimeSpan(dt.Hour, dt.Minute, dt.Second);
            return ts;
        }

        public static string GetHour(DateTime dt)
        {
            int Hour = dt.Hour;
            if (Hour <= 9)
            {
                return "0" + Hour.ToString();
            }
            else
            {
                switch (Hour)
                {
                    case 10:
                        return "10";
                    case 11:
                        return "11";
                    case 12:
                        return "12";
                    case 13:
                        return "01";
                    case 14:
                        return "02";
                    case 15:
                        return "03";
                    case 16:
                        return "04";
                    case 17:
                        return "05";
                    case 18:
                        return "06";
                    case 19:
                        return "07";
                    case 20:
                        return "08";
                    case 21:
                        return "09";
                    case 22:
                        return "10";
                    case 23:
                        return "11";
                    case 24:
                        return "12";
                    default:
                        return "00";
                }
            }
        }

        public static long ConvertDatetimeToUnixTimeStamp(DateTime dateTime)
        {
            TimeSpan timeSpan = dateTime - new DateTime(1970, 1, 1, 0, 0, 0);

            return (long)timeSpan.TotalSeconds;
        }

        /// <summary>
        /// Get difference in years
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static int GetDifferenceInYears(DateTime startDate, DateTime endDate)
        {
            //source: http://stackoverflow.com/questions/9/how-do-i-calculate-someones-age-in-c
            //this assumes you are looking for the western idea of age and not using East Asian reckoning.
            var age = endDate.Year - startDate.Year;
            if (startDate > endDate.AddYears(-age))
                age--;
            return age;
        }

        #endregion

        #region SQL Helper

        public static class SQLQueryHelper
        {
            public static string GetIntValueOrNullString(int? source)
            {
                if (source.HasValue)
                {
                    return source.Value.ToString();
                }
                else
                {
                    return "NULL";
                }
            }

            public static string GetDecimalValueOrNullString(decimal? source)
            {
                if (source.HasValue)
                {
                    return source.Value.ToString();
                }
                else
                {
                    return "NULL";
                }
            }

            public static string GetDoubleValueOrNullString(double? source)
            {
                if (source.HasValue)
                {
                    return source.Value.ToString();
                }
                else
                {
                    return "NULL";
                }
            }

            public static string GetDateTimeValueOrNullString(DateTime? source)
            {
                if (source.HasValue)
                {
                    return "'" + source.Value.ToString("MM/dd/yyyy HH:mm:ss") + "'";
                }
                else
                {
                    return "NULL";
                }
            }

            public static string GetTimeSpanValueOrNullString(TimeSpan? source)
            {
                if (source.HasValue)
                {
                    return "'" + source.Value.ToString() + "'";
                }
                else
                {
                    return "NULL";
                }
            }

            public static string GetStringValueOrNullString(string source)
            {
                if (!string.IsNullOrWhiteSpace(source))
                {
                    return "'" + source.ToString().Replace("'", "''").Replace("\0", "\\0") + "'";
                }
                else
                {
                    return "NULL";
                }
            }

        }

        #endregion

        #region Unit Conversions

        public static double ConvertMilesToKilometers(double miles)
        {
            //
            // Multiply by this constant and return the result.
            //
            return miles * 1.609344;
        }

        public static double ConvertKilometersToMiles(double kilometers)
        {
            //
            // Multiply by this constant.
            //
            return kilometers * 0.621371192;
        }

        #endregion
    }

    public static class ListExtensions
    {
        public static DataTable ToDataTable<T>(this List<T> iList)
        {
            DataTable dataTable = new DataTable();
            PropertyDescriptorCollection propertyDescriptorCollection =
                TypeDescriptor.GetProperties(typeof(T));
            for (int i = 0; i < propertyDescriptorCollection.Count; i++)
            {
                PropertyDescriptor propertyDescriptor = propertyDescriptorCollection[i];
                Type type = propertyDescriptor.PropertyType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = Nullable.GetUnderlyingType(type);


                dataTable.Columns.Add(propertyDescriptor.Name, type);
            }
            object[] values = new object[propertyDescriptorCollection.Count];
            foreach (T iListItem in iList)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = propertyDescriptorCollection[i].GetValue(iListItem);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static List<SelectListItem> ToSelectList<TEnum>(this TEnum obj)
        {
            return Enum.GetValues(typeof(TEnum)).OfType<Enum>()
                .Select(x =>
                    new SelectListItem
                    {
                        Text = Enum.GetName(typeof(TEnum), x),
                        Value = (Convert.ToInt32(x)).ToString()
                    }).ToList();
        }
    }
}
