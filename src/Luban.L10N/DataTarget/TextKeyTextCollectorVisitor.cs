

using Luban.Datas;
using Luban.DataVisitors;
using Luban.Defs;
using Luban.Types;

namespace Luban.L10N.DataTarget;

/// <summary>
/// 检查 相同key的text,原始值必须相同
/// </summary>
public class TextKeyTextCollectorVisitor : IDataActionVisitor2<TextKeyTextCollection>
{
    public static TextKeyTextCollectorVisitor Ins { get; } = new TextKeyTextCollectorVisitor();

    public void Accept(DBool data, TType type, TextKeyTextCollection x)
    {

    }

    public void Accept(DByte data, TType type, TextKeyTextCollection x)
    {

    }

    public void Accept(DShort data, TType type, TextKeyTextCollection x)
    {

    }

    public void Accept(DInt data, TType type, TextKeyTextCollection x)
    {

    }

    public void Accept(DLong data, TType type, TextKeyTextCollection x)
    {

    }

    public void Accept(DFloat data, TType type, TextKeyTextCollection x)
    {

    }

    public void Accept(DDouble data, TType type, TextKeyTextCollection x)
    {

    }

    public void Accept(DEnum data, TType type, TextKeyTextCollection x)
    {

    }

    public void Accept(DString data, TType type, TextKeyTextCollection x)
    {
        if (data != null && type.HasTag("text"))
        {
            var value = data.Value;
            var index = value.IndexOf(".");
            var key = index > 0 ? value.Substring(index + 1) : value;
            var text = data.L10nText;
            // 把换行符变成\n
            if (text != null)
            {
                text = text.Replace("\r\n", "\\n").Replace("\n", "\\n");
            }
            x.AddText(key, text);
        }
    }

    public void Accept(DDateTime data, TType type, TextKeyTextCollection x)
    {

    }

    public void Accept(DBean data, TType type, TextKeyTextCollection x)
    {

    }

    public void Accept(DArray data, TType type, TextKeyTextCollection x)
    {

    }

    public void Accept(DList data, TType type, TextKeyTextCollection x)
    {

    }

    public void Accept(DSet data, TType type, TextKeyTextCollection x)
    {

    }

    public void Accept(DMap data, TType type, TextKeyTextCollection x)
    {

    }
}
