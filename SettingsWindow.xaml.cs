using System.Windows;

namespace TodoFloat;
public partial class SettingsWindow : Window
{
    private readonly MainWindow _main;
    public SettingsWindow(MainWindow main) { InitializeComponent(); _main = main; OpacitySlider.ValueChanged += (_, _) => OpacityLabel.Text = $"{OpacitySlider.Value:P0} opaque"; TextSizeSlider.ValueChanged += (_, _) => TextSizeLabel.Text = $"{TextSizeSlider.Value:0} pt"; }
    private void Apply_Click(object sender, RoutedEventArgs e) { _main.ApplyAppearance(DarkTheme.IsChecked == true, OpacitySlider.Value, TextSizeSlider.Value, AlwaysTop.IsChecked == true); Close(); }
}
