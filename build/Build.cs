using BIMPlugins.Nuke;
using Nuke.Common;
using Nuke.Common.ProjectModel;

public class Build : BIMPluginsBuild
{
    protected override int MajorVersion => 3;
    protected override int MinorVersion => 0;
    protected override int MaintenanceVersion => 1;

    [Solution("BIMPlugins.Windows.sln")]
    protected override Solution Solution { get; }

    [Parameter("Name of the project to build")]
    protected override string ProjectName { get; } = "BIMPlugins.Windows";

    // One configuration: the project targets net48 and net8.0-windows at once, so one package serves every Revit version.
    [Parameter("Configurations to build")]
    protected override string[] Configurations { get; } = ["Release"];

    public static int Main() => Execute<Build>(x => x.Compile);
}