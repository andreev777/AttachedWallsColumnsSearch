using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using AttachedWallsColumnsSearch.Views;
using System;
using System.Reflection;
using AttachedWallsColumnsSearch.ViewModels;

namespace AttachedWallsColumnsSearch.Commands
{
    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    public class StartCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                Assembly.LoadFrom(@"P:\03_БИБЛИОТЕКА\Revit_5_ПСМ\Скрипты C#\Библиотеки\AtomStyleLibrary\AtomStyleLibrary.dll");
            }
            catch
            {
                WarningWindow warningWindow = new WarningWindow("ОШИБКА", "Ошибка при загрузке стилей библиотеки");
                warningWindow.ShowDialog();

                return Result.Cancelled;
            }

            if (AttachedWallsColumnsSearchApp.IsOpened)
            {
                WarningWindow warningWindow = new WarningWindow("ОШИБКА", "Программа уже запущена");
                warningWindow.ShowDialog();

                return Result.Cancelled;
            }

            UIApplication uiapp = commandData.Application;
            Document doc = commandData.Application.ActiveUIDocument.Document;

            try
            {
                DataManageVM dataManageVM = new DataManageVM(doc);
                doc.DocumentClosing += dataManageVM.Close;

                if (dataManageVM.IsDataEmpty())
                {
                    WarningWindow warningWindow = new WarningWindow("ПРЕДУПРЕЖДЕНИЕ", "Стены и колонны с присоединенным верхом/основанием отсутствуют");
                    warningWindow.ShowDialog();

                    return Result.Succeeded;
                }

                AttachedWallsColumnsSearchApp.IsOpened = true;

                StartWindow startWindow = new StartWindow(uiapp, dataManageVM);
                startWindow.Show();
            }
            catch (Exception e)
            {
                ExceptionWindow exceptionWindow = new ExceptionWindow(e.Message, e.StackTrace);
                exceptionWindow.ShowDialog();

                return Result.Cancelled;
            }

            return Result.Succeeded;
        }
    }
}
