using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using TMPro;
using UnityEngine;
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
        BarcodeDataDTO data = new BarcodeDataDTO();
        data.Barcodes = Barcodes;
        data.shelfOfOrigin = shelfOfOrigin;
        data.AmountCounted = AmountCounted;

        string jsonData = JsonUtility.ToJson(data);

        UnityWebRequest request = new UnityWebRequest("", "POST");
        request.SetRequestHeader("Content-Type", "application/json");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
    }
}
