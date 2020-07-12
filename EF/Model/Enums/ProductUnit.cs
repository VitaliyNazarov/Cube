using System.ComponentModel.DataAnnotations;

namespace Cube.Model.Model.Enums
{
    /// <summary>
    /// Единицы измерения.
    /// </summary>
    public enum ProductUnit
    {
        [Display(Name = "шт.")]
        pce = 0,

        [Display(Name = "компл.")]
        nmp,

        [Display(Name = "м.")]
        mt
    }
}