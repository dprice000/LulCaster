using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LulCaster.UI.WPF.Controllers
{
  public class ControllerBase : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
  }
}