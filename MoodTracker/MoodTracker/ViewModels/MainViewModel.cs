using System;
using System.Windows;
using System.Windows.Input;
using MoodTracker.Resources;
using System.Windows.Media;

namespace MoodTracker.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private bool _isMenuOpen;
        private bool _isDarkTheme = false; // 默认使用主题

        public bool IsMenuOpen
        {
            get => _isMenuOpen;
            set => SetProperty(ref _isMenuOpen, value);
        }

        public bool IsDarkTheme
        {
            get => _isDarkTheme;
            set
            {
                if (_isDarkTheme != value)
                {
                    _isDarkTheme = value;
                    OnPropertyChanged();
                    ToggleTheme();
                }
            }
        }

        public ICommand ToggleMenuCommand { get; }
        public ICommand MinimizeCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand ToggleThemeCommand { get; }

        public MainViewModel()
        {
            ToggleMenuCommand = new RelayCommand(ExecuteToggleMenu);
            MinimizeCommand = new RelayCommand(MinimizeWindow);
            CloseCommand = new RelayCommand(CloseWindow);
            ToggleThemeCommand = new RelayCommand(_ => IsDarkTheme = !IsDarkTheme);

            // 初始化主题
            ToggleTheme();
        }

        private void ExecuteToggleMenu(object parameter)
        {
            IsMenuOpen = !IsMenuOpen;
        }

        private void ToggleTheme()
        {
            var app = Application.Current;
            var dictionaries = app.Resources.MergedDictionaries;

            // 查找旧主题字典
            var oldTheme = dictionaries.FirstOrDefault(d =>
                d.Source != null && (d.Source.OriginalString.Contains("LightTheme.xaml") || d.Source.OriginalString.Contains("DarkTheme.xaml")));

            if (oldTheme != null)
            {
                dictionaries.Remove(oldTheme);
            }

            var newTheme = new ResourceDictionary
            {
                Source = new Uri($"Resources/{(IsDarkTheme ? "Dark" : "Light")}Theme.xaml", UriKind.Relative)
                //Source = new Uri($"Resources/{(IsDarkTheme ? "Light" : "Dark")}Theme.xaml", UriKind.Relative)

            };
            dictionaries.Insert(0, newTheme);

        }

        private void MinimizeWindow(object parameter)
        {
            if (parameter is Window window)
            {
                window.WindowState = WindowState.Minimized;
            }
        }

        private void CloseWindow(object parameter)
        {
            if (parameter is Window window)
            {
                window.Close();
            }
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }
    }
} 