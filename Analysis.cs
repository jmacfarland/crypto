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
        private static string NgramFilePath = "C:\\Users\\JMacfarland\\crypto\\resources\\english_quadgrams.txt";
        private Dictionary<string, double> NgramsDictionary;
        private long totalNgrams = 4224127912; //total number of ngram occurrences recorded in dictionary
        private double worstScore = -11;
        private SubstitutionCipher sb;

        public Analysis()
        {
            sb = new SubstitutionCipher();
            NgramsDictionary = parseNgramsFile();
        }

        public void breakSubstitutionCipher(string cipherText)
        {
            Console.WriteLine("Solving substitution cipher...");
            Console.WriteLine("I will print my best guess to the screen. When this looks like English, hit [CTRL + C].\n");
            Console.WriteLine("\nInitial score: " + getTextNgramFitness(cipherText));

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
                parentScore = getTextNgramFitness(sb.decode(cipherText, parentKey));

                for(int i = 0; i < 1000; i++)//i => iterations since last improvement. If > 1000, we are at local maximum
                {
                    string childKey = sb.swapTwoChars(parentKey);
                    double childScore = getTextNgramFitness(sb.decode(cipherText, childKey));

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

        public double getTextNgramFitness(string text)
        {
            //Get all possible nGrams from the text
            string processedText = Utils.RemoveSpacesAndPunctuation(text.ToUpper());
            int numNgrams = processedText.Length - 3;
            string[] textNgrams = new string[numNgrams];

            for(int i = 0; i < numNgrams; i++)
            {
                textNgrams[i] = processedText.Substring(i, 4);
            }

            //score the ngrams
            double score = 0;
            for(int i = 0; i < numNgrams; i++)
            {
                score += getNgramScore(textNgrams[i]);
            }

            return score;
        }

        //Get the "score" of a given nGram with respect to the NgramsDictionary dictionary
        public double getNgramScore(string nGram)
        {
            double n;

            NgramsDictionary.TryGetValue(nGram, out n); 

            if(n != 0)
            {
                return n;
            }
            return worstScore;
        }

        //Initializes the NgramsDictionary list with key value pairs of a nGramgram with its frequency
        private Dictionary<string, double> parseNgramsFile()
        {
            var ngrams = new Dictionary<string, double>();
            string data;
            string[] splitData;

            using (StreamReader sr = File.OpenText(NgramFilePath))
            {
                while((data = sr.ReadLine()) != null)
                {
                    splitData = data.Split(' ');
                    double value = Math.Log10(double.Parse(splitData[1]) / totalNgrams);
                    ngrams.Add(splitData[0], value);
                }
            }

            return ngrams;
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
