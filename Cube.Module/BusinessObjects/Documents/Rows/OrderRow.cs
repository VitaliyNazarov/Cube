using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Cube.Model.Contexts;
using Cube.Model.Enums;
using Cube.Model.Interfaces;
using DevExpress.ExpressApp;
using SQLite.CodeFirst;

namespace Cube.Model
{
    /// <summary>
    /// Строка заказа.
    /// </summary>
    public class OrderRow : IBaseEntity, IXafEntityObject
    {
        #region Base properties

        /// <summary>
        /// Идентификатор.
        /// </summary>
        [Autoincrement, Browsable(false)]
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
        [SqlDefaultValue(DefaultValue = CubeEFHelper.ExternalIdGenerator), Browsable(false)]
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

        #region Implementation of IXafEntityObject

        public void OnCreated()
        {
        }

        public void OnSaving()
        {
            Sum = Price * Quantity;
            Order.OnSaving();
        }

        public void OnLoaded()
        {
        }

        #endregion
    }
}