using Luban.CSharp.TemplateExtensions;
using Luban.Types;

namespace Luban.CSharp.TypeVisitors;

public class UnityGUIInitValueVisitor : CtorDefaultValueVisitor
{
    public new static UnityGUIInitValueVisitor Ins { get; } = new();

    public override string Accept(TEnum type)
    {
        return $"{(type.DefEnum.Items.Count > 0 ? $"{type.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}." + type.DefEnum.Items[0].Name : "default")}";
    }

    public override string Accept(TDateTime type)
    {
        return "\"1970-01-01 00:00:00\"";
    }

    public override string Accept(TBean type)
    {
        return type.IsNullable || type.DefBean.IsAbstractType ? "default" : $"new {type.Apply(EditorUnderlyingTypeNameVisitor.Ins)}()";
    }

    public override string Accept(TArray type)
    {
        return $"System.Array.Empty<{type.ElementType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}>()";
    }

    public override string Accept(TList type)
    {
        return $"new {ConstStrings.ListTypeName}<{type.ElementType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}>()";
    }

    public override string Accept(TSet type)
    {
        return $"new {ConstStrings.ListTypeName}<{type.ElementType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}>()";
    }

    public override string Accept(TMap type)
    {
        return $"new {ConstStrings.ListTypeName}<object[]>()";
    }

    public override string Accept(TString type)
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
}
