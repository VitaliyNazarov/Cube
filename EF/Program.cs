using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cube.Model.Model;
using Cube.Model.Model.Contexts;

namespace Cube.Model
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (var context = new CubeDbContext())
                {
                    var groups = context.ProductGroups.ToList();
                    Console.WriteLine(groups.Count);

                    var parent = context.ProductGroups.Add(new ProductGroup
                    {
                        Name = "Test1",
                        Parent = null
                    });

                    context.ProductGroups.Add(new ProductGroup
                    {
                        Name = "Child1",
                        Parent = parent
                    });

                    context.ProductGroups.Add(new ProductGroup
                    {
                        Name = "Child2",
                        Parent = parent
                    });

                    context.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            Console.Read();
        }
    }
}
