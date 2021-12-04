using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.ViewModels;
using Prism.Mvvm;

namespace TimeManager.Model
{
    public class TimeEntry : BindableBase
    {
        private int id;
        public int Id
        {
            get => id;
            set => id = value;
        }

        private string code;
        public string Code
        {
            get => code;
            set => SetProperty(ref code, value);
        }

        private string comments;
        public string Comments
        {
            get => comments;
            set => SetProperty(ref comments, value);
        }

        int inputStartHour;
        public int InputStartHour
        {
            get => inputStartHour;
            set
            {
                SetProperty(ref inputStartHour, value);
                UpdateTime();
            }
        }

        int inputStartMinute;
        public int InputStartMinute
        {
            get => inputStartMinute;
            set
            {
                SetProperty(ref inputStartMinute, value);
                UpdateTime();
            }
        }

        int inputEndHour;
        public int InputEndHour
        {
            get => inputEndHour;
            set
            {
                SetProperty(ref inputEndHour, value);
                UpdateTime();
            }
        }

        int inputEndMinute;
        public int InputEndMinute
        {
            get => inputEndMinute;
            set
            {
                SetProperty(ref inputEndMinute, value);
                UpdateTime();
            }
        }

        void UpdateTime()
        {
            DateTime fromTime = new DateTime(1994, 03, 04, InputStartHour, InputStartMinute, 0);
            DateTime toTime = new DateTime(1994, 03, 04, InputEndHour, InputEndMinute, 0);
            if (fromTime > toTime) toTime = toTime.AddDays(1);

            StartTime = fromTime;
            EndTime = toTime;
        }

        void CalculateTimeSpent()
        {
            Time = EndTime - StartTime;
        }

        DateTime startTime;
        public DateTime StartTime 
        {
            get => startTime;
            set 
            { 
                SetProperty(ref startTime, value);
                CalculateTimeSpent();
            }
        }

        DateTime endTime;
        public DateTime EndTime
        {
            get => endTime;
            set 
            { 
                SetProperty(ref endTime, value);
                CalculateTimeSpent();
            }
        }

        private TimeSpan time;
        public TimeSpan Time
        {
            get => time;
            set => SetProperty(ref time, value);
        }

        public DayEntry DayEntry { get; set; }
    }
}
