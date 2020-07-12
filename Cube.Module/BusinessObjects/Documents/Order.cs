using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cube.Model.Contexts;
using Cube.Model.Enums;
using Cube.Model.Interfaces;
using DevExpress.Persistent.BaseImpl.EF;
using SQLite.CodeFirst;
using User = Cube.Model.Security.User;

namespace Cube.Model
{
    /// <summary>
    /// Заказ.
    /// </summary>
    public class Order : IBaseEntity, IArchivable, IImageHolder//, IFileHolder
    {
        /// <summary>
        /// ctor
        /// </summary>
        public Order()
        {
            Rows = new List<OrderRow>();
            Images = new List<Image>();
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
        public bool IsArchive { get; set; }

        #endregion

        #region Implementation of IImageHolder

        /// <summary>
        /// Коллекция картинок заказа.
        /// </summary>
        public virtual IList<Image> Images { get; }

        #endregion

        #region Implementation of IFileHolder

        /// <summary>
        /// Коллекция файлов заказа.
        /// </summary>
        //public virtual IList<FileData> Files { get; }

        #endregion
    }
}