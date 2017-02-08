using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GlmSharpGenerator.Types;

namespace GlmSharpGenerator
{
    class Program
    {
        public static string Namespace = "Atma";

        private static string FindDirInParents(string path, string dir)
        {
            try
            {
                if (!Directory.Exists(Path.Combine(path, dir)))
                    return FindDirInParents(Path.Combine(path, "..\\"), dir);

                return Path.Combine(path, dir);
            }catch { }
            return null;
        }

        private static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            if (args.Length != 1)
            {
                Console.WriteLine("Usage: path/to/sln-folder/ path/to/gen-folder/");
                //Console.Read();
                return;
            }

            var basePath = FindDirInParents(Environment.CurrentDirectory, "src");
            if (string.IsNullOrEmpty(basePath))
            {
                Console.WriteLine("Dir src does not exist. Maybe wrong path?");
                return;
            }

            Console.WriteLine("GlmSharp Generator");
            foreach (var version in new[] { 45 })
            {
                string path;
                var testpath = "";
                switch (version)
                {
                    case 45:
                        path = Path.Combine(basePath, "Atma\\Math\\GlmSharp");
                        //testpath = Path.Combine(basePath, "GlmSharpTest");
                        break;
                    case 20:
                        path = Path.Combine(basePath, "GlmSharpCompat");
                        break;
                    default:
                        throw new InvalidOperationException();
                }

                AbstractType.Version = version;
                AbstractType.InitTypes();

                // see: https://www.opengl.org/sdk/docs/man4/html/ for functions

                foreach (var type in AbstractType.Types.Values)
                {
                    //Console.WriteLine("Processing " + type.NameTha);
                    // generate lib code
                    {
                        var filename = type.PathOf(path);
                        new FileInfo(filename).Directory?.Create();
                        if (type.CSharpFile.WriteToFileIfChanged(filename))
                            Console.WriteLine("    CHANGED " + filename);
                    }
                    {
                        var filename = type.GlmPathOf(path);
                        new FileInfo(filename).Directory?.Create();
                        if (type.GlmSharpFile.WriteToFileIfChanged(filename))
                            Console.WriteLine("    CHANGED " + filename);
                    }

                    // generate test code
                    if (!string.IsNullOrEmpty(testpath))
                    {
                        var filename = type.TestPathOf(Path.Combine(testpath, "Generated"));
                        new FileInfo(filename).Directory?.Create();
                        if (type.TestFile.WriteToFileIfChanged(filename))
                            Console.WriteLine("    CHANGED " + filename);
                    }

                }
            }
        }
    }
}
