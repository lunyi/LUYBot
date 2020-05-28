using Models;
using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Service;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild
{
    [Parameter("Environment", Name = "environment")]
    public string Environment { get; set; } = "development";

    private const string LocalRegistryUrl = "localhost:5000";
    private const string TGPBOT01Ip = "10.25.3.26";
    private const string UserName = "ugs.build";
    private const string UserPass = "Asd$RFV44";

    public static int Main() => Execute<Build>(x => x.UpdateDockerCompose);

    [Solution("UGS.TGPBot.sln")] readonly Solution Solution;

    Target Restore => _ => _
            .Executes(() =>
            {
                DotNetRestore(s =>
                {
                    s = s.SetProjectFile(Solution);
                    s = s.SetConfigFile($"{Solution.Directory}/.nuget/NuGet.Config");
                    return s;
                });
            });

    Project GlobalToolProject => Solution.GetProject("TGPBot").NotNull();

    Target Publish => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            Logger.Info("To publish tgpbot.");

            DotNetPublish(s => s
                .SetConfiguration("Production")
                .SetProject(GlobalToolProject)
                .EnableNoRestore()
                );
        });

    Target BuildDockerImage => _ => _
                .DependsOn(Publish)
                .Executes(() =>
                {
                    Logger.Info($"To build tgpbot image.");
                    var imageTagName = GetImageTag(LocalRegistryUrl);
                    var cmd = $"-c \"chmod +x ./Deployment/ShellScripts/buildimage.sh; ./Deployment/ShellScripts/buildimage.sh {imageTagName} \"";
                    var buildMsg = ProcessService.ProcessBash(cmd);
                    Logger.Info(buildMsg);
                });

    Target PushDockerImage => _ => _
                .DependsOn(BuildDockerImage)
                .Executes(() =>
                {
                    var imageTagName = GetImageTag(LocalRegistryUrl);
                    var cmd = $"-c \" docker push {imageTagName} \"";
                    var buildMsg = ProcessService.ProcessBash(cmd);
                    Logger.Info(buildMsg);
                });

    Target CopyDeploymentScript => _ => _
                .DependsOn(PushDockerImage)
                .Executes(() =>
                {   
                    var sourceDirectory = "TGPBot";
                    var destinationDirectory = $"/docker/{Environment}";
                    var fileByEnvironment = $"docker-compose.{Environment}.yml";

                    var uploadFiles = new []
                    {
                        new UploadFile
                        {
                            Source = $"{sourceDirectory}/docker-compose.yml",
                            Destination = $"{destinationDirectory}/docker-compose.yml"
                        },
                        new UploadFile
                        {
                            Source = $"{sourceDirectory}/{fileByEnvironment}",
                            Destination = $"{destinationDirectory}/{fileByEnvironment}"
                        },
                        new UploadFile
                        {
                            Source = "./Deployment/ShellScripts/tgpbot-deployment.sh",
                            Destination = "./tgpbot-deployment.sh"
                        }
                    };
                    
                    foreach (var f in uploadFiles)
                    {
                        Logger.Info($"Copy file from {f.Source} to {f.Destination} to server {TGPBOT01Ip}");
                    }
                    
                    SSHService.SftpWriteServerFile(TGPBOT01Ip, UserName, UserPass, uploadFiles);
                });

    Target UpdateDockerCompose => _ => _
                .DependsOn(CopyDeploymentScript)
                .Executes(() =>
                {
                    var cmd = $"chmod +x ./tgpbot-deployment.sh; ./tgpbot-deployment.sh {Environment}";
                    Logger.Info($"> command: {cmd}");

                    var msg = SSHService.SshInitialize(TGPBOT01Ip, UserName, UserPass, cmd);
                    Logger.Info(msg);
                });

    private string GetImageTag(string registryUrl) 
    {
        return $"{registryUrl}/tgpbot:{Environment}";
    }
}