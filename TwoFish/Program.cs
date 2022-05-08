using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TwoFish
{
    class Program
    {
        static void Main(string[] args)
        {
            Twofish twofish = new Twofish
            {
                KeySize = 256,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };

            string key = "4816ceb76646902674b3201d69bf254daa4ce0a6187cc107eacf172df4bec2bc";
            string iv = "91632dfbda7eb608f131b0dbdd1d2508";

            byte[] key_b = StringToByteArray(key);
            byte[] iv_b = StringToByteArray(iv);

            twofish.Key = key_b;
            twofish.IV = iv_b;

            ICryptoTransform transform = twofish.CreateEncryptor();

            string file = @"E:\dogs_nine-10_1_8.apk";

            string outFile = Path.Combine(@"E:\Encrypt", Path.GetFileName(file) + ".bin");

            using (var outFs = new FileStream(outFile, FileMode.Create)) // Create Output stream, for writing in a file.
            {
                // Now write the cipher text using
                // a CryptoStream for encrypting.

                using (var outStreamEncrypted =
                    new CryptoStream(outFs, transform, CryptoStreamMode.Write)) // Create crypto stream for encryption and writing encrypted file simultaneously. It is created using normal output stream and Crypto transformer
                {
                    // By encrypting a chunk at
                    // a time, you can save memory
                    // and accommodate large files.
                    int currBytes = 0;
                    int count = 0;


                    // blockSizeBytes can be any arbitrary size. Currently it is set to 1,250,000 Bytes
                    //int partSizeBytes = 16*1000000; //10^6 bits = 125000 Bytes
                    int blockSizeBytes = 1000000;
                    byte[] data = new byte[blockSizeBytes];
                    long bytesRead = 0;

                    long totalBytes = new FileInfo(file).Length; // Get total size of file in bytes.                        

                    using (var inFs = new FileStream(file, FileMode.Open)) // Create input stream to open and read plain file.
                    {
                        while ((currBytes = inFs.Read(data, 0, blockSizeBytes)) > 0) //Read 1 block of data using input stream
                        {
                            //Application.DoEvents(); // While encrypting, applicaion continue to respond.
                            outStreamEncrypted.Write(data, 0, currBytes); // Encrypt 1 block of data and write in bin file
                            bytesRead += currBytes; // increment no. of bytes for progress calculation.
                            //lvFiles.Items[i].SubItems[1].Text = (bytesRead * 100 / totalBytes).ToString("###"); // Calculate and display progress in percentage

                            
                        }
                    }

                    data = new byte[1];
                }

            }

        }

        static private byte[] StringToByteArray(String hex) //Hex string to Byte[] converter
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}
