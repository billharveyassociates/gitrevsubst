using System;
using System.Diagnostics;
using System.IO;

namespace gitrevsubst
{
    public class Git
    {
        public struct VersionStruct
        {
            public int major;
            public int minor;
            public int revision;
            public int build;
        }

        private string gitDirectory;

        public Git(string gitDirectory)
        {
            this.gitDirectory = gitDirectory;
        }

        /// <summary>
        /// get all the version info from the tag etc, from the git describe output
        /// </summary>
        /// <returns></returns>
        public VersionStruct ParseTag()
        {
            string previousCd = Directory.GetCurrentDirectory();
            VersionStruct version = new VersionStruct();
            try
            {
                Directory.SetCurrentDirectory(Directory.GetParent(gitDirectory).FullName);
                string output = this.GetGitOutput(string.Format(
                    "describe --always --dirty --tags",
                    this.gitDirectory));

                var majorParts = output.Split('.');

                if (majorParts.Length < 3)
                {
                    Debug.WriteLine("Tag not correct, must be a.b.c");
                }

                version.major = int.Parse(majorParts[0]);
                version.minor = int.Parse(majorParts[1]);

                var minorParts = majorParts[2].Split('-');
                version.revision = int.Parse(minorParts[0]);
                version.build = int.Parse(minorParts[1]);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ParseTag failed, you might not have a tag or it isn't in the format a.b.c " + ex.Message);
            }
            finally
            {
                Directory.SetCurrentDirectory(previousCd);
            }

            return version;
        }

        public string GetShortRevId()
        {
            string rev = this.GetGitOutput(string.Format(
                "--git-dir {0} rev-parse --short=7 HEAD",
                this.gitDirectory));
            if (rev.Length != 7)
            {
                Debug.WriteLine("Revision length invalid: [{0}]", rev);
                return null;
            }
            return rev;
        }

        public bool? GetDirtyStatus()
        {
            // --git-dir did not work with git describe, so temporarily change cd
            string previousCd = Directory.GetCurrentDirectory();
            try
            {
                Directory.SetCurrentDirectory(Directory.GetParent(gitDirectory).FullName);
                string output = this.GetGitOutput(string.Format(
                    "describe --always --dirty",
                    this.gitDirectory));

                if (output.EndsWith("-dirty"))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Git describe failed: [{0}]", ex.Message);
            }
            finally
            {
                Directory.SetCurrentDirectory(previousCd);
            }

            return null;
        }

        public string GetLongRevId()
        {
            string rev = this.GetGitOutput(string.Format(
                "--git-dir {0} rev-parse HEAD",
                this.gitDirectory));
            if (rev.Length != 40)
            {
                Debug.WriteLine(string.Format(
                    "Revision length invalid: [{0}]", rev));
                return null;
            }
            return rev;
        }

        public DateTime? GetDate(string rev)
        {
            string output = this.GetGitOutput(string.Format(
                "--git-dir {0} rev-list --format=format:'%ai' --max-count=1 {1}",
                this.gitDirectory, rev));

            string dateStr = null;
            using (var rdr = new StringReader(output))
            {
                while (true)
                {
                    string l = rdr.ReadLine();
                    if (l != null)
                    {
                        if (l.StartsWith("'") && l.EndsWith("'"))
                        {
                            dateStr = l.Substring(1, l.Length - 2);
                        }
                        else
                        {
                            if (!l.StartsWith("commit"))
                            {
                                dateStr = l.Substring(0, l.Length);
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return (dateStr == null) ? null : (DateTime?)DateTime.Parse(dateStr);
        }

        private string GetGitOutput(string arguments)
        {
            try
            {
                Process p = new Process();
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = "git";
                p.StartInfo.Arguments = arguments;
                p.Start();
                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();

                if (p.ExitCode != 0)
                {
                    Debug.WriteLine("Process returned error code " + p.ExitCode);
                    return null;
                }

                output = output.Trim(); // remove line ending

                Console.WriteLine("git command: " + arguments);
                Console.WriteLine("returned: " + output);

                return output;
            }
            catch (Exception e)
            {
                Console.WriteLine("failed to run git " + e.Message);
                return "";
            }
        }
    }
}