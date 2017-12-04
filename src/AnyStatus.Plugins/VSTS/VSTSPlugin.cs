using AnyStatus.API;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace AnyStatus
{
    public abstract class VstsPlugin : Build
    {
        private const string Category = "Visual Studio Team Services / Team Foundation Server 2018";

        protected VstsPlugin(bool aggregate) : base(aggregate)
        {
            Collection = "DefaultCollection";
        }

        [Url]
        [Required]
        [Category(Category)]
        [PropertyOrder(10)]
        [Description("Required. Visual Studio Team Services account (https://{account}.visualstudio.com) or TFS server (http://{server:port}/tfs)")]
        public string Url { get; set; }

        [Category(Category)]
        [PropertyOrder(20)]
        [Description("Required. The collection name. Default: DefaultCollection")]
        public string Collection { get; set; }

        [Required]
        [Category(Category)]
        [PropertyOrder(20)]
        [DisplayName("Project")]
        [Description("Required (case-sensitive). Enter your Visual Studio Team Services project name.")]
        public string Project { get; set; }

        [PropertyOrder(30)]
        [Category(Category)]
        [DisplayName("User Name")]
        [Description("Optional. Enter the user name of your Visual Studio Team Services account. " +
                     "To authenticate with a Personal Access Token, leave the User Name empty.")]
        public string UserName { get; set; }

        [PropertyOrder(40)]
        [Category(Category)]
        [DisplayName("Password (Token)")]
        [Description("Optional. Enter the password or Personal Access Token of your Visual Studio Team Services account. " +
                     "To authenticate with a Personal Access Token, leave the User Name empty.")]
        [Editor(typeof(PasswordEditor), typeof(PasswordEditor))]
        public string Password { get; set; }
    }
}
