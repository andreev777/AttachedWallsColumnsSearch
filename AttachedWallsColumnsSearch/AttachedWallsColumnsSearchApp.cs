using Autodesk.Revit.UI;
using AttachedWallsColumnsSearch.Commands;
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace AttachedWallsColumnsSearch
{
    public class AttachedWallsColumnsSearchApp : IExternalApplication
    {
        public static bool IsOpened = false;

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            string tabName = "Надстройки АСК";
            string panelName = "Проверки АР/КЖ";
            string assemblyName = Assembly.GetExecutingAssembly().Location;
            string commandName = typeof(StartCommand).FullName;

            string toolTip = "Получение стен и колонн с присоединенным верхом или основанием";

            try
            {
                application.CreateRibbonTab(tabName);
            }
            catch { }

            var ribbonPanels = application.GetRibbonPanels(tabName);
            var ribbonPanel = ribbonPanels.FirstOrDefault(panel => panel.Name == panelName) ?? application.CreateRibbonPanel(tabName, panelName);

            PushButtonData startCommandButtonData = new PushButtonData("StartCommand", "Присоединенные\nстены и колонны", assemblyName, commandName);

            PushButton startCommandButton = ribbonPanel.AddItem(startCommandButtonData) as PushButton;
            startCommandButton.ToolTip = toolTip;

            BitmapImage startCommandButtonLogo = new BitmapImage(new Uri("pack://application:,,,/AttachedWallsColumnsSearch;component/Images/attachedElementsImage.png"));
            startCommandButton.LargeImage = startCommandButtonLogo;

            return Result.Succeeded;
        }
    }
}
