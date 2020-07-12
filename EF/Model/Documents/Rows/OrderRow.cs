using System;
using System.ComponentModel.DataAnnotations.Schema;
using Cube.Model.Model.Contexts;
using Cube.Model.Model.Enums;
using Cube.Model.Model.Interfaces;
using SQLite.CodeFirst;

namespace Cube.Model.Model
{
    /// <summary>
    /// Строка заказа.
    /// </summary>
    public class OrderRow : IBaseEntity
    {
        #region Base properties

        /// <summary>
        /// Идентификатор.
        /// </summary>
        [Autoincrement]
        public long Id { get; set; }

        /// <summary>
        /// Дата создания.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [SqlDefaultValue(DefaultValue = CubeEFHelper.CreatedDateGenerator)]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Внешний уникальный идентификатор.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [SqlDefaultValue(DefaultValue = CubeEFHelper.ExternalIdGenerator)]
        public Guid ExternalId { get; set; }

        #endregion

        /// <summary>
        /// Позиция строки в списке.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Цена единицы.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Количество товара.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Сумма по строке.
        /// </summary>
        public double Sum { get; set; }

        /// <summary>
        /// Единицы измерения.
        /// </summary>
        public ProductUnit Unit { get; set; }

        #region References

        /// <summary>
        /// Продукт.
        /// </summary>
        public virtual Product Product { get; set; }

        /// <summary>
        /// Заказ.
        /// </summary>
        public virtual Order Order { get; set; }

        /// <summary>
        /// Цена источник.
        /// </summary>
        public virtual Price SourcePrice { get; set; }

        #endregion
    }
}