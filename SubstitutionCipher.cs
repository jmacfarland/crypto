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
        //TODO: Use a Dictionary for encoding/decoding? How hard would that be to swap two letters in?
        private Random rnd;
        private string key;

        public SubstitutionCipher()
        {
            rnd = new Random();
            generateNewKey();
        }

        public string getAlphabet()
        {
            return plainAlphabet;
        }

        public string getKey()
        {
            return key;
        }

        public void setKey(string newKey)
        {
            key = newKey;
        }

        public void generateNewKey()
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
        public void swapTwoCharsInKey()
        {
            char[] arr = key.ToCharArray();

            int k = rnd.Next(arr.Length - 1);
            int j = rnd.Next(arr.Length - 1);
            var value = arr[k];
            arr[k] = arr[j];
            arr[j] = value;

            key = new string(arr);
        }
    }
}
