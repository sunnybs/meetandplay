using System;
using System.Collections.Generic;
using System.ComponentModel;
using MeetAndPlay.Data.Enums;

namespace MeetAndPlay.Web.ViewModels
{
    public class PeriodViewModel
    {
        public bool IsEveryday { get; set; }
        public bool IsDayOfWeek { get; set; }
        public bool HasActualTime { get; set; }
        public HashSet<string> Days { get; set; } = new HashSet<string>();
        public DateTime? HoursFrom { get; set; }
        public DateTime? HoursTo { get; set; }
        public DateTime? ActualTime { get; set; }
        
        
    }
}