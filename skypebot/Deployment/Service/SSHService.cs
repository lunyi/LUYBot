using System;
using System.IO;
using Models;
using Renci.SshNet;

namespace Service
{
    public class SSHService
    {
        public static string SshInitialize(string ip, string user, string pass, string cmd)
        {
            var conInfo = new ConnectionInfo(ip, 22, user,
                new AuthenticationMethod[]
                {
                    new PasswordAuthenticationMethod(user, pass)
                });

            var msg = string.Empty;
            using (var sshClient = new SshClient(conInfo))
            {
                sshClient.Connect();

                var sshCmd = sshClient.CreateCommand($"echo {pass} | {cmd}");
                sshCmd.Execute();
                if (sshCmd.ExitStatus == 0)
                {
                    msg = sshCmd.Error.Replace("\n", Environment.NewLine);
                }
                else
                {
                    throw new Exception("SSH Error: " + sshCmd.Error);
                }
            }
            return msg;
        }

        public static void SftpWriteServerFile(string ip, string user, string pass, UploadFile[] files)
        {
            using (var client = new SftpClient(ip, user, pass))
            {
                client.Connect();

                foreach (var f in files)
                {
                    using (Stream s = File.OpenRead(f.Source))
                    {
                        client.UploadFile(s, f.Destination);
                    }
                }
            }
        }
    }
}
