using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Cube.Model.Contexts;
using Cube.Model.Interfaces;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
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
        public string Size => Product?.GetSize();

        /// <summary>
        /// Продукт
        /// </summary>
        [XafDisplayName("Продукт"),
         NotMapped,
         VisibleInListView(true),
         VisibleInDetailView(false),
         VisibleInLookupListView(true)]
        public string ProductName => Product?.Name;

        /// <summary>
        /// Группа цен
        /// </summary>
        [XafDisplayName("Группа цен"),
         NotMapped,
         VisibleInListView(true),
         VisibleInDetailView(false),
         VisibleInLookupListView(true)]
        public string GroupName => PriceGroup?.Name;

        #region References

        /// <summary>
        /// Прайс, к которому относится цена.
        /// </summary>
        public virtual PriceList PriceList { get; set; }

        /// <summary>
        /// Группа, которой принадлежит цена.
        /// </summary>
        [RuleRequiredField("Price_PriceGroup_Required",
            DefaultContexts.Save,
            SkipNullOrEmptyValues = false,
            CustomMessageTemplate = "Необходимо задать группу цен.")]
        [XafDisplayName("Группа цен"),
         VisibleInListView(true),
         VisibleInDetailView(true),
         VisibleInLookupListView(false)]
        public virtual PriceGroup PriceGroup { get; set; }

        /// <summary>
        /// Продукт, на который установлена цена.
        /// </summary>
        [RuleRequiredField("Price_Product_Required",
            DefaultContexts.Save,
            SkipNullOrEmptyValues = false,
            CustomMessageTemplate = "Необходимо задать продукт.")]
        [XafDisplayName("Продукт"),
         VisibleInListView(false),
         VisibleInDetailView(true),
         VisibleInLookupListView(false)]
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