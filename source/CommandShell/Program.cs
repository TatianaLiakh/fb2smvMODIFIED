﻿using System;
using System.IO;
using FB2SMV.Core;

namespace CommandShell
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Args Length: " + args.Length);
            if (args.Length == 0)
            {
                args = new string[] { @"C:\testfb2\TwoCylDiscrMX.fbt" };
            }

            Console.WriteLine("Input file: " + args[0]);

            var filename = args[0];
            var settings = new Settings();
            var parser = new FBClassParcer(Console.WriteLine, settings);
            parser.ParseRecursive(filename, Console.WriteLine);

            var executionModels = ExecutionModelsList.Generate(parser, true);
            var translator = new SmvCodeGenerator(parser.Storage, executionModels, settings, Console.WriteLine);

            var outFileName = Path.Combine(Path.GetDirectoryName(filename),
                Path.GetFileNameWithoutExtension(filename) + ".smv");
            var wr = new StreamWriter(outFileName);
            foreach (var fbSmv in translator.TranslateAll())
            {
                wr.Write(fbSmv + "\n");
            }

            wr.Close();
        }
    }
}