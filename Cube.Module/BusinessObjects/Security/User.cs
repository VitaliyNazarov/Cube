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
    /// Пользователь.
    /// </summary>
    public class User : IBaseEntity, IArchivable
    {
        /// <summary>
        /// ctor
        /// </summary>
        public User()
        {
            Roles = new HashSet<Role>();
        }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Логин.
        /// </summary>
        [Unique]
        [Index("UX_User_Login", IsUnique = true)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходмо указать уникальный логин пользователя.")]
        [MaxLength(64, ErrorMessage = "Логин пользователя не должнен превышать 64 символа.")]
        [MinLength(3, ErrorMessage = "Логин пользователя должен быть больше 3 символов.")]
        public string Login { get; set; }

        /// <summary>
        /// Пароль (хеш)
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходмо указать пароль пользователя.")]
        public string Password { get; set; }

        /// <summary>
        /// Роли в которые входит пользователь.
        /// </summary>
        public virtual ICollection<Role> Roles { get; set; }

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

        #region Implementation of IArchivable

        /// <inheritdoc />
        public bool IsArchive { get; set; }

        #endregion
    }
}