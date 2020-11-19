using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skaner
{
    class Code : INotifyPropertyChanged
    {
        private String codeString;
        private DateTime date;
        public String CodeString
        {
            get
            {
                return codeString;
            }
            set
            {
                this.codeString = value;
                this.OnPropertyChanged("CodeString");
            }
        }
        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                this.date = value;
                this.OnPropertyChanged("Date");
            }
        }

        protected void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
