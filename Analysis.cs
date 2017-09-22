using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace crypto
{
    class Analysis
    {
        private static string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static string quadgramsPath = "C:\\Users\\JMacfarland\\crypto\\resources\\english_quadgrams.txt";
        private List<KeyValuePair<string, int>> quadgrams;

        public Analysis()
        {
            quadgrams = getQuadgrams();

        }

        //Initializes the quadgrams list with key value pairs of a quadgram with its frequency
        private List<KeyValuePair<string, int>> getQuadgrams()
        {
            var quads = new List<KeyValuePair<string, int>>();
            string data;
            string[] splitData;

            using (StreamReader sr = File.OpenText(quadgramsPath))
            {
                while((data = sr.ReadLine()) != null)
                {
                    splitData = data.Split(' ');
                    quads.Add(new KeyValuePair<string, int>(splitData[0], int.Parse(splitData[1])));
                }
            }

            return quads;
        }

        public static int[] getNumLetterOccurrences(string text)
        {
            int[] frequency = new int[26];

            for(int i = 0; i < text.Length; i++)
            {
                for(int n = 0; n < alphabet.Length; n++)
                {
                    if(text[i] == alphabet[n])
                    {
                        frequency[n]++;
                    }
                }
            }

            return frequency;
        }
        public static int[] printNumLetterOccurrences(string text)
        {
            int[] frequency = getNumLetterOccurrences(text);

            for (int i = 0; i < 26; i++)
            {
                Console.Write(alphabet[i] + ":" + frequency[i] + ", ");
            }

            return frequency;
        }
    }
}
