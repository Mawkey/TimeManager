using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TimeManager.Model;
using TimeManager.Services;

namespace TimeManager.ViewModels
{
    class TimeEntriesViewModel : BindableBase, IDialogAware
    {
        private AppDbContext dbCtx;
        private string windowTitle;

        private DateTime originalDate;
        private bool isEditing;

        public TimeEntriesViewModel(AppDbContext dbCtx)
        {
            this.dbCtx = dbCtx;
            windowTitle = "";
        }
        public string Title => $"Time Entries - {windowTitle}";

        private DayEntry selectedDayEntry;
        public DayEntry SelectedDayEntry
        {
            get => selectedDayEntry;
            set => SetProperty(ref selectedDayEntry, value);
        }
        private DayEntry dbSelectedDayEntry;
        public ObservableCollection<TimeEntry> TimeEntries { get; set; } = new ObservableCollection<TimeEntry>();

        private TimeEntry selectedTimeEntry;
        public TimeEntry SelectedTimeEntry
        {
            get => selectedTimeEntry;
            set => SetProperty(ref selectedTimeEntry, value);
        }
        private DelegateCommand cmdDeleteTime;
        public DelegateCommand CmdDeleteTime => cmdDeleteTime ??= new DelegateCommand(DeleteTime, () => SelectedTimeEntry != null).ObservesProperty(() => SelectedTimeEntry);

        private void DeleteTime()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this?", $"Delete?", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                dbCtx.Remove<TimeEntry>(SelectedTimeEntry);
                TimeEntries.Remove(SelectedTimeEntry);
            }
        }

        private DelegateCommand cmdAddTime;
        public DelegateCommand CmdAddTime => cmdAddTime ??= new DelegateCommand(AddTime);
        void AddTime()
        {
            TimeEntry newTimeEntry = new TimeEntry();
            TimeEntries.Add(newTimeEntry);
            //dbSelectedDayEntry.TimeEntries.Add(newTimeEntry);

        }

        private DelegateCommand cmdSaveAndClose;
        public DelegateCommand CmdSaveAndClose => cmdSaveAndClose ??= new DelegateCommand(SaveAndClose);
        private void SaveAndClose()
        {
            bool dateExist = dbCtx.DayEntries.Any(x => x.Date == SelectedDayEntry.Date);
            bool hasTimeEntriesToMerge = SelectedDayEntry.TimeEntries.Count > 0;
            bool dateChanged = SelectedDayEntry.Date != originalDate;
            if (dateExist && isEditing && dateChanged)
            {
                MessageBox.Show($"The date {SelectedDayEntry.Date.ToShortDateString()} already exist. Entry was NOT saved. " +
                    $"\nMerging might be possible later on...");
            }
            else if (dateExist && !isEditing)
            {
                MessageBox.Show($"The date {SelectedDayEntry.Date.ToShortDateString()} already exist. Entry was NOT saved.");
            }
            else
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
                dbCtx.SaveChanges();
            }
        }

        void Merge()
        {
            MessageBoxResult mergeResult = MessageBox.Show(
            $"The date {SelectedDayEntry.Date} already exists, would you like to merge?",
            "Merge dates?", MessageBoxButton.YesNo);

            if (mergeResult == MessageBoxResult.Yes)
            {
                DayEntry original = dbCtx.DayEntries.First(x => x.Date == SelectedDayEntry.Date);
                original.TimeEntries.AddRange(SelectedDayEntry.TimeEntries);
                dbCtx.DayEntries.Remove(SelectedDayEntry);
                //DayEntries.Remove(SelectedDayEntry);
            }
        }

        public event Action<IDialogResult> RequestClose;

        // Todo add OK button so you can close down the dialog without pressing X

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {

        }
        public void OnDialogOpened(IDialogParameters parameters)
        {
            SelectedDayEntry = parameters.GetValue<DayEntry>("SelectedDayEntry");
            TimeEntries = SelectedDayEntry.TimeEntries;
            windowTitle = parameters.GetValue<string>("Title");
            isEditing = parameters.GetValue<bool>("IsEditing");
            originalDate = new DateTime(SelectedDayEntry.Date.Year, SelectedDayEntry.Date.Month, SelectedDayEntry.Date.Day);
        }
    }
}
