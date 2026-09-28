using System.Timers;
using AssessmentTwo.Helper;
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
        public void DisplayMainMenu()
        {
            bool isAppRunning = true;
            notificationService.OnTimerElapsed += DisplayNotification;
            notificationService.timer.Elapsed += DisplayCountDown;
            notificationService.timer.Start();

            while (isAppRunning)
            {
                ClearMenu();
                Console.Write(@"-----------  Boiler Controller ------------

1.Start Boiler Sequence
2.Stop Boiler Sequence
3.Toggle Interlock
4.Reset Lockout
5.View Event Log
6.Exit

Select a choice : ");

                if (!InputValidator.IsValidInt(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Select a valid integer");
                    continue;
                }

                if (!InputValidator.IsIntWithinRange(choice, 1, 6))
                {
                    Console.WriteLine("Select integer within range");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        {
                            Result result = boilerService.StartBoiler();
                            if (result.IsSuccess)
                            {
                                Console.WriteLine("Boiler sequence started");
                                break;
                            }
                            Console.WriteLine(result.ErrorMessage);
                            break;
                        }

                    case 2:
                        {
                            Result result = boilerService.StopBoiler();
                            if (result.IsSuccess)
                            {
                                Console.WriteLine("Boiler stopped");
                                break;
                            }
                            Console.WriteLine(result.ErrorMessage);
                            break;
                        }

                    case 3:
                        bool currentInterLockStatus = boilerService.ToggleInterLock();
                        Console.WriteLine($"Interlock is toggled - Current Status : {currentInterLockStatus}");
                        break;

                    case 4:
                        boilerService.ResetLockout();
                        Console.WriteLine("SYSTEM IN LOCKOUT STATE");
                        break;

                    case 5:
                        //To Do : Event log
                        break;

                    case 6:
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
            for(int i = 0; i< Console.WindowHeight/2; i++)
            {
                Console.WriteLine(blankline);
                Console.SetCursorPosition(0,i);
            }
            Console.SetCursorPosition(0,0);
        }

        public void DisplayCountDown(object? sender, ElapsedEventArgs e)
        {
            TimeOnly time = TimeOnly.FromDateTime(DateTime.Now);
            string customTime = time.ToString("HH:mm:ss");
            var currentCursorPosition = Console.GetCursorPosition();
            Console.SetCursorPosition(Console.WindowWidth - 8, Console.WindowHeight - 8);
            Console.Write(customTime);
            Console.SetCursorPosition(currentCursorPosition.Item1, currentCursorPosition.Item2);
        }
    }
}
