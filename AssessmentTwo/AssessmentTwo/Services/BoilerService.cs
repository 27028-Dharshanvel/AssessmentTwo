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
        System.Timers.Timer prepurgetimer = new System.Timers.Timer(3000);
        System.Timers.Timer ignitiontimer = new System.Timers.Timer(3000);


        public Result StartBoiler()
        {
            if (!GetInterLockStatus())
            {
                return Result.Failure("Toggle Interlock before starting the boiler");
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
            string message = "Pre - purge completed";
            Logger.WriteLog("[INFO] "+ message + $" at {DateTime.Now}");
            notificationService.Execute(message);
            prepurgetimer.Stop();
            StartIgnition();
        }

        public void OnIgnitionTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            string message = "Ignition completed";
            Logger.WriteLog("[INFO] " + message + $"at {DateTime.Now}");
            notificationService.Execute(message);
            ignitiontimer.Stop();
            StartOperational();
        }

        public void StartOperational()
        {
            repository.GetBoiler().BoilerStatus = BoilerState.Operational;
            string message = "Boiler in operational state";
            Logger.WriteLog("[INFO] "+ message + $"at {DateTime.Now}");
            notificationService.Execute(message);
        }

        public Result StopBoiler()
        {
            if(repository.GetBoiler().BoilerStatus == BoilerState.Idle)
            {
                return Result.Failure("Boiler is Idle. Start the boiler before using stop");
            }

            prepurgetimer.Stop();
            ignitiontimer.Stop();
            repository.GetBoiler().BoilerStatus = BoilerState.Idle;
            return Result.Success();
        }

        public bool ToggleInterLock()
        {
            if (repository.GetBoiler().InterLock)
            {
                repository.GetBoiler().InterLock = false;
            }
            repository.GetBoiler().InterLock = true;
            return repository.GetBoiler().InterLock;
        } 

        public bool ResetLockout()
        {
            repository.GetBoiler().LockOut = true;
            Logger.WriteLog($"Boiler System in Lockout State at {DateTime.Now}");
            return true;
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
