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
        private readonly Dictionary<string, PriceGroup> _priceGroups;
        private readonly PriceGroup _defaultPriceGroup;

        public ExchangeManager()
        {
            _context = new CubeDbContext(null, false);
            _defaultPriceList = _context.PriceLists.FirstOrDefault();
            _priceGroups = _context.PriceGroups.ToDictionary(x => x.Name);
            _defaultPriceGroup = _context.PriceGroups.FirstOrDefault(x => x.IsDefault);
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
                var wb = new XSSFWorkbook(file){ MissingCellPolicy = MissingCellPolicy.CREATE_NULL_AS_BLANK };
                for (int i = 0; i < wb.NumberOfSheets; i++)
                {
                    var sheet = wb.GetSheetAt(i);
                    var container = LoadNodes(sheet);
                    ValidateNodes(container);
                    SaveNodes(container);
                }
            }
        }

        /// <summary>
        /// Валидация на корректность сохранения
        /// </summary>
        private void ValidateNodes(NodeContainer container)
        {
            var nodes = container.Nodes;
            var products = nodes.Where(x => x.IsProduct).ToArray();
            var failNameProducts = products.Where(x => string.IsNullOrWhiteSpace(x.Name)).ToArray();
            if (failNameProducts.Any())
            {
                throw new ExchangeManagerException(
                    "Обнаружены продукты без названия.",
                    failNameProducts.Select(x => x.SheetName).Distinct().ToArray(),
                    failNameProducts.Select(x => x.RowNumber).Distinct().ToArray());
            }

            var failArticleProducts = products.Where(x => string.IsNullOrWhiteSpace(x.Article)).ToArray();
            if (failArticleProducts.Any())
            {
                throw new ExchangeManagerException(
                    "Обнаружены продукты без артикула.",
                    failArticleProducts.Select(x => x.SheetName).Distinct().ToArray(),
                    failArticleProducts.Select(x => x.RowNumber).Distinct().ToArray());
            }

            var failPricesProducts = products.Where(x => x.Prices == null || !x.Prices.Any()).ToArray();
            if (failPricesProducts.Any())
            {
                throw new ExchangeManagerException(
                    "Обнаружены продукты без цены.",
                    failPricesProducts.Select(x => x.SheetName).Distinct().ToArray(),
                    failPricesProducts.Select(x => x.RowNumber).Distinct().ToArray());
            }

            var groups = nodes.Where(x => !x.IsProduct && !x.IsRoot).ToArray();
            var failNameGroups = groups.Where(x => string.IsNullOrWhiteSpace(x.Name)).ToArray();
            if (failNameGroups.Any())
            {
                throw new ExchangeManagerException(
                    "Обнаружены группы без названия.",
                    failNameGroups.Select(x => x.SheetName).Distinct().ToArray(),
                    failNameGroups.Select(x => x.RowNumber).Distinct().ToArray());
            }

            var failKeysGroups = groups.Where(x => string.IsNullOrWhiteSpace(x.Key)).ToArray();
            if (failKeysGroups.Any())
            {
                throw new ExchangeManagerException(
                    "Обнаружены группы без указания типа.",
                    failKeysGroups.Select(x => x.SheetName).Distinct().ToArray(),
                    failKeysGroups.Select(x => x.RowNumber).Distinct().ToArray());
            }

        }

        /// <summary>
        /// Сохранение
        /// </summary>
        private void SaveNodes(NodeContainer container)
        {
            // Создать группы цен
            CreatePriceGroup(container);

            var rootGroup = CreateGroup(container.Nodes[0], null);

            foreach (var subRoot in container.Nodes.Skip(1).Where(x => x.ParentKey.Length == 1))
            {
                CreateEntity(subRoot, CreateGroup(subRoot, rootGroup), container.Nodes);
            }

            _context.SaveChanges();
        }

        private void CreatePriceGroup(NodeContainer container)
        {
            if (container.DefaultPriceGroupOnly)
                return;

            foreach (var groupPrice in container.GroupPrices)
            {
                if (_priceGroups.ContainsKey(groupPrice.Value))
                    continue;

                var createdGroup = _context.PriceGroups.Add(new PriceGroup
                {
                    Name = groupPrice.Value
                });

                _priceGroups.Add(createdGroup.Name, createdGroup);
            }

            _context.SaveChanges();
        }

        private void CreateEntity(Node parent, ProductGroup productGroup, List<Node> source)
        {
            if (parent.IsProduct)
                return;

            var children = source.Where(x => x.ParentKey == parent.Key).ToList();
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
            // Существующая группа
            var gr = _context.ProductGroups.FirstOrDefault(x => x.Key == node.Key);

            if (gr == null)
            {
                gr = _context.ProductGroups.Add(new ProductGroup
                {
                    Name = node.Name,
                    Parent = parent,
                    Key = node.Key
                });
            }
            else
            {
                gr.Name = node.Name;
            }

            return gr;
        }

        private void CreateProduct(Node node, ProductGroup parent)
        {
            var newProduct = false;
            var product = _context.Products.FirstOrDefault(x => x.Article == node.Article);
            
            if (product == null)
            {
                newProduct = true;
                product = new Product();
            }

            product.Name = node.Name;
            product.Width = node.Width;
            product.Height = node.Height;
            product.Length = node.Length;
            product.Article = node.Article;
            product.Category = parent;
            product.Key = node.Key;

            if (newProduct)
            {
                product = _context.Products.Add(product);
                _context.SaveChanges();
            }
            
            foreach (var item in node.Prices)
            {
                var priceGroup = node.DefaultPriceGroupOnly ? _defaultPriceGroup : _priceGroups[item.Key];

                var price = _context.Prices.FirstOrDefault(x =>
                    x.PriceList.Id == _defaultPriceList.Id && 
                    x.PriceGroup.Id == priceGroup.Id && 
                    x.Product.Id == product.Id);

                if (price == null)
                {
                    _context.Prices.Add(new Price
                    {
                        Product = product,
                        PriceGroup = priceGroup,
                        Value = item.Value,
                        CreatedDate = DateTime.Now,
                        PriceList = _defaultPriceList
                    });
                }
                else
                {
                    price.Value = item.Value;
                }
            }

            _context.SaveChanges();
        }

        private NodeContainer LoadNodes(ISheet sheet)
        {
            var container = new NodeContainer();
            container.Nodes = new List<Node>(sheet.LastRowNum + 1);

            var headers = sheet.GetRow(0);
            for (int k = 7; k < headers.LastCellNum; k++)
            {
                container.GroupPrices.Add(k, GetStringValue(GetCell(headers, k)));
            }

            var name = sheet.SheetName;
            var rootNode = new Node
            {
                Name = name,
                IsRoot = true,
                Key = $"root{sheet.SheetName}",
                SheetName = name
            };
            container.Nodes.Add(rootNode);
            for (int j = 1; j <= sheet.LastRowNum; j++)
            {
                try
                {
                    var row = sheet.GetRow(j);
                    var node = new Node();
                    node.SheetName = name;
                    node.RowNumber = j;
                    node.Key = GetStringValue(GetCell(row, 0));
                    node.Article = GetStringValue(GetCell(row, 1));
                    node.Name = GetStringValue(GetCell(row, 2));
                    node.Height = GetIntValue(GetCell(row, 3), 0);
                    node.Length = GetIntValue(GetCell(row, 4), 0);
                    node.Width = GetIntValue(GetCell(row, 5), 0);
                    for (int k = 7; k < row.LastCellNum; k++)
                    {
                        var price = GetDoubleValue(GetCell(row, k));
                        if (!price.HasValue || price.Value == 0.0)
                            continue;

                        node.Prices.Add(container.GroupPrices[k], price.Value);
                    }
                    container.Nodes.Add(node);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return container;
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            _context?.Dispose();
        }

        #endregion

        private ICell GetCell(IRow row, int index)
        {
            return row.GetCell(index, MissingCellPolicy.RETURN_NULL_AND_BLANK);
        }

        private string GetStringValue(ICell cell)
        {
            if (cell == null)
                return null;

            if (cell.CellType == CellType.Numeric)
            {
                return cell.NumericCellValue.ToString();
            }
            if (cell.CellType == CellType.String)
            {
                return cell.StringCellValue;
            }

            return null;
        }

        private double? GetDoubleValue(ICell cell)
        {
            if (cell == null)
                return null;

            if (cell.CellType == CellType.Numeric)
            {
                return cell.NumericCellValue;
            }
            if (cell.CellType == CellType.String)
            {
                var str = cell.StringCellValue;
                if (string.IsNullOrWhiteSpace(str))
                    return null;

                return double.TryParse(str, out var val) ? (double?)val : null;
            }

            return null;
        }

        private int GetIntValue(ICell cell, int defaultValue)
        {
            if (cell == null)
                return defaultValue;

            if (cell.CellType == CellType.Numeric)
            {
                return (int)cell.NumericCellValue;
            }
            if (cell.CellType == CellType.String)
            {
                var str = cell.StringCellValue;
                if (string.IsNullOrWhiteSpace(str))
                    return defaultValue;

                return int.TryParse(str, out var val) ? val : defaultValue;
            }

            return defaultValue;
        }

        private class NodeContainer
        {
            public NodeContainer()
            {
                Nodes = new List<Node>();
                GroupPrices = new Dictionary<int, string>();
            }

            public List<Node> Nodes { get; set; }

            public Dictionary<int, string> GroupPrices { get; set; }

            public bool DefaultPriceGroupOnly => GroupPrices.Count == 1;
        }

        private class Node
        {
            public Node()
            {
                Prices = new Dictionary<string, double>();
            }

            public string SheetName { get; set; }

            public int RowNumber { get; set; }

            public bool IsRoot { get; set; }

            public string Key { get; set; }

            public string Article { get; set; }

            public string Name { get; set; }

            public bool IsProduct => Key.EndsWith("P");

            public string ParentKey => Key.Substring(0, Key.Length - 1);

            public int Height { get; set; }

            public int Width { get; set; }

            public int Length { get; set; }

            public Dictionary<string, double> Prices { get; set; }

            public bool DefaultPriceGroupOnly => Prices.Count == 1;
        }
    }

    public class ExchangeManagerException : Exception
    {
        public ExchangeManagerException(string message, string[] sheetNames, int[] rowNumbers)
            : base(message)
        {
            SheetNames = sheetNames;
            RowNumbers = rowNumbers;
        }

        public string[] SheetNames { get; }

        public int[] RowNumbers { get; }
    }
}
