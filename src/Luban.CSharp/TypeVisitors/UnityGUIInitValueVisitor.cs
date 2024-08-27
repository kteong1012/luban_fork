using Luban.CSharp.TemplateExtensions;
using Luban.Types;
using Luban.TypeVisitors;

namespace Luban.CSharp.TypeVisitors;

public class UnityGUIInitValueVisitor : ITypeFuncVisitor<string>
{
    public new static UnityGUIInitValueVisitor Ins { get; } = new();

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
        return $"{(type.DefEnum.Items.Count > 0 ? $"{type.Apply(EditorUnderlyingTypeNameVisitor.Ins)}." + type.DefEnum.Items[0].Name : "default")}";
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
        return "\"1970-01-01 00:00:00\"";
    }

    public string Accept(TBean type)
    {
        if (type.DefBean.IsAbstractType)
        {
            return $"new {type.DefBean.HierarchyNotAbstractChildren[0].FullName}(){{ TypeIndex = 0}}";
        }
        else
        {
            return $"new {type.Apply(EditorUnderlyingTypeNameVisitor.Ins)}()";
        }
    }

    public string Accept(TArray type)
    {
        return $"System.Array.Empty<{type.ElementType.Apply(EditorUnderlyingTypeNameVisitor.Ins)}>()";
    }

    public string Accept(TList type)
    {
        return $"new {ConstStrings.ListTypeName}<{type.ElementType.Apply(EditorUnderlyingTypeNameVisitor.Ins)}>()";
    }

    public string Accept(TSet type)
    {
        return $"new {ConstStrings.ListTypeName}<{type.ElementType.Apply(EditorUnderlyingTypeNameVisitor.Ins)}>()";
    }

    public string Accept(TMap type)
    {
        return $"new {ConstStrings.ListTypeName}<object[]>()";
    }
}
