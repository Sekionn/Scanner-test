using Assets.Scripts.Json_parser;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BarcodeData : MonoBehaviour
{
    public List<string> Barcodes = new List<string>();
    public List<int> AmountCounted = new List<int>();
    public static BarcodeData Instance { get; private set; }

    public int shelfOfOrigin;
    public GameObject scanner, dataPage, text, editPage;
    public InputField barcodeText;
    public string tempBarcode = "";

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
#elif UNITY_IOS
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            Application.RequestUserAuthorization(UserAuthorization.WebCam);
        }
#endif

        editPage.SetActive(false);
        scanner.SetActive(false);
        dataPage.SetActive(true);
    }

    private void Update()
    {
        if (tempBarcode != "")
        {
            editPage.SetActive(true);
            scanner.SetActive(false);
            dataPage.SetActive(false);
        }
    }

    public void AddNewBarcode()
    {
        InputField amountcounted = GameObject.FindGameObjectWithTag("AmountCounted").GetComponent<InputField>();

        if (amountcounted.text == "" || barcodeText.text == "")
        {
            return;
        }

        if (Barcodes.Contains(barcodeText.text))
        {
            int index = Barcodes.FindIndex(f => f == barcodeText.text);
            AmountCounted[index] += Int32.Parse(amountcounted.text);
        }
        else
        {
            Barcodes.Add(barcodeText.text);
            AmountCounted.Add(Int32.Parse(amountcounted.text));
        }

        barcodeText.text = "";
        amountcounted.text = "";
        tempBarcode = "";
        editPage.SetActive(false);
        scanner.SetActive(true);
        dataPage.SetActive(false);
    }

    public void ShelfOfOrigin()
    {
        shelfOfOrigin = Int32.Parse(text.GetComponent<Text>().text);
        scanner.SetActive(true);
        dataPage.SetActive(false);
        editPage.SetActive(false);
    }


    public void Back()
    {
        tempBarcode = "";
        editPage.SetActive(false);
        scanner.SetActive(true);
        dataPage.SetActive(false);
    }


    public void SyncBarcodes()
    {
        BarcodeDataDTO data = new BarcodeDataDTO(Barcodes, AmountCounted, shelfOfOrigin);

        var jsonparser = new JsonParser();
        var yoyoy = JsonConvert.SerializeObject(data);
        string jsonData = JsonConvert.SerializeObject(data);
        var information = jsonparser.LoadJson();

        StartCoroutine(PostBarcodes(information, jsonData));
    }

    public IEnumerator PostBarcodes(Information information, string jsonData)
    {
        Debug.LogError(information.URL + "multiple");

        using UnityWebRequest www = UnityWebRequest.Post(information.URL + "multiple", jsonData, "application/json");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }
}
