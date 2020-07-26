using System.Linq;
using Cube.Model;
using Cube.Module.Win.Reports.Entities;

namespace Cube.Module.Win.Reports
{
    public class CubeReportManager
    {
        public OrderReportView CreateOrderReportView(Order order)
        {
            return new OrderReportView
            {
                CreatedDate = order.CreatedDate,
                CustomerName = order.CustomerName,
                OrderNumber = order.Number,
                Designer = order.Designer,
                Comments = order.Comments,
                PlanningDate = order.PlanningDate,
                ProductName = order.ProductName,
                ProductionNumber = "________",
                Rows = order.Rows.Select(Create).ToList(),
                TotalSum = order.Sum,
                Image = order.Image
            };
        }

        private OrderRowReportView Create(OrderRow row)
        {
            return new OrderRowReportView
            {
                Image = row.Image,
                ProductName = row.Product.Name,
                Quantity = row.Quantity,
                Price = row.Price,
                Sum = row.Sum,
                Description = row.Product.Description
            };
        }
    }
}
