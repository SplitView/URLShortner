using System;
using System.Collections.Generic;

namespace URLShortner.Application.Redirection
{
    public class RedirectionViewModel
    {
        public RedirectionViewModel()
        {
            TimeStamps = new();
        }

        public List<DateTime> TimeStamps { get; set; }
        public int Total { get => TimeStamps.Count; }
    }
}
