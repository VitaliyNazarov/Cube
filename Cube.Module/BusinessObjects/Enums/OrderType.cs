using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Произвольный заказ")]
        General,

        /// <summary>
        /// Кухня.
        /// </summary>
        [Display(Name = "Кухня")]
        Kitchen,

        /// <summary>
        /// Шкаф
        /// </summary>
        [Display(Name = "Шкаф")]
        Сupboard 
    }
}