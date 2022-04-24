using System;
using System.Windows.Input;

namespace BitmapFontViewer
{
    /// <summary>
    /// プリズムのコードを参考に、デリゲートコマンドを作成。
    /// </summary>
    class DelegateCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public DelegateCommand(Action executeMethod)
        {
            ExecuteMethod = executeMethod;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            ExecuteMethod?.Invoke();
        }

        private Action ExecuteMethod { get; set; }
    }
}
