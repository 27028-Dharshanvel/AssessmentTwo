namespace AssessmentTwo.Repositories
{
    internal static class Logger
    {
        private static readonly string filepath = "BoilerLog.txt";

        public static void WriteLog(string message)
        {
            File.AppendAllTextAsync(filepath, message + Environment.NewLine);
        }

        public static string GetLog()
        {
            return File.ReadAllText(filepath);
        }
    }
}
