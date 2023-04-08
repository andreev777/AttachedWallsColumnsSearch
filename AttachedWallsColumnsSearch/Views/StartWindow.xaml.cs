using AttachedWallsColumnsSearch.Models;
using AttachedWallsColumnsSearch.ViewModels;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace AttachedWallsColumnsSearch.Views
{
    public partial class StartWindow : Window
    {
        private UIApplication _uiapp;

        public StartWindow(UIApplication uiapp, DataManageVM dataManageVM)
        {
            _uiapp = uiapp;
            
            InitializeComponent();
            DataContext = dataManageVM;

            if (dataManageVM.CloseAction == null)
            {
                dataManageVM.CloseAction = new Action(Close);
            }
        }

        private void rebarAssembliesDataGrid_UnselectClick(object o, MouseButtonEventArgs e)
        {
            if (e.OriginalSource != attachedElementsDataGrid)
            {
                attachedElementsDataGrid.UnselectAll();
            }
        }

        private void selectElementsInRevitButton_Click(object sender, RoutedEventArgs e)
        {
            var allSelectedElementsIds = new List<ElementId>();

            if (attachedElementsDataGrid.SelectedItems.Count > 0)
            {
                for (int i = 0; i < attachedElementsDataGrid.SelectedItems.Count; i++)
                {
                    AttachedElement selectedElement = attachedElementsDataGrid.SelectedItems[i] as AttachedElement;
                    allSelectedElementsIds.Add(new ElementId(selectedElement.Id));
                }
            }
            else
            {
                WarningWindow warningWindow = new WarningWindow("ПРЕДУПРЕЖДЕНИЕ", "Выберите элементы");
                warningWindow.ShowDialog();
                return;
            }

            _uiapp.ActiveUIDocument.Selection.SetElementIds(allSelectedElementsIds);

            WindowState = WindowState.Minimized;
        }

        #region МЕТОДЫ ПЕРЕТАСКИВАНИЯ И ЗАКРЫТИЯ ОКНА
        private void DragWithMouse(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (WindowState == WindowState.Maximized)
                {
                    Top = 0;
                    WindowState = WindowState.Normal;
                }

                DragMove();
            }
        }

        private void CommandBinding_CanExecute_1(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_Executed_1(object sender, ExecutedRoutedEventArgs e)
        {
            AttachedWallsColumnsSearchApp.IsOpened = false;
            SystemCommands.CloseWindow(this);
        }
        #endregion МЕТОДЫ ПЕРЕТАСКИВАНИЯ И ЗАКРЫТИЯ ОКНА
    }
}
