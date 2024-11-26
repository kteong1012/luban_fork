using Luban.CodeFormat;
using Luban.CSharp.TemplateExtensions;
using Luban.Defs;
using Luban.TemplateExtensions;
using Luban.Types;
using Luban.TypeVisitors;
using Luban.Utils;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace Luban.CSharp.TypeVisitors;
class UnityGUIRender : ITypeFuncVisitor<string, int, string>
{
    public static UnityGUIRender Ins { get; } = new UnityGUIRender();

    public string Accept(TBool type, string fieldName, int depth)
    {
        return $$"""
        {{fieldName}} = UnityEditor.EditorGUILayout.Toggle({{fieldName}}, GUILayout.Width(150));
        """;
    }

    public string Accept(TByte type, string fieldName, int depth)
    {
        return $$"""
        {{fieldName}} = (byte)UnityEditor.EditorGUILayout.IntField({{fieldName}}, GUILayout.Width(150));
        """;
    }

    public string Accept(TShort type, string fieldName, int depth)
    {
        return $$"""
        {{fieldName}} = (short)UnityEditor.EditorGUILayout.IntField({{fieldName}}, GUILayout.Width(150));
        """;
    }

    public string Accept(TInt type, string fieldName, int depth)
    {
        return $$"""
        {{fieldName}} = UnityEditor.EditorGUILayout.IntField({{fieldName}}, GUILayout.Width(150));
        """;
    }

    public string Accept(TLong type, string fieldName, int depth)
    {
        return $$"""
        {{fieldName}} = UnityEditor.EditorGUILayout.LongField({{fieldName}}, GUILayout.Width(150));
        """;
    }

    public string Accept(TFloat type, string fieldName, int depth)
    {
        return $$"""
        {{fieldName}} = UnityEditor.EditorGUILayout.DoubleField({{fieldName}}, GUILayout.Width(150));
        """;
    }

    public string Accept(TDouble type, string fieldName, int depth)
    {
        return $$"""
        {{fieldName}} = UnityEditor.EditorGUILayout.DoubleField({{fieldName}}, GUILayout.Width(150));
        """;
    }

    public string Accept(TEnum type, string fieldName, int depth)
    {
        var __value = $"__index{depth}";
        var __alias = $"__alias{depth}";
        if (type.DefEnum.IsFlags)
        {
            return $$"""
            {
                if (ConfigEditorSettings.showComment)
                {
                    var {{__value}} = (int){{fieldName}};
                    var {{__alias}} = ({{type.DefEnum.FullName}}_Alias){{fieldName}};
                    {{__alias}} = ({{type.DefEnum.FullName}}_Alias)UnityEditor.EditorGUILayout.EnumFlagsField({{__alias}}, GUILayout.Width(150));
                    {{fieldName}} = ({{type.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}})((int){{__alias}});
                }
                else
                {
                    {{fieldName}} = ({{type.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}})UnityEditor.EditorGUILayout.EnumFlagsField({{fieldName}}, GUILayout.Width(150));
                }
            }
            """;
        }
        else
        {
            var __item = $"__item{depth}";
            return $$"""
            {
                if (ConfigEditorSettings.showComment)
                {
                    var {{__value}} = (int){{fieldName}};
                    var {{__alias}} = ({{type.DefEnum.FullName}}_Alias){{fieldName}};
                    {{__alias}} = ({{type.DefEnum.FullName}}_Alias)UnityEditor.EditorGUILayout.EnumPopup({{__alias}}, GUILayout.Width(150));
                    var {{__item}} = {{type.DefEnum.FullName}}_Metadata.GetByNameOrAlias({{__alias}}.ToString());
                    {{fieldName}} = ({{type.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}}){{__item}}.Value;
                }
                else
                {
                    {{fieldName}} = ({{type.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}})UnityEditor.EditorGUILayout.EnumPopup({{fieldName}}, GUILayout.Width(150));
                }
            }
            """;
        }
    }

