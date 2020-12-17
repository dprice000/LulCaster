using LulCaster.UI.WPF.Dialogs;
using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.Dialogs.Services;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace LulCaster.UI.WPF.Controls
{
  /// <summary>
  /// Interaction logic for InteractableListControl.xaml
  /// </summary>
  public partial class InteractableListControl : UserControl
  {
    #region "Public Events"

    public event EventHandler<InputDialogResult> NewItemDialogExecuted;

    public event EventHandler<LulDialogResult> DeleteItemDialogExecuted;

    public event EventHandler<InputDialogResult> ItemEditedDialogExecuted;

    public event EventHandler<IListItem> SelectionChanged;

    #endregion "Public Events"

    #region "Dependency Properties"

    public static readonly DependencyProperty TitleProperty =
    DependencyProperty.Register
    (
        "Title",
        typeof(string),
        typeof(InteractableListControl)
    );

    public static readonly DependencyProperty IsPresetProperty =
    DependencyProperty.Register
    (
        "IsPresetControl",
        typeof(bool),
        typeof(InteractableListControl)
    );

    public static readonly DependencyProperty ItemDescriptorProperty =
    DependencyProperty.Register
    (
        "ItemDescriptor",
        typeof(string),
        typeof(InteractableListControl)
    );

    public static readonly DependencyProperty ItemListProperty =
    DependencyProperty.Register
    (
        "ItemList",
        typeof(IList),
        typeof(InteractableListControl)
    );

    public static readonly DependencyProperty SelectedItemProperty =
    DependencyProperty.Register
    (
        "SelectedItem",
        typeof(IListItem),
        typeof(InteractableListControl),
        new FrameworkPropertyMetadata(SelectionDependancyPropertyChanged)
    );

    private static void SelectionDependancyPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      if (sender is InteractableListControl thisControl)
      {
        thisControl.SelectedItem = (IListItem)e.NewValue;
      }
    }

    #endregion "Dependency Properties"

    #region "Properties"

    public string Title
    {
      get { return (string)GetValue(TitleProperty); }
      set { SetValue(TitleProperty, value); }
    }

    public bool IsPresetControl
    {
      get { return (bool)GetValue(IsPresetProperty); }
      set { SetValue(IsPresetProperty, value); }
    }

    /// <summary>
    /// This is the word that will be used to describe a item in the list.
    /// </summary>
    public string ItemDescriptor
    {
      get { return (string)GetValue(ItemDescriptorProperty); }
      set { SetValue(ItemDescriptorProperty, value); }
    }

    public string AddToolTip
    {
      get => $"Add New {ItemDescriptor}";
    }

    public string DeleteToolTip
    {
      get => $"Delete Selected {ItemDescriptor}";
    }

    public string EditToolTip
    {
      get => $"Edit Selected {ItemDescriptor}";
    }

    public IList ItemList
    {
      get { return (IList)GetValue(ItemListProperty); }
      set { SetValue(ItemListProperty, value); }
    }

    public IListItem SelectedItem
    {
      get { return (IListItem)GetValue(SelectedItemProperty); }
      set
      {
        SetValue(SelectedItemProperty, value);
        SelectionChanged?.Invoke(this, value);
      }
    }

    public IDialogService<InputDialog, InputDialogResult> InputDialog { get; set; }
    public IDialogService<MessageBoxDialog, LulDialogResult> MessageBoxService { get; set; }

    public PresetInputDialog PresetInputDialog { get; set; }

    #endregion "Properties"

    public InteractableListControl()
    {
      InitializeComponent();
    }

    private void Button_btnAddPreset(object sender, RoutedEventArgs e)
    {
      var action = "Creating New";
      var dialogResult = IsPresetControl ? CreatePresetPrompt(ItemDescriptor, action, string.Empty, string.Empty) : CreateRegionPrompt(ItemDescriptor, action);

      NewItemDialogExecuted?.Invoke(this, dialogResult);
    }

    private void Button_BtnDeletePreset(object sender, RoutedEventArgs e)
    {
      if (MessageBoxService.Show($"Delete {ItemDescriptor}", $"Do you want to delete selected {ItemDescriptor}(s)?", DialogButtons.YesNo) is LulDialogResult dialogResult)
      {
        DeleteItemDialogExecuted?.Invoke(this, dialogResult);
      }
    }

    private void Button_BtnEditPreset(object sender, RoutedEventArgs e)
    {
      var action = "Editing";
      InputDialogResult dialogResult = null;

      if (IsPresetControl)
      {
        var presetItem = (IPresetListItem)SelectedItem;
        CreatePresetPrompt(ItemDescriptor, action, presetItem.Name, presetItem.ProcessName);
      }
      else
      {
        CreateRegionPrompt(ItemDescriptor, action);
      }

      ItemEditedDialogExecuted?.Invoke(this, dialogResult);
    }

    private PresetInputDialogResult CreatePresetPrompt(string itemDescriptor, string action, string presetName, string processName)
    {
      return (PresetInputDialogResult)PresetInputDialog.Show($"{action} {itemDescriptor}", $"{action} {itemDescriptor}: ", presetName, processName, DialogButtons.OkCancel);
    }

    private InputDialogResult CreateRegionPrompt(string itemDescriptor, string action)
    {
      return InputDialog.Show($"{action} {itemDescriptor}", $"{action} {itemDescriptor}: ", DialogButtons.OkCancel);
    }
  }
}