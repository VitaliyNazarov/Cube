using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;

namespace Cube.Model.Security
{
    /// <summary>
    /// Роль пользователя.
    /// </summary>
    [XafDisplayName("Name")]
    public class Role : PermissionPolicyRole
    {
        /// <summary>
        /// ctor
        /// </summary>
        public Role()
        {
        }
    }
}