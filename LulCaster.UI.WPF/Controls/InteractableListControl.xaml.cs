using LulCaster.UI.WPF.Dialogs;
using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.Dialogs.Services;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace LulCaster.UI.WPF.Controls
{
  /// <summary>
  /// Interaction logic for InteractableListControl.xaml
  /// </summary>
  public partial class InteractableListControl : UserControl
  {
    #region "Dependency Properties"

    public static readonly DependencyProperty TitleProperty =
    DependencyProperty.Register
    (
        "Title",
        typeof(string),
        typeof(InteractableListControl),
        new FrameworkPropertyMetadata(new PropertyChangedCallback(OnTitleChanged))
    );

    public static readonly DependencyProperty SingularItemDescriptorProperty =
    DependencyProperty.Register
    (
        "ItemDescriptor",
        typeof(string),
        typeof(InteractableListControl),
        new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSingularItemDescriptorChanged))
    );

    public static readonly DependencyProperty PresetListProperty =
    DependencyProperty.Register
    (
        "ItemList",
        typeof(IList<IListItem>),
        typeof(InteractableListControl),
        new FrameworkPropertyMetadata(new PropertyChangedCallback(OnItemListChanged))
    );

    public static readonly DependencyProperty SelectedPresetProperty =
    DependencyProperty.Register
    (
        "SelectedItem",
        typeof(IListItem),
        typeof(InteractableListControl),
        new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSelectedItemChanged))
    );

    #endregion "Dependency Properties"

    #region "Properties"

    public string Title
    {
      get { return (string)GetValue(TitleProperty); }
      set { SetValue(TitleProperty, value); }
    }

    /// <summary>
    /// This is the word that will be used to describe a item in the list.
    /// </summary>
    public string ItemDescriptor
    {
      get { return (string)GetValue(SingularItemDescriptorProperty); }
      set { SetValue(SingularItemDescriptorProperty, value); }
    }

    public IList<IListItem> ItemList
    {
      get { return (IList<IListItem>)GetValue(PresetListProperty); }
      set { SetValue(PresetListProperty, value); }
    }

    public IListItem SelectedItem
    {
      get { return (IListItem)GetValue(SelectedPresetProperty); }
      set { SetValue(SelectedPresetProperty, value); }
    }

    public IDialogService<InputDialog, InputDialogResult> InputDialog { get; set; }
    public IDialogService<MessageBoxDialog, LulDialogResult> MessageBoxService { get; set; }

    #endregion "Properties"

    public InteractableListControl()
    {
      InitializeComponent();
    }

    #region "OnChanged Events"

    private static void OnTitleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      if (sender is InteractableListControl thisControl)
      {
        thisControl.Title = (string)e.NewValue;
      }
    }

    private static void OnSingularItemDescriptorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      if (sender is InteractableListControl thisControl)
      {
        thisControl.ItemDescriptor = (string)e.NewValue;
      }
    }

    private static void OnSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      if (sender is InteractableListControl thisControl)
      {
        thisControl.SelectedItem = (IListItem)e.NewValue;
      }
    }

    private static void OnItemListChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      if (sender is InteractableListControl thisControl)
      {
        thisControl.ItemList = (IList<IListItem>)e.NewValue;
      }
    }

    #endregion "OnChanged Events"

    private void Button_btnAddPreset(object sender, RoutedEventArgs e)
    {
      if (InputDialog.Show($"New {ItemDescriptor}", $"Enter New {ItemDescriptor} Name: ", DialogButtons.OkCancel) is InputDialogResult dialogResult && dialogResult.DialogResult == DialogResults.Ok)
      {
        if (string.IsNullOrWhiteSpace(dialogResult.Input))
        {
          MessageBoxService.Show("Empty Preset Name", "No Preset Name Provided!!", Dialogs.DialogButtons.Ok);
          return;
        }
      }
    }

    private void Button_BtnDeletePreset(object sender, RoutedEventArgs e)
    {
      if (MessageBoxService.Show($"Delete {ItemDescriptor}", $"Do you want to delete selected {ItemDescriptor}(s)?", DialogButtons.YesNo).DialogResult != DialogResults.Yes)
      {
        return;
      }

      SelectedItem = null;
    }
  }
}