using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cube.Model.Contexts;
using Cube.Model.Interfaces;
using SQLite.CodeFirst;

namespace Cube.Model
{
    /// <summary>
    /// Прайс-лист.
    /// </summary>
    public class PriceList : IBaseEntity, ISoftDeletable, IArchivable
    {
        /// <summary>
        /// ctor
        /// </summary>
        public PriceList()
        {
            Prices = new HashSet<Price>();
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
        /// Название прайса.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходмо название прайс-листа.")]
        [MaxLength(256, ErrorMessage = "Название прайс-листа не должно превышать 256 символов.")]
        public string Name { get; set; }

        #region References

        /// <summary>
        /// Цены продуктов по прайсу.
        /// </summary>
        public virtual ICollection<Price> Prices { get; set; }

        #endregion

        #region Implementation of IArchivable

        /// <summary>
        /// Прайс в архиве.
        /// </summary>
        public bool IsArchive { get; set; }

        #endregion
    }
}