using System;

namespace crypto
{
    class Program
    {
        static void Main(string[] args)
        {
            SubstitutionCipher sb = new SubstitutionCipher();
            string plainText = "Call me Ishmael. Some years ago-never mind how long precisely-having little or no money in my purse, and nothing particular to interest me on shore, I thought I would sail about a little and see the watery part of the world.";
            string key = sb.generateNewKey();
            string cipherText = sb.encode(plainText, key);
            string newPlain = sb.decode(cipherText, key);

            Console.WriteLine("Key: " + key + "\nCipher: " + cipherText);
            Analysis analysis = new Analysis();
            analysis.breakSubstitutionCipher(cipherText);
        }
    }
}
