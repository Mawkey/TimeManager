using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManager.Model
{
    internal class PaginationLink : BindableBase
    {
        private string text;
        private int page;

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public int Page
        {
            get => page;
            set => SetProperty(ref page, value);
        }

    }
}
