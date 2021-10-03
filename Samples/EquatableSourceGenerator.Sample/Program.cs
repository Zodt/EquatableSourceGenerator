using System;
using EquatableSourceGenerator.Sample.Models;

namespace EquatableSourceGenerator.Sample
{
    internal static class Program
    {
        internal static void Main()
        {
            var dateTime = DateTime.UtcNow;
            var dummyModel = GetDummyModel(dateTime);
            var dummyModel1 = new DummyModel
            {
                Id = 4,
                IsActive = true,
                DummyName = nameof(DummyModel),
                Model = new AnotherDummyModel
                {
                    Id = 1,
                    CreationDate = dateTime
                }
            };

            Console.WriteLine(dummyModel.Equals(dummyModel1));
            Console.WriteLine(dummyModel.Model?.GetHashCode());
            Console.WriteLine(dummyModel.GetHashCode());
        }
        private static DummyModel GetDummyModel(DateTime dateTime)
        {
            return new DummyModel
            {
                Id = 1,
                IsActive = true,
                DummyName = nameof(DummyModel),
                Model = new AnotherDummyModel
                {
                    Id = 1,
                    CreationDate = dateTime
                }
            };
        }
    }
}