    public string Accept(TString type, string fieldName, int depth)
    {
        if (CsharpUnityGUIJsonTemplateExtension.IsUnityObjectFieldType(type))
        {
            var objTypeName = CsharpUnityGUIJsonTemplateExtension.GetUnityObjectTypeName(type.GetTag("obj"));
            var ret = $$"""
                            {{fieldName}} = UnityEditor.EditorGUILayout.ObjectField({{fieldName}}, typeof({{objTypeName}}), false, GUILayout.Width(150)) as {{objTypeName}};
                            """;
            var tag = type.GetTag("obj");
            if (tag == "sprite")
            {
                ret += $$"""
                        if ({{fieldName}} != null)
                        {
                            UnityEngine.GUILayout.Label(((UnityEngine.Sprite){{fieldName}}).texture, GUILayout.Height(50));
                        }
                        """;
            }
            else if (tag == "texture")
            {
                ret += $$"""
                        if ({{fieldName}} != null)
                        {
                            UnityEngine.GUILayout.Label((UnityEngine.Texture){{fieldName}}, GUILayout.Height(50));
                        }
                        """;
            }
            return ret;
        }
        else
        {
            var ret = $"{fieldName} = UnityEditor.EditorGUILayout.TextField({fieldName}, GUILayout.Width(150));";

            ret += $$"""

                if (GUILayout.Button("…", GUILayout.Width(20)))
                {
                    TextInputWindow.GetTextAsync({{fieldName}},__x =>{{fieldName}} = __x);
                }
                """;
            return ret;
        }
    }

    public string Accept(TDateTime type, string fieldName, int depth)
    {
        return $"{fieldName} = UnityEditor.EditorGUILayout.LongField({fieldName}, GUILayout.Width(150));";
    }

    public string Accept(TBean type, string fieldName, int depth)
    {
        if (type.DefBean.IsAbstractType)
        {
            var __list = $"__list{depth}";
            var __newIndex = $"__newIndex{depth}";
            var __type = $"__type{depth}";
            var __impl = $"__impl{depth}";
            var __x = $"__x";

            var createNewLine = "";
            if (type.DefBean.HierarchyNotAbstractChildren.Count > 0)
            {
                if (fieldName != "this" && fieldName != "__SelectData")
                {
                    createNewLine = $$"""
                    if ({{fieldName}} == null)
                    {   
                        {{type.Apply(UnityGUIInitFieldVisitor.Ins, fieldName, fieldName, depth + 1)}}
                    }
                    """;
                }
            }

            return $$"""
            {
                {{createNewLine}}
                {{type.DefBean.FullName}}.Render{{type.DefBean.Name}}(ref {{fieldName}});
            }
            """;
        }
        else
        {
            if (type.DefBean.HierarchyExportFields.Count > 0)
            {
                var sb = $$"""
                {
                    UnityEditor.EditorGUILayout.BeginVertical(_areaStyle);
                """;
                foreach (var f in type.DefBean.HierarchyExportFields)
                {
                    sb += $$"""
                    UnityEditor.EditorGUILayout.BeginHorizontal();
                    if (ConfigEditorSettings.showComment)
                    {
                        UnityEditor.EditorGUILayout.LabelField(new UnityEngine.GUIContent("{{f.GetCommentOrName()}}", "{{f.Name}}"), GUILayout.Width(100));
                    }
                    else
                    {
                        UnityEditor.EditorGUILayout.LabelField(new UnityEngine.GUIContent("{{f.Name}}", "{{f.Comment}}"), GUILayout.Width(100));
                    }
                    {{f.CType.Apply(this, $"{fieldName}.{TypeTemplateExtension.FormatFieldName(CodeFormatManager.Ins.CsharpDefaultCodeStyle, f.Name)}", depth + 1)}}
                    UnityEditor.EditorGUILayout.EndHorizontal();
                    """;
                }
                sb += $$"""
                    UnityEditor.EditorGUILayout.EndVertical();
                }
                """;
                return sb;
            }
            else
            {
                return "";
            }
        }
    }

