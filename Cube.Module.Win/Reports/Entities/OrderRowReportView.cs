namespace Cube.Module.Win.Reports.Entities
{
    public class OrderRowReportView
    {
        public byte[] Image { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public double Quantity { get; set; }

        public double Price { get; set; }

        public double Sum { get; set; }
    }
}