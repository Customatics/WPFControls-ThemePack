using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace ThemePack.Common.Assets
{
    /// <summary>
    /// Executes a specified <see cref="ICommand"/> when invoked passing arguments.
    /// </summary>
    public class InvokeInteractiveCommandAction : TriggerAction<DependencyObject>
    {
        /// <summary>
        /// <see cref="DependencyProperty"/> for <see cref="Command"/>.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(InvokeInteractiveCommandAction), null);

        /// <summary>
        /// <see cref="DependencyProperty"/> for <see cref="PassArguments"/>.
        /// </summary>
        public static readonly DependencyProperty PassArgumentsProperty = DependencyProperty.Register("PassArguments", typeof(bool), typeof(InvokeInteractiveCommandAction), null);

        /// <summary>
        /// Field for <see cref="CommandName"/>.
        /// </summary>
        private string commandName;

        /// <summary>
        /// Gets or sets the name of the command this action should invoke.
        /// </summary>
        /// <value>The name of the command this action should invoke.</value>
        /// <remarks>This property will be superseded by the <see cref="Command"/> property if both are set.</remarks>
        public string CommandName
        {
            get
            {
                ReadPreamble();
                return commandName;
            }
            set
            {
                if (CommandName == value)
                {
                    return;
                }
                WritePreamble();
                commandName = value;
                WritePostscript();
            }
        }

        /// <summary>
        /// Gets or sets the command this action should invoke. This is a dependency property.
        /// </summary>
        /// <value>The command to execute.</value>
        /// <remarks>This property will take precedence over the <see cref="CommandName"/> property if both are set.</remarks>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the mark to pass parameters to <see cref="Command"/>. This is a dependency property.
        /// </summary>
        /// <value>Flag if passing arguments are needed.</value>
        /// <remarks>This is the value passed to <see cref="ICommand"/>'s <see cref="ICommand.CanExecute"/> and ICommand.<see cref="ICommand.Execute"/>.</remarks>
        public bool PassArguments
        {
            get { return (bool)GetValue(PassArgumentsProperty); }
            set { SetValue(PassArgumentsProperty, value); }
        }

        /// <summary>
        /// Invokes the action.
        /// </summary>
        /// <param name="parameter">The parameter to the action. If the action does not require a parameter, the parameter may be set to a null reference.</param>
        protected override void Invoke(object parameter)
        {
            if (AssociatedObject == null)
            {
                return;
            }

            var command = ResolveCommand();
            var commandParameter = PassArguments
                ? parameter
                : null;

            if (command?.CanExecute(commandParameter) == true)
            {
                command.Execute(commandParameter);
            }
        }

        /// <summary>
        /// Resolve <see cref="ICommand"/> to invoke.
        /// </summary>
        /// <returns><see cref="ICommand"/> to invoke.</returns>
        private ICommand ResolveCommand()
        {
            ICommand command = null;
            if (Command != null)
            {
                command = Command;
            }
            else if (AssociatedObject != null)
            {
                foreach (var property in AssociatedObject.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    if (typeof(ICommand).IsAssignableFrom(property.PropertyType) &&
                        string.Equals(property.Name, CommandName, StringComparison.Ordinal))
                    {
                        command = (ICommand)property.GetValue(AssociatedObject, null);
                    }
                }
            }
            return command;
        }

        #region Overrides of TriggerAction

        /// <summary>
        /// Called after the action is attached to an AssociatedObject.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            AttachToAssociatedObject();
        }

        /// <summary>
        /// Called when the action is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            CleanCommand();
            DetachFromAssociatedProject();
        }

        #endregion

        #region AssociatedObject Events Processing

        /// <summary>
        /// Backup <see cref="ICommand"/> field.
        /// </summary>
        private ICommand commandBackup;

        /// <summary>
        /// Attach to <see cref="TriggerAction{T}.AssociatedObject"/> if it is <see cref="FrameworkElement"/>.
        /// </summary>
        private void AttachToAssociatedObject()
        {
            var associated = AssociatedObject as FrameworkElement;
            if (associated == null)
            {
                return;
            }

            associated.Loaded += OnAssociatedObjectLoaded;
            associated.Unloaded += OnAssociatedObjectUnloaded;
        }

        /// <summary>
        /// Dettach from <see cref="TriggerAction{T}.AssociatedObject"/> if it is <see cref="FrameworkElement"/>.
        /// </summary>
        private void DetachFromAssociatedProject()
        {
            var associated = AssociatedObject as FrameworkElement;
            if (associated == null)
            {
                return;
            }

            associated.Loaded -= OnAssociatedObjectLoaded;
            associated.Unloaded -= OnAssociatedObjectUnloaded;
        }

        /// <summary>
        /// Process <see cref="TriggerAction{T}.AssociatedObject"/>'s <see cref="FrameworkElement.Loaded"/> event.
        /// </summary>
        /// <param name="sender">event sender.</param>
        /// <param name="e">event arguments.</param>
        protected virtual void OnAssociatedObjectLoaded(object sender, RoutedEventArgs e)
        {
            RestoreCommand();
            commandBackup = null;
        }

        /// <summary>
        /// Process <see cref="TriggerAction{T}.AssociatedObject"/>'s <see cref="FrameworkElement.Unloaded"/> event.
        /// </summary>
        /// <param name="sender">event sender.</param>
        /// <param name="e">event arguments.</param>
        protected virtual void OnAssociatedObjectUnloaded(object sender, RoutedEventArgs e)
        {
            if (Command != null)
            {
                //BackupCommand();
            }
            else
            {
                commandBackup = null;
            }
        }

        /// <summary>
        /// Restore <see cref="Command"/>.
        /// </summary>
        private void RestoreCommand()
        {
            if ((Command == null) && (commandBackup != null))
            {
                Command = commandBackup; //Restore the Command
            }
        }

        /// <summary>
        /// Backup <see cref="Command"/>.
        /// </summary>
        private void BackupCommand()
        {
            commandBackup = Command;
            Command = null;             //We kill the Binding with this.
        }

        /// <summary>
        /// Clean <see cref="Command"/>.
        /// </summary>
        private void CleanCommand()
        {
            Command = null;             //unhook from CanExecuteChanged
            commandBackup = null;
        }

        #endregion
    }
}