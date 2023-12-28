using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AES_Matthieu_Zimmermann_MaxenceVan_Laere
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Write down the sentence you want to encrypt :");
            string originalText = Console.ReadLine();

            string key = null;
            
            char userInput;

            while (true)
            {
                Console.WriteLine("Choose the key encryption you want: 1, 2, 3 or 4 ");

                bool isCharValid = char.TryParse(Console.ReadLine(), out userInput);

                if (isCharValid && (userInput == '1' || userInput == '2' || userInput == '3' || userInput == '4'))
                {
                    switch (userInput)
                    {
                        case '1':
                            key = "mysecretkey12345";
                            break;
                        case '2':
                            key = "abcdefgh12345678";
                            break;
                        case '3':
                            key = "abc123XYZ456789!";
                            break;
                        case '4':
                            key = "😊🎶🔒🌈";
                            break;
                    }
                    break; 
                }
                else
                {
                    Console.WriteLine("Error : value is invalid");
                }
            }

            Console.WriteLine($"Original Text: {originalText}");

            string encryptedText = Encrypt(originalText, key);
            Console.WriteLine($"Encrypted Text: {encryptedText}");

            string decryptedText = Decrypt(encryptedText, key);
            Console.WriteLine($"Decrypted Text: {decryptedText}");
        }

        static string Encrypt(string text, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = new byte[16]; // Use a unique IV for each encryption

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }
                    }

                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        static string Decrypt(string cipherText, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = new byte[16]; // Use the same IV that was used for encryption

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
