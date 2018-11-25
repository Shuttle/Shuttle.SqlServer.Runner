using Shuttle.Core.Container;
using NUnit.Framework;
using Shuttle.SqlServer.Runner.Core;

namespace Shuttle.SqlServer.Runner.Tests.Integration
{
    public class ScriptRepositoryFixture : IntegrationFixture
    {
        [Test]
        public void Should_be_able_to_persist_script()
        {
            var service = IntegrationFixture.Resolver.Resolve<IScriptService>();
        }
    }
}