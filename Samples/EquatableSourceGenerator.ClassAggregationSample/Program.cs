using System;
using EquatableSourceGenerator.ClassAggregationSample.Models;

namespace EquatableSourceGenerator.ClassAggregationSample
{
    internal static partial class Program
    {
        internal static void Main()
        {
            var dateTime = DateTime.UtcNow;
            var primaryKey = Guid.NewGuid();
            var foreignKey = Guid.NewGuid();
            
            var dummyModel = GetDummyModel(dateTime, primaryKey, foreignKey);
            var dummyModel1 = GetDummyModel(dateTime, Guid.NewGuid(), foreignKey);

            Console.WriteLine(dummyModel.Equals(dummyModel1));
            Console.WriteLine(dummyModel.GetHashCode());
        }
        private static DummyModel GetDummyModel(DateTime dateTime, Guid primaryKey, Guid foreignKey)
        {
            return new DummyModel
            {
                Id = 1,
                IsActive = true,
                DummyName = nameof(DummyModel),
                Model = new AnotherDummyModel
                {
                    Id = 5,
                    CreationDate = dateTime
                },
                DummyInterface = (IDummyInterface) new DummyInterface
                {
                    PrimaryKey = primaryKey,
                    ForeignKey = foreignKey
                }
            };

        }
    }
}
