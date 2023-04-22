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
        Barcodes.Add(barcodeText.text);
        AmountCounted.Add(Int32.Parse(amountcounted.text));
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

}
