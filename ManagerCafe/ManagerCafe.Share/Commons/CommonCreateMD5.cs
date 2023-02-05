using System.Security.Cryptography;
using System.Text;

namespace ManagerCafe.Commons
{
    public static class CommonCreateMD5
    {
        public static string Create(string input)
        {
            MD5 createMD = MD5.Create();
            byte[] data = createMD.ComputeHash(Encoding.ASCII.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            // Mã hóa thành string
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
