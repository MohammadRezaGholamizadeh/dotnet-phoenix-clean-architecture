using Hangfire.Client;
using Hangfire.Logging;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;

namespace Phoenix.Infrastructure.BackgroundJobs;

public class LogJobFilter :
             IServerFilter,
             IApplyStateFilter,
             IClientFilter,
             IElectStateFilter
{
    private static ILog _logger;
    public LogJobFilter()
    {
        _logger = LogProvider.GetCurrentClassLogger();
    }

    public void OnCreating(
        CreatingContext context)
    {
        _logger.InfoFormat(
            $"Creating a job based on method {context.Job.Method.Name} ...");
    }

    public void OnCreated(CreatedContext context)
    {
        _logger.InfoFormat(
             $"Job that is based on method " +
             $"{context.Job.Method.Name} has been created " +
             $"with id {context.BackgroundJob?.Id}");
    }


    public void OnPerforming(PerformingContext context)
    {
        _logger.InfoFormat(
            $"Starting to perform job {context.BackgroundJob.Id}");
    }

    public void OnPerformed(PerformedContext context)
    {
        _logger.InfoFormat(
            $"Job {context.BackgroundJob.Id} has been performed");
    }

    public void OnStateElection(ElectStateContext context)
    {
        if (context.CandidateState is FailedState failedState)
        {
            _logger.WarnFormat(
                $"Job '{context.BackgroundJob.Id}' Has Been Failed " +
                $"Due To An Exception {failedState.Exception}");
        }
    }

    public void OnStateApplied(
        ApplyStateContext context,
        IWriteOnlyTransaction transaction)
    {
        _logger.InfoFormat(
                    $"Job {context.BackgroundJob.Id} state was changed " +
                    $"from {context.OldStateName} " +
                    $"to {context.NewState.Name}");
    }


    public void OnStateUnapplied(
        ApplyStateContext context,
        IWriteOnlyTransaction transaction)
    {
        _logger.InfoFormat(
                    $"Job {context.BackgroundJob.Id} state " +
                    $"{context.OldStateName} was unapplied.");
    }

}
