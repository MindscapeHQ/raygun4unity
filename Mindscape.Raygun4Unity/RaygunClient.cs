using System;
using System.Collections;
using System.Collections.Generic;
using Mindscape.Raygun4Unity.Messages;

namespace Mindscape.Raygun4Unity
{
  public class RaygunClient
  {
    private readonly string _apiKey;

    /// <summary>
    /// Initializes a new instance of the <see cref="RaygunClient" /> class.
    /// </summary>
    /// <param name="apiKey">The API key.</param>
    public RaygunClient(string apiKey)
    {
      _apiKey = apiKey;
    }

    private bool ValidateApiKey()
    {
      if (string.IsNullOrEmpty(_apiKey))
      {
        RaygunClient.Log("ApiKey has not been provided, exception will not be logged");
        return false;
      }
      return true;
    }

    // Returns true if the message can be sent, false if the sending is canceled.
    protected bool OnSendingMessage(RaygunMessage raygunMessage)
    {
      bool result = true;
      EventHandler<RaygunSendingMessageEventArgs> handler = SendingMessage;
      if (handler != null)
      {
        RaygunSendingMessageEventArgs args = new RaygunSendingMessageEventArgs(raygunMessage);
        handler(this, args);
        result = !args.Cancel;
      }
      return result;
    }

    /// <summary>
    /// Gets or sets the user identity string.
    /// </summary>
    public string User { get; set; }

    /// <summary>
    /// Gets or sets information about the user including the identity string.
    /// </summary>
    public RaygunIdentifierMessage UserInfo { get; set; }

    /// <summary>
    /// Gets or sets a custom application version identifier for all error messages sent to the Raygun endpoint.
    /// </summary>
    public string ApplicationVersion { get; set; }

    /// <summary>
    /// Raised just before a message is sent. This can be used to make final adjustments to the <see cref="RaygunMessage"/>, or to cancel the send.
    /// </summary>
    public event EventHandler<RaygunSendingMessageEventArgs> SendingMessage;

    /// <summary>
    /// Transmits Unity exception information to Raygun.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="stackTrace">The stack trace information.</param>
    public void Send(string message, string stackTrace)
    {
      Send(message, stackTrace, null, null);
    }

    /// <summary>
    /// Transmits Unity exception information to Raygun specifying a list of string tags associated
    /// with the message for identification, as well as sending a key-value collection of custom data.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="stackTrace">The stack trace information.</param>
    /// <param name="tags">A list of strings associated with the message.</param>
    /// <param name="userCustomData">A key-value collection of custom data that will be added to the payload.</param>
    public void Send(string message, string stackTrace, IList<string> tags, IDictionary userCustomData)
    {
      Send(BuildMessage(message, stackTrace, tags, userCustomData));
    }

    /// <summary>
    /// Transmits an exception to Raygun.
    /// </summary>
    /// <param name="exception">The exception to deliver.</param>
    public void Send(Exception exception)
    {
      Send(exception, null, null);
    }

    /// <summary>
    /// Transmits an exception to Raygun specifying a list of string tags associated
    /// with the message for identification, as well as sending a key-value collection of custom data.
    /// </summary>
    /// <param name="exception">The exception to deliver.</param>
    /// <param name="tags">A list of strings associated with the message.</param>
    /// <param name="userCustomData">A key-value collection of custom data that will be added to the payload.</param>
    public void Send(Exception exception, IList<string> tags, IDictionary userCustomData)
    {
      Send(BuildMessage(exception, tags, userCustomData));
    }

    internal RaygunMessage BuildMessage(string message, string stackTrace, IList<string> tags, IDictionary userCustomData)
    {
      RaygunMessage raygunMessage = RaygunMessageBuilder.New
        .SetEnvironmentDetails()
        .SetMachineName(UnityEngine.SystemInfo.deviceName)
        .SetExceptionDetails(message, stackTrace)
        .SetClientDetails()
        .SetVersion(ApplicationVersion)
        .SetTags(tags)
        .SetUserCustomData(userCustomData)
        .SetUser(UserInfo ?? (!String.IsNullOrEmpty(User) ? new RaygunIdentifierMessage(User) : null))
        .Build();
      return raygunMessage;
    }

    internal RaygunMessage BuildMessage(Exception exception, IList<string> tags, IDictionary userCustomData)
    {
      RaygunMessage raygunMessage = RaygunMessageBuilder.New
        .SetEnvironmentDetails()
        .SetMachineName(UnityEngine.SystemInfo.deviceName)
        .SetExceptionDetails(exception)
        .SetClientDetails()
        .SetVersion(ApplicationVersion)
        .SetTags(tags)
        .SetUserCustomData(userCustomData)
        .SetUser(UserInfo ?? (!String.IsNullOrEmpty(User) ? new RaygunIdentifierMessage(User) : null))
        .Build();
      return raygunMessage;
    }

    /// <summary>
    /// Posts a RaygunMessage to the Raygun api endpoint.
    /// </summary>
    /// <param name="raygunMessage">The RaygunMessage to send. This needs its OccurredOn property
    /// set to a valid DateTime and as much of the Details property as is available.</param>
    public void Send(RaygunMessage raygunMessage)
    {
      if (ValidateApiKey())
      {
        bool canSend = OnSendingMessage(raygunMessage);
        if (canSend)
        {
          string message = null;

          try
          {
            message = SimpleJson.SerializeObject(raygunMessage);
          }
          catch (Exception ex)
          {
            RaygunClient.Log(string.Format("Error serializing raygun message: {0}", ex.Message));
          }

          if (message != null)
          {
            SendMessage(message);
          }
        }
      }
    }

    private void SendMessage(string message)
    {
      try
      {
        var request = UnityEngine.Networking.UnityWebRequest.Post("https://api.raygun.com/entries", message);
        var customUploadHandler = new UnityEngine.Networking.UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(message));
        customUploadHandler.contentType = "application/json";
        request.uploadHandler = customUploadHandler;

        request.SetRequestHeader("X-ApiKey", _apiKey);

        request.SendWebRequest();
      }
      catch (Exception ex)
      {
        RaygunClient.Log(string.Format("Error Logging Exception to Raygun {0}", ex.Message)); 
      }
    }    

    internal static void Log(string message)
    {
      UnityEngine.Debug.Log("<color=#B90000>Raygun4Unity: </color>" + message);
    }
  }
}
