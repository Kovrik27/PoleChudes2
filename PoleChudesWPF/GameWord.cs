using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoleChudesWPF
{
    public class GameWord
    {
        public string ID { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
        public string P3 { get; set; }
        public string P4 { get; set; }
        public int Turn { get; set; }
        public string Question { get; set; }
        public List<WordChar> Word { get; set; }
    }
    public class WordChar : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Char {  get; set; }
        private bool isOpened = false;
        public bool IsOpened
        {
            get => isOpened;
            set
            {
                isOpened = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsOpened)));
            }
        }
    }
}
