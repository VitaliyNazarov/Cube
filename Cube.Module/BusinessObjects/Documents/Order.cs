using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Cube.Model.Contexts;
using Cube.Model.Enums;
using Cube.Model.Interfaces;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.EF.Utils;
using DevExpress.Persistent.Base;
using SQLite.CodeFirst;
using User = Cube.Model.Security.User;

namespace Cube.Model
{
    /// <summary>
    /// Заказ.
    /// </summary>
    public class Order : IBaseEntity, IArchivable, IImageHolder, IXafEntityObject
    {
        /// <summary>
        /// ctor
        /// </summary>
        public Order()
        {
            Rows = new List<OrderRow>();
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

        #endregion

        /// <summary>
        /// Номер заказа.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходимо указать номер заказа.")]
        [StringLength(128, ErrorMessage = "Номер заказа не должен превышать 128 символов.")]
        public string Number { get; set; }

        /// <summary>
        /// Заказчик.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходимо указать заказчика.")]
        //[StringLength(256, ErrorMessage = "Имя заказчика не должен превышать 256 символов.")]
        public string CustomerName { get; set; }

        /// <summary>
        /// Плановая дата сдачи заказа.
        /// </summary>
        public DateTime PlanningDate { get; set; }

        /// <summary>
        /// Состояние заказа.
        /// </summary>
        public OrderState State { get; set; }

        /// <summary>
        /// Тип заказного изделия.
        /// </summary>
        public OrderType OrderType { get; set; }

        /// <summary>
        /// Черновик.
        /// </summary>
        public bool IsDraft { get; set; }

        /// <summary>
        /// Сумма заказа.
        /// </summary>
        public double Sum { get; set; }

        /// <summary>
        /// Последняя дата изменения.
        /// </summary>
        public DateTime ChangedDate { get; set; }

        /// <summary>
        /// Создал заказ.
        /// </summary>
        public virtual User Creator { get; set; }

        /// <summary>
        /// Кто вносил изменения последним.
        /// </summary>
        public virtual User Modifier { get; set; }

        /// <summary>
        /// Прайс лист по которому устанавливается цена товара.
        /// </summary>
        public virtual PriceList PriceList { get; set; }

        /// <summary>
        /// Состав заказа.
        /// </summary>
        public virtual IList<OrderRow> Rows { get; set; }

        #region Implementation of IArchivable

        /// <summary>
        /// Заказ в архиве.
        /// </summary>
        [Browsable(false)]
        public bool IsArchive { get; set; }

        #endregion

        #region Implementation of IImageHolder

        /// <summary>
        /// Картинка.
        /// </summary>
        [ImageEditor(ListViewImageEditorMode = ImageEditorMode.PopupPictureEdit, ListViewImageEditorCustomHeight = 400, ImageSizeMode = ImageSizeMode.Normal), 
         Delayed, 
         VisibleInListView(true), 
         XafDisplayName("Изображение")]
        public byte[] Image { get; set; }

        #endregion

        #region Implementation of IXafEntityObject

        public void OnCreated()
        {
            
        }

        public void OnSaving()
        {
            Sum = Rows.Sum(x => x.Sum);
        }

        public void OnLoaded()
        {
            
        }

        #endregion
    }
}