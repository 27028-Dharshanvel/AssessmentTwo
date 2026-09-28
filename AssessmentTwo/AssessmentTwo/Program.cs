using AssessmentTwo.Repositories;
using AssessmentTwo.Services;
using AssessmentTwo.View;

namespace AssessmentTwo
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            NotificationService notificationService = new NotificationService();
            Repository repository = new Repository();
            BoilerService boilerService = new BoilerService(notificationService, repository);
            MainMenu mainMenu = new MainMenu(boilerService, notificationService);
            await mainMenu.DisplayMainMenu();
        }
    }
}
