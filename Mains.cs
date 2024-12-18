﻿using System;
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
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Net.NetworkInformation;

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
        } //Gets the public IP if it is a public connection

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        } //Gets the local IP if it is a local connection

        public static void Forms_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Exit the entire application when this form is closed
            Application.Exit();
        } //Handles when the form is closing

        public static async void play_notifercation()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            Console.WriteLine("base Dir -- ", baseDir);

            string notificationPath = Path.Combine(baseDir, "Resources", "Notification.mp3");
            Console.WriteLine("Not Path --", notificationPath);


            await Task.Run(() =>
            {
                using (var audio = new AudioFileReader(notificationPath))
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audio);
                    outputDevice.Play();

                    while (outputDevice.PlaybackState == PlaybackState.Playing)
                    {
                        Thread.Sleep(100);
                    }
                }
            });
        } //Plays the notifercation at the start or if the host or  client recieves a message.

        public enum EncryptionType
        {
            AES,
            RSA,
            DES
        } //Used to hold all of the encryption types

        public static Task<List<int>> TestLocalPortsAsync(string localIpAddress, int startPort, int endPort) 
        {
            var availablePorts = new List<int>();
            object lockObj = new object();

            if (!IPAddress.TryParse(localIpAddress, out IPAddress ipAddress))
            {
                Console.WriteLine($"Invalid IP Address: {localIpAddress}");
                return Task.FromResult(availablePorts);
            }

            Console.WriteLine(startPort);
            Console.WriteLine(endPort);

            for (int port = startPort; port <= endPort; port++)
            {
                Console.WriteLine(port);

                try
                {
                    TcpListener listener = new TcpListener(ipAddress, port);
                    listener.Start();

                    lock (lockObj)
                    {
                        availablePorts.Add(port);
                    }

                    Console.WriteLine($"Port {port} on {localIpAddress} is available and can be used for listening.");
                    listener.Stop();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Port {port} on {localIpAddress} is not available or there was an error: {ex.Message}");
                }
            }
            return Task.FromResult(availablePorts);
        } //Port scanner form the Local connection

        public static async Task<List<int>> TestPortsAsync(string ipAddress, int startPort, int endPort, int timeout = 100)
        {
            var openPorts = new List<int>();
            var tasks = new List<Task>();

            for (int port = startPort; port <= endPort; port++)
            {
                int currentPort = port;

                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        TcpListener listener = new TcpListener(IPAddress.Parse(ipAddress), currentPort);
                        listener.Start();

                        var listenerTask = listener.AcceptSocketAsync();
                        bool connected = await Task.WhenAny(listenerTask, Task.Delay(timeout)) == listenerTask;

                        if (connected)
                        {
                            using (Socket socket = listenerTask.Result)
                            {
                                byte[] buffer = new byte[4096];
                                int bytesRead = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);

                                if (bytesRead > 0)
                                {
                                    openPorts.Add(currentPort);
                                    Console.WriteLine($"Port {currentPort} is open and responsive!");
                                }
                                else
                                {
                                    Console.WriteLine($"Port {currentPort} is open, but no response received.");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Port {currentPort} is not available within the timeout.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error on port {currentPort}: {ex.Message}");
                    }
                }));
            }

            await Task.WhenAll(tasks);

            return openPorts;
        } //Port Scannner for the Public connection (comes up as malware when compiled...)



        //-------------------------------------------------------------------------------------------------

        public static string GenerateSessionCode(string ipAddress, int port, string sessionID, string host, string encryption)
        {
            Console.WriteLine($"{ipAddress}, {port}, {sessionID}, {host}, {encryption}");

            if (string.IsNullOrWhiteSpace(ipAddress) || string.IsNullOrWhiteSpace(sessionID) || string.IsNullOrWhiteSpace(encryption))
            {
                throw new ArgumentException("IP address, SessionID, and Encryption must not be empty.");
            }

            if (port < 0 || port > 65535)
            {
                throw new ArgumentOutOfRangeException(nameof(port), "Port must be in the range 0-65535.");
            }

            return $"{ipAddress}/{port}/{sessionID}/{host}/{encryption}";
        } //Generates a session code for the host to give to the clients.


        public static (string ipAddress, int port, string sessionID, string host, string encryption) InfoFromSessionCode(string sessionCode)
        {
            if (string.IsNullOrWhiteSpace(sessionCode))
            {
                MessageBox.Show("Session code must not be empty or null.");
                throw new ArgumentException("Session code cannot be null or empty.");
            }

            string[] parts = sessionCode.Split('/');

            if (parts.Length < 5)
            {
                MessageBox.Show("Invalid session code format. Expected format: IPaddress/Port/SessionID/HostID/Encryption.");
                throw new FormatException("Session code is missing required parts. Expected format: IPaddress/Port/SessionID/HostID/Encryption.");
            }

            string ipAddress = parts[0];

            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                MessageBox.Show("IP address must not be empty.");
                throw new ArgumentException("IP address cannot be null or empty.");
            }

            if (!int.TryParse(parts[1], out int port) || port < 0 || port > 65535)
            {
                MessageBox.Show("Port must be a number between 0 and 65535.");
                throw new FormatException("Port must be between 0 and 65535.");
            }

            string sessionID = parts[2];

            if (string.IsNullOrWhiteSpace(sessionID))
            {
                MessageBox.Show("Session ID must not be empty.");
                throw new ArgumentException("Session ID cannot be null or empty.");
            }

            string host = parts[3];

            if (string.IsNullOrWhiteSpace(host))
            {
                MessageBox.Show("Host must not be empty.");
                throw new ArgumentException("Host cannot be null or empty.");
            }

            string encryption = parts[4];

            if (string.IsNullOrWhiteSpace(encryption))
            {
                MessageBox.Show("Encryption must not be empty.");
                throw new ArgumentException("Encryption cannot be null or empty.");
            }

            return (ipAddress, port, sessionID, host, encryption);
        } //Splits up the ssession code and returns it back to its original data.




        //-------------------------------------------------------------------------------------------------

        public static async Task<TcpClient> AcceptClientAsync(TcpListener listener)
        {
            try
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                Console.WriteLine("Client connected!");
                return client;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Server error: {ex.Message}");
                return null;
            }
        } //Old accept client function


        public static async Task<(string ReceivedMessage, NetworkStream Stream)> HandleClientAsync(TcpClient client)
        {
            Console.WriteLine("Receiving message and converting to string.");
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[4096];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                if (bytesRead > 0)
                {
                    string receivedMessage = Encoding.UTF32.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Server received: {receivedMessage}");

                    return (receivedMessage, stream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling client: {ex.Message}");
            }
            return (null, null);
        } //Handle client function



        public static async Task<(TcpClient client, NetworkStream stream)> ConnectAsync(string ipAddress, int port)
        {
            TcpClient client = null;
            NetworkStream stream = null;

            try
            {
                client = new TcpClient();
                await client.ConnectAsync(ipAddress, port);
                Console.WriteLine($"Connected to {ipAddress}:{port}");

                stream = client.GetStream();

                _ = Task.Run(() => ListenForMessagesAsync(stream));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to peer: {ex.Message}");
            }
            return (client, stream);
        } //Old Client connection function

        private static async Task ListenForMessagesAsync(NetworkStream stream)
        {
            byte[] buffer = new byte[4096];

            try
            {
                while (true)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                    {
                        MessageBox.Show("Connection closed by remote peer.");
                        break;
                    }

                    string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Received: {receivedMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while listening for messages: {ex.Message}");
            }
        } //listen for messages reference 


        //-------------------------------------------------------------------------------------------------

        // encrypts string for transport via TCP

        public static (List<string> EncryptedDataChunks, string EncryptedKey, string IV) EncryptDataWithKeyTransport(
            string data,
            string encryptionType,
            RSAParameters receiverPublicKey,
            int chunkSize = 245)
        {
            if (encryptionType.ToUpper() == "RSA")
            {
                List<string> encryptedDataChunks = EncryptDataInChunksWithRSA(data, receiverPublicKey, chunkSize);

                return (encryptedDataChunks, null, null);
            }
            else
            {
                byte[] symmetricKey = GenerateRandomKey(32);
                byte[] iv = GenerateRandomKey(16);

                byte[] key = PrepareKey(encryptionType, symmetricKey);
                byte[] fixediv =PrepareIV(encryptionType, iv);

                List<string> encryptedDataChunks = EncryptDataInChunks(data, encryptionType, key, fixediv, chunkSize);

                byte[] encryptedKey = EncryptSymmetricKeyWithRSA(key, receiverPublicKey);

                if (iv != null)
                {
                    string responseIV = Convert.ToBase64String(fixediv);
                    return (encryptedDataChunks, Convert.ToBase64String(encryptedKey), responseIV);
                }
                else
                {
                    return (encryptedDataChunks, Convert.ToBase64String(encryptedKey), null);
                }
            }
        } // Takes the data and encryption type and encrypts it into the selected encryption

        public static List<string> EncryptDataInChunks(string data, string encryptionType, byte[] symmetricKey, byte[] iv, int chunkSize = 1024)
        {
            List<string> encryptedChunks = new List<string>();
            byte[] dataBytes = Encoding.UTF32.GetBytes(data);

            for (int i = 0; i < dataBytes.Length; i += chunkSize)
            {
                int currentChunkSize = Math.Min(chunkSize, dataBytes.Length - i);
                byte[] chunk = new byte[currentChunkSize];
                Array.Copy(dataBytes, i, chunk, 0, currentChunkSize);

                byte[] encryptedBytes = EncryptChunk(chunk, encryptionType, symmetricKey, iv);

                // Convert to Base64 and add to the list
                encryptedChunks.Add(Convert.ToBase64String(encryptedBytes));
            }

            return encryptedChunks;
        } //Encrypts UTF 32 data chunks into the designated encryption type returns string of data.

        private static List<string> EncryptDataInChunksWithRSA(string data, RSAParameters publicKey, int chunkSize)
        {
            List<string> encryptedChunks = new List<string>();
            byte[] dataBytes = Encoding.UTF32.GetBytes(data);

            for (int i = 0; i < dataBytes.Length; i += chunkSize)
            {
                int currentChunkSize = Math.Min(chunkSize, dataBytes.Length - i);
                byte[] chunk = new byte[currentChunkSize];
                Array.Copy(dataBytes, i, chunk, 0, currentChunkSize);

                byte[] encryptedBytes = EncryptWithRSA(chunk, publicKey);

                // Convert to Base64 and add to the list
                encryptedChunks.Add(Convert.ToBase64String(encryptedBytes));
            }

            return encryptedChunks;
        } //Encrypts chunks into RSA

        private static byte[] EncryptChunk(byte[] chunk, string encryptionType, byte[] key, byte[] iv)
        {
            switch (encryptionType.ToUpper()) // Convert to uppercase for consistency
            {
                case "AES":
                    return EncryptWithAES(chunk, key, iv);
                case "DES":

                    return EncryptWithDES(chunk, key, iv);
                default:
                    throw new ArgumentException("Unsupported encryption type for chunk encryption.");
            }
        } // IV encryption selections

        private static byte[] EncryptWithAES(byte[] dataBytes, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                using (MemoryStream msEncrypt = new MemoryStream())
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(dataBytes, 0, dataBytes.Length);
                    csEncrypt.FlushFinalBlock();
                    return msEncrypt.ToArray();
                }
            }
        } //What is says on the box

        private static byte[] TruncateKeyForDES(byte[] key)
        {
            if (key.Length > 8)
                return key.Take(8).ToArray(); 
            else if (key.Length < 8)
                throw new ArgumentException("Key is too short for DES (minimum 8 bytes required).");
            return key;
        }

        private static byte[] AdjustKeyForAES(byte[] key, int desiredKeyLength)
        {
            if (key.Length > desiredKeyLength)
                return key.Take(desiredKeyLength).ToArray(); 
            else if (key.Length < desiredKeyLength)
                return key.Concat(new byte[desiredKeyLength - key.Length]).ToArray(); 
            return key;
        }

        private static byte[] PrepareKey(string encryptionType, byte[] key)
        {
            if (encryptionType.ToUpper() == "DES")
            {
                return TruncateKeyForDES(key); 
            }
            else if (encryptionType.ToUpper() == "AES")
            {
                return AdjustKeyForAES(key, 32); 
            }
            else
            {
                throw new ArgumentException($"Unsupported encryption type: {encryptionType}");
            }
        }

        private static byte[] PrepareIV(string encryptionType, byte[] iv)
        {
            if (encryptionType.ToUpper() == "DES")
            {
                return TruncateIVForDES(iv); 
            }
            else if (encryptionType.ToUpper() == "AES")
            {
                return AdjustIVForAES(iv); 
            }
            else
            {
                throw new ArgumentException($"Unsupported encryption type: {encryptionType}");
            }
        }

        private static byte[] TruncateIVForDES(byte[] iv)
        {
            if (iv.Length >= 8)
            {
                return iv.Take(8).ToArray(); 
            }
            else
            {
                throw new ArgumentException("IV for DES must be at least 8 bytes long.");
            }
        }

        private static byte[] AdjustIVForAES(byte[] iv)
        {
            if (iv.Length >= 16)
            {
                return iv.Take(16).ToArray(); 
            }
            else
            {
                throw new ArgumentException("IV for AES must be at least 16 bytes long.");
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
                {
                    csEncrypt.Write(dataBytes, 0, dataBytes.Length);
                    csEncrypt.FlushFinalBlock();
                    return msEncrypt.ToArray();
                }
            }
        } //What is says on the box

        private static byte[] EncryptWithRSA(byte[] dataBytes, RSAParameters publicKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(publicKey);
                return rsa.Encrypt(dataBytes, false);
            }
        } //What is says on the box

        public static byte[] EncryptSymmetricKeyWithRSA(byte[] key, RSAParameters publicKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(publicKey);
                return rsa.Encrypt(key, false);
            }
        } //Encrypts the symetric key to be sent

        public static (RSAParameters PublicKey, RSAParameters PrivateKey) GenerateRSAKeyPair()
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.KeySize = 2048;
                return (rsa.ExportParameters(false), rsa.ExportParameters(true)); // Public and Private keys
            }
        } // key pair creation

        private static byte[] GenerateRandomKey(int size)
        {
            byte[] key = new byte[size];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
            }
            return key;
        } // random key generator


        //-------------------------------------------------------------------------------------------------

        public static string DecryptDataWithKeyTransport(
             List<string> encryptedDataChunks,
             string encryptedKey,
             string encryptionType,
             RSAParameters receiverPrivateKey,
             string ivString)
        {
            byte[] iv = null;

            if (encryptionType.ToUpper() != "RSA")
            {
                if (ivString == null)
                {
                    throw new ArgumentNullException(nameof(ivString), "IV cannot be null for non-RSA encryption.");
                }
                iv = Convert.FromBase64String(ivString);
            }

            byte[] symmetricKey = null;

            if (encryptionType.ToUpper() != "RSA")
            {
                if (string.IsNullOrEmpty(encryptedKey))
                {
                    throw new ArgumentNullException(nameof(encryptedKey), "Encrypted key cannot be null or empty for non-RSA encryption.");
                }
                symmetricKey = DecryptSymmetricKeyWithRSA(Convert.FromBase64String(encryptedKey), receiverPrivateKey);
            }

            if (encryptionType.ToUpper() == "RSA")
            {
                string decryptedData = DecryptDataChunksWithRSA(encryptedDataChunks, receiverPrivateKey);
                return decryptedData;
            }
            else
            {
                string decryptedData = DecryptDataChunks(encryptedDataChunks, encryptionType, symmetricKey, iv);
                return decryptedData;
            }
            
        } // Takes the transported data and key and decrpts them


        private static string DecryptDataChunksWithRSA(List<string> encryptedChunks, RSAParameters privateKey)
        {
            List<byte> decryptedBytes = new List<byte>();

            foreach (string encryptedChunk in encryptedChunks)
            {
                byte[] encryptedData = Convert.FromBase64String(encryptedChunk);
                byte[] decryptedChunk = DecryptWithRSA(encryptedData, privateKey);
                decryptedBytes.AddRange(decryptedChunk);
            }

            return Encoding.UTF32.GetString(decryptedBytes.ToArray());
        } //Decrypts Chunks with RSA

        private static string DecryptDataChunks(List<string> encryptedChunks, string encryptionType, byte[] key, byte[] iv)
        {
            List<byte> decryptedBytes = new List<byte>();

            foreach (string encryptedChunk in encryptedChunks)
            {
                byte[] encryptedData = Convert.FromBase64String(encryptedChunk);
                byte[] decryptedChunk = DecryptChunk(encryptedData, encryptionType, key, iv);
                decryptedBytes.AddRange(decryptedChunk);
            }

            return Encoding.UTF32.GetString(decryptedBytes.ToArray());
        } // Decrypts chunks selection

        private static byte[] DecryptChunk(byte[] encryptedChunk, string encryptionType, byte[] key, byte[] iv)
        {
            switch (encryptionType.ToUpper())
            {
                case "AES":
                    return DecryptWithAES(encryptedChunk, key, iv);
                case "DES":
                    return DecryptWithDES(encryptedChunk, key, iv);
                default:
                    throw new ArgumentException("Unsupported encryption type for chunk decryption.");
            }
        } //Decrypts chunk selection

        public static byte[] DecryptWithAES(byte[] encryptedData, byte[] key, byte[] iv)
        {
            if (iv == null)
            {
                throw new ArgumentNullException(nameof(iv), "The IV parameter cannot be null. Ensure the correct IV is passed for decryption.");
            }

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (MemoryStream resultStream = new MemoryStream())
                {
                    csDecrypt.CopyTo(resultStream);
                    return resultStream.ToArray();
                }
            }
        } //what it says on the box

        public static byte[] DecryptWithDES(byte[] encryptedData, byte[] key, byte[] iv)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = key;
                des.IV = iv;

                using (ICryptoTransform decryptor = des.CreateDecryptor(des.Key, des.IV))
                using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (MemoryStream resultStream = new MemoryStream())
                {
                    csDecrypt.CopyTo(resultStream);
                    return resultStream.ToArray();
                }
            }
        } //what it says on the box

        private static byte[] DecryptWithRSA(byte[] encryptedData, RSAParameters privateKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(privateKey);

                // Match PKCS#1 padding
                return rsa.Decrypt(encryptedData, false);
            }
        } //what it says on the box

        private static byte[] DecryptSymmetricKeyWithRSA(byte[] encryptedKey, RSAParameters privateKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(privateKey);
                return rsa.Decrypt(encryptedKey, false);
            }
        } //what it says on the box

        public static void ExampleUsage()
        {
            var (publicKey, privateKey) = GenerateRSAKeyPair();

            string data = "This is a secure message.";
            string encryptionType = "RSA"; 

            var (encryptedChunks, encryptedKey, iv) = EncryptDataWithKeyTransport(data, encryptionType, publicKey);

            string decryptedData = DecryptDataWithKeyTransport(encryptedChunks, encryptedKey, encryptionType, privateKey, iv);

            Console.WriteLine($"Original Data: {data}");
            Console.WriteLine($"Decrypted Data: {decryptedData}");
        } //Test function
    }
}

