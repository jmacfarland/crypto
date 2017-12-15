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
        private static string filePath = "";
	FileInfo file;
        private Dictionary<string, double> NgramsDictionary;
        private int NgramLength;
        private int timeout;
        private long totalNgrams = 0; //total number of ngram occurrences recorded in dictionary
        private double worstScore = -11;
        private SubstitutionCipher sb;

        public Analysis(int NgramLengthInit, int timeout = 10)
        {
            sb = new SubstitutionCipher();
            NgramLength = NgramLengthInit;
            NgramsDictionary = parseNgramsFile();
            worstScore = Math.Log10(0.01 / totalNgrams);

            if(timeout > 30) this.timeout = 10;
            else this.timeout = timeout;
        }

        public string breakSubstitutionCipher(string cipherText)
        {
            //current
            int count = 0;
            string maxKey = SubstitutionCipher.getAlphabet();
            double maxScore = -99e9;

            string parentKey = maxKey;
            double parentScore = maxScore;

            DateTime start = DateTime.Now;
            while(DateTime.Now.Subtract(start).Seconds <= timeout)
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

            return maxKey;
        }

        public double getTextNgramFitness(string text)
        {
            //Get all possible nGrams from the text
            string processedText = Utils.RemoveSpacesAndPunctuation(text.ToUpper());
            int numNgrams = processedText.Length - NgramLength - 1;
            string[] textNgrams = new string[numNgrams];

            for(int i = 0; i < numNgrams; i++)
            {
                textNgrams[i] = processedText.Substring(i, NgramLength);
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
            string fileName = "resources/";

            switch(NgramLength)
            {
                case 1:
                    fileName += "english_monograms.txt";
                    Console.WriteLine("Ngram analysis with NgramLength of 1 doesn't work very well...");
                    break;
                case 2:
                    fileName += "english_bigrams.txt";
                    break;
                case 3:
                    fileName += "english_trigrams.txt";
                    break;
                case 4:
                    fileName += "english_quadgrams.txt";
                    break;
                case 5:
                    fileName += "english_quintgrams.txt";
                    break;
                default:
                    Console.WriteLine("Ngram length of " + NgramLength + " not supported.");
                    return null;
            }

	    file = new FileInfo(fileName);

            //Get total Ngram occurrences
            using (StreamReader sr = File.OpenText(file.FullName))
            {
                while((data = sr.ReadLine()) != null)
                {
                    splitData = data.Split(' ');
                    totalNgrams += long.Parse(splitData[1]);
                }
            }

            using (StreamReader sr = File.OpenText(filePath + fileName))
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
