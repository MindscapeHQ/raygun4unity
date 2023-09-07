using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Mindscape.Raygun4Unity
{
  public class RaygunCrashReportingPostService : SingletonBehaviour<RaygunCrashReportingPostService>
  {
    private const string DefaultApiEndPointForCR = "https://api.raygun.com/entries";

    public void Post(string apiKey, string message)
    {
      StartCoroutine(Deliver(apiKey, System.Text.Encoding.UTF8.GetBytes(message)));
    }

    private IEnumerator Deliver(string apiKey, byte[] payload)
    {
      using (var request = new UnityWebRequest(DefaultApiEndPointForCR))
      {
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("X-ApiKey", apiKey);

        request.uploadHandler = new UploadHandlerRaw(payload);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.method = UnityWebRequest.kHttpVerbPOST;

        var requestOperation = request.SendWebRequest();

        while (!requestOperation.isDone)
        {
          yield return new WaitForEndOfFrame();
        }

        if (requestOperation.webRequest.error != null)
        {
          RaygunClient.Log("Error occurred during report delivery: " + requestOperation.webRequest.error);
        }

        LogResponseCode(requestOperation.webRequest.responseCode);
      }
    }

    private static void LogResponseCode(long responseCode)
    {
      if (responseCode == 400)
      {
        RaygunClient.Log("API Response: Bad message - could not parse the provided JSON");
      }
      else if (responseCode == 403)
      {
        RaygunClient.Log("API Response: Invalid API Key - The value specified in the header X-ApiKey did not match with an application");
      }
      else if (responseCode == 413)
      {
        RaygunClient.Log("API Response: Request entity too large - The maximum size of a JSON payload has been exceeded");
      }
      else if (responseCode == 429)
      {
        RaygunClient.Log("API Response: Too Many Requests - Plan limit exceeded for month or plan expired");
      }
    }
  }
}
