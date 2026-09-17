using BIMPlugins.Nuke;
using Nuke.Common;
using Nuke.Common.ProjectModel;

public class Build : BIMPluginsBuild
{
    protected override int MajorVersion => 1;
    protected override int MinorVersion => 0;
    protected override int MaintenanceVersion => 7;


    [Solution("BIMPlugins.Windows.sln")]
    protected override Solution Solution { get; }

    [Parameter("Имя проекта для сборки")]
    protected override string ProjectName { get; } = "BIMPlugins.Windows";

    [Parameter("Имена конфигураций для сборки (перечислить через запятую)")]
    protected override string[] Configurations { get; } = ["Release"];

    public static int Main() => Execute<Build>(x => x.Compile);
}