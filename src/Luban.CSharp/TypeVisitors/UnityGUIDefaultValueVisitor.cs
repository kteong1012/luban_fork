//using Luban.CSharp.TemplateExtensions;
//using Luban.Types;
//using Luban.TypeVisitors;

//namespace Luban.CSharp.TypeVisitors;

//public class UnityGUIDefaultValueVisitor : ITypeFuncVisitor<string>
//{
//    public static UnityGUIDefaultValueVisitor Ins { get; } = new();

//    public string Accept(TBool type)
//    {
//        return "default";
//    }

//    public string Accept(TByte type)
//    {
//        return "default";
//    }

//    public string Accept(TShort type)
//    {
//        return "default";
//    }

//    public string Accept(TInt type)
//    {
//        return "default";
//    }

//    public string Accept(TLong type)
//    {
//        return "default";
//    }

//    public string Accept(TFloat type)
//    {
//        return "default";
//    }

//    public string Accept(TDouble type)
//    {
//        return "default";
//    }

//    public string Accept(TEnum type)
//    {
//        return "default";
//    }

//    public string Accept(TString type)
//    {
//        if (CsharpUnityGUIJsonTemplateExtension.IsUnityObjectFieldType(type))
//        {
//            return "default";
//        }
//        else
//        {
//            return "\"\"";
//        }
//    }

//    public string Accept(TDateTime type)
//    {
//        return "default";
//    }

//    public string Accept(TBean type)
//    {
//        return type.DefBean.IsAbstractType ? "default" : $"new {type.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}()";
//    }

//    public string Accept(TArray type)
//    {
//        return $"System.Array.Empty<{type.ElementType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}>()";
//    }

//    public string Accept(TList type)
//    {
//        return $"new {ConstStrings.ListTypeName}<{type.ElementType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}>()";
//    }

//    public string Accept(TSet type)
//    {
//        return $"new {ConstStrings.ListTypeName}<{type.ElementType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}>()";
//    }

//    public string Accept(TMap type)
//    {
//        return $"new {ConstStrings.ListTypeName}<object[]>()";
//    }
//}
