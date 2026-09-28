using System.Timers;
using AssessmentTwo.Models;
using AssessmentTwo.Repositories;

namespace AssessmentTwo.Services
{
    public class BoilerService
    {
        public BoilerService(NotificationService service, Repository repository)
        {
            this.notificationService = service;
            this.repository = repository;
        }

        NotificationService notificationService;
        private Repository repository;
        System.Timers.Timer prepurgetimer = new System.Timers.Timer(10000);
        System.Timers.Timer ignitiontimer = new System.Timers.Timer(10000);


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

        public void StartPrePurge()
        {
            repository.GetBoiler().BoilerStatus = BoilerState.PrePurge;
            prepurgetimer.Start();
            prepurgetimer.Elapsed += OnPrePurgeTimerElapsed;
        }

        public void StartIgnition()
        {
            repository.GetBoiler().BoilerStatus = BoilerState.Ignition;
            ignitiontimer.Start();
            ignitiontimer.Elapsed += OnIgnitionTimerElapsed;
        }

        public void OnPrePurgeTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            string message = "Pre-Purge completed.";
            notificationService.Execute(message);
            _ = Logger.LogEventAsync(message);
            prepurgetimer.Stop();
            prepurgetimer.Elapsed -= OnPrePurgeTimerElapsed;
            StartIgnition();
        }

        public void OnIgnitionTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            string message = "Ignition phase completed.";
            notificationService.Execute(message);
            _ = Logger.LogEventAsync(message);
            ignitiontimer.Stop();
            ignitiontimer.Elapsed -= OnIgnitionTimerElapsed;
            StartOperational();
        }

        public void StartOperational()
        {
            repository.GetBoiler().BoilerStatus = BoilerState.Operational;
            string message = "Boiler now operational.";
            notificationService.Execute(message);
            _ = Logger.LogEventAsync(message);
        }

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

        public bool ToggleInterLock()
        {
            repository.GetBoiler().InterLock = !repository.GetBoiler().InterLock;
            return repository.GetBoiler().InterLock;
        }

        public Result ResetLockout()
        {
            if (!repository.GetBoiler().InterLock)
            {
                return Result.Failure("Please close the interlock switch before resetting lockout.");
            }
            repository.GetBoiler().LockOut = false;
            return Result.Success();
        }


        public bool GetLockOutStatus()
        {
            return repository.GetBoiler().LockOut;
        }

        public bool GetInterLockStatus()
        {
            return repository.GetBoiler().InterLock;
        }

        public BoilerState GetBoilerState()
        {
            return repository.GetBoiler().BoilerStatus;
        }
    }
}
