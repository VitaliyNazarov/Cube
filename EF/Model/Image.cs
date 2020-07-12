using System;
using System.ComponentModel.DataAnnotations.Schema;
using Cube.Model.Model.Contexts;
using Cube.Model.Model.Enums;
using Cube.Model.Model.Interfaces;
using SQLite.CodeFirst;

namespace Cube.Model.Model
{
    /// <summary>
    /// Картинка.
    /// </summary>
    public class Image : IBaseEntity
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
        /// Ширина.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Высота.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Тип (jpg, png, etc.)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Картинка.
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// Тип размера.
        /// </summary>
        public CubeImageType SizeType { get; set; }
    }
}