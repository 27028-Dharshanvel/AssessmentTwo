namespace AssessmentTwo.Repositories
{
    internal static class Logger
    {
        private static readonly string filePath = "BoilerLog.txt";

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
                    await streamWriter.WriteLineAsync($"{timestamp},{eventName},{eventData}");
                }
            }
            catch (Exception ex)
            {
                // Handle logging failure
            }
        }

        public static async Task ViewLogAsync()
        {
            if (File.Exists(filePath))
            {
                string[] lines = await File.ReadAllLinesAsync(filePath);
                foreach (var line in lines)
                {
                    Console.WriteLine(line);
                }
            }
            else
            {
                Console.WriteLine("Log file does not exist.");
            }
        }
    }
}
