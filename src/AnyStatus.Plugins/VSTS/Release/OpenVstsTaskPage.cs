using AnyStatus.API;
using System;

namespace AnyStatus
{
    class OpenVstsTaskPage : IOpenInBrowser<VSTSReleaseTask>
    {
        private readonly IProcessStarter _processStarter;

        public OpenVstsTaskPage(IProcessStarter processStarter)
        {
            _processStarter = Preconditions.CheckNotNull(processStarter, nameof(processStarter));
        }

        public void Handle(VSTSReleaseTask task)
        {
            VSTSRelease_v1 release = (VSTSRelease_v1) task.Parent.Parent;
            if (release.DefinitionId == null)
                throw new InvalidOperationException("Cannot open web page. Unknown definition id.");

            var uri = $"{release.Url}/{release.Collection}/{release.Project}/_release?definitionId={release.DefinitionId}&_a=releases";

            _processStarter.Start(uri);
        }
    }
}
