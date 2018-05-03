using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Mindscape.Raygun4Unity.Messages
{
  public class RaygunClientMessage
  {
    public RaygunClientMessage()
    {
      Name = "Raygun4Unity";
      Version = "1.0.1.0";
      ClientUrl = @"https://github.com/MindscapeHQ/raygun4unity";
    }

    public string Name { get; set; }

    public string Version { get; set; }

    public string ClientUrl { get; set; }
  }
}
