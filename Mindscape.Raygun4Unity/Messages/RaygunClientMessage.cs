namespace Mindscape.Raygun4Unity.Messages
{
  public class RaygunClientMessage
  {
    public RaygunClientMessage()
    {
      Name = "Raygun4Unity";
      Version = "1.1.2019.4";
      ClientUrl = @"https://github.com/MindscapeHQ/raygun4unity";
    }

    public string Name { get; set; }

    public string Version { get; set; }

    public string ClientUrl { get; set; }
  }
}
