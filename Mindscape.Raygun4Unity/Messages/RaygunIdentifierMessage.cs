namespace Mindscape.Raygun4Unity.Messages
{
  public class RaygunIdentifierMessage
  {
    private string _identifier;

    public RaygunIdentifierMessage(string user)
    {
      Identifier = user;
    }

    public string Identifier
    {
      get { return _identifier; }
      private set
      {
        _identifier = value;
      }
    }
  }
}
