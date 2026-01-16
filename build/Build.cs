using BIMPlugins.Nuke;
using Nuke.Common;
using Nuke.Common.ProjectModel;

public class Build : BIMPluginsBuild
{
    protected override int MajorVersion => 1;
    protected override int MinorVersion => 0;
    protected override int MaintenanceVersion => 5;


    [Solution("BIMPlugins.Windows.sln")]
    public override Solution Solution { get; }

    [Parameter("Project to build name")]
    public override string ProjectName { get; } = "BIMPlugins.Windows";

    [Parameter("Build configurations to run sequentially")]
    public override Configuration[] Configurations { get; } = new[] { Configuration.Release };

    public static int Main() => Execute<Build>(x => x.Compile);
}