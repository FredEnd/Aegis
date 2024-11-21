using System;
using System.Security.Cryptography;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio;
using NAudio.Wave;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;

namespace Aegis_main
{
    class Mains
    {
        public static string GetPublicIPAddress()
        {
            try
            {
                // Using ipify.org to get the public IP address
                string url = "https://api.ipify.org";
                WebClient client = new WebClient();
                string publicIP = client.DownloadString(url);
                return publicIP.Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine( "Unable to get public IP: " + ex.Message );
                return "null";
            }
        }

        public static void Forms_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Exit the entire application when this form is closed
            Application.Exit();
        }

        public static async void play_notifercation()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            // Navigate back one directory to the Aegis folder
            string resourcesPath = Path.Combine(baseDir, "..\\..\\Resources");

            // Combine and new working db path
            string notificationPath = Path.Combine(resourcesPath, "Notification.mp3");

            // Run the audio playback in a separate task
            await Task.Run(() =>
            {
                using (var audio = new AudioFileReader(notificationPath))
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audio);
                    outputDevice.Play();

                    // Wait until playback is finished
                    while (outputDevice.PlaybackState == PlaybackState.Playing)
                    {
                        Thread.Sleep(100); // Keep checking playback state
                    }
                }
            });
        }

        public enum EncryptionType
        {
            AES,
            RSA,
            DES
        }

        public static List<int> TestPorts(string ipAddress, int startPort, int endPort, int timeout = 100)
        {
            var openPorts = new List<int>();
            object lockObj = new object();

            Parallel.For(startPort, endPort + 1, port =>
            {
                using (TcpClient client = new TcpClient())
                {
                    try
                    {
                        var connectTask = client.ConnectAsync(ipAddress, port);
                        bool connected = connectTask.Wait(timeout);
                        if (connected)
                        {
                            lock (lockObj)
                            {
                                openPorts.Add(port);
                            }
                            Console.WriteLine($"Port {port} is open!");
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"Port {port} is closed.");
                    }
                }
            });

            return openPorts;
        }

        public static async Task StartServerAsync(int port)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine($"Server listening on port {port}...");

            while (true)
            {
                try
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    Console.WriteLine("Client connected!");

                    _ = HandleClientAsync(client);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Server error: {ex.Message}");
                }
            }
        }

        private static async Task HandleClientAsync(TcpClient client)
        {
            using (NetworkStream stream = client.GetStream())
            {
                byte[] buffer = new byte[1024];

                while (true)
                {
                    try
                    {
                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        if (bytesRead == 0) break;

                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Console.WriteLine($"Received: {message}");

                        byte[] response = Encoding.UTF8.GetBytes($"Echo: {message}");
                        await stream.WriteAsync(response, 0, response.Length);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error handling client: {ex.Message}");
                        break;
                    }
                }
            }

            Console.WriteLine("Client disconnected.");
        }

        /*
        public static string EncryptData(string data, EncryptionType encryptionType)
        {
            byte[] encryptedBytes;

            // Convert the data to a byte array
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            switch (encryptionType)
            {
                case EncryptionType.AES:
                    using (Aes aesAlg = Aes.Create())
                    {
                        aesAlg.Key = aesAlg.GenerateKey(); // Generate a random key
                        aesAlg.IV = aesAlg.GenerateIV();   // Generate a random IV
                        encryptedBytes = EncryptWithAES(dataBytes, aesAlg.Key, aesAlg.IV);
                    }
                    break;

                case EncryptionType.RSA:
                    using (RSA rsa = RSA.Create())
                    {
                        rsa.KeySize = 2048; // Set RSA key size
                        rsa.GenerateKeyPair(); // Generate keys
                        encryptedBytes = EncryptWithRSA(dataBytes, rsa.ExportParameters(false)); // Only public key is needed for encryption
                    }
                    break;

                case EncryptionType.DES:
                    using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                    {
                        des.Key = des.GenerateKey(); // Generate a random key
                        des.IV = des.GenerateIV();   // Generate a random IV
                        encryptedBytes = EncryptWithDES(dataBytes, des.Key, des.IV);
                    }
                    break;

                default:
                    throw new ArgumentException("Unsupported encryption type.");
            }

            // Return the encrypted data as a base64 string
            return Convert.ToBase64String(encryptedBytes);
        }

        private static byte[] EncryptWithAES(byte[] dataBytes, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                using (MemoryStream msEncrypt = new MemoryStream())
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(Encoding.UTF8.GetString(dataBytes));
                    return msEncrypt.ToArray();
                }
            }
        }

        private static byte[] EncryptWithRSA(byte[] dataBytes, RSAParameters publicKey)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportParameters(publicKey);
                return rsa.Encrypt(dataBytes, RSAEncryptionPadding.OaepSHA256);
            }
        }

        private static byte[] EncryptWithDES(byte[] dataBytes, byte[] key, byte[] iv)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = key;
                des.IV = iv;
                using (ICryptoTransform encryptor = des.CreateEncryptor(des.Key, des.IV))
                using (MemoryStream msEncrypt = new MemoryStream())
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(Encoding.UTF8.GetString(dataBytes));
                    return msEncrypt.ToArray();
                }
            }
        }
        */
    }
}
