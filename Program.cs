using System;

namespace crypto
{
    class Program
    {
        static void Main(string[] args)
        {
            SubstitutionCipher sc = new SubstitutionCipher();

            string plainText = "This is a test string. There are many like it, but this one is mine.";
            string cipherText = sc.encode(plainText);
            string newPlain = sc.decode(cipherText);

            var frequency = FrequencyAnalysis.printNumLetterOccurrences(cipherText);
        }
    }
}
