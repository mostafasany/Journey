using System.Windows.Input;
using Prism.Mvvm;

namespace Models.SplitMenu
{
    public class SplitMenuItem : BindableBase
    {
        public string DisplayName { get; set; }

        public string FontIcon { get; set; }

        public ICommand Command { get; set; }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}