using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cube.Model;
using Cube.Model.Contexts;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Cube.Module.Win.Exchange
{
    internal class ExchangeManager : IDisposable
    {
        /* 0    1       2        3                   4      5       6       7           8
         * ТИП	Артикул	Название Группы или Продукта Высота	Глубина	Ширина	Особенности	Цена ->
         */
        private readonly CubeDbContext _context;
        private readonly PriceList _defaultPriceList;

        public ExchangeManager()
        {
            _context = new CubeDbContext(null, false);
            _defaultPriceList = _context.PriceLists.FirstOrDefault();
        }

        #region Export

        public void Export(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                var wb = new XSSFWorkbook();

                // Рутовые группы - это листы
                foreach (var root in _context.ProductGroups.Where(x => x.Parent == null))
                {
                    var sheet = wb.CreateSheet(root.Name);
                    ExportGroups(sheet, root.Children);
                    ExportProducts(sheet, root.Products);
                }

                wb.Write(stream);
            }
        }

        private void ExportGroups(ISheet sheet, IList<ProductGroup> groups)
        {
            if (groups == null || !groups.Any())
                return;

            foreach (var productGroup in groups)
            {
                ExportGroup(sheet, productGroup);
                ExportGroups(sheet, productGroup.Children);
                ExportProducts(sheet, productGroup.Products);
            }
        }

        private void ExportProducts(ISheet sheet, IList<Product> products)
        {
            foreach (var product in products)
            {
                ExportProduct(sheet, product);
            }
        }

        private void ExportGroup(ISheet sheet, ProductGroup productGroup)
        {
            var row = sheet.CreateRow(sheet.LastRowNum + 1);
            row.CreateCell(0, CellType.String).SetCellValue(productGroup.Key); // Тип
            row.CreateCell(1, CellType.Blank); // Артикул
            row.CreateCell(2, CellType.String).SetCellValue(productGroup.Name); // Название
            row.CreateCell(3, CellType.Numeric).SetCellValue(0); // Высота
            row.CreateCell(4, CellType.Numeric).SetCellValue(0); // Глубина
            row.CreateCell(5, CellType.Numeric).SetCellValue(0); // Ширина
            row.CreateCell(6, CellType.String).SetCellValue(string.Empty); // Особенности
            row.CreateCell(7, CellType.Numeric).SetCellValue(0); // Цена
        }

        private void ExportProduct(ISheet sheet, Product product)
        {
            var row = sheet.CreateRow(sheet.LastRowNum + 1);
            row.CreateCell(0, CellType.String).SetCellValue(product.Key); // Тип
            row.CreateCell(1, CellType.String).SetCellValue(product.Article); // Артикул
            row.CreateCell(2, CellType.String).SetCellValue(product.Name); // Название
            row.CreateCell(3, CellType.Numeric).SetCellValue(product.Height); // Высота
            row.CreateCell(4, CellType.Numeric).SetCellValue(product.Length); // Глубина
            row.CreateCell(5, CellType.Numeric).SetCellValue(product.Width); // Ширина
            row.CreateCell(6, CellType.String).SetCellValue(string.Empty); // Особенности
            row.CreateCell(7, CellType.Numeric).SetCellValue(0); // Цена
        }

        #endregion

        #region Import

        public void Import(string filePath)
        {
            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var wb = new XSSFWorkbook(file);
                for (int i = 0; i < wb.NumberOfSheets; i++)
                {
                    var sheet = wb.GetSheetAt(i);
                    var nodes = LoadNodes(sheet);
                    SaveNodes(nodes);
                }
            }
        }

        private void SaveNodes(List<Node> nodes)
        {
            var rootGroup = CreateGroup(nodes[0], null);

            foreach (var subRoot in nodes.Skip(1).Where(x => x.ParentKey == null))
            {
                CreateEntity(subRoot, CreateGroup(subRoot, rootGroup), nodes);
            }

            _context.SaveChanges();
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
                Parent = parent,
                Key = node.Key
            });
        }

        private void CreateProduct(Node node, ProductGroup parent)
        {
            var product = _context.Products.Add(new Product
            {
                Name = node.Name,
                Width = node.Width,
                Height = node.Height,
                Length = node.Length,
                Article = node.Article,
                Category = parent,
                Key = node.Key
            });

            if (node.Prices.Any())
            {
                _context.Prices.Add(new Price
                {
                    PriceList = _defaultPriceList,
                    Product = product,
                    Value = node.Prices[0]
                });
            }
        }

        private List<Node> LoadNodes(ISheet sheet)
        {
            var nodes = new List<Node>(sheet.LastRowNum + 1);
            var name = sheet.SheetName;
            var rootNode = new Node
            {
                Name = name,
                IsRoot = true,
                Key = "root"
            };
            nodes.Add(rootNode);
            for (int j = 1; j <= sheet.LastRowNum; j++)
            {
                var row = sheet.GetRow(j);
                var node = new Node();
                node.Key = GetValue(row.Cells[0]);
                node.Article = GetValue(row.Cells[1]);
                node.Name = GetValue(row.Cells[2]);
                node.Height = (int)row.Cells[3].NumericCellValue;
                node.Length = (int)row.Cells[4].NumericCellValue;
                node.Width = (int)row.Cells[5].NumericCellValue;
                for (int k = 7; k < row.LastCellNum; k++)
                {
                    var cellType = row.Cells[k].CellType;
                    var price = 0.0;
                    switch (cellType)
                    {
                        case CellType.Blank:
                            break;
                        case CellType.Numeric:
                            price = row.Cells[k].NumericCellValue;
                            break;
                        case CellType.String:
                            var str = row.Cells[k].StringCellValue;
                            price = string.IsNullOrWhiteSpace(str) ? 0.0 : double.Parse(str);
                            break;
                    }
                    node.Prices.Add(price);
                }
                nodes.Add(node);
            }

            return nodes;
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            _context?.Dispose();
        }

        #endregion

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
            public Node()
            {
                Prices = new List<double>();
            }

            public bool IsRoot { get; set; }

            public string Group => Key.Substring(0, 1);

            public string Key { get; set; }

            public string Article { get; set; }

            public string Name { get; set; }

            public int Level => Key.Where(char.IsDigit).Count();

            public bool IsProduct => Key.EndsWith("P");

            public string ParentKey => Level == 1 ? null : Key.Substring(0, Key.Length - 1);

            public int Height { get; set; }

            public int Width { get; set; }

            public int Length { get; set; }

            public List<double> Prices { get; set; }
        }
    }
}
