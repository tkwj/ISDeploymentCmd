using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ISDeploymentCmd
{
    internal class Program
    {
        public static string SourcePath { get; set; }
        public static string DestinationServer { get; set; }
        public static string DestinationPath { get; set; }
        public static bool HelpOrDefaultCallled { get; set; }

        private static void Main(string[] args)
        {
            ParseArguments(args);

            if (HelpOrDefaultCallled)
            {
                HelpText();
            }
            else
            {
                var rc = 1;

                var builder =
                    new SqlConnectionStringBuilder
                    {
                        ["Data Source"] = DestinationServer,
                        ["integrated Security"] = true,
                        ["Initial Catalog"] = "SSISDB"
                    };

                var cp = new CatalogProc(builder.ConnectionString);

                Console.WriteLine("Deploy Started at " + DateTime.Now);
                Console.WriteLine("Source File: {0}", SourcePath);
                Console.WriteLine("Server: {0}", DestinationServer);
                Console.WriteLine("Destination Path: {0}", DestinationPath);

                try
                {
                    rc = cp.DeployProject(GetPartFromPath(DestinationPath, PathPart.FolderName),
                        GetPartFromPath(DestinationPath, PathPart.ProjectName),
                        File.ReadAllBytes(SourcePath));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }

                Console.WriteLine(rc == 0 ? "Deployed successfully" : "Deploy failed");
            }

            if (Debugger.IsAttached)
                Console.ReadKey();
        }

        private static void ParseArguments(string[] arguments)
        {
            if (!arguments.Any())
            {
                HelpOrDefaultCallled = true;
                return;
            }
            var i = 0;
            foreach (var arg in arguments)
            {
                if (arg.StartsWith("/"))
                    if (arg.Contains(":"))
                    {
                        var parsedargument = arg.Substring(1, arg.IndexOf(':') - 1);
                        switch (parsedargument)
                        {
                            case "SourcePath":
                            case "SP":
                                SourcePath = arguments[i].Substring(arguments[i].IndexOf(':') + 1);
                                break;
                            case "DestinationServer":
                            case "DS":
                                DestinationServer = arguments[i].Substring(arguments[i].IndexOf(':') + 1);
                                break;
                            case "DestinationPath":
                            case "DP":
                                DestinationPath = arguments[i].Substring(arguments[i].IndexOf(':') + 1);
                                break;
                            default:
                                HelpOrDefaultCallled = true;
                                return;
                        }
                    }
                    else if (arg == "/Silent")
                    {
                    }
                    else
                    {
                        HelpOrDefaultCallled = true;
                        return;
                    }
                i++;
            }
        }

        private static string GetPartFromPath(string destinationPath, PathPart part)
        {
            if (destinationPath != null)
            {
                var parts = destinationPath.Split('/');
                return parts[(int) part];
            }
            return "";
        }

        private static void HelpText()
        {
            Console.WriteLine(@"Usage:
{0}
[/Silent]
{{/SourcePath:<string> | /SP:<string>}}
{{/DestinationServer:<string> | /DS:<string>}}
{{/DestinationPath:<string> | /DP:<string>}}

Example:
{0} /SourcePath:""C:\Packages\MyProject.ispac"" /DestinationServer:""Server\Instance"" /DestinationPath:""/SSISDB/MyFolder/MyProject""
", AppDomain.CurrentDomain.FriendlyName);
        }

        private enum PathPart
        {
            //DatabaseName = 1,
            FolderName = 2,
            ProjectName = 3
        }
    }
}