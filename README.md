Raygun4Unity
============

Supported platforms
====================

Raygun4Unity has been tested to work on:

* Windows 7 - 8.1
* Windows Phone 8 - 8.1
* Mac
* iOS
* Android

Raygun4Unity does not work in Web Player or Windows Store apps, most likely due to sand boxing preventing messages to be sent.

Where is my app API key?
====================

When sending exceptions to the Raygun.io service, an app API key is required to map the messages to your application.

When you create a new application on your Raygun.io dashboard, your app API key is displayed at the top of the instructions page. You can also find the API key by clicking the "Application Settings" button in the side bar of the Raygun.io dashboard.

Namespace
====================
The main classes can be found in the Mindscape.Raygun4Unity namespace.

Usage
====================

Raygun4Unity can be used in two main ways: automatically, and manually.

####Automatically

You can setup Raygun4Unity to automatically send all unhandled exceptions with the following single line of code. This code should be placed in a C# script file that will be called in the initialization process of the game.

```
RaygunClient.Attach("YOUR_APP_API_KEY");
```

**WARNING** The RaygunClient uses Application.RegisterLogCallback to listen to exceptions. RegisterLogCallback can only have one handler at a time, so if you already are using RegisterLogCallback,
then you'll want to send the exception information manually within your existing callback.

####Manually

If you already have your own centralized logging system in place, then you many want to send messages to Raygun.io manually within your logging handlers.
This can be done by createing a new instance of RaygunClient and using one of the Send method overloads. There are 3 different types of data that you can use in the Send methods:

* **Strings** provided by Unity for the error message and stack trace.
* **Exception** .Net objects. Useful if you need to send handled exceptions in try/catch blocks.
* **RaygunMessage** Allowing you to fully specify all the data fields that get sent to Raygun.io

```
RaygunClient client = new RaygunClient("YOUR_APP_API_KEY");
client.Send(e);
```

Options
====================

####Application version

The current version of Raygun4Unity does not automatically obtain the application version number. You can however specify this by setting the ApplicationVersion property of the RaygunClient instance.
If you are using the Attach method, you can get the RaygunClient instance from the static RaygunClient.Current property.

####User

To keep track of how many users are affected by each exception, you can set the User property of the RaygunClient instance. This can be any id string of your choosing to identify each user.
Ideally, try to use an id that you can use to relate back to an actual user such as a database id, or an email address. If you use an email address, the users gravitars (if found) will displayed on the Raygun.io error dashboards.
If you are using the Attach method, you can get the RaygunClient instance from the static RaygunClient.Current property.

####Tags and custom data

The Send method overloads allow you to send an optional list of tags or a dictionary of object data. You can use these to provide whatever additional information you want to help you debug exceptions.

####Message modifications before sending

By listening to the RaygunClient.SendingMessage event, you can make modifications to any part of the message just before it is serialized and sent to Raygun.io.
Setting e.Cancel = true will prevent Raygun4Unity from sending the message. This is useful for filtering out types of exceptions that you don't want.
