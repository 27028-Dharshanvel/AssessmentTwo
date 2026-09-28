namespace AssessmentTwo.Models
{
    /// <summary>
    /// Boiler class represents the boiler system.
    /// </summary>
    public class Boiler
    {
        public bool LockOut { get; set; } = true;

        public bool InterLock { get; set; } = false;

        public BoilerState BoilerStatus { get; set; } = BoilerState.Idle;
    }
}
