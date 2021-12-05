using System;

namespace EquatableSourceGenerator.PrimeSample
{
    internal static partial class Program
    {
        internal static void Main()
        {
            var dummyModel = GetDummyModel();
            var dummyModel1 = GetDummyModel();

            Console.WriteLine(dummyModel.Equals(dummyModel1));
            Console.WriteLine(dummyModel.GetHashCode());
        }
        private static DummyModel GetDummyModel()
        {
            return new DummyModel
            {
                Id = 1,
                IsActive = true,
                DummyName = nameof(DummyModel)
            };

        }
    }
}
