using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cube.Model.Model.Contexts;
using Cube.Model.Model.Interfaces;
using SQLite.CodeFirst;

namespace Cube.Model.Model
{
    /// <summary>
    /// Группа продуктов.
    /// </summary>
    public class ProductGroup : IBaseEntity, ISoftDeletable, IArchivable
    {
        /// <summary>
        /// ctor
        /// </summary>
        public ProductGroup()
        {
            Products = new HashSet<Product>();
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
        /// Дата удаления группы.
        /// </summary>
        public DateTime? DeletedDate { get; set; }

        /// <summary>
        /// Удаленная группа.
        /// </summary>
        public bool Deleted { get; set; }

        #endregion

        /// <summary>
        /// Название группы.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходмо название группы.")]
        [MaxLength(256, ErrorMessage = "Название группы не должно превышать 256 символов.")]
        public string Name { get; set; }

        /// <summary>
        /// Название группы.
        /// </summary>
        [MaxLength(1024, ErrorMessage = "Название группы не должно превышать 1024 символа.")]
        public string Description { get; set; }

        #region References

        /// <summary>
        /// Родительская группа.
        /// </summary>
        public virtual ProductGroup Parent { get; set; }

        /// <summary>
        /// Продукты в группе.
        /// </summary>
        public virtual ICollection<Product> Products { get; set; }

        #endregion

        #region Implementation of IArchivable

        /// <summary>
        /// Группа в архиве.
        /// </summary>
        public bool IsArchive { get; set; }

        #endregion
    }
}