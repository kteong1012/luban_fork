using Luban.CSharp.TemplateExtensions;
using Luban.DataLoader;
using Luban.Types;
using Luban.TypeVisitors;

namespace Luban.CSharp.TypeVisitors;

class UnityGUIJsonLoad : ITypeFuncVisitor<string, string, int, string>
{
    public static UnityGUIJsonLoad Ins { get; } = new();

    public string Accept(TBool type, string json, string x, int depth)
    {
        return $"if(!{json}.IsBoolean) {{ throw new SerializationException(); }}  {x} = {json};";
    }

    public string Accept(TByte type, string json, string x, int depth)
    {
        return $"if(!{json}.IsNumber) {{ throw new SerializationException(); }}  {x} = {json};";
    }

    public string Accept(TShort type, string json, string x, int depth)
    {
        return $"if(!{json}.IsNumber) {{ throw new SerializationException(); }}  {x} = {json};";
    }

    public string Accept(TInt type, string json, string x, int depth)
    {
        return $"if(!{json}.IsNumber) {{ throw new SerializationException(); }}  {x} = {json};";
    }

    public string Accept(TLong type, string json, string x, int depth)
    {
        return $"if(!{json}.IsNumber) {{ throw new SerializationException(); }}  {x} = {json};";
    }

    public string Accept(TFloat type, string json, string x, int depth)
    {
        return $"if(!{json}.IsNumber) {{ throw new SerializationException(); }}  {x} = {json};";
    }

    public string Accept(TDouble type, string json, string x, int depth)
    {
        return $"if(!{json}.IsNumber) {{ throw new SerializationException(); }}  {x} = {json};";
    }

    public string Accept(TEnum type, string json, string x, int depth)
    {
        return $"if({json}.IsString) {{ {x} = ({type.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)})System.Enum.Parse(typeof({type.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}), {json}); }} else if({json}.IsNumber) {{ {x} = ({type.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)})(int){json}; }} else {{ throw new SerializationException(); }}  ";
    }

    public string Accept(TString type, string json, string x, int depth)
    {
        if (CsharpUnityGUIJsonTemplateExtension.IsUnityObjectFieldType(type))
        {
            var jsonVar = $"{x}_Json";
            var objTag = type.GetTag("obj");
            var objTypeName = CsharpUnityGUIJsonTemplateExtension.GetUnityObjectTypeName(objTag);
            return $$"""
                if(!{{json}}.IsString) {throw new SerializationException(); } var {{jsonVar}} = {{json}};
                {{x}} = UnityEditor.AssetDatabase.LoadAssetAtPath<{{objTypeName}}>({{jsonVar}});
                """;
        }
        else
        {
            return $"if(!{json}.IsString) {{ throw new SerializationException(); }}  {x} = {json};";
        }
    }

    public string Accept(TBean type, string json, string x, int depth)
    {
        if (type.DefBean.IsAbstractType)
        {
            var __index = $"__index{depth}";
            return
            $$"""

            if (!{{json}}.IsObject)
            {
                throw new SerializationException();
            }
            {{x}} = {{type.Apply(RawDefineTypeNameVisitor.Ins)}}.LoadJson{{type.DefBean.Name}}({{json}});
            """;
        }
        else
        {
            return $"if(!{json}.IsObject) {{ throw new SerializationException(); }}  {x} = {type.Apply(RawDefineTypeNameVisitor.Ins)}.LoadJson{type.DefBean.Name}({json});";
        }
    }

    public string Accept(TArray type, string json, string x, int depth)
    {
        var __e = $"__e{depth}";
        var __v = $"__v{depth}";
        var __i = $"__i{depth}";
        var __n = $"__n{depth}";
        return $"if(!{json}.IsArray) {{ throw new SerializationException(); }} int {__n} = {json}.Count; {x} = new {type.ElementType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}[{__n}]; int {__i}=0; foreach(SimpleJSON.JSONNode {__e} in {json}.Children) {{ {type.ElementType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)} {__v};  {type.ElementType.Apply(this, __e, __v, depth + 1)}  {x}[{__i}++] = {__v}; }}  ";
    }

    public string Accept(TList type, string json, string x, int depth)
    {
        var __e = $"__e{depth}";
        var __v = $"__v{depth}";
        return $"if(!{json}.IsArray) {{ throw new SerializationException(); }} {x} = new {type.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}(); foreach(SimpleJSON.JSONNode {__e} in {json}.Children) {{ {type.ElementType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)} {__v};  {type.ElementType.Apply(this, __e, __v, depth + 1)}  {x}.Add({__v}); }}  ";
    }

    public string Accept(TSet type, string json, string x, int depth)
    {
        var __e = $"__e{depth}";
        var __v = $"__v{depth}";
        return $"if(!{json}.IsArray) {{ throw new SerializationException(); }} {x} = new {type.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}(); foreach(SimpleJSON.JSONNode {__e} in {json}.Children) {{ {type.ElementType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)} {__v};  {type.ElementType.Apply(this, __e, __v, depth + 1)}  {x}.Add({__v}); }}  ";
    }

    public string Accept(TMap type, string json, string x, int depth)
    {
        var __e = $"__e{depth}";
        var __k = $"__k{depth}";
        var __v = $"__v{depth}";
        var ret = @$"if(!{json}.IsArray) {{ throw new SerializationException(); }} {x} = new {type.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}(); foreach(SimpleJSON.JSONNode {__e} in {json}.Children) {{ {type.KeyType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)} {__k};  {type.KeyType.Apply(this, $"{__e}[0]", __k, depth + 1)} {type.ValueType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)} {__v};  {type.ValueType.Apply(this, $"{__e}[1]", __v, depth + 1)}  {x}.Add(new object[] {{ {__k}, {__v} }}); }}  ";
        return ret;
    }

    public string Accept(TDateTime type, string json, string x, int depth)
    {
        return $$"""
        if ({{json}}.IsString)
        {
            {{x}} = {{json}};
        }
        else if ({{json}}.IsNumber)
        {
            {{x}} = {{json}};
        }
        else
        {
            throw new SerializationException();
        }
        """;
    }
}
