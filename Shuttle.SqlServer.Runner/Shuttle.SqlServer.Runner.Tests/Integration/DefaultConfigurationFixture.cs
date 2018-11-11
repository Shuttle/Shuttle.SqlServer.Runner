using System;
using System.IO;
using NUnit.Framework;
using Shuttle.SqlServer.Runner.Core;

namespace Shuttle.SqlServer.Runner.Tests.Integration
{
    [TestFixture]
    public class DefaultConfigurationFixture
    {
        private void Compare(IConfiguration configuration)
        {
            Assert.That(configuration.Environment, Is.EqualTo("development"));
            Assert.That(configuration.RegistryConnectionString, Is.EqualTo("registry-connection-string"));
            Assert.That(configuration.ScriptFolder, Is.EqualTo("c:\\script-folder"));
            Assert.That(configuration.Recursive, Is.True);
            Assert.That(configuration.SqlCmdArguments, Is.EqualTo("sqlcmd-arguments"));
        }

        [Test]
        public void Should_be_able_to_get_from_file()
        {
            Compare(DefaultConfiguration.Read(Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                @"integration\.files\configuration.xml")));
        }

        [Test]
        public void Should_be_able_to_get_from_xml()
        {
            var xml = @"
<configuration
  environment=""development""
  registryConnectionString=""registry-connection-string""
  scriptFolder=""c:\script-folder""
  recursive=""true""
  sqlCmdArguments=""sqlcmd-arguments""
  />
";
            Compare(DefaultConfiguration.From(xml));
        }
    }
}