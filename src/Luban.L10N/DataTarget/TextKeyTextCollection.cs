namespace Luban.L10N.DataTarget;


public class TextKeyTextCollection
{
    private readonly Dictionary<string, string> _texts = new();

    public Dictionary<string, string> Texts => _texts;

    public void AddText(string key, string text)
    {
        if (_texts.TryGetValue(key, out var existingText))
        {
            if (existingText != text)
            {
                throw new Exception($"发现重复的key:{key}，不同的text值: {existingText} 和 {text}");
            }
        }
        else
        {
            _texts[key] = text;
        }
    }
}
