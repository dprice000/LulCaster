namespace LulCaster.UI.WPF.Config
{
  public interface IConfigManagerService
  {
    string this[int index] { get; }
    string this[string name] { get; }

    int GetAsInteger(string name);
  }
}