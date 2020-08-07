using LulCaster.UI.WPF.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace LulCaster.UI.WPF.Converter
{
  internal class ToIListItem : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return ((IList<IListItem>)value) ?? null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return value;
    }
  }
}