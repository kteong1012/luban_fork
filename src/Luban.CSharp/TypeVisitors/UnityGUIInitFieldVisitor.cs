using Luban.CSharp.TemplateExtensions;
using Luban.Types;
using Luban.TypeVisitors;

namespace Luban.CSharp.TypeVisitors;

public class UnityGUIInitFieldVisitor : ITypeFuncVisitor<string, int, string>
{
    public static UnityGUIInitFieldVisitor Ins { get; } = new();

    private string CommonAccept(TType type, string fieldName)
    {
        return $$"""
        {{fieldName}} = {{type.Apply(UnityGUIInitValueVisitor.Ins)}};
        """;
    }

    public string Accept(TBool type, string fieldName, int depth)
    {
        return CommonAccept(type, fieldName);
    }

    public string Accept(TByte type, string fieldName, int depth)
    {
        return CommonAccept(type, fieldName);
    }

    public string Accept(TShort type, string fieldName, int depth)
    {
        return CommonAccept(type, fieldName);
    }

    public string Accept(TInt type, string fieldName, int depth)
    {
        return CommonAccept(type, fieldName);
    }

    public string Accept(TLong type, string fieldName, int depth)
    {
        return CommonAccept(type, fieldName);
    }

    public string Accept(TFloat type, string fieldName, int depth)
    {
        return CommonAccept(type, fieldName);
    }

    public string Accept(TDouble type, string fieldName, int depth)
    {
        return CommonAccept(type, fieldName);
    }

    public string Accept(TEnum type, string fieldName, int depth)
    {
        return CommonAccept(type, fieldName);
    }

    public string Accept(TString type, string fieldName, int depth)
    {
        return CommonAccept(type, fieldName);
    }

    public string Accept(TDateTime type, string fieldName, int depth)
    {
        return CommonAccept(type, fieldName);
    }

    public string Accept(TBean type, string fieldName, int depth)
    {
        if (type.DefBean.IsAbstractType)
        {
            var firstImplType = type.DefBean.HierarchyNotAbstractChildren.First();
            return $$"""
            void _Func(Luban.EditorBeanBase __x)
            {
                {{fieldName}} = __x as {{type.DefBean.FullName}};
            }
            {{fieldName}} = {{type.DefBean.FullName}}.Create("{{CsharpUnityGUIJsonTemplateExtension.GetImplTypeName(firstImplType)}}", _Func);
            """;
        }
        else
        {
            return $$"""
            void _Func(Luban.EditorBeanBase __x)
            {
                {{fieldName}} = __x as {{type.DefBean.FullName}};
            }
            {{fieldName}} = new {{type.DefBean.FullName}}(_Func);
            """;
        }
    }

    public string Accept(TArray type, string fieldName, int depth)
    {
        return CommonAccept(type, fieldName);
    }

    public string Accept(TList type, string fieldName, int depth)
    {
        return CommonAccept(type, fieldName);
    }

    public string Accept(TSet type, string fieldName, int depth)
    {
        return CommonAccept(type, fieldName);
    }

    public string Accept(TMap type, string fieldName, int depth)
    {
        return CommonAccept(type, fieldName);
    }
}
