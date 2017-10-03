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
        private Dictionary<string, int> quadgrams;
        private double totalQuadgrams; //total number of quadgram occurrences recorded in dictionary
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
            string key = SubstitutionCipher.generateNewKey();
            string plaintext = SubstitutionCipher.decode(cipherText, key);
            double fitness = getFitness(plaintext);

            //local best
            string newKey = key;
            string newPlaintext = plaintext;
            double newFitness = fitness;
            
            //actual best
            string bestKey = key;
            string bestPlaintext = plaintext;
            double bestFitness = fitness;            

            while(true)
            {
                for(int i = 0; i < 1000; i++)//i => iterations since last improvement. If > 1000, we are at local maximum
                {
                    newKey = SubstitutionCipher.shuffleNumChars(key, 2);
                    newPlaintext = SubstitutionCipher.decode(cipherText, newKey);
                    newFitness = getFitness(newPlaintext);

                    if(newFitness > fitness)//if improvement, save changes
                    {
                        i = 0;
                        key = newKey;
                        fitness = newFitness;
                        plaintext = newPlaintext;
                    }
                    else //if no improvement, roll back and try swapping two other chars
                    {
                        newKey = key;
                        newFitness = fitness;
                        newPlaintext = plaintext;
                    }
                }

                //once 1000 iterations with no improvement, check if local best is better than actual best
                if(fitness > bestFitness)
                {
                    bestKey = key;
                    bestFitness = fitness;
                    bestPlaintext = plaintext;
                    Console.WriteLine(bestPlaintext + "\nKey: " + key + "\n Score: " + bestFitness + "\n\n");
                }

                //finally, get an entirely new key and reset for the next try
                //current
                key = SubstitutionCipher.generateNewKey();
                plaintext = SubstitutionCipher.decode(cipherText, key);
                fitness = getFitness(plaintext);

                //local best
                newKey = key;
                newPlaintext = plaintext;
                newFitness = fitness;
            }
        }

        public double getFitness(string text)
        {
            //Get all possible quads from the text
            char[] charArray = text.ToUpper().ToCharArray();
            string processedText = removeSpacesAndPunctuation(charArray);
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
            int n;

            quadgrams.TryGetValue(quad, out n); 

            if(n != 0)
            {
                return Math.Log10(n / totalQuadgrams);
            }
            return 0;
        }

        public string removeSpacesAndPunctuation(char[] text)
        {
            List<char> result = new List<char>();

            for(int i = 0; i < text.Length; i++)
            {
                if(isBasicLetter(text[i]))
                {
                    result.Add(text[i]);
                }
            }

            return new string(result.ToArray());
        }

        //Initializes the quadgrams list with key value pairs of a quadgram with its frequency
        private Dictionary<string, int> parseQuadgramsFile()
        {
            var quads = new Dictionary<string, int>();
            string data;
            string[] splitData;

            using (StreamReader sr = File.OpenText(quadgramsPath))
            {
                while((data = sr.ReadLine()) != null)
                {
                    splitData = data.Split(' ');
                    quads.Add(splitData[0], int.Parse(splitData[1]));
                    totalQuadgrams += int.Parse(splitData[1]);
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

        public static char[] cloneArray(char[] array)
        {
            char[] result = new char[array.Length];
            for(int i = 0; i < array.Length; i++)
            {
                result[i] = array[i];
            }
            return result;
        }

        public static bool isBasicLetter(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }
    }
}
