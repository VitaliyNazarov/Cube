using System.Collections.Generic;

namespace Cube.Module.Win.Reports.Entities
{
    /// <summary>
    /// Источник данных для генерации отчета "Производственная спецификация"
    /// </summary>
    public class ProductionSpecification
    {
        public List<OrderReportView> Rows { get; set; }
    }
}
