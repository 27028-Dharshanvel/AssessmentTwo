namespace AssessmentTwo.Models
{
    public class Boiler
    {
        public bool LockOut { get; set; } = true;

        public bool InterLock { get; set; } = false;

        public BoilerState BoilerStatus { get; set; } = BoilerState.Idle;
    }
}
