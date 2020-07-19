using Cube.Model;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Win.SystemModule;

namespace Cube.Module.Win.Controllers
{
    public partial class OrderPrintController : ViewController
    {
        private SimpleAction printProductionSpecification;
        private PrintingController printingService;

        public OrderPrintController()
        {
            TargetObjectType = typeof(Order);
            TargetViewType = ViewType.DetailView;

            printProductionSpecification = new SimpleAction(
                this, "OrderPrintScpecification",
                DevExpress.Persistent.Base.PredefinedCategory.Reports)
            {
                ImageName = "ExportToImg",
                Caption = "Спецификация"
            };
            printProductionSpecification.Execute += (s, e) =>
            {
                View.ObjectSpace.CommitChanges();
                View.ObjectSpace.Refresh();
                printingService.PrintPreviewAction.DoExecute();
            };
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            printingService = Frame.GetController<PrintingController>();
        }
    }

    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
}