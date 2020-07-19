using System.ComponentModel.DataAnnotations;
using DevExpress.ExpressApp.DC;

namespace Cube.Model.Enums
{
    /// <summary>
    /// Тип заказа.
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// Произвольный заказ.
        /// </summary>
        [XafDisplayName("Не задан")]
        General,

        /// <summary>
        /// Кухня.
        /// </summary>
        [XafDisplayName("Кухня")]
        Kitchen,

        /// <summary>
        /// Шкаф
        /// </summary>
        [XafDisplayName("Шкаф")]
        Сupboard 
    }
}