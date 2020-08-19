using DevExpress.ExpressApp.DC;

namespace Cube.Model.Enums
{
    ///// <summary>
    ///// Типы фасадов
    ///// </summary>
    //public enum FacadeType
    //{
    //    [XafDisplayName("-")]
    //    Undefined,

    //    [XafDisplayName("ЛДСП")]
    //    Ldsp,

    //    [XafDisplayName("ЛДСП AL")]
    //    LdspAl,

    //    [XafDisplayName("ПВХ")]
    //    Pvh,

    //    [XafDisplayName("ПВХ пат")]
    //    PvhPat,

    //    [XafDisplayName("ПЛАСТ пост")]
    //    PlastPost,

    //    [XafDisplayName("ПЛАСТ преф")]
    //    PlastPref,

    //    [XafDisplayName("Полотно AGT AL")]
    //    PolotnoAgtAl,

    //    [XafDisplayName("Эмаль")]
    //    Emal,

    //    [XafDisplayName("Массив Лак")]
    //    MassivLak,

    //    [XafDisplayName("Массив Эмаль")]
    //    MassivEmal,

    //    [XafDisplayName("Без фасада")]
    //    NoFacade
    //}

    /// <summary>
    /// Предопределенные варианты продукта.
    /// Используются для разделения цены для одного
    /// продукта и разных вариантов.
    /// </summary>
    public class PredefinedProductVariantType
    {
        public const string Default = "-";

        public const string Ldsp = "ЛДСП";

        public const string LdspAl = "ЛДСП AL";

        public const string Pvh = "ПВХ";

        public const string PvhPat = "ПВХ пат";

        public const string PlastPost = "ПЛАСТ пост";

        public const string PlastPref = "ПЛАСТ преф";

        public const string PolotnoAgtAl = "Полотно AGT AL";

        public const string Emal = "Эмаль";

        public const string MassivLak = "Массив Эмаль";
    }
}