using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.Model;
using Prism.Mvvm;

namespace TimeManager.ViewModels
{
    class DayEntriesVM : BindableBase
    {
        private List<DayEntry> dayEntries;
        public List<DayEntry> DayEntries
        {
            get => dayEntries;
            set => SetProperty(ref dayEntries, value);
        }
    }
}
