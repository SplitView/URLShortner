namespace RedirectLog.Application.RedirectLog;

public class RedirectLogViewModel
{
    public RedirectLogViewModel()
    {
            TimeStamps = new();
        }

    public List<DateTime> TimeStamps { get; set; }
    public int Total { get => TimeStamps.Count; }
}