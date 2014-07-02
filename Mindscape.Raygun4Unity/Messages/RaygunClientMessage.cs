using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Mindscape.Raygun4Unity.Messages
{
  public class RaygunClientMessage
  {
    private string _name;
    private string _version;
    private string _clientUrl;

    public RaygunClientMessage()
    {
      Name = "Raygun4Unity";
      Version = "0.1.0.0";
      ClientUrl = @"https://github.com/MindscapeHQ/raygun4unity";
    }

    public string Name
    {
      get { return _name; }
      set
      {
        _name = value;
      }
    }

    public string Version
    {
      get { return _version; }
      set
      {
        _version = value;
      }
    }

    public string ClientUrl
    {
      get { return _clientUrl; }
      set
      {
        _clientUrl = value;
      }
    }
  }
}
