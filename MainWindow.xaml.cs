using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TodoFloat;

public partial class MainWindow : Window
{
    public ObservableCollection<TodoItem> Todos { get; } = new();
    private bool _isDark;

    public MainWindow()
    {
        InitializeComponent(); DataContext = this;
        Todos.CollectionChanged += (_, _) => UpdateSummary();
        Todos.Add(new TodoItem { Title = "Welcome — add your first task", Category = "General", Status = "In progress", DueDate = DateTime.Today });
        UpdateSummary();
    }
    private void Add_Click(object sender, RoutedEventArgs e) => AddTask();
    private void NewTaskBox_KeyDown(object sender, KeyEventArgs e) { if (e.Key == Key.Enter) AddTask(); }
    private void AddTask()
    {
        var title = NewTaskBox.Text.Trim(); if (title.Length == 0) return;
        Todos.Add(new TodoItem { Title = title, Category = (NewCategory.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "General", DueDate = NewDueDate.SelectedDate });
        NewTaskBox.Clear(); NewTaskBox.Focus(); UpdateSummary();
    }
    private void Delete_Click(object sender, RoutedEventArgs e) { if ((sender as Button)?.Tag is TodoItem task) Todos.Remove(task); UpdateSummary(); }
    private void ClearCompleted_Click(object sender, RoutedEventArgs e) { foreach (var task in Todos.Where(t => t.IsComplete).ToList()) Todos.Remove(task); }
    private void Calendar_SelectedDatesChanged(object? sender, SelectionChangedEventArgs e) { if (MonthCalendar.SelectedDate is { } date) NewDueDate.SelectedDate = date; }
    private void HeaderDrag(object sender, MouseButtonEventArgs e)
    {
        // Let the title area drag the borderless window, but never intercept header buttons.
        if (e.OriginalSource is DependencyObject source && FindAncestor<Button>(source) is not null) return;
        if (e.LeftButton == MouseButtonState.Pressed) DragMove();
    }

    private static T? FindAncestor<T>(DependencyObject current) where T : DependencyObject
    {
        while (current is not null)
        {
            if (current is T found) return found;
            current = VisualTreeHelper.GetParent(current);
        }
        return null;
    }
    private void Minimize_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
    private void Close_Click(object sender, RoutedEventArgs e) => Close();
    private void Settings_Click(object sender, RoutedEventArgs e)
    {
        var popup = new SettingsWindow(this) { Owner = this }; popup.ShowDialog();
    }
    public void ApplyAppearance(bool dark, double opacity, double textSize, bool alwaysOnTop)
    {
        _isDark = dark;
        Opacity = opacity;
        Topmost = alwaysOnTop;

        // Swap theme resource dictionary
        var merged = Application.Current.Resources.MergedDictionaries;
        for (int i = merged.Count - 1; i >= 0; i--)
        {
            var src = merged[i].Source?.OriginalString ?? string.Empty;
            if (src.Contains("Themes/")) merged.RemoveAt(i);
        }
        var theme = new ResourceDictionary { Source = new Uri($"Themes/{(dark ? "Dark" : "Light")}.xaml", UriKind.Relative) };
        merged.Add(theme);

        Resources[SystemFonts.MessageFontSizeKey] = textSize;
    }
    private void UpdateSummary() { var done = Todos.Count(t => t.IsComplete); SummaryText.Text = $"{Todos.Count} task{(Todos.Count == 1 ? "" : "s")}  •  {done} completed"; }
}
