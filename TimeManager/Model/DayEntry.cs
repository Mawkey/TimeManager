using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.ViewModels;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using TimeManager.Tools.Interfaces;

namespace TimeManager.Model
{
    public class DayEntry : BindableBase, IComparable<DayEntry>, IEquatable<DayEntry>, IHasDate
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



        // Interface implementations

        public int CompareTo(DayEntry other)
        {
            return other == null ? 1 : Date.CompareTo(other.Date);
        }

        public bool Equals(DayEntry other)
        {
            return Date.Equals(other.date);
        }

        public static bool operator >(DayEntry dayEntry1, DayEntry dayEntry2)
        {
            return dayEntry1.CompareTo(dayEntry2) > 0;
        }

        public static bool operator <(DayEntry dayEntry1, DayEntry dayEntry2)
        {
            return dayEntry1.CompareTo(dayEntry2) < 0;
        }

        public static bool operator >=(DayEntry dayEntry1, DayEntry dayEntry2)
        {
            return dayEntry1.CompareTo(dayEntry2) >= 0;
        }

        public static bool operator <=(DayEntry dayEntry1, DayEntry dayEntry2)
        {
            return dayEntry1.CompareTo(dayEntry2) <= 0;
        }
    }
}
