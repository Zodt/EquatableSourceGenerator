using System;

namespace EquatableSourceGenerator.InterfacesImplementationSample
{
    internal static partial class Program
    {
        internal static void Main()
        {
            var primaryKey = Guid.NewGuid();
            var foreignKey = Guid.NewGuid();
            
            var dummyInterface = GetDummyInterface(primaryKey, foreignKey);
            var dummyInterface1= GetDummyInterface(primaryKey, foreignKey);

            Console.WriteLine(dummyInterface.Equals(dummyInterface1));
            Console.WriteLine(dummyInterface.GetHashCode());
        }
        private static IDummyInterface GetDummyInterface(Guid primaryKey, Guid foreignKey)
        {
            return new DummyInterface
            {
                PrimaryKey = primaryKey,
                ForeignKey = foreignKey
            };
        }
    }
}
