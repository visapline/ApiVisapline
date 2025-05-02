using System.Security.Cryptography;
using System.Text;

namespace ApiPruebaVive.Utils
{
    public static class Md5Helper
    {
        public static string CalculateMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2")); // Formato hexadecimal
                }
                return sb.ToString();
            }
        }
    }
}