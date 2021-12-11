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
using System.Windows.Data;
using TimeManager.Model;
using TimeManager.Services;
using TimeManager.Tools;
using TimeManager.Views;

namespace TimeManager.ViewModels
{
    class MainContentViewModel : BindableBase
    {
        // Fields
        private IDialogService dialogService;
        private AppDbContext dbCtx;
        private Paginator<DayEntry> paginator;
        private ObservableCollection<PaginationLink> paginationLinks;
        private ObservableCollection<DayEntry> dayEntries;
        private DayEntry selectedDayEntry;
        private DelegateCommand cmdEdit;
        private DelegateCommand cmdAddDay;
        private DelegateCommand cmdDeleteDay;
        private DelegateCommand<int?> cmdOpenPage;

        public MainContentViewModel(IDialogService dialogService, AppDbContext dbCtx)
        {
            this.dbCtx = dbCtx;
            this.dbCtx.Database.EnsureCreated();
            this.dialogService = dialogService;
            paginator = new Paginator<DayEntry>(dbCtx, 7, 10, x => x.DayEntries);
            DayEntries.AddRange(paginator.Page(0));
            UpdatePagination();
        }

        // Properties
        public DayEntry NewDayEntry { get; private set; }
        public DayEntry SelectedDayEntry
        {
            get => selectedDayEntry;
            set => SetProperty(ref selectedDayEntry, value);
        }
        public ObservableCollection<PaginationLink> PaginationLinks
        {
            get => paginationLinks ??= new ObservableCollection<PaginationLink>();
            set => SetProperty(ref paginationLinks, value);
        }
        public ObservableCollection<DayEntry> DayEntries
        {
            get => dayEntries ??= new ObservableCollection<DayEntry>();
            set => SetProperty(ref dayEntries, value);
        }
        public DelegateCommand CmdEdit => cmdEdit ??= new DelegateCommand(Edit, 
            () => { return SelectedDayEntry != null; }).ObservesProperty(() => SelectedDayEntry);
        public DelegateCommand CmdAddDay => cmdAddDay ??= new DelegateCommand(AddDay);
        public DelegateCommand CmdDeleteDay => cmdDeleteDay ??= new DelegateCommand(DeleteDay, 
            () => SelectedDayEntry != null).ObservesProperty(() => SelectedDayEntry);
        public DelegateCommand<int?> CmdOpenPage => cmdOpenPage ??= new DelegateCommand<int?>(x => OpenPage(x.GetValueOrDefault()));

        // Methods
        void DeleteDay()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this?\n" +
                            $"{SelectedDayEntry.DateDayString}\n{SelectedDayEntry.DateString}\nEntries: {SelectedDayEntry.TimeEntries.Count}", $"Delete {SelectedDayEntry.DateDayString}?", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                dbCtx.Remove<DayEntry>(SelectedDayEntry);
                DayEntries.Remove(SelectedDayEntry);
                dbCtx.SaveChanges();
                DayEntries.Order();
                UpdateList();
            }
        }
        private void AddDay()
        {
            DateTime currentDate = DateTime.Now;
            DayEntry newDay = new DayEntry()
            {
                Date = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day),
                TimeEntries = new ObservableCollection<TimeEntry>()
            };

            DialogParameters parameters = new DialogParameters
            {
                { nameof(SelectedDayEntry), newDay },
                {"Title", "Add day" },
                {"IsEditing", false}
            };

            dialogService.ShowDialog(nameof(TimeEntries), parameters, dialog =>
            {
                if (dialog.Result == ButtonResult.OK)
                {
                    DayEntries.Add(newDay);
                    dbCtx.Add(newDay);
                    dbCtx.SaveChanges();
                    UpdateList();
                }
                else if (dialog.Result == ButtonResult.Abort)
                {
                    MessageBox.Show($"The date {newDay.Date} already exists.");
                }
            });
        }
        private void Edit()
        {
            DialogParameters parameters = new DialogParameters
            {
                { nameof(SelectedDayEntry), SelectedDayEntry },
                {"Title", "Edit day" },
                {"IsEditing", true }
            };

            dialogService.ShowDialog(nameof(TimeEntries), parameters, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    dbCtx.SaveChanges();
                    DayEntries.Order();
                }
                UpdateList();
            });
        }
        private void OpenPage(int page)
        {
            DayEntries.Clear();
            DayEntries.AddRange(paginator.Page(page));
            UpdatePagination();
        }

        void UpdatePagination()
        {
            paginationLinks ??= new ObservableCollection<PaginationLink>();
            paginationLinks.Clear();
            paginationLinks.AddRange(paginator.GetPaginationLinks());
        }

        void UpdateList()
        {
            DayEntries.Clear();
            DayEntries.AddRange(paginator.CurrentPage());
            UpdatePagination();
        }
    }

    public static class Extensions
    {
        public static ObservableCollection<DayEntry> Order(this ObservableCollection<DayEntry> days)
        {
            DayEntry day;
            int count = days.Count;
            if (count < 2) return days;

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    if (days[i].Date > days[j].Date) Switch(i, j);

                }
            }

            void Switch(int day1, int day2)
            {
                day = days[day2];
                days[day2] = days[day1];
                days[day1] = day;
            }

            return days;
        }
    }
}
