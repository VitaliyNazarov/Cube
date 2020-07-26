using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cube.Module.Win.Reports.Entities
{
    /// <summary>
    /// Источник данных для генерации отчета "Карточка заказа"
    /// </summary>
    public class OrderReportView
    {
        /// <summary>
        /// Номер заказа
        /// </summary>
        [Display(Name = "№ заказа")]
        public string OrderNumber { get; set; }

        [Display(Name = "Производственный №")]
        public string ProductionNumber { get; set; }

        [Display(Name = "Изделие")]
        public string ProductName { get; set; }

        [Display(Name = "Комментарии")]
        public string Comments { get; set; }

        [Display(Name = "Заказчик")]
        public string CustomerName { get; set; }

        [Display(Name = "Дизайнер")]
        public string Designer { get; set; }

        [Display(Name = "Дата создания")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Дата сдачи")]
        public DateTime PlanningDate { get; set; }

        [Display(Name = "Сумма")]
        public double TotalSum { get; set; }

        [Display(Name = "Картинка")]
        public byte[] Image { get; set; }

        public List<OrderRowReportView> Rows { get; set; }

        public string GetImageFileName()
        {
            return $"Заказ_{OrderNumber}_{DateTime.Now:ddMMyyyyHHmmss}";
        }
    }
}