using System;

namespace crypto
{
    class Program
    {
        static void Main(string[] args)
        {
            SubstitutionCipher sb = new SubstitutionCipher();
            string plainText = "Mother died today. Or maybe yesterday, I don't know. I had a telegram from the home: ‘Mother passed away. Funeral tomorrow. Yours sincerely.' That doesn’t mean anything. It may have been yesterday.";
            plainText = plainText.Replace('’', '\'').Replace('‘', '\'');
            string key = sb.generateNewKey();
            string cipherText = sb.encode(plainText, key);
            string newPlain = sb.decode(cipherText, key);

            Console.WriteLine("Key: " + key + "\nCipher: " + cipherText);
            Analysis analysis = new Analysis();
            analysis.breakSubstitutionCipher(cipherText);
        }
    }
}
