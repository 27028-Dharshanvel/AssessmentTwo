namespace AssessmentTwo.Models
{
    /// <summary>
    /// Represents the various states of the system
    /// </summary>
    public enum BoilerState
    {
        Idle = 1,

        PrePurge,

        Ignition,

        Operational
    }
}
