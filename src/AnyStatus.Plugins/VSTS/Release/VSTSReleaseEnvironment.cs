using AnyStatus.API;
using System.ComponentModel;

namespace AnyStatus
{
    [Browsable(true)]
    [DisplayName("VSTS Release Environment")]
    public class VSTSReleaseEnvironment : Widget, ICanOpenInBrowser
    {
        [ReadOnly(true)]
        [DisplayName("Environment Id")]
        public long EnvironmentId { get; set; }

        [ReadOnly(true)]
        [DisplayName("Definition Environment ID")]
        public long DefinitionEnvironmentId { get; set; }

        public bool CanOpenInBrowser()
        {
            return true;
        }
    }

    [Browsable(false)]
    [DisplayName("Failing tasks")]
    public class VSTSReleaseTask : Plugin
    {
        [ReadOnly(true)]
        [DisplayName("Task name")]
        public string Identifier { get; set; }
    }
}
