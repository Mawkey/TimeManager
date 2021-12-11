using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManager.Tools.Interfaces
{
    internal interface IHasPrimaryKey
    {
        public int Id { get; set; }
    }
}
