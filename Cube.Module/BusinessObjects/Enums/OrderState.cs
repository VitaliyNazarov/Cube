using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace Cube.Model.Enums
{
    /// <summary>
    /// Состояние заказа.
    /// </summary>
    public enum OrderState
    {
        [ImageName("New")]
        [XafDisplayName("Новый")]
        New,

        [ImageName("InProgress")]
        [XafDisplayName("В работе")]
        InProgress,

        [ImageName("Completed")]
        [XafDisplayName("Завершен")]
        Completed
    }
}