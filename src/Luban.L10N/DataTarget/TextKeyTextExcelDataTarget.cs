using Luban.DataTarget;
using Luban.DataVisitors;
using Luban.Defs;
using System.Text;

namespace Luban.L10N.DataTarget;

[DataTarget("key-text-excel")]
internal class TextKeyTextExcelDataTarget : DataTargetBase
{
    protected override string DefaultOutputFileExt => "txt";

    public override bool ExportAllRecords => true;

    public override AggregationType AggregationType => AggregationType.Table;

    public override OutputFile ExportTable(DefTable table, List<Record> records)
    {
        var textCollection = new TextKeyTextCollection();

        var visitor = new DataActionHelpVisitor2<TextKeyTextCollection>(TextKeyTextCollectorVisitor.Ins);

        TableVisitor.Ins.Visit(table, visitor, textCollection);
        var texts = textCollection.Texts;

        var sb = new StringBuilder();
        foreach (var (key, text) in texts)
        {
            sb.AppendLine($"{key} {text}");
        }

        var content = sb.ToString();
        if (string.IsNullOrEmpty(content))
        {
            return null;
        }
        var fileName = table.FullName.Replace('.', '_');
        var outputFilePath = $"{fileName}.{OutputFileExt}";
        var file = CreateOutputFile(outputFilePath, content);

        return file;
    }
}
