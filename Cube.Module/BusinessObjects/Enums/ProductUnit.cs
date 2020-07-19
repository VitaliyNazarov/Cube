using DevExpress.ExpressApp.DC;

namespace Cube.Model.Enums
{
    /// <summary>
    /// Единицы измерения.
    /// </summary>
    public enum ProductUnit
    {
        /// <summary>
        /// Шт
        /// </summary>
        [XafDisplayName("шт.")]
        pce = 0,

        /// <summary>
        /// Комплект
        /// </summary>
        [XafDisplayName("комплект")]
        nmp,

        /// <summary>
        /// Метры погонные
        /// </summary>
        [XafDisplayName("м.п.")]
        mt,

        /// <summary>
        /// Метры кв
        /// </summary>
        [XafDisplayName("м2")]
        mt2
    }
}