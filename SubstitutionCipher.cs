using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crypto
{
    class SubstitutionCipher
    {
        static string plainAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public SubstitutionCipher()
        {
            generateNewKey();
        }

        public static string generateNewKey()
        {
            string key = shuffle(plainAlphabet);
            return key;
        }

        public static string encode(string plain, string key)
        {
            char[] text = plain.ToUpper().ToCharArray();

            for (int i = 0; i < text.Length; i++)
            {
                for (int n = 0; n < plainAlphabet.Length; n++)
                {
                    if (text[i] == plainAlphabet[n])
                    {
                        text[i] = key[n];
                        break;
                    }
                }
            }
            return new string(text);
        }

        public static string decode(string cipher, string key)
        {
            char[] text = cipher.ToUpper().ToCharArray();

            for (int i = 0; i < text.Length; i++)
            {
                for (int n = 0; n < plainAlphabet.Length; n++)
                {
                    if (text[i] == key[n])
                    {
                        text[i] = plainAlphabet[n];
                        break;
                    }
                }
            }
            return new string(text);
        }

        //Randomly reorders a string
        public static string shuffle(string plain)
        {
            char[] plainArr = plain.ToCharArray();
            Random rnd = new Random();
            int n = plainArr.Length;

            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                var value = plainArr[k];
                plainArr[k] = plainArr[n];
                plainArr[n] = value;
            }
            return new string(plainArr);
        }

        //Swaps two chars at random within a string
        //Change to shuffle num chars
        public static string shuffleNumChars(string text, int num)
        {
            char[] arr = text.ToCharArray();
            Random rnd = new Random();

            for(int i = 0; i < num; i++)
            {
                int k = rnd.Next(arr.Length - 1);
                int j = rnd.Next(arr.Length - 1);
                var value = arr[k];
                arr[k] = arr[j];
                arr[j] = value;
            }

            return new string(arr);
        }
    }
}
