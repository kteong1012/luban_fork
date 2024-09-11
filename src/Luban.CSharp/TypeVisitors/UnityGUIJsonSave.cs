using Luban.CSharp.TemplateExtensions;
using Luban.Types;
using Luban.TypeVisitors;

namespace Luban.CSharp.TypeVisitors;
class UnityGUIJsonSave : ITypeFuncVisitor<string, string, string, int, string>
{
    public static UnityGUIJsonSave Ins { get; } = new();

    public string Accept(TBool type, string jsonName, string jsonFieldName, string value, int depth)
    {
        return $"{jsonName}[\"{jsonFieldName}\"] = new JSONBool({value});";
    }

    public string Accept(TByte type, string jsonName, string jsonFieldName, string value, int depth)
    {
        return $"{jsonName}[\"{jsonFieldName}\"] = new JSONNumber({value});";
    }

    public string Accept(TShort type, string jsonName, string jsonFieldName, string value, int depth)
    {
        return $"{jsonName}[\"{jsonFieldName}\"] = new JSONNumber({value});";
    }

    public string Accept(TInt type, string jsonName, string jsonFieldName, string value, int depth)
    {
        return $"{jsonName}[\"{jsonFieldName}\"] = new JSONNumber({value});";
    }

    public string Accept(TLong type, string jsonName, string jsonFieldName, string value, int depth)
    {
        return $"{jsonName}[\"{jsonFieldName}\"] = new JSONNumber({value});";
    }

    public string Accept(TFloat type, string jsonName, string jsonFieldName, string value, int depth)
    {
        return $"{jsonName}[\"{jsonFieldName}\"] = new JSONNumber({value});";
    }

    public string Accept(TDouble type, string jsonName, string jsonFieldName, string value, int depth)
    {
        return $"{jsonName}[\"{jsonFieldName}\"] = new JSONNumber({value});";
    }

    public string Accept(TEnum type, string jsonName, string jsonFieldName, string value, int depth)
    {
        return $"{jsonName}[\"{jsonFieldName}\"] = new JSONNumber((int){value});";
    }

    public string Accept(TString type, string jsonName, string jsonFieldName, string value, int depth)
    {
        if (CsharpUnityGUIJsonTemplateExtension.IsUnityObjectFieldType(type))
        {
            var pathVar = $"{jsonFieldName}_Path";
            var objTag = type.GetTag("obj");
            var objTypeName = CsharpUnityGUIJsonTemplateExtension.GetUnityObjectTypeName(objTag);
            return $$"""
                var {{pathVar}} = {{value}} == null ? "" : UnityEditor.AssetDatabase.GetAssetPath({{value}});
                {{jsonName}}["{{jsonFieldName}}"] = new JSONString({{pathVar}});
                """;
        }
        else
        {
            return $"{jsonName}[\"{jsonFieldName}\"] = new JSONString({value});";
        }
    }

    public string Accept(TDateTime type, string jsonName, string jsonFieldName, string value, int depth)
    {
        return $"{jsonName}[\"{jsonFieldName}\"] = new JSONNumber({value});";
    }

    public string Accept(TBean type, string jsonName, string jsonFieldName, string value, int depth)
    {
        return $"{{ var __bjson = new JSONObject();  {type.Apply(RawDefineTypeNameVisitor.Ins)}.SaveJson{type.DefBean.Name}({value}, __bjson); {jsonName}[\"{jsonFieldName}\"] = __bjson; }}";
    }

    public string Accept(TArray type, string jsonName, string jsonFieldName, string value, int depth)
    {
        //return $"{{ var __cjson = new JSONArray(); foreach(var _e in {value}) {{ {type.ElementType.Apply(this, "__cjson", "null", "_e")} }} {jsonName}[\"{jsonFieldName}\"] = __cjson; }}";
        var __e = $"__e{depth}";
        var __v = $"__v{depth}";
        var __i = $"__i{depth}";
        var __n = $"__n{depth}";
        var __cjson = $"__cjson{depth}";
        return $"{{ var {__cjson} = new JSONArray(); foreach(var {__e} in {value}) {{ {type.ElementType.Apply(this, __cjson, "null", __e, depth + 1)} }} {__cjson}.Inline = {__cjson}.Count == 0; {jsonName}[\"{jsonFieldName}\"] = {__cjson}; }}";
    }

    public string Accept(TList type, string jsonName, string jsonFieldName, string value, int depth)
    {
        //return $"{{ var __cjson = new JSONArray(); foreach(var _e in {value}) {{ {type.ElementType.Apply(this, "__cjson", "null", "_e")} }} {jsonName}[\"{jsonFieldName}\"] = __cjson; }}";
        var __e = $"__e{depth}";
        var __v = $"__v{depth}";
        var __i = $"__i{depth}";
        var __n = $"__n{depth}";
        var __cjson = $"__cjson{depth}";
        return $"{{ var {__cjson} = new JSONArray(); foreach(var {__e} in {value}) {{ {type.ElementType.Apply(this, __cjson, "null", __e, depth + 1)} }} {__cjson}.Inline = {__cjson}.Count == 0; {jsonName}[\"{jsonFieldName}\"] = {__cjson}; }}";
    }

    public string Accept(TSet type, string jsonName, string jsonFieldName, string value, int depth)
    {
        //return $"{{ var __cjson = new JSONArray(); foreach(var _e in {value}) {{ {type.ElementType.Apply(this, "__cjson", "null", "_e")} }} {jsonName}[\"{jsonFieldName}\"] = __cjson; }}";
        var __e = $"__e{depth}";
        var __v = $"__v{depth}";
        var __i = $"__i{depth}";
        var __n = $"__n{depth}";
        var __cjson = $"__cjson{depth}";
        return $"{{ var {__cjson} = new JSONArray(); foreach(var {__e} in {value}) {{ {type.ElementType.Apply(this, __cjson, "null", __e, depth + 1)} }} {__cjson}.Inline = {__cjson}.Count == 0; {jsonName}[\"{jsonFieldName}\"] = {__cjson}; }}";
    }

    public string Accept(TMap type, string jsonName, string jsonFieldName, string value, int depth)
    {
        //return $"{{ var __cjson = new JSONArray(); foreach(var _e in {value}) {{ var __entry = new JSONArray(); __cjson[null] = __entry; {type.KeyType.Apply(this, "__entry", "null", "_e.Key")} {type.ValueType.Apply(this, "__entry", "null", "_e.Value")} }} {jsonName}[\"{jsonFieldName}\"] = __cjson; }}";
        var __e = $"__e{depth}";
        var __cjson = $"__cjson{depth}";
        var __entry = $"__entry{depth}";
        var keyTypeStr = type.KeyType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins);
        var valueTypeStr = type.ValueType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins);

        return $$"""
        {
            var {{__cjson}} = new JSONArray();
            foreach(var {{__e}} in {{value}})
            {
                var {{__entry}} = new JSONArray();
                {{__cjson}}[null] = {{__entry}};
                {{type.KeyType.Apply(this, __entry, "null", $"({keyTypeStr}){__e}[0]", depth + 1)}}
                {{type.ValueType.Apply(this, __entry, "null", $"({valueTypeStr}){__e}[1]", depth + 1)}}
            }
            {{__cjson}}.Inline = {{__cjson}}.Count == 0;
            {{jsonName}}["{{jsonFieldName}}"] = {{__cjson}};
        }
        """;


    }
}
