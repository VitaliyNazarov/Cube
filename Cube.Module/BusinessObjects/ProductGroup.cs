using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cube.Model.Contexts;
using Cube.Model.Interfaces;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.Validation;
using SQLite.CodeFirst;

namespace Cube.Model
{
    /// <summary>
    /// Группа продуктов.
    /// </summary>
    public class ProductGroup : ITreeNode, ITreeNodeSvgImageProvider, IBaseEntity, ISoftDeletable, IArchivable
    {
        /// <summary>
        /// ctor
        /// </summary>
        public ProductGroup()
        {
            Products = new List<Product>();
            Children = new BindingList<ProductGroup>();
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
        /// Дата удаления группы.
        /// </summary>
        public DateTime? DeletedDate { get; set; }

        /// <summary>
        /// Удаленная группа.
        /// </summary>
        public bool Deleted { get; set; }

        #endregion

        /// <summary>
        /// Название группы.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходмо название группы.")]
        [MaxLength(256, ErrorMessage = "Название группы не должно превышать 256 символов.")]
        public string Name { get; set; }

        /// <summary>
        /// Название группы.
        /// </summary>
        [MaxLength(1024, ErrorMessage = "Название группы не должно превышать 1024 символа.")]
        public string Description { get; set; }

        [NotMapped]
        [Browsable(false)]
        [RuleFromBoolProperty(
            "ProductGroupCircularReferences",
            DefaultContexts.Save,
            "Обнаружена циклическая ссылка. Для исправления ошибки измените значение родительской группы.",
            UsedProperties = "Parent")]
        public bool IsValid
        {
            get
            {
                var currentObj = Parent;
                while (currentObj != null)
                {
                    if (currentObj == this)
                    {
                        return false;
                    }
                    currentObj = currentObj.Parent;
                }
                return true;
            }
        }

        #region References

        /// <summary>
        /// Родительская группа.
        /// </summary>
        public virtual ProductGroup Parent { get; set; }

        /// <summary>
        /// Дочерние группы.
        /// </summary>
        public virtual IList<ProductGroup> Children { get; set; }

        /// <summary>
        /// Продукты в группе.
        /// </summary>
        public virtual IList<Product> Products { get; set; }

        #endregion

        #region Implementation of IArchivable

        /// <summary>
        /// Группа в архиве.
        /// </summary>
        public bool IsArchive { get; set; }

        #endregion

        #region Implementation of ITreeNode

        ITreeNode ITreeNode.Parent => Parent;

        IBindingList ITreeNode.Children => Children as IBindingList;

        #endregion

        #region Implementation of ITreeNodeImageProvider

        public DevExpress.Utils.Svg.SvgImage GetSvgImage(out string imageName)
        {
            imageName = Products != null && Products.Count > 0 ? "BO_Product_Group" : "BO_Category";
            return ImageLoader.Instance.GetImageInfo(imageName).CreateSvgImage();
        }
        #endregion
    }
}