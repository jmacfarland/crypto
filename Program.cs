using System;

namespace crypto
{
    class Program
    {
        static void Main(string[] args)
        {
            SubstitutionCipher sc = new SubstitutionCipher();

            string plainText = "Attack the east wall of the castle at dawn.";
            string cipherText = sc.encode(plainText);
            string newPlain = sc.decode(cipherText);

            Analysis analysis = new Analysis();
            Console.WriteLine("Plain score: " + analysis.getFitness(plainText));
            Console.WriteLine("Cipher score: " + analysis.getFitness(cipherText));
        }
    }
}
