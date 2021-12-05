using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EquatableSourceGenerator.Test.Helpers;

public abstract class ReadDataGenerator : IEnumerable<object[]>
{
    private readonly Type _type;
    private readonly string _declared;
    private readonly string _generated;

    protected ReadDataGenerator(Type type)
    {
        _type = type;
        _declared = GetDeclaredType(type);
        _generated = GetGeneratedType(type);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            _declared, _generated, _type.Name
        };
    }
        
    private static string GetDeclaredType(Type type) => ReadFileText(type, "Declared");
    private static string GetGeneratedType(Type type) => ReadFileText(type, "Generated");

    private static string ReadFileText(Type type, string filePostfix)
    {
        return ReadTextFromFile(FindFile(type, $"{type.Name}.{filePostfix}"));
    }
    private static FileInfo FindFile(Type type, string fileName)
    {
        var location = Assembly.GetAssembly(type)?.Location ?? throw new NullReferenceException();
        var directoryInfo = new DirectoryInfo(location).Parent ?? throw new NullReferenceException();
        var directoryInfos = directoryInfo.GetDirectories("Models").FirstOrDefault() ?? throw new NullReferenceException();
        var fileInfo = directoryInfos.GetFiles($"{fileName}.cs").FirstOrDefault() ?? throw new NullReferenceException();
        return fileInfo;
    }
    private static string ReadTextFromFile(FileInfo fileInfo)
    {
        using var fileStream = fileInfo.OpenRead();
        var array = new byte[fileStream.Length];
        fileStream.Read(array, 0, array.Length);
        var readTextFromFile = System.Text.Encoding.Default.GetString(array);
        
        return readTextFromFile[0] == (char)65279 ? readTextFromFile[1..] : readTextFromFile;
    }
}
