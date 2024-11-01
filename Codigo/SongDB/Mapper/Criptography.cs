using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace PnT.SongDB.Mapper
{
    /// <summary>
    /// Enumerator with cryptography class types.
    /// </summary>
    public enum CryptProvider
    {
        /// <summary>
        /// Represents the base class for implementation of Rijndael cryptography algorithm.
        /// </summary>
        Rijndael,

        /// <summary>
        /// Represents the base class for implementation of RC2 cryptography algorithm.
        /// </summary>
        RC2,

        /// <summary>
        /// Represents the base class for implementation of DES Data cryptography.
        /// </summary>
        DES,

        /// <summary>
        /// Represents the base class for implementation of TripleDES cryptography.
        /// </summary>
        TripleDES
    }

    /// <summary>
    /// Class used for cryptography Services.
    /// </summary>
    public class Criptography
    {

        #region Fields ****************************************************************

        private string _key = string.Empty;

        /// <summary>
        /// Gets or sets the secret key used by the symmetric cryptography algorithm.
        /// </summary>
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        private const string efKey = "EFT3stEncrIpti0nKey!";
        private CryptProvider _cryptProvider;
        private SymmetricAlgorithm _algorithm;

        #endregion Fields


        #region Constructors **********************************************************

        /// <summary>
        /// Class default constructor sets a default encryption algorithm (TripleDES).
        /// </summary>
        public Criptography()
        {
            _algorithm = new TripleDESCryptoServiceProvider();
            _cryptProvider = CryptProvider.TripleDES;
            _algorithm.Mode = CipherMode.CBC;
        }

        /// <summary>
        /// Constructor that accepts a cryptography type.
        /// </summary>
        /// <param name="cryptProvider">Criptography type.</param>
        public Criptography(CryptProvider cryptProvider)
        {
            // Selects symmetric algorithm
            switch (cryptProvider)
            {
                case CryptProvider.Rijndael:
                    _algorithm = new RijndaelManaged();
                    _cryptProvider = CryptProvider.Rijndael;
                    break;
                case CryptProvider.RC2:
                    _algorithm = new RC2CryptoServiceProvider();
                    _cryptProvider = CryptProvider.RC2;
                    break;
                case CryptProvider.DES:
                    _algorithm = new DESCryptoServiceProvider();
                    _cryptProvider = CryptProvider.DES;
                    break;
                case CryptProvider.TripleDES:
                    _algorithm = new TripleDESCryptoServiceProvider();
                    _cryptProvider = CryptProvider.TripleDES;
                    break;
            }
            _algorithm.Mode = CipherMode.CBC;
        }

        #endregion Constructors


        #region Public methods ********************************************************

        /// <summary>
        /// Generates valid cryptographic key in the array.
        /// </summary>
        public byte[] GetKey()
        {
            string salt = string.Empty;

            // Adjusts key size if needed.
            if (_algorithm.LegalKeySizes.Length > 0)
            {
                // Key size in bits
                int keySize = _key.Length * 8;
                int minSize = _algorithm.LegalKeySizes[0].MinSize;
                int maxSize = _algorithm.LegalKeySizes[0].MaxSize;
                int skipSize = _algorithm.LegalKeySizes[0].SkipSize;

                if (keySize > maxSize)
                {
                    // gets key max size
                    _key = _key.Substring(0, maxSize / 8);
                }
                else if (keySize < maxSize)
                {
                    // sets key valid size
                    int validSize = (keySize <= minSize) ? minSize : (keySize - keySize % skipSize) + skipSize;
                    if (keySize < validSize)
                    {
                        // completes key size with "*"
                        _key = _key.PadRight(validSize / 8, '*');
                    }
                }
            }
            PasswordDeriveBytes key = new PasswordDeriveBytes(_key, ASCIIEncoding.ASCII.GetBytes(salt));
            return key.GetBytes(_key.Length);
        }

        /// <summary>
        /// Encrypts given data.
        /// </summary>
        /// <param name="plainText">Text to be ciphered.</param>
        /// <returns>Ciphered text.</returns>
        public string GenericEncrypt(string plainText)
        {
            byte[] plainByte = ASCIIEncoding.ASCII.GetBytes(plainText);
            byte[] keyByte = GetKey();

            // Sets private key
            _algorithm.Key = keyByte;
            SetIV();

            // Cryptography interface / Creates cryptography object
            ICryptoTransform cryptoTransform = _algorithm.CreateEncryptor();

            MemoryStream _memoryStream = new MemoryStream();

            CryptoStream _cryptoStream = new CryptoStream(_memoryStream, cryptoTransform, CryptoStreamMode.Write);

            // Places ciphered data into MemoryStream
            _cryptoStream.Write(plainByte, 0, plainByte.Length);
            _cryptoStream.FlushFinalBlock();

            // Gets encrypted byte length on memory stream.
            byte[] cryptoByte = _memoryStream.ToArray();

            // Converts to base 64 string for xml
            return Convert.ToBase64String(cryptoByte, 0, cryptoByte.GetLength(0));
        }

        /// <summary>
        /// Encrypts given data.
        /// </summary>
        /// <param name="key">Text to be ciphered.</param>
        /// <returns>Ciphered text.</returns>
        public string EncryptKey(string key)
        {
            //keep old key value
            string oldKeyValue = _key;

            //same as encrypt, but uses default private key
            _key = efKey;

            string result = GenericEncrypt(key);

            _key = oldKeyValue;

            return result;
        }

        /// <summary>
        /// Decrypts given data.
        /// </summary>
        /// <param name="cryptoText">Text to be decrypted.</param>
        /// <returns>Decrypted text.</returns>
        public string GenericDecrypt(string cryptoText)
        {
            // Converts a base 64 string in a byte array
            byte[] cryptoByte = Convert.FromBase64String(cryptoText);
            byte[] keyByte = GetKey();

            // sets private key
            _algorithm.Key = keyByte;
            SetIV();

            // Cryptographic interface / Creates decryption object
            ICryptoTransform cryptoTransform = _algorithm.CreateDecryptor();
            try
            {
                MemoryStream _memoryStream = new MemoryStream(cryptoByte, 0, cryptoByte.Length);

                CryptoStream _cryptoStream = new CryptoStream(_memoryStream, cryptoTransform, CryptoStreamMode.Read);

                // Gets result
                StreamReader _streamReader = new StreamReader(_cryptoStream);
                return _streamReader.ReadToEnd();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Decrypts given data.
        /// </summary>
        /// <param name="cryptoKey">Text to be decrypted.</param>
        /// <returns>Decrypted text.</returns>
        public string DecryptKey(string cryptoKey)
        {
            //keep old key value
            string oldKeyValue = _key;

            //same as encrypt, but uses default private key
            _key = efKey;

            string result = GenericDecrypt(cryptoKey);

            _key = oldKeyValue;

            return result;
        }

        /// <summary>
        /// Encrypts a string to a byte code with Md5 Encryption.
        /// </summary>
        /// <param name="TextValue">text to be encrypted.</param>
        /// <returns>byte array with encrypted value.</returns>
        static public byte[] Encrypt(string TextValue)
        {
            System.Text.UTF8Encoding UTFEncoder = new System.Text.UTF8Encoding();

            //Translates our text value into a byte array.
            Byte[] data = UTFEncoder.GetBytes(TextValue);

            SHA1 sha = new SHA1CryptoServiceProvider();
            return sha.ComputeHash(data);
        }

        /// <summary>
        /// C# to convert a string to a byte array. 
        /// </summary>
        /// <param name="str">String to be converted.</param>
        public static byte[] StringToByteArray(string str)
        {
            //Convert string in char array.
            char[] stage = str.ToCharArray();
            //Create array of bytes
            byte[] bArray = new byte[stage.Length];
            //Copy array of char to array of bytes
            for (int i = 0; i < stage.Length; i++)
            {
                bArray[i] = (byte)stage[i];
            }

            return bArray;
        }

        /// <summary>
        /// C# to convert a byte array to a string.
        /// </summary>
        /// <param name="dBytes">Byte array to be converted.</param>
        public static string ByteArrayToString(byte[] dBytes)
        {
            //Create char array to accommodate bytes.
            char[] chrArray = new char[dBytes.Length];
            //Copy bytes to char array
            for (int i = 0; i < dBytes.Length; i++)
            {
                chrArray[i] = (char)dBytes[i];
            }
            //Create string from char array
            string str = new string(chrArray);

            //return string
            return str;
        }

        /// <summary>
        /// Convert a array of bytes into a sequence of numeric values.
        /// Each numeric value is separated by a ';'.
        /// </summary>
        /// <param name="dBytes">The input array of bytes</param>
        /// <returns>The output string of numeric values.</returns>
        public static string ByteArrayToValueSequence(byte[] dBytes)
        {
            StringBuilder outputContent = new StringBuilder();
            foreach (byte currentByte in dBytes)
            {
                //add byte as a string
                outputContent.Append(currentByte.ToString());

                //add separator
                outputContent.Append(';');
            }

            //return created content
            return outputContent.ToString();
        }

        /// <summary>
        /// Recover an array of bytes from a sequence of numeric values.
        /// Each numeric value correspond to one byte.
        /// </summary>
        /// <param name="str">The sequence of numeri values.</param>
        /// <returns>The recovered array of bytes.</returns>
        public static byte[] ValueSequenceToByteArray(string str)
        {
            //split numeric values
            string[] numericValues = str.Split(new char[] { ';' });

            //list of bytes
            List<byte> recoveredBytes = new List<byte>();

            //check each numeric value
            foreach (string numericValue in numericValues)
            {
                //check length
                if (numericValue.Length == 0)
                {
                    continue;
                }

                //parse byte
                byte newByte = byte.Parse(numericValue);

                //add byte to list of bytes
                recoveredBytes.Add(newByte);
            }

            //return array of bytes
            return recoveredBytes.ToArray();
        }

        /// <summary>
        /// Reads license information and returns the data read.
        /// A list of strings with MAC address, date, key and product name.
        /// </summary>
        public List<string> ReadLicense(string data)
        {
            string encryptedKey = data.Split('\r')[0];

            string key = this.DecryptKey(encryptedKey);

            this.Key = key;

            string license = this.GenericDecrypt(data.Split('\r')[1]);

            string m = license.Substring(0, 3) + license.Substring(5, 3) + license.Substring(10, 3) + license.Substring(15, 3);

            string d = license.Substring(3, 2) + license.Substring(8, 2) + license.Substring(13, 2) + license.Substring(18, 2);

            string name = license.Substring(20);

            return new List<string>() { m, d, key, name };
        }

        #endregion Public methods


        #region Private Methods *******************************************************

        private void SetIV()
        {
            switch (_cryptProvider)
            {
                case CryptProvider.Rijndael:
                    _algorithm.IV = new byte[] { 0xf, 0x6f, 0x13, 0x2e, 0x35, 0xc2,
                        0xcd, 0xf9, 0x5, 0x46, 0x9c, 0xea, 0xa8, 0x4b, 0x73, 0xcc };
                    break;
                default:
                    _algorithm.IV = new byte[] { 0xf, 0x6f, 0x13, 0x2e, 0x35, 0xc2, 0xcd, 0xf9 };
                    break;
            }
        }

        #endregion Private Methods

    } //end of class Criptography

} //end of namespace PnT.SongDB.Mapper
