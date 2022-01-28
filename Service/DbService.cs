using System;
using System.IO;

using Microsoft.Data.Sqlite;

using EveListener.Model;

namespace EveListener.Service
{
    public class DbService
    {
        public const string DB_NAME = "eve.db";
        public const string INSERT_QUERY = @"
                    INSERT INTO measurements (timestamp, co2, temp, humidity)
                    VALUES ($now, $co2, $temp, $humidity)
                    ";

        private string fullDbPath = "";

        public DbService(string pathToDb = ".")
        {
            if (!Directory.Exists(pathToDb))
            {
                Console.WriteLine("Provided path does not exist.");
            }
            else
            {
                fullDbPath = Path.Join(fullDbPath, DB_NAME);
            }
        }

        public void AddMeasurement(Measurement data)
        {
            if (string.IsNullOrEmpty(fullDbPath))
            {
                Console.WriteLine("Cannot open DB - no valid path provided.");
            }
            else
            {
                try 
                {
                    // opening connection to sqlite db
                    using (var connection = new SqliteConnection($"Data Source={fullDbPath}"))
                    {
                        connection.Open();

                        var command = connection.CreateCommand();

                        command.CommandText = INSERT_QUERY;

                        command.Parameters.AddWithValue("$now", DateTime.UtcNow);
                        command.Parameters.AddWithValue("$co2", data.CO2);
                        command.Parameters.AddWithValue("$temp", data.Temp);
                        command.Parameters.AddWithValue("$humidity", data.Humidity);

                        command.ExecuteNonQuery();
                    }
                }
                catch
                {
                    Console.WriteLine("An error occurred while inserting a measurement to the database.");
                    Environment.Exit(1);
                }
            }
        }
    }
}