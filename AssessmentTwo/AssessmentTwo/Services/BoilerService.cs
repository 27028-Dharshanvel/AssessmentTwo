using System.Timers;
using AssessmentTwo.Models;
using AssessmentTwo.Repositories;

namespace AssessmentTwo.Services
{
    /// <summary>
    /// Handles the boiler services
    /// </summary>
    public class BoilerService
    {
        /// <summary>
        /// Instantiates the BoilerService class
        /// </summary>
        /// <param name="service">Instance of notification service</param>
        /// <param name="repository">Instance of Repository</param>
        public BoilerService(NotificationService service, Repository repository)
        {
            this.notificationService = service;
            this.repository = repository;
        }

        NotificationService notificationService;
        private Repository repository;
        System.Timers.Timer prepurgetimer = new System.Timers.Timer(10000);
        System.Timers.Timer ignitiontimer = new System.Timers.Timer(10000);


        /// <summary>
        /// Starts the boiler
        /// </summary>
        /// <returns>Result</returns>
        public Result StartBoiler()
        {
            if (repository.GetBoiler().LockOut)
            {
                return Result.Failure("System is in Lockout state. Please reset lockout first.");
            }
            if (!GetInterLockStatus())
            {
                return Result.Failure("Toggle Interlock before starting the boiler");
            }
            if (repository.GetBoiler().BoilerStatus != BoilerState.Idle)
            {
                return Result.Failure("Boiler is already running.");
            }
            StartPrePurge();
            return Result.Success();
        }

        /// <summary>
        /// Initialiazes the pre purge state.
        /// </summary>
        public void StartPrePurge()
        {
            repository.GetBoiler().BoilerStatus = BoilerState.PrePurge;
            prepurgetimer.Start();
            prepurgetimer.Elapsed += OnPrePurgeTimerElapsed;
        }

        /// <summary>
        /// Initializes the Ignition state.
        /// </summary>
        public void StartIgnition()
        {
            repository.GetBoiler().BoilerStatus = BoilerState.Ignition;
            ignitiontimer.Start();
            ignitiontimer.Elapsed += OnIgnitionTimerElapsed;
        }

        /// <summary>
        /// Triggers when PrePurge timer is elapsed
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">ElapsedEventArgs</param>
        public void OnPrePurgeTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            string message = "Pre-Purge completed.";
            notificationService.Execute(message);
            _ = Logger.LogEventAsync(message);
            prepurgetimer.Stop();
            prepurgetimer.Elapsed -= OnPrePurgeTimerElapsed;
            StartIgnition();
        }

        /// <summary>
        /// Triggers when Ignition Timer is elapsed.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">ElapsedEventArgs</param>
        public void OnIgnitionTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            string message = "Ignition phase completed.";
            notificationService.Execute(message);
            _ = Logger.LogEventAsync(message);
            ignitiontimer.Stop();
            ignitiontimer.Elapsed -= OnIgnitionTimerElapsed;
            StartOperational();
        }

        /// <summary>
        /// Initializes the operational state of the system.
        /// </summary>
        public void StartOperational()
        {
            repository.GetBoiler().BoilerStatus = BoilerState.Operational;
            string message = "Boiler now operational.";
            notificationService.Execute(message);
            _ = Logger.LogEventAsync(message);
        }

        /// <summary>
        /// Stops the Boiler system.
        /// </summary>
        /// <returns>Result</returns>
        public Result StopBoiler()
        {
            if (repository.GetBoiler().BoilerStatus == BoilerState.Idle)
            {
                return Result.Failure("Boiler is Idle. Start the boiler before using stop");
            }

            prepurgetimer.Stop();
            ignitiontimer.Stop();
            repository.GetBoiler().BoilerStatus = BoilerState.Idle;
            return Result.Success();
        }

        /// <summary>
        /// Simulates an error during operational state.
        /// </summary>
        /// <returns>Result</returns>
        public Result SimulateError()
        {
            if (repository.GetBoiler().BoilerStatus != BoilerState.Operational)
            {
                return Result.Failure("Error can only be simulated when the boiler is in Operational mode.");
            }

            prepurgetimer.Stop();
            ignitiontimer.Stop();
            repository.GetBoiler().BoilerStatus = BoilerState.Idle;
            repository.GetBoiler().LockOut = true;
            _ = Logger.LogEventAsync("Error", "Simulated Error. System in Lockout.");
            return Result.Success();
        }

        /// <summary>
        /// Toggles the Interlock state
        /// </summary>
        /// <returns>bool</returns>
        public Result ToggleInterLock()
        {
            if(repository.GetBoiler().BoilerStatus != BoilerState.Idle)
            {
                repository.GetBoiler().InterLock = !repository.GetBoiler().InterLock;
                StopBoiler();
                Result failuerResult = new Result(repository.GetBoiler().InterLock, @"Boiler is currently running .....
Reset Lockout to start the Boiler again");
                return failuerResult;

            }
            repository.GetBoiler().InterLock = !repository.GetBoiler().InterLock;
            Result successResult = new Result(repository.GetBoiler().InterLock, "");
            return successResult;
        }

        /// <summary>
        /// Resets the LockOut state.
        /// </summary>
        /// <returns>Result</returns>
        public Result ResetLockout()
        {
            repository.GetBoiler().LockOut = false;
            StopBoiler();
            return Result.Success();
        }

        /// <summary>
        /// Gets the Lockout status
        /// </summary>
        /// <returns>bool</returns>
        public bool GetLockOutStatus()
        {
            return repository.GetBoiler().LockOut;
        }

        /// <summary>
        /// Gets the InterLock status.
        /// </summary>
        /// <returns>bool</returns>
        public bool GetInterLockStatus()
        {
            return repository.GetBoiler().InterLock;
        }

        /// <summary>
        /// Gets the BoilerState.
        /// </summary>
        /// <returns>BoilerState</returns>
        public BoilerState GetBoilerState()
        {
            return repository.GetBoiler().BoilerStatus;
        }

    }
}
