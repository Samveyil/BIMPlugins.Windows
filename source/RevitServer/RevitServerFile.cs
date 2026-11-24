namespace BIMPlugins.Windows.RevitServer
{
    /// <summary>A project chosen on Revit Server.</summary>
    public sealed class RevitServerFile
    {
        internal RevitServerFile(string name, string path, string version)
        {
            Name = name;
            Path = path;
            Version = version;
        }

        /// <summary>The file's name, such as <c>OLP_R23_NVD11_BASE.rvt</c>.</summary>
        public string Name { get; }

        /// <summary>The path to open it by, such as <c>RSN://srv4/Новая дорога 11/00_КООРД/OLP_R23_NVD11_BASE.rvt</c>.</summary>
        public string Path { get; }

        /// <summary>The Revit version of its server, such as <c>2023</c>.</summary>
        public string Version { get; }

        /// <summary>The <see cref="Path"/>.</summary>
        public override string ToString() => Path;
    }
}
