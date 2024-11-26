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
                Console.WriteLine("Unable to get public IP: " + ex.Message);
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


        public static List<string> EncryptDataInChunks(string data, EncryptionType encryptionType, int chunkSize = 1024)
        {
            List<string> encryptedChunks = new List<string>();
            byte[] dataBytes = Encoding.UTF32.GetBytes(data);

            for (int i = 0; i < dataBytes.Length; i += chunkSize)
            {
                int currentChunkSize = Math.Min(chunkSize, dataBytes.Length - i);
                byte[] chunk = new byte[currentChunkSize];
                Array.Copy(dataBytes, i, chunk, 0, currentChunkSize);

                byte[] encryptedBytes = EncryptChunk(chunk, encryptionType);
                encryptedChunks.Add(Convert.ToBase64String(encryptedBytes));
            }

            return encryptedChunks;
        }

        private static byte[] EncryptChunk(byte[] chunk, EncryptionType encryptionType)
        {
            switch (encryptionType)
            {
                case EncryptionType.AES:
                    return EncryptWithAES(chunk);
                case EncryptionType.RSA:
                    return EncryptWithRSA(chunk);
                case EncryptionType.DES:
                    return EncryptWithDES(chunk);
                default:
                    throw new ArgumentException("Unsupported encryption type.");
            }
        }

        private static byte[] EncryptWithAES(byte[] dataBytes)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = GenerateRandomKey(aesAlg.KeySize / 8);
                aesAlg.IV = GenerateRandomKey(aesAlg.BlockSize / 8);

                using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                using (MemoryStream msEncrypt = new MemoryStream())
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(dataBytes, 0, dataBytes.Length);
                    csEncrypt.FlushFinalBlock();
                    return msEncrypt.ToArray();
                }
            }
        }

        private static byte[] EncryptWithRSA(byte[] dataBytes)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.KeySize = 2048;
                return rsa.Encrypt(dataBytes, RSAEncryptionPadding.OaepSHA256);
            }
        }

        private static byte[] EncryptWithDES(byte[] dataBytes)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = GenerateRandomKey(8);
                des.IV = GenerateRandomKey(8); 

                using (ICryptoTransform encryptor = des.CreateEncryptor(des.Key, des.IV))
                using (MemoryStream msEncrypt = new MemoryStream())
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(dataBytes, 0, dataBytes.Length);
                    csEncrypt.FlushFinalBlock();
                    return msEncrypt.ToArray();
                }
            }
        }

        private static byte[] GenerateRandomKey(int size)
        {
            byte[] key = new byte[size];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
            }
            return key;
        }
    }
}

