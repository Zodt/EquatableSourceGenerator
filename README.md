<div>
     <p align="center">
          <a href="LICENSE.md">
               <img src="https://img.shields.io/badge/license-MIT-success.svg?style=for-the-badge&logo=appveyor" 
                    alt="License: We are using the MIT License"  />
          </a> 
          <a href="../../../">
               <img src="https://img.shields.io/badge/Author-Zodt-success.svg?style=for-the-badge&logo=appveyor"  />
          </a>
     </p>
     <h1> </h1>
     <h3 align="center">
          What's EquatableSourceGenerator?
     </h3>
</div>
<div>
     <h4 align="center">
          The `EquatableSourceGenerator` is a simple generator that implements the implementation of the interface IEquatable`1 in a partial class
     </h4>
</div>
<h1> </h1>
<div>
<h4 align="center">
    Explanation of how it works:
</h4>
<h5 align="center">
    To engage a class generator, class must be marked as partial and implement the IEquatable interface
</h5>

```csharp
public partial class DummyModel : IEqutable<DummyModel>
{
    //Your properties
}
```
<h5 align="center">
    The `EquatableSourceGenerator` will generate a partial class of `DummyModel`. It will like that:
</h5>

```csharp
using System;

namespace EquatableSourceGenerator.Sample.Models
{
    partial class DummyModel
    {
        public bool Equals(DummyModel? other)
        {
            return other is not null 
                /*&& All your properties*/;
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            
            return obj.GetType() == this.GetType() 
                || obj is DummyModel self && Equals(self);
        }
        public override int GetHashCode()
        {
            HashCode hashCode = new();
	    /*&& hashCode will add all your properties*/;
	    return hashCode.ToHashCode();
        }

        public static bool operator == (DummyModel? self, DummyModel? other)
        {
            return other?.Equals(self) ?? self is null;
        }
        public static bool operator != (DummyModel? self, DummyModel? other)
        {
            return !(self == other);
        }
    }
}
```
</div>
