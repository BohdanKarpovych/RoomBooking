using System.Security.Cryptography;
using System.Text;

namespace RoomBooking.ServiceClasses
{
    public class MD5HashCalculating
    {
        public string CalculateMD5Hash(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            foreach(var item in hash)
            {
                sb.Append(item.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}