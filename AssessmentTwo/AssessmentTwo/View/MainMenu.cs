using System.Timers;
using AssessmentTwo.Helper;
using AssessmentTwo.Repositories;
using AssessmentTwo.Services;

namespace AssessmentTwo.View
{
    internal class MainMenu
    {
        private BoilerService boilerService;
        private NotificationService notificationService;
        public MainMenu(BoilerService boilerservice, NotificationService notificationservice)
        {
            this.boilerService = boilerservice;
            this.notificationService = notificationservice;
        }
        public async Task DisplayMainMenu()
        {
            bool isAppRunning = true;
            notificationService.OnTimerElapsed += DisplayNotification;
            notificationService.timer.Elapsed += DisplayCountDown;
            notificationService.timer.Start();

            await Logger.LogEventAsync("Boiler Initialized");

            while (isAppRunning)
            {
                ClearMenu();
                Console.Write(@"-----------  Boiler Controller ------------

1.Start Boiler Sequence
2.Stop Boiler Sequence
3.Simulate Boiler Error
4.Toggle Interlock
5.Reset Lockout
6.View Event Log
7.Exit

Select a choice : ");

                if (!InputValidator.IsValidInt(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Select a valid integer");
                    Console.ReadKey();
                    continue;
                }

                if (!InputValidator.IsIntWithinRange(choice, 1, 7))
                {
                    Console.WriteLine("Select integer within range");
                    Console.ReadKey();
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        var startResult = boilerService.StartBoiler();
                        if (!startResult.IsSuccess)
                            Console.WriteLine(startResult.ErrorMessage);
                        break;

                    case 2:
                        var stopResult = boilerService.StopBoiler();
                        if (!stopResult.IsSuccess)
                            Console.WriteLine(stopResult.ErrorMessage);
                        break;

                    case 3:
                        var errorResult = boilerService.SimulateError();
                        if (!errorResult.IsSuccess)
                            Console.WriteLine(errorResult.ErrorMessage);
                        else
                            Console.WriteLine("Error: Simulated Error. System in Lockout.");
                        break;

                    case 4:
                        bool currentInterLockStatus = boilerService.ToggleInterLock();
                        string statusStr = currentInterLockStatus ? "Closed" : "Open";
                        Console.WriteLine($"Interlock is toggled - Current Status : {statusStr}");
                        await Logger.LogEventAsync("Interlock Switch toggled to " + statusStr);
                        break;

                    case 5:
                        var resetResult = boilerService.ResetLockout();
                        if (resetResult.IsSuccess)
                        {
                            Console.WriteLine("Boiler Status changed to Ready");
                            await Logger.LogEventAsync("Boiler Status changed to Ready");
                        }
                        else
                        {
                            Console.WriteLine(resetResult.ErrorMessage);
                        }
                        break;

                    case 6:
                        await Logger.ViewLogAsync();
                        break;

                    case 7:
                        isAppRunning = false;
                        break;
                }
                Console.ReadKey();
            }
        }

        public void DisplayNotification(string message)
        {
            var currentCursorPosition = Console.GetCursorPosition();
            Console.SetCursorPosition(Console.WindowWidth / 2, 0);
            Console.WriteLine(message);
            Console.SetCursorPosition(currentCursorPosition.Item1, currentCursorPosition.Item2);
        }

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

        public void DisplayCountDown(object? sender, ElapsedEventArgs e)
        {
            TimeOnly time = TimeOnly.FromDateTime(DateTime.Now);
            string customTime = time.ToString("HH:mm:ss");
            var currentCursorPosition = Console.GetCursorPosition();
            Console.SetCursorPosition(Console.WindowWidth - 8, 2);
            Console.Write(customTime);
            Console.SetCursorPosition(currentCursorPosition.Item1, currentCursorPosition.Item2);
        }
    }
}
