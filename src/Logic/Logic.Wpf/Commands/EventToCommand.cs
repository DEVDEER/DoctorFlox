namespace devdeer.DoctorFlox.Logic.Wpf.Commands
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;

    using Interfaces;

    /// <summary>
    /// This <see cref="T:System.Windows.Interactivity.TriggerAction`1" /> can be
    /// used to bind any event on any FrameworkElement to an <see cref="ICommand" />.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Be sure to reference System.Windows.Interactivity in the UI assembly.
    /// </para>
    /// <para>
    /// Typically, this element is used in XAML to connect the attached element
    /// to a command located in a ViewModel. This trigger can only be attached
    /// to a FrameworkElement or a class deriving from FrameworkElement.
    /// </para>
    /// <para>
    /// To access the EventArgs of the fired event, use a <see cref="RelayCommand{T}" />
    /// and leave the CommandParameter and CommandParameterValue empty!
    /// </para>
    /// </remarks>
    public class EventToCommand : TriggerAction<DependencyObject>
    {
        #region member vars

        private object _commandParameterValue;

        private bool? _mustToggleValue;

        #endregion

        #region constants

        /// <summary>
        /// The <see cref="EventArgsConverterParameter" /> dependency property's name.
        /// </summary>
        public const string EventArgsConverterParameterPropertyName = "EventArgsConverterParameter";

        /// <summary>
        /// Identifies the <see cref="CommandParameter" /> dependency property
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter",
            typeof(object),
            typeof(EventToCommand),
            new PropertyMetadata(
                null,
                (s, e) =>
                {
                    var sender = s as EventToCommand;
                    if (sender == null)
                    {
                        return;
                    }
                    if (sender.AssociatedObject == null)
                    {
                        return;
                    }
                    sender.EnableDisableElement();
                }));

        /// <summary>
        /// Identifies the <see cref="Command" /> dependency property
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(EventToCommand),
            new PropertyMetadata(null, (s, e) => OnCommandChanged(s as EventToCommand, e)));

        /// <summary>
        /// Identifies the <see cref="MustToggleIsEnabled" /> dependency property
        /// </summary>
        public static readonly DependencyProperty MustToggleIsEnabledProperty = DependencyProperty.Register(
            "MustToggleIsEnabled",
            typeof(bool),
            typeof(EventToCommand),
            new PropertyMetadata(
                false,
                (s, e) =>
                {
                    var sender = s as EventToCommand;
                    if (sender == null)
                    {
                        return;
                    }
                    if (sender.AssociatedObject == null)
                    {
                        return;
                    }
                    sender.EnableDisableElement();
                }));

        /// <summary>
        /// Identifies the <see cref="EventArgsConverterParameter" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty EventArgsConverterParameterProperty = DependencyProperty.Register(
            EventArgsConverterParameterPropertyName,
            typeof(object),
            typeof(EventToCommand),
            new PropertyMetadata(null));

        #endregion

        #region methods

        /// <summary>
        /// Provides a simple way to invoke this trigger programatically without any EventArgs.
        /// </summary>
        public void Invoke()
        {
            Invoke(null);
        }

        /// <summary>
        /// Executes the trigger.
        /// <para>
        /// To access the EventArgs of the fired event, use a <see cref="RelayCommand{EventArgs}" />
        /// and leave the CommandParameter and CommandParameterValue empty!
        /// </para>
        /// </summary>
        /// <param name="parameter">The EventArgs of the fired event.</param>
        protected override void Invoke(object parameter)
        {
            if (AssociatedElementIsDisabled())
            {
                return;
            }
            var command = GetCommand();
            var commandParameter = CommandParameterValue;
            if (commandParameter == null && PassEventArgsToCommand)
            {
                commandParameter = EventArgsConverter == null ? parameter : EventArgsConverter.Convert(parameter, EventArgsConverterParameter);
            }
            if (command != null && command.CanExecute(commandParameter))
            {
                command.Execute(commandParameter);
            }
        }

        /// <summary>
        /// Called when this trigger is attached to a FrameworkElement.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            EnableDisableElement();
        }

        /// <summary>
        /// Retrieves if the associated element is disabled currently.
        /// </summary>
        /// <returns><c>true</c> if the element is disabled otherwise <c>false</c>.</returns>
        private bool AssociatedElementIsDisabled()
        {
            var element = GetAssociatedObject();
            return AssociatedObject == null || element != null && !element.IsEnabled;
        }

        /// <summary>
        /// Sets the elements IsEnabled property according to the CanExecute state of the associated command.
        /// </summary>
        private void EnableDisableElement()
        {
            var element = GetAssociatedObject();
            if (element == null)
            {
                return;
            }
            var command = GetCommand();
            if (MustToggleIsEnabledValue && command != null)
            {
                element.IsEnabled = command.CanExecute(CommandParameterValue);
            }
        }

        /// <summary>
        /// This method is here for compatibility with the Silverlight version.
        /// </summary>
        /// <returns>
        /// The FrameworkElement to which this trigger is attached.
        /// </returns>
        private FrameworkElement GetAssociatedObject()
        {
            return AssociatedObject as FrameworkElement;
        }

        /// <summary>
        /// This method is here for compatibility with the Silverlight 3 version.
        /// </summary>
        /// <returns>
        /// The command that must be executed when this trigger is invoked.
        /// </returns>
        private ICommand GetCommand()
        {
            return Command;
        }

        /// <summary>
        /// Is called whenever the CanExecute property of the command has changed it's value.
        /// </summary>
        /// <param name="sender">The command.</param>
        /// <param name="e">The event arguments.</param>
        private void OnCommandCanExecuteChanged(object sender, EventArgs e)
        {
            EnableDisableElement();
        }

        /// <summary>
        /// Is called whenever the associated command gets a new command binding.
        /// </summary>
        /// <param name="element">The command binding containing the source event.</param>
        /// <param name="e">The informations for the associated element.</param>
        private static void OnCommandChanged(EventToCommand element, DependencyPropertyChangedEventArgs e)
        {
            if (element == null)
            {
                return;
            }
            if (e.OldValue != null)
            {
                ((ICommand)e.OldValue).CanExecuteChanged -= element.OnCommandCanExecuteChanged;
            }
            var command = (ICommand)e.NewValue;
            if (command != null)
            {
                command.CanExecuteChanged += element.OnCommandCanExecuteChanged;
            }
            element.EnableDisableElement();
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the ICommand that this trigger is bound to. This
        /// is a DependencyProperty.
        /// </summary>
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);

            set => SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Gets or sets an object that will be passed to the <see cref="Command" />
        /// attached to this trigger. This is a DependencyProperty.
        /// </summary>
        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        /// <summary>
        /// Gets or sets an object that will be passed to the <see cref="Command" />
        /// attached to this trigger. This property is here for compatibility
        /// with the Silverlight version. This is NOT a DependencyProperty.
        /// For databinding, use the <see cref="CommandParameter" /> property.
        /// </summary>
        public object CommandParameterValue
        {
            get => _commandParameterValue ?? CommandParameter;
            set
            {
                _commandParameterValue = value;
                EnableDisableElement();
            }
        }

        /// <summary>
        /// Gets or sets a converter used to convert the EventArgs when using
        /// <see cref="PassEventArgsToCommand" />. If PassEventArgsToCommand is false,
        /// this property is never used.
        /// </summary>
        public IEventArgsConverter EventArgsConverter { get; set; }

        /// <summary>
        /// Gets or sets a parameters for the converter used to convert the EventArgs when using
        /// <see cref="PassEventArgsToCommand" />. If PassEventArgsToCommand is false,
        /// this property is never used. This is a dependency property.
        /// </summary>
        public object EventArgsConverterParameter
        {
            get => GetValue(EventArgsConverterParameterProperty);
            set => SetValue(EventArgsConverterParameterProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the attached element must be
        /// disabled when the <see cref="Command" /> property's CanExecuteChanged
        /// event fires. If this property is true, and the command's CanExecute
        /// method returns false, the element will be disabled. If this property
        /// is false, the element will not be disabled when the command's
        /// CanExecute method changes. This is a DependencyProperty.
        /// </summary>
        public bool MustToggleIsEnabled
        {
            get => (bool)GetValue(MustToggleIsEnabledProperty);
            set => SetValue(MustToggleIsEnabledProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the attached element must be
        /// disabled when the <see cref="Command" /> property's CanExecuteChanged
        /// event fires. If this property is true, and the command's CanExecute
        /// method returns false, the element will be disabled. This property is here for
        /// compatibility with the Silverlight version. This is NOT a DependencyProperty.
        /// For databinding, use the <see cref="MustToggleIsEnabled" /> property.
        /// </summary>
        public bool MustToggleIsEnabledValue
        {
            get => _mustToggleValue ?? MustToggleIsEnabled;
            set
            {
                _mustToggleValue = value;
                EnableDisableElement();
            }
        }

        /// <summary>
        /// Specifies whether the EventArgs of the event that triggered this
        /// action should be passed to the bound RelayCommand. If this is true,
        /// the command should accept arguments of the corresponding
        /// type (for example <see cref="RelayCommand{MouseButtonEventArgs}" />).
        /// </summary>
        public bool PassEventArgsToCommand { get; set; }

        #endregion
    }
}