﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cube.Model.Contexts;
using Cube.Model.Enums;
using Cube.Model.Interfaces;
using DevExpress.Persistent.Base.General;
using SQLite.CodeFirst;

namespace Cube.Model
{
    /// <summary>
    /// Продукт.
    /// </summary>
    public class Product : IBaseEntity, ISoftDeletable, IImageHolder, IArchivable, ICategorizedItem
    {
        /// <summary>
        /// ctor
        /// </summary>
        public Product()
        {
            Images = new List<Image>();
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
        /// Дата удаления группы.
        /// </summary>
        public DateTime? DeletedDate { get; set; }

        /// <summary>
        /// Удаленная группа.
        /// </summary>
        public bool Deleted { get; set; }

        #endregion

        [Unique]
        [Index("UX_Product_Article", IsUnique = true)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходмо задать артикул.")]
        public string Article { get; set; }

        /// <summary>
        /// Название продукта.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходмо название продукта.")]
        [MaxLength(256, ErrorMessage = "Название продукта не должно превышать 256 символов.")]
        public string Name { get; set; }

        /// <summary>
        /// Описание продукта.
        /// </summary>
        [MaxLength(1024, ErrorMessage = "Название продукта не должно превышать 1024 символа.")]
        public string Description { get; set; }

        /// <summary>
        /// Ширина.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Высота.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Длина.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Единицы измерения.
        /// </summary>
        public ProductUnit Unit { get; set; }

        #region References

        /// <summary>
        /// Группа в которую включен продукт.
        /// </summary>
        public virtual ProductGroup Category { get; set; }

        #endregion

        #region Implementation of IImageHolder

        /// <inheritdoc />
        public virtual IList<Image> Images { get; set; }

        #endregion

        #region Implementation of IArchivable

        /// <summary>
        /// Продукт в архиве.
        /// </summary>
        public bool IsArchive { get; set; }

        ITreeNode ICategorizedItem.Category
        {
            get => Category; 
            set => Category = (ProductGroup)value;
        }

        #endregion

    }
}