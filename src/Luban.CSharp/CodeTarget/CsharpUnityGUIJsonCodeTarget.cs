using Luban.CodeTarget;
using Luban.CSharp.TemplateExtensions;
using Luban.Defs;
using Luban.Utils;
using Scriban;
using Scriban.Runtime;

namespace Luban.CSharp.CodeTarget;

[CodeTarget("cs-unity-gui-json")]
public class CsharpUnityGUIJsonCodeTarget : CsharpCodeTargetBase
{
    protected override void OnCreateTemplateContext(TemplateContext ctx)
    {
        base.OnCreateTemplateContext(ctx);
        ctx.PushGlobal(new CsharpUnityGUIJsonTemplateExtension());
    }

    public override void GenerateBean(GenerationContext ctx, DefBean bean, CodeWriter writer)
    {
        var template = GetTemplate("bean");
        var tplCtx = CreateTemplateContext(template);
        string topModule = ctx.Target.TopModule;
        var extraEnvs = new ScriptObject
        {
            { "__ctx", ctx},
            { "__top_module", topModule },
            { "__name", bean.Name },
            { "__namespace", bean.Namespace },
            { "__namespace_with_top_module", TypeUtil.MakeFullName(topModule, bean.Namespace) },
            { "__full_name_with_top_module", TypeUtil.MakeFullName(topModule, bean.FullName) },
            { "__bean", bean },
            { "__this", bean },
            {"__fields", bean.Fields},
            {"__hierarchy_fields", bean.HierarchyFields},
            {"__parent_def_type", bean.ParentDefType},
            { "__code_style", CodeStyle},
        };
        tplCtx.PushGlobal(extraEnvs);
        writer.Write(template.Render(tplCtx));
    }

    public override void GenerateEnum(GenerationContext ctx, DefEnum @enum, CodeWriter writer)
    {
        var template = GetTemplate("enum");
        var tplCtx = CreateTemplateContext(template);
        string topModule = ctx.Target.TopModule;
        var extraEnvs = new ScriptObject
        {
            { "__ctx", ctx},
            { "__name", @enum.Name },
            { "__namespace", @enum.Namespace },
            { "__top_module", topModule },
            { "__namespace_with_top_module", TypeUtil.MakeFullName(topModule, @enum.Namespace) },
            { "__full_name_with_top_module", TypeUtil.MakeFullName(topModule, @enum.FullName) },
            { "__enum", @enum },
            { "__this", @enum },
            { "__code_style", CodeStyle},
        };
        tplCtx.PushGlobal(extraEnvs);
        writer.Write(template.Render(tplCtx));
    }
}
