namespace AssessmentTwo.Services
{
    /// <summary>
    /// Notification service class
    /// </summary>
    public class NotificationService
    {
        public event Action<string> OnTimerElapsed;

        public void Execute(string message)
        {
            OnTimerElapsed?.Invoke(message);
        }
    }
}
