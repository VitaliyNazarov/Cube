using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Cube.Model.Contexts;
using Cube.Model.Enums;
using Cube.Model.Interfaces;
using DevExpress.ExpressApp.DC;
using SQLite.CodeFirst;

namespace Cube.Model
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

        /// <summary>
        /// Дата удаления.
        /// </summary>
        [Browsable(false)]
        public DateTime? DeletedDate { get; set; }

        /// <summary>
        /// Удаленный прайс.
        /// </summary>
        [Browsable(false)]
        public bool Deleted { get; set; }

        #endregion

        /// <summary>
        /// Значение цены за единицу продукта.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Размер
        /// </summary>
        [XafDisplayName("Размер")]
        [NotMapped]
        public string Size => Product.GetSize();

        /// <summary>
        /// Фасад
        /// </summary>
        [XafDisplayName("Фасад")]
        [NotMapped]
        public FacadeType Facade => Product.Facade;

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
        [Browsable(false)]
        public bool IsArchive { get; set; }

        #endregion
    }
}