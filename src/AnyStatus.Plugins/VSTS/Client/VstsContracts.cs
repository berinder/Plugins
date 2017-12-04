using AnyStatus.API;
using System.Collections.Generic;
using System.Linq;

namespace AnyStatus
{
    public class VstsConnection
    {
        public string Url { get; set; }

        public string Collection { get; set; }

        public string Project { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

    }

    public class VSTSBuildDefinition
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }

    public class VSTSBuild
    {
        public string Result { get; set; }

        public string Status { get; set; }

        public State State
        {
            get
            {
                switch (Status)
                {
                    case "notStarted":
                        return State.Queued;
                    case "inProgress":
                        return State.Running;
                }

                switch (Result)
                {
                    case "notStarted":
                        return State.Running;
                    case "succeeded":
                        return State.Ok;
                    case "failed":
                        return State.Failed;
                    case "partiallySucceeded":
                        return State.PartiallySucceeded;
                    case "canceled":
                        return State.Canceled;
                    default:
                        return State.Unknown;
                }
            }
        }
    }

    public class Collection<T>
    {
        public int Count { get; set; }

        public List<T> Value { get; set; }
    }

    public class VSTSRelease
    {
        public long Id { get; set; }
    }

    public class VSTSReleaseDetails : VSTSRelease
    {
        public ReleaseEnvironment[] Environments { get; set; }
    }

    public class ReleaseEnvironment
    {
        public long Id { get; set; }

        public string Status { get; set; }

        public string Name { get; set; }

        public State State
        {
            get
            {
                //https://www.visualstudio.com/en-us/docs/integrate/api/rm/contracts#EnvironmentStatus

                switch (Status)
                {
                    case "notStarted":
                        return State.None;
                    case "inProgress":
                        return PreDeployApprovals.Any(k => k.Status != "approved") ? State.Unknown : State.Running;
                    case "succeeded":
                        return State.Ok;
                    case "canceled":
                        return State.Canceled;
                    case "rejected":
                        return State.Failed;
                    case "queued":
                        return State.Queued;
                    case "scheduled":
                        return State.None;
                    case "partiallySucceeded":
                        return State.PartiallySucceeded;
                    default:
                        return State.Unknown;
                }
            }
        }

        public DeploymentAttempt[] DeploySteps { get; set; }

        public ReleasePreDeployApproval[] PreDeployApprovals { get; set; }
    }

    public class DeploymentAttempt
    {
        public string Status { get; set; }

        public ReleaseDeployPhase[] ReleaseDeployPhases { get; set; }


        public State State
        {
            get
            {
                // https://www.visualstudio.com/en-us/docs/integrate/api/rm/contracts#DeploymentStatus

                switch (Status)
                {
                    case "notDeployed":
                        return State.None;
                    case "inProgress":
                        return State.Running;
                    case "succeeded":
                        return State.Ok;
                    case "partiallySucceeded":
                        return State.PartiallySucceeded;
                    case "failed":
                        return State.Failed;
                    case "all":
                        return State.Invalid; // TODO : Figure out what "all" means....
                    default:
                        return State.Unknown;
                }
            }
        }
    }

    public class ReleaseDeployPhase
    {

        public string Status { get; set; }

        public DeploymentJob[] DeploymentJobs { get; set; }

        public State State
        {
            get
            {
                // https://www.visualstudio.com/en-us/docs/integrate/api/rm/contracts#DeployPhaseStatus

                switch (Status)
                {
                    case "notStarted":
                        return State.None;
                    case "inProgress":
                        return State.Running;
                    case "succeeded":
                        return State.Ok;
                    case "partiallySucceeded":
                        return State.PartiallySucceeded;
                    case "failed":
                        return State.Failed;
                    case "canceled":
                        return State.Canceled;
                    case "skipped":
                        return State.None;
                    default:
                        return State.Unknown;
                }
            }
        }
    }

    public class DeploymentJob
    {
        public ReleaseTask[] Tasks;
    }

    public class ReleaseTask
    {
        public string Name { get; set; }

        public string Status { get; set; }

        public State State
        {
            get
            {
                // https://www.visualstudio.com/en-us/docs/integrate/api/rm/contracts#TaskStatus

                switch (Status)
                {
                    case "pending":
                        return State.Running;
                    case "inProgress":
                        return State.Running;
                    case "success":
                        return State.Ok;
                    case "failure":
                        return State.Failed;
                    case "canceled":
                        return State.Canceled;
                    case "skipped":
                        return State.None;
                    case "succeeded":
                        return State.Ok;
                    case "failed":
                        return State.Failed;
                    case "partiallySucceeded":
                        return State.PartiallySucceeded;
                    default:
                        return State.Unknown;
                }
            }
        }
    }

    public class VSTSReleaseDefinition
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }

    public class ReleasePreDeployApproval
    {
        public string Status { get; set; }
    }
}
