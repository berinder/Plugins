using AnyStatus.API;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Xml;

//todo: unautherized state when 401

//https://www.visualstudio.com/en-us/docs/integrate/api/rm/contracts#ReleaseStatus

namespace AnyStatus
{
    public class VSTSReleaseMonitor : IMonitor<VSTSRelease_v1>
    {
        [DebuggerStepThrough]
        public void Handle(VSTSRelease_v1 vstsRelease)
        {
            var client = new VstsClient
            {
                Connection = new VstsConnection
                {
                    Url = vstsRelease.Url,
                    Collection = vstsRelease.Collection,
                    Project = vstsRelease.Project,
                    UserName = vstsRelease.UserName,
                    Password = vstsRelease.Password,
                }
            };

            if (vstsRelease.DefinitionId == null)
            {
                var definition = client.GetReleaseDefinitionAsync(vstsRelease.ReleaseDefinitionName).Result;

                vstsRelease.DefinitionId = definition.Id;
            }

            var latestRelease = client.GetLatestReleaseAsync(vstsRelease.DefinitionId.Value).Result;

            var releaseDetails = client.GetReleaseDetailsAsync(latestRelease.Id).Result;

            RemoveEnvironments(vstsRelease, releaseDetails);

            AddEnvironments(vstsRelease, releaseDetails);

            RemoveTasks(vstsRelease, releaseDetails);

            AddTasks(vstsRelease, releaseDetails);
        }

        private void AddTasks(VSTSRelease_v1 vstsRelease, VSTSReleaseDetails releaseDetails)
        {
            foreach (var environment in releaseDetails.Environments.Where(e => e.State != State.Ok))
            {
                var node = vstsRelease.Items.First(i => i.Name == environment.Name);
                foreach (var deploymentAttempt in environment.DeploySteps.Where(ds => ds.State != State.Ok))
                {
                    foreach (var releaseTask in deploymentAttempt.ReleaseDeployPhases.SelectMany(rdp => rdp.DeploymentJobs).SelectMany(dj => dj.Tasks).Where(t => t.State != State.Ok))
                    {
                        Item task = new VSTSReleaseTask() { Name = releaseTask.Name, State = releaseTask.State};
                        Application.Current.Dispatcher.Invoke(() => node.Add(task));
                    }
                }
                
            }
        }

        private void RemoveTasks(VSTSRelease_v1 node, VSTSReleaseDetails releaseDetails)
        {
            if (node == null || node.Items == null)
                throw new InvalidOperationException();

            foreach (var nodeItem in node.Items)
            {
                foreach (var nodeItemItem in nodeItem.Items)
                {
                    Application.Current.Dispatcher.Invoke(() => nodeItem.Remove(nodeItemItem));
                }
            }
        }

        private static void RemoveEnvironments(VSTSRelease_v1 node, VSTSReleaseDetails releaseDetails)
        {
            if (node == null || node.Items == null)
                throw new InvalidOperationException();

            var environments = node.Items
                .Where(k => !releaseDetails.Environments.Any(e => e.Name == k.Name))
                .ToList();

            foreach (var environment in environments)
            {
                Application.Current.Dispatcher.Invoke(() => node.Remove(environment));
            }
        }

        private static void AddEnvironments(VSTSRelease_v1 vstsRelease, VSTSReleaseDetails releaseDetails)
        {
            if (vstsRelease == null || vstsRelease.Items == null)
                throw new InvalidOperationException();

            foreach (var environment in releaseDetails.Environments)
            {
                var node = vstsRelease.Items.FirstOrDefault(i => i.Name == environment.Name);

                if (node == null)
                {
                    node = new VSTSReleaseEnvironment
                    {
                        Name = environment.Name,
                        EnvironmentId = environment.Id
                    };
                    
                    Application.Current.Dispatcher.Invoke(() => vstsRelease.Add(node));
                    if (environment.State == State.PartiallySucceeded)
                    {
                        Item guiNode = new VSTSReleaseTask() { Name = "Test!" };
                        Application.Current.Dispatcher.Invoke(() => node.Add(guiNode));
                    }
                }

                node.State = environment.State;
                
            }
        }
    }
}
