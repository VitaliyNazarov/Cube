using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cube.Model;
using Cube.Model.Contexts;
using Cube.Model.Enums;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Cube.Import.Excel
{
    internal class KitchenProductLoader : IDisposable
    {
        private readonly CubeDbContext _context;
        private readonly PriceList _defaultPriceList;
        private static int Index = 1;

        public KitchenProductLoader(bool clearData)
        {
            _context = new CubeDbContext(null, clearData);
            _defaultPriceList = _context.PriceLists.FirstOrDefault();
        }

        public void Load(string path)
        {
            var nodes = GetNodes(path);
            var kitchenGroup = _context.ProductGroups.Add(new ProductGroup { Name = "Кухня" });
            var nameGroups = nodes.GroupBy(x => x.Name).ToList();
            foreach (var nameGroup in nameGroups)
            {
                // Группа с именем продукта
                var entityNameGroup = _context.ProductGroups.Add(new ProductGroup {Name = nameGroup.Key, Parent = kitchenGroup});

                // Группа высоты
                var heigthGroups = nameGroup.GroupBy(x => x.H).ToList();
                foreach (var heigthGroup in heigthGroups.OrderBy(x => x.Key))
                {
                    var entityHeigthGroup = _context.ProductGroups.Add(new ProductGroup {Name = $"Высота {heigthGroup.Key}", Parent = entityNameGroup});

                    // Группа ширины
                    var lengthGroups = heigthGroup.GroupBy(x => x.W).ToList();
                    foreach (var lengthGroup in lengthGroups)
                    {
                        var entityLengthGroup = _context.ProductGroups.Add(new ProductGroup {Name = $"Ширина {lengthGroup.Key}", Parent = entityHeigthGroup});
                        foreach (var node in lengthGroup)
                        {
                            var prices = node.Prices.Where(x => x.Value > 0).ToList();
                            foreach (var price in prices)
                            {
                                var product = _context.Products.Add(new Product
                                {
                                    Name = node.Name,
                                    Article = node.ResolveArticle(),
                                    Category = entityLengthGroup,
                                    Facade = price.Key,
                                    Height = heigthGroup.Key,
                                    Length = lengthGroup.Key,
                                    Width = node.W,
                                    Unit = ProductUnit.pce
                                });

                                _context.Prices.Add(new Price
                                {
                                    PriceList = _defaultPriceList,
                                    Product = product,
                                    Value = price.Value
                                });
                            }
                        }
                    }
                }

                _context.SaveChanges();
            }

            _context.SaveChanges();

            Console.WriteLine(nodes.Count);
        }

        //private void CreateEntity(Node parent, ProductGroup productGroup, List<Node> source)
        //{
        //    if (parent.IsProduct)
        //        return;

        //    var children = source.Where(x => x.ParentKey == parent.Key);
        //    foreach (var child in children)
        //    {
        //        if (child.IsProduct)
        //            CreateProduct(child, productGroup);
        //        else
        //        {
        //            CreateEntity(child, CreateGroup(child, productGroup), source);
        //        }
        //    }
        //}

        private ProductGroup CreateGroup(Node node, ProductGroup parent)
        {
            return _context.ProductGroups.Add(new ProductGroup
            {
                Name = node.Name,
                Parent = parent
            });
        }

        //private void CreateProduct(Node node, ProductGroup parent)
        //{
        //    var product = _context.Products.Add(new Product
        //    {
        //        Name = node.Name,
        //        Article = $"{node.Key}-{Index++}",
        //        Category = parent
        //    });

        //    _context.Prices.Add(new Price
        //    {
        //        PriceList = _defaultPriceList,
        //        Product = product,
        //        Value = 1.0
        //    });
        //}

        private List<Node> GetNodes(string path)
        {
            var nodes = new List<Node>();
            using (var file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var wb = new XSSFWorkbook(file);
                var sheet = wb.GetSheetAt(0);

                int num = 0;
                Node mainNode = null;
                for (int i = 2; i <= sheet.LastRowNum; i++)
                {
                    Node node = null;
                    var row = sheet.GetRow(i);
                    if (row == null)
                        continue;

                    try
                    {
                        if (GetValue(row.Cells[0]) == (num + 1).ToString())
                        {
                            mainNode = ReadMainNode(row);
                            node = mainNode;
                            num++;
                        }
                        else
                        {
                            node = ReadSubNode(row, ref mainNode);
                        }
                        nodes.Add(node);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(i);
                        throw;
                    }
                }
            }

            return nodes;
        }

        private string GetValue(ICell cell)
        {
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

        private Node ReadMainNode(IRow row)
        {
            var node = new Node();
            node.Article = row.Cells[1].StringCellValue;
            node.Name = row.Cells[2].StringCellValue;
            node.H = (int)row.Cells[3].NumericCellValue;
            node.L = (int)row.Cells[5].NumericCellValue;
            node.W = (int)row.Cells[7].NumericCellValue;
            ReadPrices(node, row, 8);
            return node;
        }

        private Node ReadSubNode(IRow row, ref Node mainNode)
        {
            var s = GetValue(row.Cells[0]) == "х";

            var node = new Node();
            node.Name = mainNode.Name;
            node.Article = mainNode.Article;
            node.H = s ? mainNode.H : (int)row.Cells[0].NumericCellValue;
            node.L = s ? mainNode.L : (int)row.Cells[2].NumericCellValue;
            node.W = s ? (int)row.Cells[1].NumericCellValue: (int)row.Cells[4].NumericCellValue;
            ReadPrices(node, row, s ? 2 : 5);

            if (!s)
                mainNode = node;

            return node;
        }

        private void ReadPrices(Node node, IRow row, int pos)
        {
            int p = pos;
            var keys = node.Prices.Keys.ToList();
            foreach (var key in keys)
            {
                var cell = row.Cells[p++];
                var v = GetValue(cell);
                if (string.IsNullOrWhiteSpace(v) || v.Contains("-"))
                {
                    node.Prices[key] = -1;
                }
                else
                {
                    node.Prices[key] = cell.NumericCellValue;    
                }
            }
        }

        private class Node
        {
            public Node()
            {
                Prices = new Dictionary<FacadeType, double>
                {
                    {FacadeType.Ldsp, 0},
                    {FacadeType.LdspAl, 0},
                    {FacadeType.Pvh, 0},
                    {FacadeType.PvhPat, 0},
                    {FacadeType.PlastPost, 0},
                    {FacadeType.PlastPref, 0},
                    {FacadeType.PolotnoAgtAl, 0},
                    {FacadeType.Emal, 0},
                    {FacadeType.MassivLak, 0},
                    {FacadeType.MassivEmal, 0},
                    {FacadeType.NoFacade, 0}
                };
            }

            public string Article { get; set; }

            public string Name { get; set; }

            /// <summary>
            /// Высота
            /// </summary>
            public int H { get; set; }

            /// <summary>
            /// Глубина
            /// </summary>
            public int L { get; set; }

            /// <summary>
            /// Ширина
            /// </summary>
            public int W { get; set; }

            public Dictionary<FacadeType, double> Prices { get; set; }

            public string ResolveArticle()
            {
                var art = Article
                    .Replace("(В)", H.ToString())
                    .Replace("(Ш)", W.ToString());


                if (art.EndsWith("Ш"))
                {
                    art = art.Replace("Ш", W.ToString());
                }

                return art;
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

    }
}