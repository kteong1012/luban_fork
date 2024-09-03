using Luban.CSharp.TypeVisitors;
using Luban.DataLoader;
using Luban.Defs;
using Luban.Types;
using Luban.Utils;
using Scriban.Runtime;

namespace Luban.CSharp.TemplateExtensions;

public class CsharpUnityGUIJsonTemplateExtension : ScriptObject
{
    public static string DeclaringTypeName(TType type)
    {
        return type.Apply(UnityGUIDeclaringTypeNameVisitor.Ins);
    }

    public static string Deserialize(string jsonName, string fieldName, TType type)
    {
        return $"{type.Apply(UnityGUIJsonLoad.Ins, jsonName, fieldName, 0)}";
    }

    public static string Serialize(string jsonName, string jsonFieldName, string fieldName, TType type)
    {
        return $"{type.Apply(UnityGUIJsonSave.Ins, jsonName, jsonFieldName, fieldName, 0)}";
    }

    public static bool IsRawNullable(TType type)
    {
        return type.Apply(IsRawNullableTypeVisitor.Ins);
    }

    public static bool NeedInit(TType type)
    {
        if (type.IsNullable)
        {
            return true;
        }
        return type.Apply(EditorNeedInitVisitor.Ins);
    }

    public static string InitValue(TType type)
    {
        return type.Apply(UnityGUIInitValueVisitor.Ins);
    }

    public static string InitField(TType type, string fieldName, string fieldName2)
    {
        return type.Apply(UnityGUIInitFieldVisitor.Ins, fieldName, fieldName2, 0);
    }

    public static string RenderBean(DefBean defBean, string name)
    {
        var ttype = TBean.Create(false, defBean, new Dictionary<string, string>());
        return ttype.Apply(UnityGUIRender.Ins, name, 0);
    }

    public static bool IsUnityObjectFieldType(TType type)
    {
        if (type is TString str)
        {
            var tag = str.GetTag("obj");
            if (string.IsNullOrEmpty(tag))
            {
                return false;
            }
            return GetUnityObjectTypeName(tag) != null;
        }
        else if (type is TMap map)
        {
            return IsUnityObjectFieldType(map.ValueType);
        }
        else if (type.IsCollection)
        {
            return IsUnityObjectFieldType(type.ElementType);
        }
        else
        {
            return false;
        }
    }

    public static bool IsUnityObjectFieldTypeByField(DefField field)
    {
        var type = field.CType;
        return IsUnityObjectFieldType(type);
    }

    public static string GetUnityObjectTypeName(string objType)
    {
        if (objType == "sprite")
        {
            return "UnityEngine.Sprite";
        }
        else if (objType == "texture")
        {
            return "UnityEngine.Texture2D";
        }
        else if (objType == "audioClip")
        {
            return "UnityEngine.AudioClip";
        }
        else if (objType == "animationClip")
        {
            return "UnityEngine.AnimationClip";
        }
        else if (objType == "material")
        {
            return "UnityEngine.Material";
        }
        else if (objType == "gameObject")
        {
            return "UnityEngine.GameObject";
        }
        else if (objType == "prefab")
        {
            return "UnityEngine.GameObject";
        }
        else if (objType == "font")
        {
            return "UnityEngine.Font";
        }
        else if (objType == "textAsset")
        {
            return "UnityEngine.TextAsset";
        }
        else if (objType == "shader")
        {
            return "UnityEngine.Shader";
        }
        else if (objType == "scriptableObject")
        {
            return "UnityEngine.ScriptableObject";
        }
        else
        {
            throw new Exception($"unknown unity object type: {objType}");
        }
    }

    public static string GetImplTypeName(DefBean defBean)
    {
        var parentDefBean = defBean.ParentDefType;

        return DataUtil.GetImplTypeName(defBean, parentDefBean);
    }

    public static string GetTypeAliasOrName(DefBean defBean)
    {
        return string.IsNullOrWhiteSpace(defBean.Alias) ? defBean.Name : defBean.Alias;
    }
}
