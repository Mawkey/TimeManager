using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TimeManager.Model;
using TimeManager.Services;

namespace TimeManager
{
    public partial class MainWindow : Window
    {
        private AppDbContext dbCtx;

        public MainWindow(AppDbContext dbCtx)
        {
            InitializeComponent();
            this.dbCtx = dbCtx;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            List<DayEntry> test = dbCtx.DayEntries.ToList();
            dbCtx.SaveChanges();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.MinHeight = this.Height;
            this.MinWidth = this.Width;
        }
    }
}
