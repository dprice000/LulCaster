using System;
using System.Globalization;
using System.Windows.Data;

namespace LulCaster.UI.WPF.Converter
{
  public class EnumToStringConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null)
        return null;

      return Enum.GetNames(value.GetType());
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null)
        return null;

      return Enum.Parse(targetType, value.ToString());
    }
  }
}