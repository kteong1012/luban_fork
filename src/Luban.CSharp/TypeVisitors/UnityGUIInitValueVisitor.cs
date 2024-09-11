using Luban.CSharp.TemplateExtensions;
using Luban.Types;
using Luban.TypeVisitors;

namespace Luban.CSharp.TypeVisitors;

public class UnityGUIInitValueVisitor : ITypeFuncVisitor<string>
{
    public static UnityGUIInitValueVisitor Ins { get; } = new();

    public string Accept(TBool type)
    {
        return "false";
    }

    public string Accept(TByte type)
    {
        return "0";
    }

    public string Accept(TShort type)
    {
        return "0";
    }

    public string Accept(TInt type)
    {
        return "0";
    }

    public string Accept(TLong type)
    {
        return "0";
    }

    public string Accept(TFloat type)
    {
        return "0";
    }

    public string Accept(TDouble type)
    {
        return "0";
    }

    public string Accept(TEnum type)
    {
        return $"{(type.DefEnum.Items.Count > 0 ? $"{type.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}." + type.DefEnum.Items[0].Name : "default")}";
    }

    public string Accept(TString type)
    {
        if (CsharpUnityGUIJsonTemplateExtension.IsUnityObjectFieldType(type))
        {
            return "null";
        }
        else
        {
            return "\"\"";
        }
    }

    public string Accept(TDateTime type)
    {
        return "0";
    }

    public string Accept(TBean type)
    {
        return $"new {type.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}()";
    }

    public string Accept(TArray type)
    {
        return $"System.Array.Empty<{type.ElementType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}>()";
    }

    public string Accept(TList type)
    {
        return $"new {ConstStrings.ListTypeName}<{type.ElementType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}>()";
    }

    public string Accept(TSet type)
    {
        return $"new {ConstStrings.ListTypeName}<{type.ElementType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}>()";
    }

    public string Accept(TMap type)
    {
        return $"new {type.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}()";
    }
}
