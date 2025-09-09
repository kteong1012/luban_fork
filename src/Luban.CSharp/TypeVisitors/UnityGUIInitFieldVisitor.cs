using Luban.CSharp.TemplateExtensions;
using Luban.Types;
using Luban.TypeVisitors;

namespace Luban.CSharp.TypeVisitors;

public class UnityGUIInitFieldVisitor : ITypeFuncVisitor<string, string, int, string>
{
    public static UnityGUIInitFieldVisitor Ins { get; } = new();

    private string CommonAccept(TType type, string fieldName)
    {
        return $$"""
        {{fieldName}} = {{type.Apply(UnityGUIInitValueVisitor.Ins)}};
        """;
    }

    public string Accept(TBool type, string fieldName, string fieldName2, int depth)
    {
        return CommonAccept(type, fieldName);
    }

    public string Accept(TByte type, string fieldName, string fieldName2, int depth)
    {
        return CommonAccept(type, fieldName);
    }

    public string Accept(TShort type, string fieldName, string fieldName2, int depth)
    {
        return CommonAccept(type, fieldName);
    }

    public string Accept(TInt type, string fieldName, string fieldName2, int depth)
    {
        return CommonAccept(type, fieldName);
    }

    public string Accept(TLong type, string fieldName, string fieldName2, int depth)
    {
        return CommonAccept(type, fieldName);
    }

    public string Accept(TFloat type, string fieldName, string fieldName2, int depth)
    {
        return CommonAccept(type, fieldName);
    }

    public string Accept(TDouble type, string fieldName, string fieldName2, int depth)
    {
        return CommonAccept(type, fieldName);
    }

    public string Accept(TEnum type, string fieldName, string fieldName2, int depth)
    {
        return CommonAccept(type, fieldName);
    }

    public string Accept(TString type, string fieldName, string fieldName2, int depth)
    {
        return CommonAccept(type, fieldName);
    }

    public string Accept(TDateTime type, string fieldName, string fieldName2, int depth)
    {
        return CommonAccept(type, fieldName);
    }

    public string Accept(TBean type, string fieldName, string fieldName2, int depth)
    {
        if (type.DefBean.IsAbstractType)
        {
            var firstImplType = type.DefBean.HierarchyNotAbstractChildren.First();
            return $$"""
            {{fieldName}} = {{type.DefBean.FullName}}.Create("{{CsharpUnityGUIJsonTemplateExtension.GetImplTypeName(firstImplType)}}");
            """;
        }
        else
        {
            return $$"""
            {{fieldName}} = new {{type.DefBean.FullName}}();
            """;
        }
    }

    public string Accept(TArray type, string fieldName, string fieldName2, int depth)
    {
        return CommonAccept(type, fieldName);
    }

    public string Accept(TList type, string fieldName, string fieldName2, int depth)
    {
        return CommonAccept(type, fieldName);
    }

    public string Accept(TSet type, string fieldName, string fieldName2, int depth)
    {
        return CommonAccept(type, fieldName);
    }

    public string Accept(TMap type, string fieldName, string fieldName2, int depth)
    {
        return CommonAccept(type, fieldName);
    }
}
