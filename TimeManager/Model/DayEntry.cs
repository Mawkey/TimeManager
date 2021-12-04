using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.ViewModels;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace TimeManager.Model
{
    public class DayEntry : BindableBase
    {
        private int id;
        public int Id
        {
            get => id;
            set => id = value;
        }

        private ObservableCollection<TimeEntry> timeEntries;
        public virtual ObservableCollection<TimeEntry> TimeEntries
        {
            get => timeEntries ??= new ObservableCollection<TimeEntry>();
            set => SetProperty(ref timeEntries, value);
        }

        private DateTime date;
        public DateTime Date
        {
            get => date;
            set => SetProperty(ref date, value);
        }

        public string DateString
        {
            get => date.ToString("yyyy-MM-dd");
        }

        public string DateDayString
        {
            get => date.ToString("dddd");
        }
    }
}
