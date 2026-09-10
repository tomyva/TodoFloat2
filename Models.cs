using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TodoFloat;

public sealed class TodoItem : INotifyPropertyChanged
{
    private string _title = "";
    private string _category = "General";
    private string _status = "Not started";
    private bool _isComplete;
    private DateTime? _dueDate;

    public string Title { get => _title; set => Set(ref _title, value); }
    public string Category { get => _category; set => Set(ref _category, value); }
    public string Status { get => _status; set => Set(ref _status, value); }
    public bool IsComplete { get => _isComplete; set { if (Set(ref _isComplete, value)) Status = value ? "Completed" : "Not started"; } }
    public DateTime? DueDate { get => _dueDate; set => Set(ref _dueDate, value); }

    public event PropertyChangedEventHandler? PropertyChanged;
    private bool Set<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        return true;
    }
}
