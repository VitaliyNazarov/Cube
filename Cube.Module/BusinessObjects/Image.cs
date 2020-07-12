using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Cube.Model.Contexts;
using Cube.Model.Enums;
using Cube.Model.Interfaces;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.EF.Utils;
using DevExpress.Persistent.Base;
using SQLite.CodeFirst;

namespace Cube.Model
{
    /// <summary>
    /// Картинка.
    /// </summary>
    public class Image : IBaseEntity, IXafEntityObject 
    {
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

        #endregion

        /// <summary>
        /// Картинка.
        /// </summary>
        [ImageEditor(ListViewImageEditorMode = ImageEditorMode.PopupPictureEdit, ListViewImageEditorCustomHeight = 400, ImageSizeMode = ImageSizeMode.Normal), 
         Delayed, 
         VisibleInListView(true), 
         XafDisplayName("Изображение")]
        public byte[] Content { get; set; }

        /// <summary>
        /// Тип размера.
        /// </summary>
        public CubeImageType SizeType { get; set; }

        #region Implementation of IXafEntityObject

        public void OnCreated()
        {
        }

        public void OnSaving()
        {
        }

        public void OnLoaded()
        {
        }

        #endregion
    }
}