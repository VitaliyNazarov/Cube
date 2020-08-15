using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Cube.Model.Contexts;
using Cube.Model.Enums;
using Cube.Model.Interfaces;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.EF.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.Validation;
using SQLite.CodeFirst;

namespace Cube.Model
{
    /// <summary>
    /// Продукт.
    /// </summary>
    public class Product : IBaseEntity, ISoftDeletable, IImageHolder, IArchivable, ICategorizedItem
    {
        /// <summary>
        /// ctor
        /// </summary>
        public Product()
        {
            Facade = FacadeType.Undefined;
        }

        #region Base properties

        /// <summary>
        /// Идентификатор.
        /// </summary>
        [Autoincrement, Browsable(false)]
        public long Id { get; set; }

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

        /// <summary>
        /// Дата удаления продукта.
        /// </summary>
        [Browsable(false)]
        public DateTime? DeletedDate { get; set; }

        /// <summary>
        /// Удаленный продукт.
        /// </summary>
        [Browsable(false)]
        public bool Deleted { get; set; }

        #endregion

        //[Unique]
        [System.ComponentModel.DataAnnotations.Schema.Index("UX_Product_Article", IsUnique = false)]
        //[RuleUniqueValue("Product_Article_Unique",
        //    DefaultContexts.Save, 
        //    CustomMessageTemplate = "Указанный артикул продукта уже существует.", 
        //    ResultType = ValidationResultType.Error)]
        [RuleRequiredField("Product_Article_Required",
            DefaultContexts.Save,
            SkipNullOrEmptyValues = false,
            CustomMessageTemplate = "Необходмо задать артикул.")]
        public string Article { get; set; }

        /// <summary>
        /// Название продукта.
        /// </summary>
        [RuleRequiredField("Product_Name_Required",
            DefaultContexts.Save,
            SkipNullOrEmptyValues = false,
            CustomMessageTemplate = "Необходмо указать название продукта.")]
        public string Name { get; set; }

        /// <summary>
        /// Описание продукта.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Ширина.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Высота.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Длина.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Единицы измерения.
        /// </summary>
        public ProductUnit Unit { get; set; }

        /// <summary>
        /// Фасад
        /// </summary>
        public FacadeType Facade { get; set; }

        #region References

        /// <summary>
        /// Группа в которую включен продукт.
        /// </summary>
        public virtual ProductGroup Category { get; set; }

        #endregion

        #region Implementation of IImageHolder

        /// <summary>
        /// Картинка.
        /// </summary>
        [ImageEditor(ListViewImageEditorMode = ImageEditorMode.PictureEdit, ListViewImageEditorCustomHeight = 200, ImageSizeMode = ImageSizeMode.StretchImage), 
         VisibleInListView(true), 
         VisibleInDetailView(true),
         XafDisplayName("Изображение")]
        public byte[] Image { get; set; }

        /// <summary>
        /// Ключ продукта для экспорта\импорта.
        /// </summary>
        [Browsable(false)]
        public string Key { get; set; }

        #endregion

        #region Implementation of IArchivable

        /// <summary>
        /// Продукт в архиве.
        /// </summary>
        [Browsable(false)]
        public bool IsArchive { get; set; }

        ITreeNode ICategorizedItem.Category
        {
            get => Category; 
            set => Category = (ProductGroup)value;
        }

        #endregion

        public string GetSize()
        {
            return $"{Height} x {Length} x {Width}";
        }
    }
}