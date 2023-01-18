using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarcodeData : MonoBehaviour
{
    public List<string> Barcodes = new List<string>();
    public List<int> AmountCounted = new List<int>();
    public static BarcodeData Instance { get; private set; }

    public int shelfOfOrigin;
    public GameObject scanner, dataPage, text, editPage, barcodeText;
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

    private void Update()
    {
        if (tempBarcode != "")
        {
            editPage.SetActive(true);
            scanner.SetActive(false);
            dataPage.SetActive(false);
        }
    }

    public void AddNewBarcode(string barcode)
    {
        Barcodes.Add(barcode);
    }

    public void ShelfOfOrigin()
    {
        shelfOfOrigin = Int32.Parse(text.GetComponent<Text>().text);
        scanner.SetActive(true);
        dataPage.SetActive(false);
    }

}
