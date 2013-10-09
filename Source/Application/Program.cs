namespace IntelligentInclude
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Library;

    public sealed class Program
    {
        private static int Main(string[] args)
        {
            try
            {
                printUsageShort();

                if (checkPrintArgs(args))
                {
                    Console.WriteLine("Working directory is '{0}'.", Environment.CurrentDirectory);

                    var rawPath = args[0].Trim('"', '\'', ' ', '\t');
                    var pathInformation = PathInformationController.CreatePathInformation(rawPath, Console.WriteLine);
                    if (pathInformation != null)
                    {
                        PathInformationController.Process(pathInformation, args.Length > 1 && isRecursive(args[1]), Console.WriteLine);
                        return 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    printUsageLong();
                    return -1;
                }
            }
            catch (Exception x)
            {
                Console.Error.WriteLine("[ERROR] Error during processing: {0}", x);
                return -1;
            }
        }

        private static bool checkPrintArgs(IList<string> args)
        {
            if (args.Count <= 0)
            {
                Console.Error.WriteLine("[ERROR] No parameters specified.");
                return false;
            }
            else if (args.Count > 2)
            {
                Console.Error.WriteLine("[ERROR] To many parameters specified (Max. 2, found {0}).", args.Count);
                return false;
            }
            else
            {
                Console.WriteLine("Found parameter 1 as '{0}'.", args[0]);

                if (args.Count > 1)
                {
                    Console.WriteLine("Found parameter 2 as '{0}'.", args[1]);
                }

                return true;
            }
        }

        private static bool isRecursive(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }
            else
            {
                s = s.ToLowerInvariant().Trim();
                return s == @"/s" || s == @"-s" || s == @"--s" || s == @"/r" || s == @"-r" || s == @"--r";
            }
        }

        private static void printUsageShort()
        {
            Console.WriteLine("Zeta Intelligent Include, version {0}.", getAssemblyVersion());
        }

        private static void printUsageLong()
        {
            Console.WriteLine("Zeta Intelligent Include.");
            Console.WriteLine("");
            Console.WriteLine("Mimics the #bbinclude syntax for the Persistent Includes feature of");
            Console.WriteLine("the BBEdit editor tool to inline-include files into other files while");
            Console.WriteLine("preserving the include directive itself after the include.");
            Console.WriteLine("");
            Console.WriteLine("Supports any file that is acceptable to contain include directives, ");
            Console.WriteLine("usually inside comments in the language of the document type.");
            Console.WriteLine("");
            Console.WriteLine("Version {0}.", getAssemblyVersion());
            Console.WriteLine("Copyright 2013 www.zeta-producer.com.");
            Console.WriteLine("");

            // --

            Console.WriteLine("Usage:");
            Console.WriteLine("");
            Console.WriteLine("    ii.exe <path to folder or file or wildcard> [/r]");
            Console.WriteLine("");
            Console.WriteLine("        First parameter is any path to a folder or a folder with wildcards.");
            Console.WriteLine("        Second, optional parameter to specify whether to recurse folders.");
            Console.WriteLine("");

            // --

            Console.WriteLine("Examples:");
            Console.WriteLine("");
            Console.WriteLine("    ii \"C:\\MyFolder\\*.css\" /r");
            Console.WriteLine("");
            Console.WriteLine("        Process all CSS files in C:\\MyFolder and all subfolders.");
            Console.WriteLine("");
            Console.WriteLine("    ii \"C:\\MyFolder\\MyFile.html\"");
            Console.WriteLine("");
            Console.WriteLine("        Processes the single file MyFile.html.");
            Console.WriteLine("");

            // --

            Console.WriteLine("Description:");
            Console.WriteLine("");
            Console.WriteLine("    The program parses input text files for include directives");
            Console.WriteLine("    and inline-replaces the lines between the starting include");
            Console.WriteLine("    directive and the ending include directive with the file that");
            Console.WriteLine("    is specified to be included.");
            Console.WriteLine("");
            Console.WriteLine("    An include start directive has the syntax:");
            Console.WriteLine("");
            Console.WriteLine("        #zetainclude \"..\\myfolder\\filetoinclude.txt\"");
            Console.WriteLine("");
            Console.WriteLine("    An include end directive has the syntax:");
            Console.WriteLine("");
            Console.WriteLine("        #endzetainclude");
            Console.WriteLine("");
            Console.WriteLine("    The program assumes that both directives are writen in separate");
            Console.WriteLine("    lines in the input file. Therefore you cannot write both in the");
            Console.WriteLine("    same line.");
            Console.WriteLine("");
            Console.WriteLine("    Usually you would write the include directives inside comments,");
            Console.WriteLine("    so that the include would not break the meaning of the file itself.");
            Console.WriteLine("");
            Console.WriteLine("    Since the program makes no assumption regarding comments that");
            Console.WriteLine("    surround the include directives, you can use it within any file");
            Console.WriteLine("    that supports comments like e.g. CSS, HTML.");
            Console.WriteLine("");
            Console.WriteLine("    Real-world example (inside an HTML file):");
            Console.WriteLine("");
            Console.WriteLine("        <!-- #zetainclude \"..\\myfolder\\filetoinclude.html\" -->");
            Console.WriteLine("        <!-- #endzetainclude -->");
            Console.WriteLine("");
            Console.WriteLine("    Other real-world example (inside a CSS file):");
            Console.WriteLine("");
            Console.WriteLine("        /* #zetainclude \"..\\myfolder\\filetoinclude.css\" */");
            Console.WriteLine("        /* #endzetainclude */");
            Console.WriteLine("");

            // --

            Console.WriteLine("Remarks:");
            Console.WriteLine("");
            Console.WriteLine("    - Currently the program works best with input and include files");
            Console.WriteLine("      both are being UTF-8 encoded.");
            Console.WriteLine("");
            Console.WriteLine("    - Recursive includes are supported. I.e. an included file may include");
            Console.WriteLine("      other files, too.");
            Console.WriteLine("");
            Console.WriteLine("    - You may have multiple include directives inside a single file.");
            Console.WriteLine("");
            Console.WriteLine("    - Currently we do only support a limited subset of the syntax the the");
            Console.WriteLine("      BBEdit editor supports. E.g. we do not support including files with");
            Console.WriteLine("      variables. See pine.barebones.com/manual/BBEdit_10_User_Manual.pdf,");
            Console.WriteLine("      appendix C for a full documentation of the BBEdit program.");
            Console.WriteLine("");

            // --

            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
        }

        private static Version getAssemblyVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version;
        }
    }
}