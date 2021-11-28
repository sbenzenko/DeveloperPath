using System.Text.Json;

namespace DeveloperPath.BuildInfo
{
    public record BuildInfo(string BranchName, string BuildNumber, string BuildId, string CommitHash);

    public static class AppVersionInfo
    {
        private const string _buildFileName = "buildinfo.json";
        private static BuildInfo _fileBuildInfo = new(
            BranchName: "",
            BuildNumber: DateTime.UtcNow.ToString("yyyyMMdd") + ".0",
            BuildId: "xxxxxx",
            CommitHash: $"Not yet initialised - call {nameof(InitialiseBuildInfoGivenPath)}"
        );

        public static void InitialiseBuildInfoGivenPath(string path)
        {
            var buildFilePath = Path.Combine(path, _buildFileName);
            if (File.Exists(buildFilePath))
            {
                try
                {
                    var buildInfoJson = File.ReadAllText(buildFilePath);
                    var buildInfo = JsonSerializer.Deserialize<BuildInfo>(buildInfoJson, new JsonSerializerOptions
                    {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

                    if (buildInfo == null) throw new Exception($"Failed to deserialise {_buildFileName}");

                    _fileBuildInfo = buildInfo;
                }
                catch (Exception)
                {
                    _fileBuildInfo = new BuildInfo(
                        BranchName: "",
                        BuildNumber: DateTime.UtcNow.ToString("yyyyMMdd") + ".0",
                        BuildId: "xxxxxx",
                        CommitHash: "Failed to load build info from buildinfo.json"
                    );
                }
            }
        }

        public static BuildInfo GetBuildInfo() => _fileBuildInfo;
    }
}