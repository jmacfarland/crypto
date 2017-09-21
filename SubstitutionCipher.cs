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
        static string key;

        public SubstitutionCipher()
        {
            generateNewKey();
        }

        public static void generateNewKey()
        {
            key = shuffle(plainAlphabet);
        }

        public string encode(string plain)
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

        public string decode(string cipher)
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

        static string shuffle(string plain)
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
    }
}
