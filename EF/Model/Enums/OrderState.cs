using System.ComponentModel.DataAnnotations;

namespace Cube.Model.Model.Enums
{
    /// <summary>
    /// Состояние заказа.
    /// </summary>
    public enum OrderState
    {
        [Display(Name = "Новый")]
        New,

        [Display(Name = "В работе")]
        InProgress,

        [Display(Name = "Завершен")]
        Completed
    }
}