using System;
using Cube.Model;
using Cube.Model.Enums;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;

namespace Cube.Module.Win.Controllers
{
    public class OrderCompletedController : ViewController
    {
        private SimpleAction markCompletedAction;

        public OrderCompletedController()
        {
            TargetObjectType = typeof(Order);
            TargetViewType = ViewType.Any;

            markCompletedAction = new SimpleAction(
                this, "OrderMarkCompleted",
                DevExpress.Persistent.Base.PredefinedCategory.Edit)
            {
                TargetObjectsCriteria = (CriteriaOperator.Parse("Status != ?", OrderState.Completed)).ToString(),
                ConfirmationMessage = "Вы действительно хотите завершить заказы?",
                ImageName = "State_Task_Completed",
                Caption = "Завершить заказ"
            };
            markCompletedAction.Enabled.SetItemValue("CurrentObject", false);
            markCompletedAction.Execute += (s, e) =>
            {
                foreach (Order order in e.SelectedObjects)
                {
                    order.ChangedDate = DateTime.Now;
                    order.State = OrderState.Completed;
                    View.ObjectSpace.SetModified(order);
                }
                View.ObjectSpace.CommitChanges();
                View.ObjectSpace.Refresh();
            };
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            View.CurrentObjectChanged += View_CurrentObjectChanged;
        }

        private void View_CurrentObjectChanged(object sender, EventArgs e)
        {
            var o = View.CurrentObject as Order;
            markCompletedAction.Enabled.SetItemValue("CurrentObject", o != null && o.State != OrderState.Completed);
        }

        protected override void OnDeactivated()
        {
            View.CurrentObjectChanged -= View_CurrentObjectChanged;
            base.OnDeactivated();
        }
    }
}