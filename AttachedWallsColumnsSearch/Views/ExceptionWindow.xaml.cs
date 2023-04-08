using System.Windows;
using System.Windows.Input;

namespace AttachedWallsColumnsSearch.Views
{
    public partial class ExceptionWindow : Window
    {
        /// <summary>
        /// Создает окно с текстом исключения.
        /// </summary>
        public ExceptionWindow(string exceptionMessage, string exceptionStackTrace)
        {
            InitializeComponent();
            exceptionMessageTextBlock.Text = exceptionMessage;
            exceptionStrackTraceTextBox.Text = exceptionStackTrace;
        }

        #region МЕТОДЫ ПЕРЕТАСКИВАНИЯ И ЗАКРЫТИЯ ОКНА

        private void DragWithMouse(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void CommandBinding_CanExecute_1(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_Executed_1(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        #endregion МЕТОДЫ ПЕРЕТАСКИВАНИЯ И ЗАКРЫТИЯ ОКНА
    }
}
