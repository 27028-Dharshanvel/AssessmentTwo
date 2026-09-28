using AssessmentTwo.Models;

namespace AssessmentTwo.Repositories
{
    public class Repository
    {
        private Boiler boiler = new Boiler();

        public Boiler GetBoiler() 
        { 
            return boiler; 
        }
    }
}
