using AutoServiceContainer;
using Phoenix.DataSources.Infrastructures.DBContexts;

namespace Phoenix.TestTools.Infrastructure
{
    public class SpecTestSut<T> : AutoServiceImplementation
        where T : class
    {
        public T Sut { get; }
        public EFDataContext Context { get; set; }

        public SpecTestSut()
        {
            Sut = CreateService<T>(DataBaseType.SqlServerDataBase);
            Context = GetContext<EFDataContext>();
        }

        public SpecTestSut(Dictionary<Type, object> mockedObjects)
        {
            MockedObjects = mockedObjects;
            Sut = CreateService<T>(DataBaseType.SqlServerDataBase);
            Context = GetContext<EFDataContext>();
        }
    }
}
