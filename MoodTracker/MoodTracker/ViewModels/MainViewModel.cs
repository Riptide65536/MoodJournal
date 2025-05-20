using System;
using System.Windows;
using System.Windows.Input;
using MoodTracker.Resources;
using System.Windows.Media;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MoodTracker.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private bool _isMenuOpen;
        private bool _isDarkTheme;

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
                if (SetProperty(ref _isDarkTheme, value))
                {
                    ThemeManager.ApplyTheme(_isDarkTheme);
                }
            }
        }

        public ICommand ToggleMenuCommand { get; }
        public ICommand MinimizeCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand ToggleThemeCommand { get; }
        //public ICommand CreateCommand => new RelayCommand<object>(CreateItem);


        public MainViewModel()
        {
            ToggleMenuCommand = new RelayCommand(ExecuteToggleMenu);
            MinimizeCommand = new RelayCommand(MinimizeWindow);
            CloseCommand = new RelayCommand(CloseWindow);
            ToggleThemeCommand = new RelayCommand(_ => IsDarkTheme = !IsDarkTheme);

            // 初始化主题状态
            InitializeTheme();

            SearchItems = new ObservableCollection<string>();
            SearchItems.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(IsSearchOpen));
            };

            // 初始化搜索结果
            InitializeSearchResults();
        }

        private void InitializeTheme()
        {
            var app = Application.Current;
            var dictionaries = app.Resources.MergedDictionaries;
            
            // 检查当前主题
            var currentTheme = dictionaries.FirstOrDefault(d =>
                d.Source != null && (d.Source.OriginalString.Contains("LightTheme.xaml") || d.Source.OriginalString.Contains("DarkTheme.xaml")));

            if (currentTheme != null)
            {
                // 根据当前主题设置IsDarkTheme
                _isDarkTheme = currentTheme.Source.OriginalString.Contains("DarkTheme.xaml");
                OnPropertyChanged(nameof(IsDarkTheme));
            }
        }

        private void ExecuteToggleMenu(object parameter)
        {
            IsMenuOpen = !IsMenuOpen;
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

        private void CreateItem(object obj)
        {
            //"创建"逻辑
            MessageBox.Show("你点击了创建按钮！");


        }

        //搜索框相关逻辑实现（5.20）

    public ObservableCollection<string> SearchItems { get; set; } = new();

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                    FilterSearchResults();
                }
            }
        }

        private bool _isSearchOpen;
        public bool IsSearchOpen
        {
            get => _isSearchOpen;
            set
            {
                _isSearchOpen = value;
                OnPropertyChanged();
            }
        }

        public void InitializeSearchResults()
        {
            var allItems = new[]
            {
                "🎵 周杰伦 - 夜曲",
                "😊 今日心情：放松",
                "📝 日记内容：今天听了很棒的歌"
            };

            Application.Current.Dispatcher.Invoke(() =>
            {
                SearchItems.Clear();
                foreach (var item in allItems)
                    SearchItems.Add(item);

                OnPropertyChanged(nameof(SearchItems));
                IsSearchOpen = true;
            });
        }

        private void FilterSearchResults()
        {
            var allItems = new[]
            {
                "🎵 周杰伦 - 夜曲",
                "😊 今日心情：放松",
                "📝 日记内容：今天听了很棒的歌"
            };

            Application.Current.Dispatcher.Invoke(() =>
            {
                var filtered = allItems
                    .Where(i => string.IsNullOrEmpty(SearchText) || i.ToLower().Contains(SearchText.ToLower()))
                    .ToList();

                SearchItems.Clear();
                foreach (var item in filtered)
                    SearchItems.Add(item);

                OnPropertyChanged(nameof(SearchItems));
                IsSearchOpen = true;
            });
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