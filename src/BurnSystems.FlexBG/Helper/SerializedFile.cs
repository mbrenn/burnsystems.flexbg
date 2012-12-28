using BurnSystems.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Helper
{
    /// <summary>
    /// Helper to load and store from and into serialized file
    /// </summary>
    public static class SerializedFile
    {
        /// <summary>
        /// Stores the logger instance for this class
        /// </summary>
        private static ILog classLogger = new ClassLogger(typeof(SerializedFile));

        /// <summary>
        /// Loads database from file
        /// </summary>
        public static T LoadFromFile<T>(string filename) where T : class
        {
            var filePath = Path.Combine("data", filename);

            if (!File.Exists(filePath))
            {
                classLogger.LogEntry(LogLevel.Message, "No file for " + filename);
                return default(T);
            }

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    var formatter = new BinaryFormatter();
                    T result = formatter.Deserialize(stream) as T;

                    classLogger.LogEntry(LogLevel.Notify, "Serialized File: '" + filename + "' loaded");

                    return result;

                }
            }
            catch (Exception exc)
            {
                classLogger.LogEntry(LogLevel.Fatal, "Loading for " + filename + " failed: " + exc.Message);

                return default(T);
            }
        }

        /// <summary>
        /// Stores database to file
        /// </summary>
        public static void StoreToFile<T>(string filename, T db)
        {
            var filePath = Path.Combine("data", filename);
            if (!Directory.Exists("data"))
            {
                Directory.CreateDirectory("data");
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, db);

                classLogger.LogEntry(LogLevel.Notify, "Serialized File: '" + filename + "' stored");
            }
        }
    }
}
