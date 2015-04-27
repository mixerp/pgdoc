/********************************************************************************
Copyright (C) Binod Nepal, Mix Open Foundation (http://mixof.org).

This file is part of MixERP.

MixERP is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

MixERP is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MixERP.  If not, see <http://www.gnu.org/licenses/>.
***********************************************************************************/
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MixERP.Net.Utilities.PgDoc.Generators;
using MixERP.Net.Utilities.PgDoc.Helpers;
using MixERP.Net.Utilities.PgDoc.Parsers;

namespace MixERP.Net.Utilities.PgDoc
{
    internal class Program
    {
        internal static readonly string AppName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

        internal static string Database;
		internal static int Port;
		internal static string UserId;
		internal static string Password;
		internal static string Server;

        internal static string DisqusName;
        internal static bool DeleteFilesBeforePublish;
        internal static string OutputDirectory;
		internal static string SchemaPattern = ".*"; // Regex-Pattern for schemas to include
		internal static string xSchemaPattern = string.Empty; // Regex-Pattern for schemas to exclude
		
        internal static void Build(string[] args)
        {
            foreach (string argument in args)
            {
                if (argument.StartsWith("-s"))
                {
                    Server = ArgumentParser.Parse(argument, "-s");
					ExtractPort();
                }

                if (argument.StartsWith("-d"))
                {
                    Database = ArgumentParser.Parse(argument, "-d");
                }

                if (argument.StartsWith("-u"))
                {
                    UserId = ArgumentParser.Parse(argument, "-u");
                }

                if (argument.StartsWith("-p"))
                {
                    Password = ArgumentParser.Parse(argument, "-p");
                }

                if (argument.StartsWith("-o"))
                {
                    OutputDirectory = ArgumentParser.Parse(argument, "-o");
                }

                if (argument.StartsWith("-q"))
                {
                    DisqusName = ArgumentParser.Parse(argument, "-q");
                }

				// kwrl: is == include schemata; format: "-is=RegExpPattern" sample: "-is=(public|def.*)
				if (argument.StartsWith("-is")) {
					SchemaPattern = ArgumentParser.Parse(argument, "-is");
				}
				if (argument.StartsWith("-xs")) {
					xSchemaPattern = ArgumentParser.Parse(argument, "-xs");
				}

                if (!argument.StartsWith("-f")) continue;

                string value = ArgumentParser.Parse(argument, "-f");

                if (!string.IsNullOrWhiteSpace(value))
                {
                    Console.WriteLine("ERROR: Invalid value {0} for parameter \"-f\".", value);
                    return;
                }

                DeleteFilesBeforePublish = true;
            }


            if (string.IsNullOrWhiteSpace(Server))
            {
                Console.WriteLine("Invalid or missing parameter \"-s\" for PostgreSQL Server.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Database))
            {
                Console.WriteLine("Invalid or missing parameter \"-d\" for PostgreSQL Database.");
                return;
            }

            if (string.IsNullOrWhiteSpace(UserId))
            {
                Console.WriteLine("Invalid or missing parameter \"-u\" for PostgreSQL User.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                Console.WriteLine("Invalid or missing parameter \"-p\" for PostgreSQL User Password.");
                return;
            }

            if (string.IsNullOrWhiteSpace(OutputDirectory))
            {
                Console.WriteLine("Invalid or missing parameter \"-o\" for documentation output directory.");
                return;
            }


            Run();
        }

        internal static string ReadPassword()
        {
            string password = "";

            ConsoleKeyInfo info = Console.ReadKey(true);

            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    Console.Write("*");

                    password += info.KeyChar;
                }

                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        // remove one character from the list of password characters
                        password = password.Substring(0, password.Length - 1);

                        // get the location of the cursor
                        int pos = Console.CursorLeft;

                        // move the cursor to the left by one character
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);

                        // replace it with space
                        Console.Write(" ");

                        // move the cursor to the left by one character again
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }
                }

                info = Console.ReadKey(true);
            }


            // add a new line because user pressed enter at the end of their password
            Console.WriteLine();

            return password;
        }

        private static bool WriteQuestion(string question)
        {
            Console.Write(question);

            string result = Console.ReadLine();

            return result == null || new string[]{"Y", "YES", "OK", "OKAY"}.Contains(result.ToUpperInvariant());
        }

		private static void ExtractPort() {
			// if there's a colon, I will extract the port
			var ServerParts = Server.Split(':');
			Server = ServerParts[0];
			if (ServerParts.Length == 1 || !int.TryParse(ServerParts[1], out Port)) {
				Port = 5432;
			}
		}

        private static void DisplayHelpInfo()
        {
            Console.WriteLine("Generates HTML documentation from PostgreSQL database.");
            Console.WriteLine();
            Console.WriteLine("Usage: {0} -s=[server[:port]] -d=[database] -u=[pg_user] -p=[pwd] -o=[output_dir]", AppName);
            Console.WriteLine();

            Console.WriteLine("WARNING: No parameter supplied.");
            Console.WriteLine();

            if (!WriteQuestion("Would you like to provide parameters now? [Yes/No*]"))
            {
                Console.WriteLine();
                Console.WriteLine("No");
                Console.WriteLine();
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Yes");
            Server = GetParameter("PostgreSQL Server host name or IP address:", "localhost");
			ExtractPort();

            Database = GetParameter("Enter the name of your PostgreSQL Database:", "mixerp");
            UserId = GetParameter("Enter PostgreSQL Database UserId:", "postgres");
            Password = GetPassword();
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\" +
                          Database + "-docs";
            OutputDirectory = GetParameter("Output directory to generate documentation to:", path);

            Run();
        }

        private static string GetParameter(string message, string defaultValue)
        {
            Console.WriteLine();
            Console.Write(message);

            if (!string.IsNullOrWhiteSpace(defaultValue))
            {
                SendKeys.SendWait(defaultValue);
            }

            string parameter = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(parameter))
            {
                GetParameter(message, defaultValue);
            }

            return parameter;
        }

        private static string GetPassword()
        {
            Console.WriteLine();
            Console.Write("Enter password for user \"{0}\":", UserId);
            string password = ReadPassword();

            if (string.IsNullOrWhiteSpace(password))
            {
                GetPassword();
            }

            return password;
        }

        [STAThread]
        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += DependencyHandler.ResolveEventHandler;
			//AppName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

            if ((args.Length.Equals(0)) ||
                args.Length.Equals(1) && (args[0].Contains("/?") || args[0].Contains("--help")))
            {
                DisplayHelpInfo();
                Console.ReadKey();
                return;
            }

            Build(args);
            Console.WriteLine();
            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();
        }

        private static void Run()
        {
            if (!FileHelper.IsOutputDirectoryEmpty())
            {
                if (!DeleteFilesBeforePublish)
                {
                    Console.WriteLine("WARNING: The output directory is not empty.");
                    bool result = WriteQuestion("Do you want empty this directory?[Yes/No*]");

                    if (!result)
                    {
                        Console.WriteLine("ERROR: Cannot create documentation.");
                        return;
                    }
                }

                FileHelper.EmptyOutputDirectory();
            }

            Console.WriteLine("{0} initialized.", AppName);

            try
            {
                Runner.Run();
                Console.WriteLine();
                Console.WriteLine("{0} completed.", AppName);
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("ERROR: Cannot create documentation.");
                Console.WriteLine();
                Console.Write(ex.Message);
                Console.WriteLine();
            }
        }
    }
}