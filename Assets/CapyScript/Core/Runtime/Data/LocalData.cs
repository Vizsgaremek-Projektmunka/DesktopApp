using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CapyScript.Data
{
    public static class LocalData
    {
        private const string KEY = "uZWmBDQi9SSLZz956QENylMbOFw1huSdcR3mSiIjhBo=";
        private const string IV = "clZpGsdMXT5UHeCvTJJXHQ==";

        public enum Encryption
        {
            None,
            Encrypted
        }
        
        public static bool Save<T>(string path, T data, Encryption encryption = Encryption.None)
        {
            if (data == null)
            {
                Debug.LogError("Saving of " + typeof(T).ToString() + " failed because it's null.");
                return false;
            }

            string realPath = Path.Combine(Application.persistentDataPath, path);

            if (File.Exists(realPath))
            {
                File.Delete(realPath);
            }

            FileStream stream = File.Create(realPath);

            if (encryption == Encryption.None)
            {
                stream.Close();
            }

            try
            {
                if (encryption == Encryption.None)
                {
                    File.WriteAllText(realPath, JsonConvert.SerializeObject(data));
                }
                else
                {
                    using Aes aes = Aes.Create();
                    aes.Key = Convert.FromBase64String(KEY);
                    aes.IV = Convert.FromBase64String(IV);
                    using ICryptoTransform cryptoTransform = aes.CreateEncryptor();
                    using CryptoStream cryptoStream = new CryptoStream(stream, cryptoTransform, CryptoStreamMode.Write);

                    cryptoStream.Write(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data)));
                    stream.Close();
                }
            }
            catch (Exception e)
            {
                if (File.Exists(realPath))
                {
                    File.Delete(realPath);
                }

                if (encryption == Encryption.Encrypted)
                {
                    stream.Close();
                }

                Debug.LogException(e);
                Debug.LogError("Saving of " + typeof(T).ToString() + " failed!");

                return false;
            }

            return true;
        }

        public static async Task<bool> SaveAsync<T>(string path, T data, Encryption encryption = Encryption.None)
        {
            if (data == null)
            {
                Debug.LogError("Saving of " + typeof(T).ToString() + " failed because it's null.");
                return false;
            }

            string realPath = Path.Combine(Application.persistentDataPath, path);

            if (File.Exists(realPath))
            {
                File.Delete(realPath);
            }

            FileStream stream = File.Create(realPath);

            if (encryption == Encryption.None)
            {
                stream.Close();
            }

            try
            {
                if (encryption == Encryption.None)
                {
                    await File.WriteAllTextAsync(realPath, JsonConvert.SerializeObject(data));
                }
                else
                {
                    using Aes aes = Aes.Create();
                    aes.Key = Convert.FromBase64String(KEY);
                    aes.IV = Convert.FromBase64String(IV);
                    using ICryptoTransform cryptoTransform = aes.CreateEncryptor();
                    using CryptoStream cryptoStream = new CryptoStream(stream, cryptoTransform, CryptoStreamMode.Write);

                    await cryptoStream.WriteAsync(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data)));
                    stream.Close();
                }
            }
            catch (Exception e)
            {
                if (File.Exists(realPath))
                {
                    File.Delete(realPath);
                }

                if (encryption == Encryption.Encrypted)
                {
                    stream.Close();
                }

                Debug.LogException(e);
                Debug.LogError("Saving of " + typeof(T).ToString() + " failed!");

                return false;
            }

            return true;
        }

        public static T Load<T>(string path, Encryption encryption = Encryption.None)
        {
            string realPath = Path.Combine(Application.persistentDataPath, path);

            if (!File.Exists(realPath))
            {
                Debug.LogError("Unable to load " + realPath + " because the file doesn't exist!");
                return default;
            }

            try
            {
                T data;

                if (encryption == Encryption.None)
                {
                    data = JsonConvert.DeserializeObject<T>(File.ReadAllText(realPath));
                }
                else
                {
                    byte[] bytes = File.ReadAllBytes(realPath);
                    
                    using Aes aes = Aes.Create();
                    aes.Key = Convert.FromBase64String(KEY);
                    aes.IV = Convert.FromBase64String(IV);
                    using ICryptoTransform cryptoTransform = aes.CreateDecryptor(aes.Key, aes.IV);
                    using MemoryStream decryptionStream = new MemoryStream(bytes);
                    using CryptoStream cryptoStream = new CryptoStream(decryptionStream, cryptoTransform, CryptoStreamMode.Read);
                    using StreamReader reader = new StreamReader(cryptoStream);

                    string result = reader.ReadToEnd();
                    data = JsonConvert.DeserializeObject<T>(result);
                }

                return data;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                Debug.LogError("Unable to load " + realPath + " as " + typeof(T).ToString() + "!");
                return default;
            }
        }

        public static async Task<T> LoadAsync<T>(string path, Encryption encryption = Encryption.None)
        {
            string realPath = Path.Combine(Application.persistentDataPath, path);

            if (!File.Exists(realPath))
            {
                Debug.LogError("Unable to load " + realPath + " because the file doesn't exist!");
                return default;
            }

            try
            {
                T data;

                if (encryption == Encryption.None)
                {
                    data = JsonConvert.DeserializeObject<T>(await File.ReadAllTextAsync(realPath));
                }
                else
                {
                    byte[] bytes = await File.ReadAllBytesAsync(realPath);

                    using Aes aes = Aes.Create();
                    aes.Key = Convert.FromBase64String(KEY);
                    aes.IV = Convert.FromBase64String(IV);
                    using ICryptoTransform cryptoTransform = aes.CreateDecryptor(aes.Key, aes.IV);
                    using MemoryStream decryptionStream = new MemoryStream(bytes);
                    using CryptoStream cryptoStream = new CryptoStream(decryptionStream, cryptoTransform, CryptoStreamMode.Read);
                    using StreamReader reader = new StreamReader(cryptoStream);

                    string result = await reader.ReadToEndAsync();
                    data = JsonConvert.DeserializeObject<T>(result);
                }

                return data;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                Debug.LogError("Unable to load " + realPath + " as " + typeof(T).ToString() + "!");
                return default;
            }
        }
    }
}