    public string Accept(TArray type, string fieldName, int depth)
    {
        var __n = $"__n{depth}";
        var __i = $"__i{depth}";
        var __e = $"__e{depth}";
        var __list = $"__list{depth}";
        var __importJsonText = $"__importJsonText{depth}";
        var __importJson = $"__importJson{depth}";
        var __importElement = $"__importElement{depth}";

        return $$"""
        {
            UnityEditor.EditorGUILayout.BeginVertical(_areaStyle);
            int {{__n}} = {{fieldName}}.Length;
            UnityEditor.EditorGUILayout.LabelField("长度: " + {{__n}}.ToString());
            for (int {{__i}} = 0; {{__i}} < {{__n}}; {{__i}}++)
            {
                UnityEditor.EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    var {{__list}} = new System.Collections.Generic.List<{{type.ElementType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}}>({{fieldName}});
                    {{__list}}.RemoveAt({{__i}});
                    {{fieldName}} = {{__list}}.ToArray();
                    UnityEditor.EditorGUILayout.EndHorizontal();
                    break;
                }
                UnityEditor.EditorGUILayout.LabelField({{__i}}.ToString(), GUILayout.Width(20));
                {{type.ElementType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}} {{__e}} = {{fieldName}}[{{__i}}];
                {{type.ElementType.Apply(this, __e, depth + 1)}};
                {{fieldName}}[{{__i}}] = {{__e}};
                UnityEditor.EditorGUILayout.EndHorizontal();
            }
            UnityEditor.EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+", GUILayout.Width(20)))
            {
                var {{__list}} = new System.Collections.Generic.List<{{type.ElementType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}}>({{fieldName}});
                {{type.ElementType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}} {{__e}};
                {{type.ElementType.Apply(UnityGUIInitFieldVisitor.Ins, __e, GetArrayIndexExpression(fieldName, __e), depth + 1)}};
                {{__list}}.Add({{__e}});
                {{fieldName}} = {{__list}}.ToArray();
            }
            if (ConfigEditorSettings.showImportButton && GUILayout.Button("import", GUILayout.Width(100)))
            {
                ConfigEditorImportWindow.Open(({{__importJsonText}}) => 
                {
                    var {{__importJson}} = SimpleJSON.JSON.Parse({{__importJsonText}});
                    {{type.ElementType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}} {{__importElement}};
                    {{type.ElementType.Apply(UnityGUIJsonLoad.Ins, __importJson, __importElement, depth + 1)}}
                    var {{__list}} = new System.Collections.Generic.List<{{type.ElementType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}}>({{fieldName}});
                    {{__list}}.Add({{__importElement}});
                    {{fieldName}} = {{__list}}.ToArray();
                });
            }
            UnityEditor.EditorGUILayout.EndHorizontal();
            UnityEditor.EditorGUILayout.EndVertical();
        }
        """;
    }

    public string Accept(TList type, string fieldName, int depth)
    {
        var __n = $"__n{depth}";
        var __i = $"__i{depth}";
        var __e = $"__e{depth}";
        var __importJsonText = $"__importJsonText{depth}";
        var __importJson = $"__importJson{depth}";
        var __importElement = $"__importElement{depth}";

        return $$"""
        {
            UnityEditor.EditorGUILayout.BeginVertical(_areaStyle);
            int {{__n}} = {{fieldName}}.Count;
            UnityEditor.EditorGUILayout.LabelField("长度: " + {{__n}}.ToString());
            for (int {{__i}} = 0; {{__i}} < {{__n}}; {{__i}}++)
            {
                UnityEditor.EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    {{fieldName}}.RemoveAt({{__i}});
                    UnityEditor.EditorGUILayout.EndHorizontal();
                    break;
                }
                UnityEditor.EditorGUILayout.LabelField({{__i}}.ToString(), GUILayout.Width(20));
                {{type.ElementType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}} {{__e}} = {{fieldName}}[{{__i}}];
                {{type.ElementType.Apply(this, __e, depth + 1)}};
                {{fieldName}}[{{__i}}] = {{__e}};
                UnityEditor.EditorGUILayout.EndHorizontal();
            }
            UnityEditor.EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+", GUILayout.Width(20)))
            {
                {{type.ElementType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}} {{__e}};
                {{type.ElementType.Apply(UnityGUIInitFieldVisitor.Ins, __e, GetListIndexExpression(fieldName, __e), depth + 1)}};
                {{fieldName}}.Add({{__e}});
            }
            if (ConfigEditorSettings.showImportButton && GUILayout.Button("import", GUILayout.Width(100)))
            {
                ConfigEditorImportWindow.Open(({{__importJsonText}}) => 
                {
                    var {{__importJson}} = SimpleJSON.JSON.Parse({{__importJsonText}});
                    {{type.ElementType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}} {{__importElement}};
                    {{type.ElementType.Apply(UnityGUIJsonLoad.Ins, __importJson, __importElement, depth + 1)}}
                    {{fieldName}}.Add({{__importElement}});
                });
            }
            UnityEditor.EditorGUILayout.EndHorizontal();
            UnityEditor.EditorGUILayout.EndVertical();
        }
        """;
    }

