using System;
using System.Transactions;
using Castle.Windsor;
using NUnit.Framework;
using Shuttle.Core.Castle;
using Shuttle.Core.Container;
using Shuttle.Core.Data;
using Shuttle.Core.Transactions;

namespace Shuttle.SqlServer.Runner.Tests
{
    [TestFixture]
    public class IntegrationFixture
    {
        public static IDatabaseContextCache DatabaseContextCache => Resolver.Resolve<IDatabaseContextCache>();
        public static IDatabaseGateway DatabaseGateway => Resolver.Resolve<IDatabaseGateway>();
        public static IDatabaseContextFactory DatabaseContextFactory => Resolver.Resolve<IDatabaseContextFactory>();
        public static IQueryMapper QueryMapper => Resolver.Resolve<IQueryMapper>();
        public static ITransactionScopeFactory TransactionScopeFactory { get; }
        public static IComponentResolver Resolver { get; }

        static IntegrationFixture()
        {
            var container = new WindsorComponentContainer(new WindsorContainer());

            container.RegistryBoostrap();

            container.RegisterSuffixed("Shuttle.SqlServer.Runner.Core");

            TransactionScopeFactory =
                new DefaultTransactionScopeFactory(true, IsolationLevel.ReadCommitted, TimeSpan.FromSeconds(120));

            Resolver = container;
        }
    }
}