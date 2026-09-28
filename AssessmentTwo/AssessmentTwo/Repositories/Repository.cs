using AssessmentTwo.Models;

namespace AssessmentTwo.Repositories
{
    /// <summary>
    /// Repository stores the original boiler system
    /// </summary>
    public class Repository
    {
        private Boiler boiler = new Boiler();

        /// <summary>
        /// Gets the boiler instance
        /// </summary>
        /// <returns>Boiler</returns>
        public Boiler GetBoiler() 
        { 
            return boiler; 
        }
    }
}
