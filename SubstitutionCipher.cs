using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crypto
{
    class SubstitutionCipher
    {
        private string plainAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private Random rnd;

        public SubstitutionCipher()
        {
            rnd = new Random();
        }

        public string getAlphabet()
        {
            return plainAlphabet;
        }

        public string generateNewKey()
        {
            return shuffle(plainAlphabet);
        }

        public string encode(string plain, string key)
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

        public string decode(string cipher, string key)
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
        public string shuffle(string plain)
        {
            char[] plainArr = plain.ToCharArray();
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
        public string swapTwoChars(string key)
        {
            char[] arr = key.ToCharArray();

            int k = rnd.Next(arr.Length - 1);
            int j = rnd.Next(arr.Length - 1);
            var value = arr[k];
            arr[k] = arr[j];
            arr[j] = value;

            return new string(arr);
        }
    }
}
