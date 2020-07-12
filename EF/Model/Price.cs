using System;
using System.ComponentModel.DataAnnotations.Schema;
using Cube.Model.Model.Contexts;
using Cube.Model.Model.Interfaces;
using SQLite.CodeFirst;

namespace Cube.Model.Model
{
    /// <summary>
    /// Цена - элемент прайс листа.
    /// </summary>
    public class Price : IBaseEntity, ISoftDeletable, IArchivable
    {
        /// <summary>
        /// ctor
        /// </summary>
        public Price()
        {
        }

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

        /// <summary>
        /// Дата удаления.
        /// </summary>
        public DateTime? DeletedDate { get; set; }

        /// <summary>
        /// Удаленный прайс.
        /// </summary>
        public bool Deleted { get; set; }

        #endregion

        /// <summary>
        /// Значение цены за единицу продукта.
        /// </summary>
        public double Value { get; set; }

        #region References

        /// <summary>
        /// Прайс, к которому относится цена.
        /// </summary>
        public virtual PriceList PriceList { get; set; }

        /// <summary>
        /// Продукт, на который установлена цена.
        /// </summary>
        public virtual Product Product { get; set; }

        #endregion

        #region Implementation of IArchivable

        /// <summary>
        /// Цена в архиве.
        /// </summary>
        public bool IsArchive { get; set; }

        #endregion
    }
}