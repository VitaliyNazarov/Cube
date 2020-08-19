using System;
using System.Windows.Forms;
using Cube.Model;
using Cube.Module.Win.Exchange;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Win;
using DevExpress.XtraEditors;

namespace Cube.Module.Win.Controllers
{
    public class ProductsImportExportController : ViewController
    {
        private const string ExcelFilesFilter = "Excel Files|*.xlsx";
        private SimpleAction exportAction;
        private SimpleAction importAction;

        public ProductsImportExportController()
        {
            TargetObjectType = typeof(Product);
            TargetViewType = ViewType.ListView;

            exportAction = new SimpleAction(
                this, "ExportCatalogAction",
                DevExpress.Persistent.Base.PredefinedCategory.Edit)
            {
                ImageName = "Export",
                Caption = "Экспорт каталога"
            };
            exportAction.Execute += ExportAction_Execute;

            importAction = new SimpleAction(
                this, "ImportCatalogAction",
                DevExpress.Persistent.Base.PredefinedCategory.Edit)
            {
                ImageName = "Import",
                Caption = "Импорт каталога"
            };
            importAction.Execute += ImportAction_Execute;
        }

        private void ImportAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var winApplication = (WinApplication) Application;
            var form = ((WinWindow)winApplication.MainWindow).Form;
            var filePath = string.Empty;
            using (var dialog = new OpenFileDialog
            {
                Multiselect = false,
                Title = "Импорт каталога",
                Filter = ExcelFilesFilter
            })
            {
                if (dialog.ShowDialog(form) != DialogResult.OK)
                    return;

                filePath = dialog.FileName;
            }

            Run(form, () =>
            {
                using (var manager = new ExchangeManager())
                {
                    manager.Import(filePath);
                }
            }, "Импорт каталога");
        }

        private void ExportAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var winApplication = (WinApplication) Application;
            var form = ((WinWindow)winApplication.MainWindow).Form;
            string filePath;
            using (var dialog = new SaveFileDialog
            {
                Title = "Экспорт каталога",
                Filter = ExcelFilesFilter
            })
            {
                if (dialog.ShowDialog(form) != DialogResult.OK)
                    return;

                filePath = dialog.FileName;
            }

            Run(form, () =>
            {
                using (var manager = new ExchangeManager())
                {
                    manager.Export(filePath);
                }
            }, "Экспорт каталога");
        }

        private bool Run(IWin32Window form, Action action, string actionName)
        {
            var winApplication = (WinApplication) Application;

            winApplication.StartSplash(SplashType.Image);
            try
            {
                action();
                XtraMessageBox.Show(form, "Операция успешно завершена.", actionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (ExchangeManagerException exception)
            {
                XtraMessageBox.Show(form, exception.Message, actionName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(form, exception.Message, actionName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                winApplication.StartSplash(SplashType.Image);
            }
            return false;
        }
    }
}