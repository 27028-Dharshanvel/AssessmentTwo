using System.Timers;
using AssessmentTwo.Helper;
using AssessmentTwo.Models;
using AssessmentTwo.Repositories;
using AssessmentTwo.Services;

namespace AssessmentTwo.View
{
    internal class MainMenu
    {
        private BoilerService boilerService;
        private NotificationService notificationService;
        private int counter = 0;
        public MainMenu(BoilerService boilerservice, NotificationService notificationservice)
        {
            this.boilerService = boilerservice;
            this.notificationService = notificationservice;
        }
        public async Task DisplayMainMenu()
        {
            Console.Title = "Boiler Controller";
            bool isAppRunning = true;
            notificationService.OnTimerElapsed += DisplayNotification;
            notificationService.timer.Elapsed += DisplayCountDown;
            notificationService.timer.Start();

            await Logger.LogEventAsync("Boiler Initialized");

            while (isAppRunning)
            {
                ClearMenu();
                Console.Write(@" ------------- Boiler Controller -------------

1.Start Boiler Sequence
2.Stop Boiler Sequence
3.Simulate Boiler Error
4.Toggle Interlock
5.Reset Lockout
6.Clear Notifications
7.View Event Log
8.Exit

Select a choice : ");

                if (!InputValidator.IsValidInt(Console.ReadLine(), out int choice))
                {
                    OutputColor.Error("Enter a valid integer");
                    Console.ReadKey();
                    continue;
                }

                if (!InputValidator.IsIntWithinRange(choice, 1, 8))
                {
                    OutputColor.Error("Select integer within range (1-8)");
                    Console.ReadKey();
                    continue;
                }

                BoilerOperations userChoice = (BoilerOperations)choice;

                switch (userChoice)
                {
                    case BoilerOperations.StartBoiler:
                        var startResult = boilerService.StartBoiler();
                        if (!startResult.IsSuccess)
                        {
                            OutputColor.Error(startResult.ErrorMessage);
                            break;
                        }
                        DisplayNotification("Boiler has started..");
                        break;

                    case BoilerOperations.StopBoiler:
                        var stopResult = boilerService.StopBoiler();
                        if (!stopResult.IsSuccess)
                        {
                            OutputColor.Error(stopResult.ErrorMessage);
                            break;
                        }
                        OutputColor.Warn("Boiler has stopped.");
                        break;

                    case BoilerOperations.SimulateError:
                        var errorResult = boilerService.SimulateError();
                        if (!errorResult.IsSuccess)
                        {
                            OutputColor.Error(errorResult.ErrorMessage);
                            break;
                        }
                        else
                            OutputColor.Error("Error: Simulated Error. System in Lockout.");
                        break;

                    case BoilerOperations.ToggleInterLock:
                        Result InterLockResult = boilerService.ToggleInterLock();
                        string statusStr = InterLockResult.IsSuccess ? "Closed" : "Open";
                        Console.Write($"Interlock is toggled - Current Status : ");
                        OutputColor.Info(statusStr);
                        OutputColor.Warn(InterLockResult.ErrorMessage);
                        await Logger.LogEventAsync("Interlock Switch toggled to " + statusStr);
                        break;

                    case BoilerOperations.ResetLockout:
                        var resetResult = boilerService.ResetLockout();
                        if (resetResult.IsSuccess)
                        {
                            OutputColor.Success("Boiler Status changed to Ready");
                            await Logger.LogEventAsync("Boiler Status changed to Ready");
                        }
                        else
                        {
                            OutputColor.Error(resetResult.ErrorMessage);
                        }
                        break;

                    case BoilerOperations.ClearNotifications:
                        ClearNotifications();
                        break;

                    case BoilerOperations.ViewLog:
                        Task<string[]> ReadLog = Logger.ViewLogAsync();
                        string[] LogData = await ReadLog;
                        if(LogData == null)
                        {
                            OutputColor.Warn("No log exists");
                            break;
                        }
                        foreach(string line in LogData)
                        {
                            OutputColor.Shade(line);
                        }
                        break;

                    case BoilerOperations.Exit:
                        isAppRunning = false;
                        OutputColor.Warn("Application exiting...");
                        break;
                }
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Displays the notification in right half of the console.
        /// </summary>
        /// <param name="message"></param>
        public void DisplayNotification(string message)
        {
            var currentCursorPosition = Console.GetCursorPosition();
            Console.SetCursorPosition(Console.WindowWidth / 2, counter++);
            OutputColor.Success(message);
            Console.SetCursorPosition(currentCursorPosition.Item1, currentCursorPosition.Item2);
        }

        /// <summary>
        /// Clears the left half of the console.
        /// </summary>
        public void ClearMenu()
        {
            string blankline = new string(' ', Console.WindowWidth / 2);
            var currentCursorPosition = Console.GetCursorPosition();
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < Console.WindowHeight; i++)
            {
                Console.WriteLine(blankline);
                Console.SetCursorPosition(0, i);
            }
            Console.SetCursorPosition(0, 0);
        }

        /// <summary>
        /// Clears the right half of the console.
        /// </summary>
        public void ClearNotifications()
        {
            string blankline = new string(' ', Console.WindowWidth / 2);
            var currentCursorPosition = Console.GetCursorPosition();
            Console.SetCursorPosition(Console.WindowWidth/2, 0);
            for (int i = 0; i < Console.WindowHeight; i++)
            {
                Console.WriteLine(blankline);
                Console.SetCursorPosition(Console.WindowWidth / 2, i);
            }
            Console.SetCursorPosition(currentCursorPosition.Item1, currentCursorPosition.Item2);
            counter = 0;
        }

        /// <summary>
        /// Displays a clock running in the background
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">ElapsedEventargs</param>
        public void DisplayCountDown(object? sender, ElapsedEventArgs e)
        {
            TimeOnly time = TimeOnly.FromDateTime(DateTime.Now);
            string customTime = time.ToString("HH:mm:ss");
            var currentCursorPosition = Console.GetCursorPosition();
            Console.SetCursorPosition(Console.WindowWidth - 8, 2);
            OutputColor.Info(customTime);
            Console.SetCursorPosition(currentCursorPosition.Item1, currentCursorPosition.Item2);
        }
    }
}
