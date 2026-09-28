namespace AssessmentTwo.Services
{
    /// <summary>
    /// Notification service class
    /// </summary>
    public class NotificationService
    {
        public event Action<string> OnTimerElapsed;

        /// <summary>
        /// Invoke the methods subscribed to the event.
        /// </summary>
        /// <param name="message"></param>
        public void Execute(string message)
        {
            OnTimerElapsed?.Invoke(message);
        }

        public System.Timers.Timer timer = new System.Timers.Timer(1000);

    }
}
