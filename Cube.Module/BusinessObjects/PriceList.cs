using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Cube.Model.Contexts;
using Cube.Model.Interfaces;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Validation;
using SQLite.CodeFirst;

namespace Cube.Model
{
    /// <summary>
    /// Прайс-лист.
    /// </summary>
    [XafDefaultProperty("Name")]
    [XafDisplayName("Прайс-лист")]
    public class PriceList : IBaseEntity, ISoftDeletable, IArchivable
    {
        /// <summary>
        /// ctor
        /// </summary>
        public PriceList()
        {
            Prices = new List<Price>();
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
        /// Название прайса.
        /// </summary>
        [RuleUniqueValue("PriceList_Name_Unique",
            DefaultContexts.Save, 
            CustomMessageTemplate = "Указанное название прайс-листа уже существует.", 
            ResultType = ValidationResultType.Error)]
        [RuleRequiredField("PriceList_Name_Required",
            DefaultContexts.Save,
            SkipNullOrEmptyValues = false,
            CustomMessageTemplate = "Необходмо задать имя прайс-листа.")]
        public string Name { get; set; }

        #region References

        /// <summary>
        /// Цены продуктов по прайсу.
        /// </summary>
        [Aggregated]
        public virtual IList<Price> Prices { get; set; }

        #endregion

        #region Implementation of IArchivable

        /// <summary>
        /// Прайс в архиве.
        /// </summary>
        [Browsable(false)]
        public bool IsArchive { get; set; }

        #endregion
    }
}