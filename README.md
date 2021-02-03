Raygun4Unity
============

Supported platforms
====================

Raygun4Unity has been tested to work on:

* Windows Desktop
* Windows Phone
* Mac
* iOS
* Android

Raygun4Unity does not work in Web Player or Windows Store apps, most likely due to sand boxing preventing messages to be sent.

Where is my app API key?
====================

When sending exceptions to the Raygun service, an app API key is required to map the messages to your application.

When you create a new application in your Raygun account, your app API key is displayed at the top of the instructions page. You can also find the API key by clicking the "Application Settings" button in the side bar of the Raygun dashboard.

Namespace
====================
The main classes can be found in the Mindscape.Raygun4Unity namespace.

Usage
====================

To send exception messages to your Raygun application, create an instance of the RaygunClient by passing your application API key into the constructor. Then call one of the Send methods.
There are 3 different types of exception data that you can use in the Send methods:

* **Strings** provided by Unity for the error message and stack trace.
* **Exception** .Net objects. Useful if you need to send handled exceptions in try/catch blocks.
* **RaygunMessage** Allowing you to fully specify all the data fields that get sent to Raygun

If you already have a central place in your game for catching unhandled exceptions (to write the details to a log for example), then that will be a great place to send the exception information to Raygun.
If you aren't currently catching unhandled exceptions in your game, then a good way to do this is by listening to Application.logMessageReceived.
In the following example, Application.logMessageReceived has been set up in a MonoBehaviour that will be run during the initialization process of the game.
In the handler, you can check to see if the type of the log is an exception or error. Alternatively, you could send all types of log messages.

```
using Mindscape.Raygun4Unity;
using UnityEngine;

public class Logger : MonoBehaviour
{
  void Start()
  {
    Application.logMessageReceived += Application_logMessageReceived;
  }

  private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
  {
    if (type == LogType.Exception || type == LogType.Error)
    {
      RaygunClient raygunClient = new RaygunClient("YOUR_APP_API_KEY");
      raygunClient.Send(condition, stackTrace);
    }
  }
}
```

Options
====================

### Application version

The current version of Raygun4Unity does not automatically obtain the application version number. You can however specify this by setting the ApplicationVersion property of the RaygunClient instance.

### Customers

To keep track of how many customers are affected by each exception, you can set the `User` or `UserInfo` property of the RaygunClient instance. The customer can be any id string of your choosing to identify each customer.
Ideally, try to use an id that you can use to relate back to an actual customer such as a database id, or an email address. If you use an email address, the customer gravitars (if found) will displayed on the Raygun error dashboards.
The `UserInfo` property lets you provide additional customer information such as their name.

### Tags and custom data

The Send method overloads allow you to send an optional list of tags or/and a dictionary of object data. You can use these to provide whatever additional information you want to help you debug exceptions.

### Message modifications before sending

By listening to the RaygunClient.SendingMessage event, you can make modifications to any part of the message just before it is serialized and sent to Raygun.
Setting e.Cancel = true will prevent Raygun4Unity from sending the message. This is useful for filtering out types of exceptions that you don't want.
