using UnityEngine.Networking;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    [SerializeField]
    private Text timetext;
    public void GetMoscowTime()
    {
        StartCoroutine(MoscowTime());
    }

    private IEnumerator MoscowTime()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://worldtimeapi.org/api/timezone/Europe/Moscow"))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError("ошибка при запросе к api: " + www.error);
            }
            else
            {
                string jsonResponse = www.downloadHandler.text;
                var timeData = JsonUtility.FromJson<TimeData>(jsonResponse);
                string moscowTime = timeData.datetime;
                CallJavaScriptFunction(moscowTime);
            }
        }
    }

    private void CallJavaScriptFunction(string time)
    {
        string message = "Время по Москве: " + time;
        Application.ExternalCall("Alert", message);
    }

    [System.Serializable]
    private class TimeData
    {
        public string datetime;
    }
}

