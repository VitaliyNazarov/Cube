using DevExpress.ExpressApp.DC;

namespace Cube.Model.Enums
{
    /// <summary>
    /// Типы фасадов
    /// </summary>
    public enum FacadeType
    {
        [XafDisplayName("-")]
        Undefined,

        [XafDisplayName("ЛДСП")]
        Ldsp,

        [XafDisplayName("ЛДСП AL")]
        LdspAl,

        [XafDisplayName("ПВХ")]
        Pvh,

        [XafDisplayName("ПВХ пат")]
        PvhPat,

        [XafDisplayName("ПЛАСТ пост")]
        PlastPost,

        [XafDisplayName("ПЛАСТ преф")]
        PlastPref,

        [XafDisplayName("Полотно AGT AL")]
        PolotnoAgtAl,

        [XafDisplayName("Эмаль")]
        Emal,

        [XafDisplayName("Массив Лак")]
        MassivLak,

        [XafDisplayName("Массив Эмаль")]
        MassivEmal,

        [XafDisplayName("Без фасада")]
        NoFacade
    }
}