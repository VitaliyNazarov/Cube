using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Cube.Model.Contexts;
using Cube.Model.Enums;
using Cube.Model.Interfaces;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.EF.Utils;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using SQLite.CodeFirst;
using User = Cube.Model.Security.User;

namespace Cube.Model
{
    /// <summary>
    /// Заказ.
    /// </summary>
    [XafDefaultProperty("Number"), XafDisplayName("Заказ")]
    public class Order : IBaseEntity, IArchivable, IImageHolder, IXafEntityObject, IObjectSpaceLink
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
        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "G")]
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
        [RuleRequiredField("Number_Required",
            DefaultContexts.Save,
            SkipNullOrEmptyValues = false,
            CustomMessageTemplate = "Необходимо указать номер заказа.",
            ResultType = ValidationResultType.Error)]
        [RuleUniqueValue("Number_Unique",
            DefaultContexts.Save, 
            CustomMessageTemplate = "Указанный № заказа уже существует.", 
            ResultType = ValidationResultType.Error)]
        [StringLength(128, ErrorMessage = "Номер заказа не должен превышать 128 символов.")]
        public string Number { get; set; }

        /// <summary>
        /// Заказчик.
        /// </summary>
        [RuleRequiredField("CustomerName_Required",
            DefaultContexts.Save,
            SkipNullOrEmptyValues = false,
            CustomMessageTemplate = "Необходимо указать ФИО заказчика.")]
        public string CustomerName { get; set; }

        /// <summary>
        /// Плановая дата сдачи заказа.
        /// </summary>
        [ModelDefault("DisplayFormat", "G")]
        [RuleRequiredField("PlanningDate_Required",
            DefaultContexts.Save,
            SkipNullOrEmptyValues = false,
            CustomMessageTemplate = "Укажите плановую дату сдачи заказа.",
            ResultType = ValidationResultType.Error)]
        [RuleValueComparison("PlanningDate_GreaterThan_CreatedDate",
            DefaultContexts.Save,
            ValueComparisonType.GreaterThan,
            nameof(CreatedDate),
            ParametersMode.Expression,
            CustomMessageTemplate = "Плановая дата сдачи заказа должна позже даты создания заказа.")]  
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
        [ModelDefault("AllowEdit", "False")]
        public double Sum { get; set; }

        /// <summary>
        /// Последняя дата изменения.
        /// </summary>
        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "G")]
        public DateTime ChangedDate { get; set; }

        /// <summary>
        /// Создал заказ.
        /// </summary>
        [ModelDefault("AllowEdit", "False")]
        public virtual User Creator { get; set; }

        /// <summary>
        /// Кто вносил изменения последним.
        /// </summary>
        [ModelDefault("AllowEdit", "False")]
        public virtual User Modifier { get; set; }

        /// <summary>
        /// Прайс лист по которому устанавливается цена товара.
        /// </summary>
        public virtual PriceList PriceList { get; set; }

        /// <summary>
        /// Состав заказа.
        /// </summary>
        [Aggregated]
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
            CreatedDate = DateTime.Now;
            Creator = GetCurrentUser();
            State = OrderState.New;
            PriceList = ObjectSpace.GetObjects<PriceList>(new BinaryOperator("Name", "Основной прайс-лист")).FirstOrDefault();
        }

        public void OnSaving()
        {
            if (_objectSpace != null)
            {
                Modifier = GetCurrentUser();
                ChangedDate = DateTime.Now;
            }
            Sum = Rows.Sum(x => x.Sum);
        }

        public void OnLoaded()
        {

        }

        #endregion

        #region Implementation of IObjectSpaceLink

        private IObjectSpace _objectSpace;

        [Browsable(false)]
        public IObjectSpace ObjectSpace
        {
            get => _objectSpace;
            set => _objectSpace = value;
        }

        #endregion

        private User GetCurrentUser()
        {
            return _objectSpace.GetObjectByKey<User>(SecuritySystem.CurrentUserId);
        }
    }
}