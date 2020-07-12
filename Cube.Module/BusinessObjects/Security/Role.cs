using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cube.Model.Contexts;
using Cube.Model.Interfaces;
using SQLite.CodeFirst;

namespace Cube.Model.Security
{
    /// <summary>
    /// Роль пользователя.
    /// </summary>
    public class Role : IBaseEntity
    {
        /// <summary>
        /// ctor
        /// </summary>
        public Role()
        {
            Users = new HashSet<User>();
        }

        /// <summary>
        /// Название роли.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходмо указать название роли.")]
        [MaxLength(256, ErrorMessage = "Название роли не должно превышать 256 символов.")]
        public string Name { get; set; }

        /// <summary>
        /// Тип роли.
        /// </summary>
        public RoleType Type { get; set; }

        /// <summary>
        /// Пользователи входящие в роль.
        /// </summary>
        public virtual ICollection<User> Users { get; set; }

        #region Implementation of IBaseEntity

        /// <inheritdoc />
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
    }
}