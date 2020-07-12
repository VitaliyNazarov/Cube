using System;

namespace Cube.Import.Excel
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 2)
                {
                    Console.WriteLine($"Format: {typeof(Program).Assembly.GetName().Name} -command -path");
                    return;
                }

                var command = args[0];
                var path = args[1];

                switch (command)
                {
                    case "group":
                        using (var loader = new GroupLoader())
                        {
                            loader.Load(path);
                        }
                        break;
                    case "kitchen":
                        var kitchenLoader = new KitchenProductLoader();
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
