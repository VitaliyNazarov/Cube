using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cube.Model;
using Cube.Model.Contexts;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Cube.Import.Excel
{
    internal class GroupLoader : IDisposable
    {
        private readonly CubeDbContext _context;
        private static int Index = 1;

        public GroupLoader()
        {
            _context = new CubeDbContext();    

        }

        public void Load(string path)
        {
            var nodes = GetNodes(path);

            foreach (var root in nodes.Where(x => x.ParentKey == null))
            {
                CreateEntity(root, CreateGroup(root, null), nodes);
            }

            _context.SaveChanges();

            Console.WriteLine(nodes.Count);
        }

        private void CreateEntity(Node parent, ProductGroup productGroup, List<Node> source)
        {
            if (parent.IsProduct)
                return;

            var children = source.Where(x => x.ParentKey == parent.Key);
            foreach (var child in children)
            {
                if (child.IsProduct)
                    CreateProduct(child, productGroup);
                else
                {
                    CreateEntity(child, CreateGroup(child, productGroup), source);
                }
            }
        }

        private ProductGroup CreateGroup(Node node, ProductGroup parent)
        {
            return _context.ProductGroups.Add(new ProductGroup
            {
                Name = node.Name,
                Parent = parent
            });
        }

        private void CreateProduct(Node node, ProductGroup parent)
        {
            _context.Products.Add(new Product
            {
                Name = node.Name,
                Article = $"{node.Key}-{Index++}",
                Category = parent
            });
        }

        private List<Node> GetNodes(string path)
        {
            var nodes = new List<Node>();
            using (var file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var wb = new XSSFWorkbook(file);
                var sheet = wb.GetSheetAt(0);
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    var row = sheet.GetRow(i);
                    if (row == null)
                        continue;

                    var node = new Node
                    {
                        Key = GetValue(row.Cells[0])
                    };
                    for (int j = 1; j < row.Cells.Count; j++)
                    {
                        var name = GetValue(row.Cells[j]);
                        if (string.IsNullOrWhiteSpace(name))
                            continue;

                        node.Name = name;
                    }
                    nodes.Add(node);
                }
            }

            return nodes;
        }

        private string GetValue(ICell cell)
        {
            if (cell.CellType == CellType.Numeric)
            {
                return cell.NumericCellValue.ToString();
                //Console.WriteLine("Row[{0},{1}] = {2}", i, j, cell.NumericCellValue);     
            }
            if (cell.CellType == CellType.String)
            {
                return cell.StringCellValue;
                //Console.WriteLine("Row[{0},{1}] = {2}", i, j, cell.StringCellValue);     
            }

            return null;
        }

        private class Node
        {
            public string Group => Key.Substring(0, 1);

            public string Key { get; set; }

            public string Name { get; set; }

            public int Level => Key.Where(char.IsDigit).Count();

            public bool IsProduct => Key.EndsWith("P");

            public string ParentKey => Level == 1 ? null : Key.Substring(0, Key.Length - 1);
        }

        #region IDisposable

        public void Dispose()
        {
            _context?.Dispose();
        }

        #endregion
    }
}