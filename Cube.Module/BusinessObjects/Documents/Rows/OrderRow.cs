using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Cube.Model.Contexts;
using Cube.Model.Enums;
using Cube.Model.Interfaces;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using SQLite.CodeFirst;

namespace Cube.Model
{
    /// <summary>
    /// Строка заказа.
    /// </summary>
    public class OrderRow : IBaseEntity, IXafEntityObject, IObjectSpaceLink
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
        /// Позиция строки в списке.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Цена единицы.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Количество товара.
        /// </summary>
        [RuleRequiredField("OrderRow_Quantity_Required",
            DefaultContexts.Save,
            SkipNullOrEmptyValues = false,
            CustomMessageTemplate = "Количество товара должно быть больше 0.")]
        public int Quantity { get; set; }

        /// <summary>
        /// Сумма по строке.
        /// </summary>
        public double Sum { get; set; }

        /// <summary>
        /// Единицы измерения.
        /// </summary>
        public ProductUnit Unit { get; set; }

        /// <summary>
        /// Размеры
        /// </summary>
        [XafDisplayName("Размер")]
        [Appearance("Kitchen_Size", Visibility = ViewItemVisibility.Hide, Criteria = "!IsKitchen", Context = "Any")]
        public string Size { get; set; }

        /// <summary>
        /// Фасад для кухни
        /// </summary>
        [XafDisplayName("Фасад")]
        [Appearance("Kitchen_Facade", Visibility = ViewItemVisibility.Hide, Criteria = "!IsKitchen", Context = "Any")]
        public FacadeType Facade { get; set; }

        /// <summary>
        /// Признак того что продукт для кухни
        /// </summary>
        [Browsable(false)]
        [NotMapped]
        public bool IsKitchen => (Order?.OrderType ?? OrderType.General) == OrderType.Kitchen;

        [ImageEditor(ListViewImageEditorMode = ImageEditorMode.PictureEdit, ListViewImageEditorCustomHeight = 100, ImageSizeMode = ImageSizeMode.AutoSize), 
         VisibleInListView(true), 
         VisibleInDetailView(true),
         XafDisplayName("Изображение")]
        public byte[] Image => Product?.Image;

        #region References

        /// <summary>
        /// Продукт.
        /// </summary>
        [ImmediatePostData]
        [RuleRequiredField("OrderRow_Product_Required",
            DefaultContexts.Save,
            SkipNullOrEmptyValues = false,
            CustomMessageTemplate = "Необходмо выбрать продукт.")]
        public virtual Product Product { get; set; }

        /// <summary>
        /// Заказ.
        /// </summary>
        public virtual Order Order { get; set; }

        /// <summary>
        /// Цена источник.
        /// </summary>
        public virtual Price SourcePrice { get; set; }

        #endregion

        #region Implementation of IXafEntityObject

        public void OnCreated()
        {
            CreatedDate = DateTime.Now;
        }

        public void OnSaving()
        {
            if (Product != null && Order != null)
            {
                SourcePrice = _objectSpace.FindObject<Price>(CriteriaOperator.And(
                    new BinaryOperator("Product.Id", Product.Id),
                    new BinaryOperator("PriceList.Id", Order.PriceList.Id)));

                if (SourcePrice == null)
                {
                    throw new Exception("Не найдена цена.");
                }

                Facade = Product.Facade;
                Size = Product.GetSize();
                Unit = Product.Unit;
                Price = SourcePrice?.Value ?? 0;
                Sum = Price * Quantity;
                Order?.OnSaving();
            }

            if (Position == 0)
            {
                Position = (Order?.Rows.Count ?? 0 );
            }
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
    }
}