using AssessmentTwo.Repositories;
using AssessmentTwo.Services;
using AssessmentTwo.View;

namespace AssessmentTwo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            NotificationService notificationService = new NotificationService();
            Repository repository = new Repository();
            BoilerService boilerService = new BoilerService( notificationService, repository);
            MainMenu mainMenu = new MainMenu(boilerService, notificationService);
            mainMenu.DisplayMainMenu();
            Console.ReadKey();
        }
    }
}
