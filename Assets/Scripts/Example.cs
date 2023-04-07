using System;
using System.IO;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Example : MonoBehaviour
{
    string Local => Application.persistentDataPath;

    [SerializeField] TMP_InputField link;
    [SerializeField] TMP_InputField text;
    [SerializeField] TMP_InputField imageFile;
    [SerializeField] TMP_InputField videoFile;
    [SerializeField] Button shareImageButton;
    [SerializeField] Button shareVideoButton;
    
    NativeShare _nativeShare;
    
    void Awake(){
        shareImageButton.onClick.AddListener(ShareImage);
        shareVideoButton.onClick.AddListener(ShareVideo);
        link.text = "http://bit.ly/3XKaViI";
        text.text = "Join me in Garden Joy: ";
        videoFile.text = "https://flora-assets-dev.s3.amazonaws.com/videos/Nurture_Stage1_2.mp4";
        imageFile.text = "https://secure.img1-cg.wfcdn.com/im/72551637/resize-h600-w600%5Ecompr-r85/1093/109315834/Live+Plants.jpg";
    }

    void OnDestroy(){
        shareImageButton.onClick.RemoveListener(ShareImage);
        shareVideoButton.onClick.RemoveListener(ShareVideo);
    }

    void ShareImage(){
        Share(imageFile.text);
    }
    
    void ShareVideo(){
        Share(videoFile.text);
    }
    
    void Share(string media){
        var local = DownloadFile(media);
        _nativeShare ??= new NativeShare();
        _nativeShare
            .Clear()
            .AddFile(local)
            .SetText(text.text)
            .SetUrl(link.text)
            .Share();
    }

    string DownloadFile(string remote){
        var local = Path.Combine(Local, remote).Replace("https://", string.Empty).Replace("?", string.Empty);
        Download(remote, local);
        return local;
    }

    void Download(string remote, string local){
        try{
            using (WebClient myWebClient = new WebClient()){
                string target = Path.Combine(local);
                string dir = Path.GetDirectoryName(target);
                if(!Directory.Exists(dir)){
                    Directory.CreateDirectory(dir);
                }
                myWebClient.DownloadFile(remote, target);
            }

        }catch(Exception e){
            throw e;
        }
    }
}
