using System;

namespace Cube.Import.Excel
{
    class Program
    {
        static void Main(string[] args)
        {
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
    }
}
