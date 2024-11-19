using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LocalDatabaseApp
{
    class DB
    {
        private static readonly string dbFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\Resources", "localdatabase.db");

        public static void Database_Check()
        {
            if (!File.Exists(dbFilePath))
            {
                SQLiteConnection.CreateFile(dbFilePath);
                Console.WriteLine("DATABASE CREATED SUCCESSFULLY");

                using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dbFilePath};Version=3;"))
                {
                    connection.Open();
                    string createTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Users (
                            UserID INTEGER PRIMARY KEY AUTOINCREMENT,
                            IPaddress VARCHAR(45) NOT NULL,
                            PC_NAME TEXT NOT NULL,
                            CreatedAt TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP, 'localtime'))
                        );
                        CREATE TABLE IF NOT EXISTS Sessions (
                            SessionID TEXT PRIMARY KEY,
                            HostUserID INTEGER NOT NULL,
                            CreatedAt TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP, 'localtime')),
                            MaxConnections INTEGER DEFAULT 10,
                            ConnectionCount INTEGER DEFAULT 0,
                            FOREIGN KEY (HostUserID) REFERENCES Users(UserID)
                        );
                        CREATE TABLE IF NOT EXISTS Session_Connections (
                            ConnectionID INTEGER PRIMARY KEY AUTOINCREMENT,
                            SessionID TEXT NOT NULL,
                            UserID INTEGER NOT NULL,
                            ConnectedAt TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP, 'localtime')),
                            IsHost BOOLEAN DEFAULT 0,
                            FOREIGN KEY (SessionID) REFERENCES Sessions(SessionID) ON DELETE CASCADE,
                            FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE,
                            UNIQUE (SessionID, UserID)
                        );
                        CREATE TABLE IF NOT EXISTS Messages (
                            MessageID INTEGER PRIMARY KEY AUTOINCREMENT,
                            SessionID TEXT NOT NULL,
                            UserID INTEGER NOT NULL,
                            Message_Content TEXT NOT NULL,
                            Direction TEXT CHECK(Direction IN ('sent', 'received')) NOT NULL,
                            SentAt TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP, 'localtime')),
                            FOREIGN KEY (SessionID) REFERENCES Sessions(SessionID) ON DELETE CASCADE,
                            FOREIGN KEY (UserID) REFERENCES Users(UserID)
                        );
                        CREATE TABLE IF NOT EXISTS Files (
                            FileID INTEGER PRIMARY KEY AUTOINCREMENT,
                            SessionID TEXT NOT NULL,
                            UserID INTEGER NOT NULL,
                            FileName TEXT NOT NULL,
                            FilePath TEXT NOT NULL,
                            FileSize INTEGER NOT NULL,
                            TransferredAt TIMESTAMP DEFAULT (datetime(CURRENT_TIMESTAMP, 'localtime')),
                            FOREIGN KEY (SessionID) REFERENCES Sessions(SessionID) ON DELETE CASCADE,
                            FOREIGN KEY (UserID) REFERENCES Users(UserID)
                        );";
                    using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine($"TABLES CREATED SUCCESSFULLY! DB PATH: {dbFilePath}");
                    }
                }
            }
            else
            {
                Console.WriteLine("DB AND TABLES EXIST");
            }
        } //Checks if the database exists

        public static string RecallDB()
        {
            StringBuilder allData = new StringBuilder();
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dbFilePath};Version=3;"))
                {
                    connection.Open();
                    string tableQuery = "SELECT name FROM sqlite_master WHERE type='table';";
                    using (SQLiteCommand tableCommand = new SQLiteCommand(tableQuery, connection))
                    using (SQLiteDataReader tableReader = tableCommand.ExecuteReader())
                    {
                        while (tableReader.Read())
                        {
                            string tableName = tableReader.GetString(0);
                            allData.AppendLine($"Table: {tableName}");
                            allData.AppendLine(new string('-', 30));

                            string dataQuery = $"SELECT * FROM {tableName}";
                            using (SQLiteCommand dataCommand = new SQLiteCommand(dataQuery, connection))
                            using (SQLiteDataReader dataReader = dataCommand.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    for (int i = 0; i < dataReader.FieldCount; i++)
                                    {
                                        string columnName = dataReader.GetName(i);
                                        object value = dataReader.GetValue(i);
                                        allData.AppendLine($"{columnName}: {value}");
                                    }
                                    allData.AppendLine("------");
                                }
                            }
                            allData.AppendLine("\n\n");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error retrieving database data: " + e, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return allData.ToString();
        } //Recalls all of the information form the database

        public static async Task NewMessageFromClientAsync(string messageInput, string sessionID)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dbFilePath};Version=3;"))
                {
                    await connection.OpenAsync();

                    // Retrieve the UserID for the current PC
                    long userID;
                    string pcName = Environment.MachineName;
                    using (SQLiteCommand userCommand = new SQLiteCommand("SELECT UserID FROM Users WHERE PC_NAME = @PCName", connection))
                    {
                        userCommand.Parameters.AddWithValue("@PCName", pcName);
                        userID = (long)(await userCommand.ExecuteScalarAsync());
                    }

                    // Insert the new message into the Messages table
                    string direction = "sent";
                    string insertMessageDataQuery = @"
                INSERT INTO Messages (SessionID, UserID, Message_Content, Direction) 
                VALUES (@SessionID, @UserID, @MessageContent, @Direction)";

                    using (SQLiteCommand command = new SQLiteCommand(insertMessageDataQuery, connection))
                    {
                        command.Parameters.AddWithValue("@SessionID", sessionID);
                        command.Parameters.AddWithValue("@UserID", userID);
                        command.Parameters.AddWithValue("@MessageContent", messageInput);
                        command.Parameters.AddWithValue("@Direction", direction);

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in NewMessageFromClientAsync: {e}");
            }
        }

        public static List<(string SessionID, string CreatedAt)> LoadChatSessions()
        {
            List<(string SessionID, string CreatedAt)> chatSessions = new List<(string, string)>();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dbFilePath};Version=3;"))
                {
                    connection.Open();

                    string query = "SELECT SessionID, CreatedAt FROM Sessions";  // Updated to `Sessions` table
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string sessionID = reader["SessionID"].ToString();
                                string createdAt = reader["CreatedAt"].ToString();

                                chatSessions.Add((sessionID, createdAt));
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return chatSessions;
        }

        public static async Task<List<(string MessageContent, string Direction, string SentAt, string UserID)>> GetMessagesBySessionAsync(string sessionId)
        {
            string connectionString = $"Data Source={dbFilePath};Version=3;";
            List<(string MessageContent, string Direction, string SentAt, string UserID)> messages = new List<(string, string, string, string)>();

            try
            {
                // Using the asynchronous open method
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"
                        SELECT Message_Content, Direction, SentAt, UserID
                        FROM Messages 
                        WHERE SessionID = @SessionID 
                        ORDER BY SentAt ASC";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SessionID", sessionId);

                        using (SQLiteDataReader reader = (SQLiteDataReader)await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                string messageContent = reader["Message_Content"].ToString();
                                string direction = reader["Direction"].ToString();
                                string sentAt = reader["SentAt"].ToString();
                                string userId = reader["UserID"].ToString();

                                messages.Add((messageContent, direction, sentAt, userId));
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return messages;
        }

        public static void DeleteDb()
        {

            if (File.Exists(dbFilePath))
            {
                try
                {
                    File.Delete(dbFilePath); 
                    Console.WriteLine("User data dropped successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error dropping database: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Database file does not exist.");
            }
        } //Deletes the database

        public static void Database_Test_Input_sessions(string pcName, string ipAddress)
        {
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dbFilePath};Version=3;"))
            {
                connection.Open();

                // Insert into Users table
                string insertUserDataQuery = "INSERT INTO Users (PC_NAME, IPaddress) VALUES (@PCName, @IPaddress)";
                using (SQLiteCommand command = new SQLiteCommand(insertUserDataQuery, connection))
                {
                    command.Parameters.AddWithValue("@PCName", pcName);
                    command.Parameters.AddWithValue("@IPaddress", ipAddress);
                    command.ExecuteNonQuery();
                    Console.WriteLine("USER DATA ADDED SUCCESSFULLY!");
                }

                // Retrieve the newly created UserID
                long userID;
                using (SQLiteCommand command = new SQLiteCommand("SELECT last_insert_rowid()", connection))
                {
                    userID = (long)command.ExecuteScalar();
                }

                // Insert Sessions data for this user
                for (int i = 0; i < 10; i++)
                {
                    string sessionID = Guid.NewGuid().ToString();

                    string insertSessionDataQuery = "INSERT INTO Sessions (HostUserID, SessionID) VALUES (@HostUserID, @SessionID)";
                    using (SQLiteCommand command = new SQLiteCommand(insertSessionDataQuery, connection))
                    {
                        command.Parameters.AddWithValue("@HostUserID", userID);  // Use the actual UserID
                        command.Parameters.AddWithValue("@SessionID", sessionID);
                        command.ExecuteNonQuery();
                        Console.WriteLine("SESSION DATA ADDED SUCCESSFULLY!");
                    }

                    // Insert Messages for each session
                    for (int j = 0; j < 5; j++)
                    {
                        string direction = j % 2 == 0 ? "sent" : "received";
                        string messageContent = $"Test message {j + 1} in session {i + 1}";

                        string insertMessageDataQuery = @"
                    INSERT INTO Messages (SessionID, UserID, Message_Content, Direction) 
                    VALUES (@SessionID, @UserID, @MessageContent, @Direction)";

                        using (SQLiteCommand command = new SQLiteCommand(insertMessageDataQuery, connection))
                        {
                            command.Parameters.AddWithValue("@SessionID", sessionID);
                            command.Parameters.AddWithValue("@UserID", userID);  // Use the UserID obtained
                            command.Parameters.AddWithValue("@MessageContent", messageContent);
                            command.Parameters.AddWithValue("@Direction", direction);
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
}

