using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace Mindscape.Raygun4Unity.Messages
{
  public class RaygunEnvironmentMessage
  {
    private int _processorCount;
    private string _osVersion;
    private double _windowBoundsWidth;
    private double _windowBoundsHeight;
    private string _resolutionScale;
    private string _architecture;
    private ulong _totalVirtualMemory;
    private ulong _availableVirtualMemory;
    private ulong _totalPhysicalMemory;
    private ulong _availablePhysicalMemory;
    private double _utcOffset;
    private string _locale;

    public RaygunEnvironmentMessage()
    {
      try
      {
        DateTime now = DateTime.Now;
        UtcOffset = TimeZone.CurrentTimeZone.GetUtcOffset(now).TotalHours;

        WindowBoundsWidth = Screen.width;
        WindowBoundsHeight = Screen.height;
        
        ProcessorCount = SystemInfo.processorCount;
        Cpu = SystemInfo.processorType;
        OSVersion = SystemInfo.operatingSystem;
        DeviceModel = SystemInfo.deviceModel;
        DeviceType = SystemInfo.deviceType.ToString();
        SystemMemorySize = SystemInfo.systemMemorySize;
        
        Locale = CultureInfo.CurrentCulture.DisplayName;
      }
      catch (Exception ex)
      {
        System.Diagnostics.Debug.WriteLine(string.Format("Error getting environment info: {0}", ex.Message));
      }
    }

    public int ProcessorCount
    {
      get {return _processorCount; }
      private set
      {
        _processorCount = value;
      }
    }

    public string OSVersion
    {
      get {return _osVersion; }
      private set
      {
        _osVersion = value;
      }
    }

    public double WindowBoundsWidth
    {
      get { return _windowBoundsWidth; }
      private set
      {
        _windowBoundsWidth = value;
      }
    }

    public double WindowBoundsHeight
    {
      get { return _windowBoundsHeight; }
      private set
      {
        _windowBoundsHeight = value;
      }
    }

    public string Cpu { get; set; }

    public string Architecture
    {
      get {return _architecture; }
      private set
      {
        _architecture = value;
      }
    }

    public string DeviceModel { get; set; }

    public string DeviceType { get; set; }

    public int SystemMemorySize { get; set; } // MB

    public double UtcOffset
    {
      get { return _utcOffset; }
      private set
      {
        _utcOffset = value;
      }
    }

    public string Locale
    {
      get { return _locale; }
      private set
      {
        _locale = value;
      }
    }
  }
}
