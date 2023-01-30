using AutoServiceContainer;
using Phoenix.DataSources.Infrastructures.DBContexts;

namespace Phoenix.TestTools.Infrastructure
{
    public class UnitTestSut<T> : AutoServiceImplementation
        where T : class
    {
        public T Sut { get; }
        public EFDataContext Context { get; set; }

        public UnitTestSut()
        {
            Sut = CreateService<T>(DataBaseType.SqlLiteDataBase);
            Context = GetContext<EFDataContext>();
        }

        public UnitTestSut(Dictionary<Type, object> mockedObjects)
        {
            MockedObjects = mockedObjects;
            Sut = CreateService<T>(DataBaseType.SqlLiteDataBase);
            Context = GetContext<EFDataContext>();
        }
    }
}
