using System;
using System.Diagnostics;

namespace Service
{
    public class ProcessService
    {
        public static string ProcessBash(string cmd, bool redirectStandardOutput = true, bool redirectStandardError = true)
        {
            var ps = new Process
            {
                StartInfo =
                {
                    FileName = "/bin/bash",
                    Arguments = cmd,
                    RedirectStandardOutput = redirectStandardOutput,
                    RedirectStandardError = redirectStandardError
                }
            };
            ps.Start();
            var buildMsg = ps.StandardOutput.ReadToEnd();
            var errorMsg = ps.StandardError.ReadToEnd();
            if (errorMsg.Trim() != string.Empty)
            {
                ps.Close();
                throw new Exception(errorMsg);
            }
            ps.Close();
            return buildMsg;
        }
    }
}
