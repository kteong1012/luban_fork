using Luban.DataTarget;
using Luban.Defs;
using Luban.Utils;
using System.Text.Json;

namespace Luban.DataExporter.Builtin.Json;

[DataTarget("json3")]
public class Json3DataTarget : DataTargetBase
{
    protected override string DefaultOutputFileExt => "json";

    public static bool UseCompactJson => EnvManager.Current.GetBoolOptionOrDefault("json", "compact", true, false);

    protected virtual JsonDataVisitor ImplJsonDataVisitor => JsonDataVisitor.Ins;

    public void WriteAsArray(List<Record> datas, Utf8JsonWriter x, JsonDataVisitor jsonDataVisitor)
    {
        x.WriteStartArray();
        foreach (var d in datas)
        {
            d.Data.Apply(jsonDataVisitor, x);
        }
        x.WriteEndArray();
    }

    public override OutputFile ExportTable(DefTable table, List<Record> records)
    {
        var ss = new MemoryStream();
        var jsonWriter = new Utf8JsonWriter(ss, new JsonWriterOptions()
        {
            Indented = !UseCompactJson,
            SkipValidation = false,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        });
        WriteAsArray(records, jsonWriter, ImplJsonDataVisitor);
        jsonWriter.Flush();

        // 使用 SimpleJSON 重新format一遍
        var bytes = DataUtil.StreamToBytes(ss);
        var jsonStr = System.Text.Encoding.UTF8.GetString(bytes);
        var json = SimpleJSON.JSON.Parse(jsonStr);
        var content = json.ToString(4);

        return new OutputFile()
        {
            File = $"{table.OutputDataFile}.{OutputFileExt}",
            Content = content,
        };
    }
}
