﻿using AnyStatus.API;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace AnyStatus.Plugins.Tests.VSTS
{
    [TestClass]
    public class VstsReleaseTests
    {
        [TestMethod]
        public void OpenInBrowserTest()
        {
            var processstarter = Substitute.For<IProcessStarter>();

            var vstsRelease = new VSTSRelease_v1
            {
                Url = "account",
                Project = "project",
                DefinitionId = 1
            };

            var openVstsReleasePage = new OpenVstsReleasePage(processstarter);

            openVstsReleasePage.Handle(vstsRelease);

            var expected = "https://account.visualstudio.com/project/_release?definitionId=1&_a=releases";

            processstarter.Received().Start(expected);
        }
    }
}
