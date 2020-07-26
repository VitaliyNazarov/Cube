using System;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Cube.Model;
using Cube.Module.Win.Reports;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;

namespace Cube.Module.Win.Controllers
{
    public partial class OrderReportsController : ViewController
    {
        private SimpleAction productionSpecificationAction;
        private SimpleAction customerOrderCardAction;
        private CubeReportManager _reportManager;

        public OrderReportsController()
        {
            TargetObjectType = typeof(Order);

            productionSpecificationAction = new SimpleAction(
                this, "OrderProductSpecification",
                DevExpress.Persistent.Base.PredefinedCategory.ObjectsCreation)
            {
                ImageName = "BO_KPI_Scorecard",
                Caption = "Спецификация"
            };

            productionSpecificationAction.Execute += ProductionSpecificationAction_Execute;

            customerOrderCardAction = new SimpleAction(
                this, "CusomerOrderCard",
                DevExpress.Persistent.Base.PredefinedCategory.ObjectsCreation)
            {
                ImageName = "Detailed",
                Caption = "Карточка заказа"
            };
            customerOrderCardAction.Execute += CustomerOrderCardAction_Execute;

            _reportManager = new CubeReportManager();
        }

        private void CustomerOrderCardAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var dataSource = View.SelectedObjects
                .OfType<Order>()
                .Select(_reportManager.CreateOrderReportView)
                .ToList();

            var orderView = dataSource.First();
            var report = new CusomerOrderCardReport
            {
                DataSource = dataSource
            };
            if (!Directory.Exists(".\\Reports"))
                Directory.CreateDirectory(".\\Reports");

            var filePath = $".\\Reports\\{orderView.GetImageFileName()}.jpeg";
            report.ExportToImage(filePath, new ImageExportOptions
            {
                ExportMode = ImageExportMode.SingleFile,
                Format = ImageFormat.Jpeg,
                Resolution = 600,
                TextRenderingMode = TextRenderingMode.AntiAlias
            });

            Process.Start(filePath);
        }

        private void ProductionSpecificationAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            throw new Exception("Test");
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            View.SelectionChanged += View_SelectionChanged;
        }
        protected override void OnDeactivated()
        {
            View.SelectionChanged -= View_SelectionChanged;
            base.OnDeactivated();
        }

        private void View_SelectionChanged(object sender, System.EventArgs e)
        {
            UpdateActivity();
        }

        private void UpdateActivity()
        {
            productionSpecificationAction.Enabled.SetItemValue("OrderSelected", View.SelectedObjects.Count == 1); 
            customerOrderCardAction.Enabled.SetItemValue("OrderSelected", View.SelectedObjects.Count == 1);
        }
    }
}