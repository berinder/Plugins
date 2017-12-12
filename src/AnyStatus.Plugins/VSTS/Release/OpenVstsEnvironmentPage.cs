using AnyStatus.API;
using System;

namespace AnyStatus
{
    class OpenVstsEnvironmentPage : IOpenInBrowser<VSTSReleaseEnvironment>
    {
        private readonly IProcessStarter _processStarter;

        public OpenVstsEnvironmentPage(IProcessStarter processStarter)
        {
            _processStarter = Preconditions.CheckNotNull(processStarter, nameof(processStarter));
        }
        public void Handle(VSTSReleaseEnvironment environment)
        {
            VSTSRelease_v1 parent = (VSTSRelease_v1) environment.Parent;
            if (parent.DefinitionId == null)
                throw new InvalidOperationException("Cannot open web page. Unknown definition id.");
            var uri = $"{parent.Url}/{parent.Collection}/{parent.Project}/_release?definitionId={parent.DefinitionId}&definitionEnvironmentId={environment.DefinitionEnvironmentId}&_a=environment-summary";

            _processStarter.Start(uri);
        }
    }
}
