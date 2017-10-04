using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

namespace crypto
{
    class Utils
    {
        public static string RemoveSpacesAndPunctuation(char[] text)
        {
            List<char> result = new List<char>();

            for(int i = 0; i < text.Length; i++)
            {
                if(IsBasicLetter(text[i]))
                {
                    result.Add(text[i]);
                }
            }

            return new string(result.ToArray());
        }

        public static bool IsBasicLetter(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }

        
        public static char[] cloneArray(char[] array)
        {
            char[] result = new char[array.Length];
            for(int i = 0; i < array.Length; i++)
            {
                result[i] = array[i];
            }
            return result;
        }

        //Randomly reorders a string
        public static string ShuffleString(string plain)
        {
            char[] plainArr = plain.ToCharArray();
            int n = plainArr.Length;
            Random rnd = new Random();

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

        public static string ShuffleString(string plain, Random rnd)
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
    }
}