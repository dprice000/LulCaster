using LulCaster.UI.WPF.Controls.EventArgs;
using LulCaster.UI.WPF.ViewModels;
using LulCaster.Utility.Common.Logic;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LulCaster.UI.WPF.Controls
{
  public partial class RegionConfiguration : UserControl
  {
    private const string ITEM_DESCRIPTOR = "Trigger"; 

    //HACK: This being static will cause issues if there are more than one instance of this control
    public static event EventHandler<RegionViewModel> SaveConfigTriggered;

    public event EventHandler<ButtonClickArgs> AddTriggerClicked;

    public event EventHandler<ButtonClickArgs> DeleteTriggerClicked;

    #region "Dependency Properties"

    public static readonly DependencyProperty SelectedRegionProperty =
    DependencyProperty.Register
    (
        "SelectedRegion",
        typeof(RegionViewModel),
        typeof(RegionConfiguration),
        new FrameworkPropertyMetadata(SelectedRegionPropertyChanged)
    );

    public static readonly DependencyProperty SelectedTriggerProperty =
    DependencyProperty.Register
    (
        "SelectedTrigger",
        typeof(TriggerViewModel),
        typeof(RegionConfiguration)
    );

    public static readonly DependencyProperty IsTriggerListEmptyProperty =
    DependencyProperty.Register
    (
        "IsTriggerListEmpty",
        typeof(bool),
        typeof(RegionConfiguration)
    );

    #endregion "Dependency Properties"

    #region "Properties"

    public RegionViewModel SelectedRegion
    {
      get { return (RegionViewModel)GetValue(SelectedRegionProperty); }
      set { SetValue(SelectedRegionProperty, value); }
    }

    public TriggerViewModel SelectedTrigger 
    {
      get { return (TriggerViewModel)GetValue(SelectedTriggerProperty); }
      set { SetValue(SelectedTriggerProperty, value); }
    }

    public bool IsTriggerListEmpty
    {
      get { return ((RegionViewModel)GetValue(SelectedRegionProperty)).Triggers?.Count > 0; }
      set { SetValue(IsTriggerListEmptyProperty, value); }
    }

    public IEnumerable<TriggerTypes> TriggerTypes { get => Enum.GetValues(typeof(TriggerTypes)).Cast<TriggerTypes>(); } 

    #endregion "Properties"

    public RegionConfiguration()
    {
      InitializeComponent();
    }

    private static void SelectedRegionPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      if (sender is RegionConfiguration thisControl)
      {
        if (thisControl.SelectedRegion != null)
        {
          thisControl.SelectedRegion.PropertyChanged += SelectedRegion_PropertyChanged;
        }

        thisControl.IsTriggerListEmpty = thisControl?.SelectedRegion?.Triggers?.Count > 0;
      }
    }

    private static void SelectedRegion_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      SaveConfigTriggered?.Invoke(null, (RegionViewModel)sender);
    }

    private void BtnAddTrigger_Click(object sender, RoutedEventArgs e)
    {
      AddTriggerClicked?.Invoke(null, new ButtonClickArgs("Add", ITEM_DESCRIPTOR));
    }

    private void BtnDeleteTrigger_Click(object sender, RoutedEventArgs e)
    {
      DeleteTriggerClicked?.Invoke(null, new ButtonClickArgs("Delete", ITEM_DESCRIPTOR));
    }

    private void btnSoundBrowser_Click(object sender, RoutedEventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "MP3|*.mp3|WAV (*.wav)|*.wav|WMA|*.wma";

      if (openFileDialog.ShowDialog() == true)
        SelectedTrigger.SoundFilePath = openFileDialog.FileName;
    }
  }
}