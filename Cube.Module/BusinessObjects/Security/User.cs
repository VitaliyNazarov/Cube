using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Cube.Model.Contexts;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using SQLite.CodeFirst;

namespace Cube.Model.Security
{
    /// <summary>
    /// Пользователь.
    /// </summary>
    [ImageName("BO_User"), 
     XafDisplayName("Пользователь"),
     XafDefaultProperty("Name")]
    public class User : PermissionPolicyUser
    {
        [XafDisplayName("Имя пользователя")]
        public string Name
        {
            get => UserName;
            set => UserName = value;
        }

        #region Implementation of IBaseEntity

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

        #endregion
    }
}