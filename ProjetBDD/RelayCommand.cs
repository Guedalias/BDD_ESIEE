using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjetBDD
{
	public class RelayCommand<T> : ICommand
	{
		Action<T> _cmd;

		public RelayCommand(Action<T> cmd)
		{
			_cmd = cmd;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public event EventHandler CanExecuteChanged;

		public void Execute(object parameter)
		{
			_cmd((T)parameter);
		}
	}
}