﻿using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class ShareImageCanvas : MonoBehaviour {

    // public
    // private
    private bool isProcessing = false;
    public Image buttonShare;
    private string mensaje;

    //function called from a button
    public void ButtonShare(bool mainmenu)
    {
//<<<<<<< HEAD
//=======
        if (mainmenu)
            mensaje = "Check out Red Red Square! My highscore is " + PlayerPrefs.GetInt("highscoreKey").ToString() + "!\nAvailable in Google Play. \nhttps://goo.gl/nQYDCP";
        else
            mensaje = "Check out Red Red Square! I just got a score of " + manageScore.score.ToString() + " and my highscore is " + PlayerPrefs.GetInt("highscoreKey").ToString() + "!\nAvailable in Google Play.\nhttps://goo.gl/nQYDCP";
//>>>>>>> 05e4978bbbf16228fb1243c837e193b3af2b61cb
        //buttonShare.enabled = false;
        if (!isProcessing)
        {
            StartCoroutine(ShareScreenshot());
        }
    }
    public IEnumerator ShareScreenshot()
    {
        isProcessing = true;
        // wait for graphics to render
        yield return new WaitForEndOfFrame();
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- PHOTO
        // create the texture
        Texture2D screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
        // put buffer into texture
        screenTexture.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);
        // apply
        screenTexture.Apply();
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- PHOTO
        byte[] dataToSave = screenTexture.EncodeToPNG();
        string destination = Path.Combine(Application.persistentDataPath, System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".png");
        File.WriteAllBytes(destination, dataToSave);
        if (!Application.isEditor)
        {
            // block to open the file and share it ------------START
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + destination);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);

            intentObject.Call<AndroidJavaObject>("setType", "text/plain");
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "" + mensaje);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "Check out Red Red Square!");

            intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");
            //AndroidJavaClass unity = new AndroidJavaClass("com.MortalDolphin.RedRedSquare");
			AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

            currentActivity.Call("startActivity", intentObject);
        }
        isProcessing = false;
        //buttonShare.enabled = true;
    }
}