    public string Accept(TSet type, string fieldName, int depth)
    {
        var __n = $"__n{depth}";
        var __i = $"__i{depth}";
        var __e = $"__e{depth}";
        var __importJsonText = $"__importJsonText{depth}";
        var __importJson = $"__importJson{depth}";
        var __importElement = $"__importElement{depth}";

        return $$"""
        {
            UnityEditor.EditorGUILayout.BeginVertical(_areaStyle);
            int {{__n}} = {{fieldName}}.Count;
            UnityEditor.EditorGUILayout.LabelField("长度: " + {{__n}}.ToString());
            for (int {{__i}} = 0; {{__i}} < {{__n}}; {{__i}}++)
            {
                UnityEditor.EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    {{fieldName}}.RemoveAt({{__i}});
                    UnityEditor.EditorGUILayout.EndHorizontal();
                    break;
                }
                UnityEditor.EditorGUILayout.LabelField({{__i}}.ToString(), GUILayout.Width(20));
                {{type.ElementType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}} {{__e}} = {{fieldName}}[{{__i}}];
                {{type.ElementType.Apply(this, __e, depth + 1)}};
                {{fieldName}}[{{__i}}] = {{__e}};
                UnityEditor.EditorGUILayout.EndHorizontal();
            }
            UnityEditor.EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+", GUILayout.Width(20)))
            {
                {{type.ElementType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}} {{__e}};
                {{type.ElementType.Apply(UnityGUIInitFieldVisitor.Ins, __e, GetListIndexExpression(fieldName, __e), depth + 1)}};
                {{fieldName}}.Add({{__e}});
            }
            if (ConfigEditorSettings.showImportButton && GUILayout.Button("import", GUILayout.Width(100)))
            {
                ConfigEditorImportWindow.Open(({{__importJsonText}}) => 
                {
                    var {{__importJson}} = SimpleJSON.JSON.Parse({{__importJsonText}});
                    {{type.ElementType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}} {{__importElement}};
                    {{type.ElementType.Apply(UnityGUIJsonLoad.Ins, __importJson, __importElement, depth + 1)}}
                    {{fieldName}}.Add({{__importElement}});
                });
            }
            UnityEditor.EditorGUILayout.EndHorizontal();
            UnityEditor.EditorGUILayout.EndVertical();
        }
        """;
    }

    public string Accept(TMap type, string fieldName, int depth)
    {
        // Map在编辑器里是List<object[]>类型，其中object的程度固定为2
        var __n = $"__n{depth}";
        var __i = $"__i{depth}";
        var __e = $"__e{depth}";
        var __key = $"__key{depth}";
        var __value = $"__value{depth}";

        return $$"""
        {
            UnityEditor.EditorGUILayout.BeginVertical(_areaStyle);
            int {{__n}} = {{fieldName}}.Count;
            UnityEditor.EditorGUILayout.LabelField("长度: " + {{__n}}.ToString());
            for (int {{__i}} = 0; {{__i}} < {{__n}}; {{__i}}++)
            {
                UnityEditor.EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    {{fieldName}}.RemoveAt({{__i}});
                    UnityEditor.EditorGUILayout.EndHorizontal();
                    break;
                }
                UnityEditor.EditorGUILayout.LabelField({{__i}}.ToString(), GUILayout.Width(20));
                var {{__e}} = {{fieldName}}[{{__i}}];
                {{type.KeyType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}} {{__key}} = {{type.KeyType.Apply(UnityGUIInitValueVisitor.Ins)}};
                {{type.ValueType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}} {{__value}} = {{type.ValueType.Apply(UnityGUIInitValueVisitor.Ins)}};
                if ({{__e}} == null)
                {
                    {{__e}} = new object[2] { {{__key}}, {{__value}} };
                    {{fieldName}}[{{__i}}] = {{__e}};
                }
                else
                {
                    {{__key}} = {{__e}}[0] != null ? ({{type.KeyType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}}){{__e}}[0] : {{__key}};
                    {{__value}} = {{__e}}[1] != null ? ({{type.ValueType.Apply(UnityGUIDeclaringTypeNameVisitor.Ins)}}){{__e}}[1] : {{__value}};
                }
                {{type.KeyType.Apply(this, __key, depth + 1)}};
                {{type.ValueType.Apply(this, __value, depth + 1)}};
                {{__e}}[0] = {{__key}};
                {{__e}}[1] = {{__value}};
                UnityEditor.EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button("+", GUILayout.Width(20)))
            {
                {{fieldName}}.Add(new object[2]);
            }
            UnityEditor.EditorGUILayout.EndVertical();
        }
        """;
    }

    private static string GetArrayIndexExpression(string arrayName, string elementName)
    {
        return $"{arrayName}[System.Array.IndexOf({arrayName}, {elementName})]";
    }

    private static string GetListIndexExpression(string listName, string elementName)
    {
        return $"{listName}[{listName}.IndexOf({elementName})]";
    }
}
