using Luban.CSharp.TemplateExtensions;
using Luban.Types;
using Luban.TypeVisitors;

namespace Luban.CSharp.TypeVisitors;

public class UnityGUIDeclaringTypeNameVisitor : DeclaringTypeNameVisitor
{
    public new static UnityGUIDeclaringTypeNameVisitor Ins { get; } = new();

    protected override ITypeFuncVisitor<string> UnderlyingVisitor => EditorUnderlyingTypeNameVisitor.Ins;

    public override string Accept(TDateTime type)
    {
        return "string";
    }

    public override string Accept(TMap type)
    {
        return $"{ConstStrings.ListTypeName}<object[]>";
    }

    public override string Accept(TSet type)
    {
        return $"{ConstStrings.ListTypeName}<{type.ElementType.Apply(this)}>";
    }

    public override string Accept(TString type)
    {
        if(CsharpUnityGUIJsonTemplateExtension.IsUnityObjectFieldType(type))
        {
            return "UnityEngine.Object";
        }
        else
        {
            return "string";
        }
    }
}
