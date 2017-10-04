using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

namespace crypto
{
    class Analysis
    {
        private static string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static string quadgramsPath = "C:\\Users\\JMacfarland\\crypto\\resources\\english_quadgrams.txt";
        private Dictionary<string, double> quadgrams;
        private long totalQuadgrams = 4224127912; //total number of quadgram occurrences recorded in dictionary
        private double worstScore = -11;
        private SubstitutionCipher sb;

        public Analysis()
        {
            sb = new SubstitutionCipher();
            quadgrams = parseQuadgramsFile();
        }

        public void breakSubstitutionCipher(string cipherText)
        {
            Console.WriteLine("Solving substitution cipher...");
            Console.WriteLine("I will print my best guess to the screen. When this looks like English, hit [CTRL + C].\n");

            //current
            int count = 0;
            string maxKey = sb.getAlphabet();
            double maxScore = -99e9;

            string parentKey = maxKey;
            double parentScore = maxScore;

            while(true)
            {
                count++;
                parentKey = sb.generateNewKey();
                parentScore = getFitness(sb.decode(cipherText, parentKey));

                for(int i = 0; i < 1000; i++)//i => iterations since last improvement. If > 1000, we are at local maximum
                {
                    string childKey = sb.swapTwoChars(parentKey);
                    double childScore = getFitness(sb.decode(cipherText, childKey));

                    if(childScore > parentScore)
                    {
                        parentScore = childScore;
                        parentKey = childKey;
                        i = 0;//made an improvement, reset counter
                    }
                }

                if(parentScore > maxScore)
                {
                    maxScore = parentScore;
                    maxKey = parentKey;

                    Console.WriteLine("\nBest score so far: " + maxScore + " on iteration " + count);
                    Console.WriteLine("     Best key: " + maxKey);
                    Console.WriteLine("     Best Plaintext: " + sb.decode(cipherText, maxKey));
                }
            }
        }

        public double getFitness(string text)
        {
            //Get all possible quads from the text
            char[] charArray = text.ToUpper().ToCharArray();
            string processedText = Utils.RemoveSpacesAndPunctuation(charArray);
            int numQuads = processedText.Length - 3;
            string[] textQuads = new string[numQuads];

            for(int i = 0; i < numQuads; i++)
            {
                textQuads[i] = processedText.Substring(i, 4);
            }

            //score the quads
            double score = 0;
            for(int i = 0; i < numQuads; i++)
            {
                score += getQuadScore(textQuads[i]);
            }

            return score;
        }

        //Get the "score" of a given quad with respect to the quadgrams dictionary
        public double getQuadScore(string quad)
        {
            double n;

            quadgrams.TryGetValue(quad, out n); 

            if(n != 0)
            {
                return n;
            }
            return worstScore;
        }

        //Initializes the quadgrams list with key value pairs of a quadgram with its frequency
        private Dictionary<string, double> parseQuadgramsFile()
        {
            var quads = new Dictionary<string, double>();
            string data;
            string[] splitData;

            using (StreamReader sr = File.OpenText(quadgramsPath))
            {
                while((data = sr.ReadLine()) != null)
                {
                    splitData = data.Split(' ');
                    double value = Math.Log10(double.Parse(splitData[1]) / totalQuadgrams);
                    quads.Add(splitData[0], value);
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
