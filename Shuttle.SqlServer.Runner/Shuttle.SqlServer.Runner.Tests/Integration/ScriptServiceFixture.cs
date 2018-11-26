using System;
using System.IO;
using Moq;
using Shuttle.Core.Container;
using NUnit.Framework;
using Shuttle.SqlServer.Runner.Core;

namespace Shuttle.SqlServer.Runner.Tests.Integration
{
    public class ScriptServiceFixture : IntegrationFixture
    {
        [Test]
        public void Should_be_able_to_persist_script()
        {
            var service = IntegrationFixture.Resolver.Resolve<IScriptService>();

            var configuration = new Mock<IConfiguration>();

            configuration.Setup(m => m.Environment).Returns("test");
            configuration.Setup(m => m.Recursive).Returns(false);
            configuration.Setup(m => m.RegistryConnectionString).Returns("server=.\\sqlexpress;database=ScriptRegistry;integrated security=sspi");
            configuration.Setup(m => m.ScriptFolder).Returns(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Integration\\.scripts"));
            configuration.Setup(m => m.SqlCmdArguments).Returns(".\\sqlexpress -E -d ScriptRegistry");

            using (TransactionScopeFactory.Create())
            {
                service.Execute(configuration.Object);
            }
        }
    }
}