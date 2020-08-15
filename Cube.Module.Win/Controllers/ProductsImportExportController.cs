using System;
using System.Windows.Forms;
using Cube.Model;
using Cube.Module.Win.Exchange;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Win;

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

            Run(() =>
            {
                using (var manager = new ExchangeManager())
                {
                    manager.Import(filePath);
                }
            });
        }

        private void ExportAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var winApplication = (WinApplication) Application;
            var form = ((WinWindow)winApplication.MainWindow).Form;
            var filePath = string.Empty;
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

            Run(() =>
            {
                using (var manager = new ExchangeManager())
                {
                    manager.Export(filePath);
                }
            });
        }

        private void Run(Action action)
        {
            var winApplication = (WinApplication) Application;

            winApplication.StartSplash(SplashType.Image);
            try
            {
                action();
            }
            finally
            {
                winApplication.StartSplash(SplashType.Image);
            }
        }
    }
}