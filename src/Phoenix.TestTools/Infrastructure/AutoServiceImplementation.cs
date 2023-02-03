using Autofac;
using AutoServiceContainer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using Phoenix.Application.Common.Tokens;
using Phoenix.Application.Services.Colors;
using Phoenix.DataSources.Infrastructures.DBContexts;
using Phoenix.DataSources.Infrastructures.UnitOfWorks;
using Phoenix.Persistance.EF.Repositories.Colors;
using Phoenix.SharedConfiguration.Common.Contracts.Repositories;
using Phoenix.SharedConfiguration.Common.Contracts.Services;
using Phoenix.SharedConfiguration.Common.Contracts.UnitOfWorks;

namespace Phoenix.TestTools.Infrastructure
{
    public class AutoServiceImplementation : AutoServiceConfiguration
    {
        public override void ServicesConfiguration(
            ContainerBuilder container,
            Dictionary<Type, object> mockedServiceParameters,
            DbContext context)
        {
            container.RegisterAssemblyTypes(typeof(ColorAppService).Assembly)
                     .AssignableTo<ScopedService>()
                     .AsImplementedInterfaces()
                     .WithConstructorParameters(mockedServiceParameters)
                     .WithDbContext(context as EFDataContext)
                     .InstancePerLifetimeScope();

            container.RegisterAssemblyTypes(typeof(ColorAppService).Assembly)
                     .AssignableTo<TransientService>()
                     .AsImplementedInterfaces()
                     .WithConstructorParameters(mockedServiceParameters)
                     .WithDbContext(context as EFDataContext)
                     .InstancePerLifetimeScope();

            container.RegisterAssemblyTypes(typeof(ColorAppService).Assembly)
                     .AssignableTo<SingletonService>()
                     .AsImplementedInterfaces()
                     .WithConstructorParameters(mockedServiceParameters)
                     .WithDbContext(context as EFDataContext)
                     .InstancePerLifetimeScope();

            container.RegisterAssemblyTypes(typeof(EFColorRepository).Assembly)
                     .AssignableTo<ScopedRepository>()
                     .AsImplementedInterfaces()
                     .WithConstructorParameters(mockedServiceParameters)
                     .WithDbContext(context as EFDataContext)
                     .InstancePerLifetimeScope();

            container.RegisterAssemblyTypes(typeof(EFColorRepository).Assembly)
                     .AssignableTo<TransientRepository>()
                     .AsImplementedInterfaces()
                     .WithConstructorParameters(mockedServiceParameters)
                     .WithDbContext(context as EFDataContext)
                     .InstancePerLifetimeScope();


            container.RegisterAssemblyTypes(typeof(EFColorRepository).Assembly)
                     .AssignableTo<SingletonRepository>()
                     .AsImplementedInterfaces()
                     .WithConstructorParameters(mockedServiceParameters)
                     .WithDbContext(context as EFDataContext)
                     .InstancePerLifetimeScope();

            container.RegisterType<EFUnitOfWork>()
                     .As<UnitOfWork>()
                     .WithDbContext(context as EFDataContext)
                     .InstancePerLifetimeScope();
        }

        public override DbContext SqlLiteConfiguration(
            SqliteConnection sqliteConnection)
        {
            string connectionString = GetConnectionString();
            var constructorParameters =
                AutoServiceTools.MockObjectListCreator();
            var userTokenService = new Mock<UserTokenService>();
            userTokenService.Setup(_ => _.TenantId).Returns("1");
            constructorParameters
            .AddMockedParameter(typeof(UserTokenService), userTokenService.Object);
            return new InMemoryDataBase()
                .CreateInMemoryDataContext<EFDataContext>(
                sqliteConnection,
                constructorParameters);
        }

        public override DbContext SqlServerConfiguration()
        {
            var userTokenService = new Mock<UserTokenService>();
            userTokenService.Setup(_ => _.TenantId).Returns("1");
            return new EFDataContext(GetConnectionString() , userTokenService.Object);
        }
    }
}
