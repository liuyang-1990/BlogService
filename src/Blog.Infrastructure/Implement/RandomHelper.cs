using System;
using System.Text;

namespace Blog.Infrastructure.Implement
{
    public class RandomHelper
    {
        private static readonly char[] RandChar =
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k',
            'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y',
            'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
            'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };

        public static string GetRandomNum(int len)
        {

            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
            StringBuilder tmpstr = new StringBuilder();
            int randNum;

            for (int i = 0; i < len; i++)
            {
                randNum = rand.Next(RandChar.Length);
                tmpstr.Append(RandChar[randNum]);
            }
            return tmpstr.ToString();


        }


    }
}