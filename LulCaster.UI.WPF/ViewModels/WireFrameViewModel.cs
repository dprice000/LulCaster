﻿using LulCaster.UI.WPF.Workers;
using LulCaster.UI.WPF.Utility;
using LulCaster.UI.WPF.Workers.Events.Arguments;
using LulCaster.Utility.ScreenCapture.Windows.Snipping;

using System;
using System.Windows;
using System.Windows.Media;

namespace LulCaster.UI.WPF.ViewModels
{
  public class WireFrameViewModel : ViewModelBase
  {
    #region "Private Members"
    private const int DRAW_TIMEOUT_MS = 200;
    private DateTime _nextDrawTime = DateTime.MinValue;
    private PresetControlViewModel _presetControlViewModel;
    private RegionControlViewModel _regionControlViewModel;
    private RegionConfigViewModel _regionConfigViewModel;

    private Brush _screenCaptureBrush;
    private readonly BoundingBoxBrush _boundingBoxBrush = new BoundingBoxBrush();

    #endregion "Private Members"

    #region "Properties"

    public bool ShowDebug { get; set; } = false;

    public Brush ScreenCaptureBrush
    {
      get
      {
        return _screenCaptureBrush;
      }
      set
      {
        _screenCaptureBrush = value;
        OnPropertyChanged(nameof(ScreenCaptureBrush));
      }
    }

    public PresetControlViewModel PresetControl
    {
      get
      {
        return _presetControlViewModel;
      }
      private set
      {
        _presetControlViewModel = value;
        OnPropertyChanged(nameof(PresetControl));
      }
    }

    public RegionControlViewModel RegionControl
    {
      get
      {
        return _regionControlViewModel;
      }
      private set
      {
        _regionControlViewModel = value;
        OnPropertyChanged(nameof(RegionControl));
      }
    }

    public RegionConfigViewModel RegionConfigControl
    {
      get
      {
        return _regionConfigViewModel;
      }
      set
      {
        _regionConfigViewModel = value;
        OnPropertyChanged(nameof(RegionConfigControl));
      }
    }

    #endregion "Properties"

    #region "Constructor"

    public WireFrameViewModel(PresetControlViewModel presetControlViewModel
      , RegionControlViewModel regionControlViewModel
      , RegionConfigViewModel regionConfigViewModel)
    {
      _presetControlViewModel = presetControlViewModel;
      _regionControlViewModel = regionControlViewModel;
      _regionConfigViewModel = regionConfigViewModel;

      PresetControl.SelectionChanged += PresetControl_SelectionChanged;
      RegionControl.SelectionChanged += RegionControl_SelectionChanged;
    }

    private void RegionControl_SelectionChanged(object sender, RegionViewModel e)
    {
      RegionConfigControl.SelectedRegion = e;
    }

    #endregion "Constructor"

    private void PresetControl_SelectionChanged(object sender, PresetViewModel e)
    {
      RegionControl.SelectedPreset = e;
      RegionConfigControl.SelectedPreset = e;
    }

    internal void screenCaptureWorker_ScreenCaptureCompleted(object sender, ScreenCaptureCompletedArgs captureArgs)
    {
      if (DateTime.Now < _nextDrawTime)
      {
        return;
      }

      _nextDrawTime = DateTime.Now.AddMilliseconds(DRAW_TIMEOUT_MS);
      captureArgs.HasBeenDrawn = true;

      if (!ShowDebug)
      {
        Draw(BitmapHelper.ConvertArgsToScreenCap(captureArgs, RegionControl.Regions, DateTime.Now));
      }

      //TODO: This needs to be cleaned up.....just don't know a better way to do it see RegionWorkerPool as well
      if (captureArgs.HasBeenProcessed && captureArgs.HasBeenDrawn)
      {
        captureArgs.Dispose();
      }
    }

    internal void regionWorkerPool_ProcessingCompleted(object sender, ScreenCapture screenCapture)
    {
      Draw(screenCapture);
    }

    public void Draw(ScreenCapture screenCapture)
    {
      if (ShowDebug)
      {
        DrawDebugView(screenCapture);
      }
      else
      {
        DrawEntireScreen(screenCapture);
      }
    }

    private void DrawEntireScreen(ScreenCapture screenCapture)
    {
      Application.Current.Dispatcher.Invoke(() =>
      {
        ScreenCaptureBrush = new ImageBrush(BitmapHelper.ConvertStreamToBitmap(screenCapture.MemoryStream));
      });
    }

    private void DrawDebugView(ScreenCapture screenCapture)
    {

    }
  }
}