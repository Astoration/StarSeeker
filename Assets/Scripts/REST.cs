using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Networking;
using LitJson;

public class REST : MonoBehaviour

{

    public static REST instance;
    void Awake()
    {
        if (!instance) instance = this;
    } 
    public delegate void Callback(WWW www);
    public delegate void WebCallback(UnityWebRequest www);

    public WWW GET(string url, Callback callback)
    {
        WWW www = new WWW(url);
        StartCoroutine(WaitForRequest(www, callback));
        return www;
    }
    public WWW POST(string url, Dictionary<string, string> post, Callback callback)
    {

        WWWForm form = new WWWForm();

        foreach (KeyValuePair<string, string> post_arg in post)
        {

            form.AddField(post_arg.Key, post_arg.Value);

        }

        WWW www = new WWW(url, form);
        StartCoroutine(WaitForRequest(www, callback));
        return www;
    }
    public WWW GET(string url)
    {
        WWW www = new WWW(url);
        StartCoroutine(WaitForRequest(www));
        return www;
    }
    public WWW POST(string url, Dictionary<string, string> post)
    {

        WWWForm form = new WWWForm();

        foreach (KeyValuePair<string, string> post_arg in post)
        {

            form.AddField(post_arg.Key, post_arg.Value);

        }

        WWW www = new WWW(url, form);
        StartCoroutine(WaitForRequest(www));
        return www;
    }
    public UnityWebRequest PUT(string url,string data,WebCallback callback)
    {
        UnityWebRequest www = UnityWebRequest.Put(url,System.Text.Encoding.UTF8.GetBytes(data));
        WaitForRequest(www, callback);
        return www;
    }
    public UnityWebRequest DELETE(string url, WebCallback callback)
    {
        UnityWebRequest www = UnityWebRequest.Delete(url);
        WaitForRequest(www,callback);
        return www;
    }
    private IEnumerator WaitForRequest(WWW www, Callback callback)
    {
        yield return www;
        callback(www);
    }
    private IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
    }
    private IEnumerator WaitForRequest(UnityWebRequest www)
    {
        yield return www;
    }
    private IEnumerator WaitForRequest(UnityWebRequest www, WebCallback callback)
    {
        yield return www.Send();
        callback(www);
    }
}
