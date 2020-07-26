using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Cube.Import.Excel
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = GenerateWixComponents(@"c:\!!!Work\!!!Cube\src\Cube\Output\Release\", new []{".xml", ".log", ".pdb"});
            try
            {
                if (args.Length < 3)
                {
                    Console.WriteLine($"Format: {typeof(Program).Assembly.GetName().Name} -command -path -clearAllData");
                    return;
                }

                var command = args[0];
                var path = args[1];
                var clearAllData = bool.TryParse(args[2], out var c) && c;

                switch (command)
                {
                    case "group":
                        using (var loader = new GroupLoader(clearAllData))
                        {
                            loader.Load(path);
                        }
                        break;
                    case "kitchen":
                        var kitchenLoader = new KitchenProductLoader(clearAllData);
                        kitchenLoader.Load(path);
                        break;
                }

                Console.WriteLine("Success");
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error");
                Console.WriteLine(exception);
            }

            Console.Read();
        }

        private static Tuple<string, string> GenerateWixComponents(string folder, string[] excludeFileExtensions)
        {
            var store = new Tuple<StringBuilder, StringBuilder>(new StringBuilder(1000), new StringBuilder(1000));
            excludeFileExtensions = excludeFileExtensions ?? new string[0];
            GenerateWixComponentsInternal(folder, excludeFileExtensions, null, store);
            return new Tuple<string, string>(store.Item1.ToString(), store.Item2.ToString());
        }

        private static void GenerateWixComponentsInternal(
            string folder,
            string[] excludeFileExtensions,
            string subFolderName = null, 
            Tuple<StringBuilder, StringBuilder> store = null)
        {
            var componentTemplate = "<Component Id='{0}' Guid='{1}'><File Id='{0}' Source='$(var.bin.path){2}' /></Component>";
            var componentRefTemplate = "<ComponentRef Id='{0}'/>";

            store = store ?? new Tuple<StringBuilder, StringBuilder>(new StringBuilder(1000), new StringBuilder(1000));
            var isMainFolder = string.IsNullOrWhiteSpace(subFolderName);
            var folderPath = isMainFolder
                ? folder
                : Path.Combine(folder, subFolderName);

            var files = Directory.EnumerateFiles(folderPath).Where(x => !excludeFileExtensions.Contains(Path.GetExtension(x)));
            foreach (var file in files)
            {
                var fileId = Path.GetFileName(file);
                var filePath = isMainFolder ? fileId : $"{subFolderName}\\{fileId}";

                store.Item1.AppendLine(string.Format(componentTemplate, fileId, "{" + Guid.NewGuid().ToString().ToUpper() + "}", filePath));
                store.Item2.AppendLine(string.Format(componentRefTemplate, fileId));
            }

            var subDirs = Directory.EnumerateDirectories(folderPath);
            foreach (var dir in subDirs)
            {
                var subDirName = dir.Replace(folder, string.Empty);

                store.Item1.AppendLine($"<Directory Id='{subDirName}' Name='{subDirName}'>");
                GenerateWixComponentsInternal(folder, excludeFileExtensions, subDirName, store);
                store.Item1.AppendLine("</Directory>");
            }
        }
    }
}
