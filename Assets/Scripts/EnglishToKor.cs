using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;
using System.IO;
using Newtonsoft.Json.Linq;

public class EnglishToKor : MonoBehaviour
{
    public GameObject[] engText;
    public string[] originText;
    public List<string> tranedText = new List<string>();
    /*public string[] tranedText;*/
    public bool isKor = false;

    public void EnglishToKorean()
    {
        
        if (isKor == false)
        {
            
            engText = GameObject.FindGameObjectsWithTag("englishText");
            originText = new string[engText.Length];

            for (int i = 0; i < engText.Length; i++)
            {

                originText[i] = engText[i].GetComponent<TextMeshProUGUI>().text;
                if (tranedText.Count <= i )
                {
                    tranedText.Add(translateEnglish(originText[i]));
                    
                }

                engText[i].GetComponent<TextMeshProUGUI>().text = tranedText[i];


            }

            

            isKor = true;
        }

    }
    
    public void KoreanToEnglish()
    {
        
        if (isKor == true)
        {
            for (int i = 0; i < engText.Length; i++)
            {
                engText[i].GetComponent<TextMeshProUGUI>().text = originText[i];
            }
            isKor = false;
        }
        
        
    }

    public string translateEnglish(string english_text)
    {
        string url = "https://openapi.naver.com/v1/papago/n2mt";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Headers.Add("X-Naver-Client-Id", "client_id");
        request.Headers.Add("X-Naver-Client-Secret", "client_secret");
        request.Method = "POST";
        string query = english_text;
        byte[] byteDataParams = Encoding.UTF8.GetBytes("source=en&target=ko&text=" + query);
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = byteDataParams.Length;
        Stream st = request.GetRequestStream();
        st.Write(byteDataParams, 0, byteDataParams.Length);
        st.Close();
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        Stream stream = response.GetResponseStream();
        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
        string trans_text = reader.ReadToEnd();
        stream.Close();
        response.Close();
        reader.Close();

        JObject ret = JObject.Parse(trans_text);

        return ret["message"]["result"]["translatedText"].ToString();
    }
}
