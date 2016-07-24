using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class REST : MonoBehaviour

{

    public static REST instance;
    void Awake()
    {
        if (!instance) instance = this;
    } 
    public delegate void Callback(WWW www);

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
    private IEnumerator WaitForRequest(WWW www, Callback callback)
    {
        yield return www;
        callback(www);
    }
    private IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
    }
}
