using System.Windows;
using System.Linq;

namespace TodoFloat;
public partial class SettingsWindow : Window
{
    private readonly MainWindow _main;
    public SettingsWindow(MainWindow main) { InitializeComponent(); _main = main; OpacitySlider.ValueChanged += (_, _) => OpacityLabel.Text = $"{OpacitySlider.Value:P0} opaque"; TextSizeSlider.ValueChanged += (_, _) => TextSizeLabel.Text = $"{TextSizeSlider.Value:0} pt";
        // Initialize controls from current app state
        OpacitySlider.Value = _main.Opacity;
        AlwaysTop.IsChecked = _main.Topmost;
        // font size stored in resources
        if (Application.Current.Resources.Contains(SystemFonts.MessageFontSizeKey))
            TextSizeSlider.Value = (double)Application.Current.Resources[SystemFonts.MessageFontSizeKey];
        // determine current theme by merged dictionaries
        var merged = Application.Current.Resources.MergedDictionaries;
        DarkTheme.IsChecked = merged.Any(d => (d.Source?.OriginalString ?? string.Empty).EndsWith("Dark.xaml"));
        // update labels
        OpacityLabel.Text = $"{OpacitySlider.Value:P0} opaque";
        TextSizeLabel.Text = $"{TextSizeSlider.Value:0} pt";
    }
    private void Apply_Click(object sender, RoutedEventArgs e) { _main.ApplyAppearance(DarkTheme.IsChecked == true, OpacitySlider.Value, TextSizeSlider.Value, AlwaysTop.IsChecked == true); Close(); }
}
