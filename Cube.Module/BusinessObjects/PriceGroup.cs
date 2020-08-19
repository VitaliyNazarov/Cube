using System;
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
    /// Группа цен. Для разделения цен на продукт
    /// по группам, например, по фасаду.
    /// </summary>
    [XafDefaultProperty(nameof(PriceGroup.Name))]
    public class PriceGroup : IBaseEntity, ISoftDeletable
    {
        /// <summary>
        /// ctor
        /// </summary>
        public PriceGroup()
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
        /// Название группы.
        /// </summary>
        [XafDisplayName("Название")]
        [RuleUniqueValue("PriceGroup_Name_Unique",
            DefaultContexts.Save, 
            CustomMessageTemplate = "Указанное название группы цен уже существует.", 
            ResultType = ValidationResultType.Error)]
        [RuleRequiredField("PriceGroup_Name_Required",
            DefaultContexts.Save,
            SkipNullOrEmptyValues = false,
            CustomMessageTemplate = "Необходимо задать имя группы цен.")]
        public string Name { get; set; }

        /// <summary>
        /// Группа цен по-умолчанию.
        /// </summary>
        [XafDisplayName("Группа по-умолчанию")]
        public bool IsDefault { get; set; }
    }
}