using AnyStatus.API;
using System.ComponentModel;

namespace AnyStatus
{
    [Browsable(false)]
    [DisplayName("VSTS Release Environment")]
    public class VSTSReleaseEnvironment : Plugin
    {
        [ReadOnly(true)]
        [DisplayName("Environment Id")]
        public long EnvironmentId { get; set; }
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
