namespace RedirectLog.Application.RedirectLog;

public class RedirectLogViewModel
{
    public RedirectLogViewModel()
    {
        TimeStamps = new List<DateTime>();
    }

    public List<DateTime> TimeStamps { get; set; }
    public int Total => TimeStamps.Count;
}