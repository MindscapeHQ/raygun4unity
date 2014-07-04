using System;
using System.Collections.Generic;
using System.Text;

namespace Mindscape.Raygun4Unity
{
  public class RaygunSettings
  {
    private static RaygunSettings _settings;
    private const string DefaultApiEndPoint = "https://api.raygun.io/entries";

    public static RaygunSettings Settings
    {
      get
      {
        if (_settings == null)
        {
          _settings = new RaygunSettings();
          _settings.ApiEndpoint = new Uri(DefaultApiEndPoint);
        }
        return _settings;
      }
    }

    public Uri ApiEndpoint { get; set; }
  }
}
