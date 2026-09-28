namespace AssessmentTwo.Repositories
{
    internal static class Logger
    {
        private static readonly string filePath = "BoilerLog.txt";

        /// <summary>
        /// Logs the events asynchronously.
        /// </summary>
        /// <param name="eventName">Event that needs to be logged</param>
        /// <param name="eventData">Event data</param>
        /// <returns></returns>
        public static async Task LogEventAsync(string eventName, string eventData = "")
        {
            try
            {
                bool fileExists = File.Exists(filePath);
                using (StreamWriter streamWriter = new StreamWriter(filePath, true))
                {
                    if (!fileExists)
                    {
                        await streamWriter.WriteLineAsync("Timestamp,Event,Event Data");
                    }
                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    await streamWriter.WriteLineAsync($"{timestamp},[INFO] : {eventName},{eventData}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected Error occured!");
            }
        }

        /// <summary>
        /// Views the logged data asynchronously
        /// </summary>
        /// <returns>Task</returns>
        public static async Task<string[]> ViewLogAsync()
        {
            if (File.Exists(filePath))
            {
                string[] lines = await File.ReadAllLinesAsync(filePath);
                return lines;
            }
            else
            {
                return null;
            }
        }
    }
}
