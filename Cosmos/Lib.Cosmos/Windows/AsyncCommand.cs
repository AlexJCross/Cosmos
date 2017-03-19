namespace Lib.Cosmos.Windows
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public abstract class AsyncCommandBase : ICommand
    {
        #region Fields

        private readonly Func<object, bool> canExecuteMethod;

        private readonly Func<object, Task> executeMethod;

        private bool isExecuting;

        #endregion

        protected AsyncCommandBase(Func<object, Task> executeMethod, Func<object, bool> canExecuteMethod)
        {
            if (executeMethod == null)
            {
                throw new ArgumentNullException(nameof(executeMethod));
            }

            if (canExecuteMethod == null)
            {
                throw new ArgumentNullException(nameof(canExecuteMethod));
            }

            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }

        async void ICommand.Execute(object parameter)
        {
            await this.ExecuteAsync(parameter);
        }

        bool ICommand.CanExecute(object parameter)
        {
            return this.CanExecute(parameter);
        }

        protected async Task ExecuteAsync(object parameter)
        {
            this.isExecuting = true;
            RaiseCanExecuteChanged();

            try
            {
                await this.executeMethod(parameter);
            }
            finally
            {
                this.isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        protected bool CanExecute(object parameter)
        {
            return !this.isExecuting && this.canExecuteMethod(parameter);
        }

        protected static void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public class AsyncCommand : AsyncCommandBase
    {
        public AsyncCommand(Func<Task> command)
            : this(command, () => true)
        {
        }

        public AsyncCommand(Func<Task> command, Func<bool> canExecuteMethod)
            : base(o => command(), o => canExecuteMethod())
        {
        }

        public bool CanExecute()
        {
            return base.CanExecute(null);
        }

        public Task ExecuteAsync()
        {
            return base.ExecuteAsync(null);
        }
    }

    public class AsyncCommand<T> : AsyncCommandBase
    {
        public AsyncCommand(Func<T, Task> command)
            : this(command, arg => true)
        {
        }

        public AsyncCommand(Func<T, Task> command, Func<T, bool> canExecuteMethod)
            : base(arg => command((T)arg), arg => canExecuteMethod((T)arg))
        {
        }

        public bool CanExecute()
        {
            return base.CanExecute(null);
        }

        public async Task ExecuteAsync()
        {
            await base.ExecuteAsync(null);
        }
    }
}
