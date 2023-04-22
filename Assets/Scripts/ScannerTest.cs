using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BarcodeScanner;
using BarcodeScanner.Scanner;
using UnityEngine.UI;
using Wizcorp.Utils.Logger;

public class ScannerTest : MonoBehaviour
{
    IScanner barcodeScanner;
    RawImage viewPort;
    public Text scannerbuttonText;

    private void Awake()
    {
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        barcodeScanner = new Scanner();
        viewPort = this.gameObject.GetComponent<RawImage>();
    }

    // Start is called before the first frame update
    void Start()
    {
        barcodeScanner.Camera.Play();
        barcodeScanner.OnReady += (sender, arg) => {
            // Set Orientation & Texture
            viewPort.transform.localEulerAngles = barcodeScanner.Camera.GetEulerAngles();
            viewPort.transform.localScale = barcodeScanner.Camera.GetScale();
            viewPort.texture = barcodeScanner.Camera.Texture;
            
            // Keep Image Aspect Ratio
            var rect = viewPort.GetComponent<RectTransform>();
            var newHeight = rect.sizeDelta.x * barcodeScanner.Camera.Height / barcodeScanner.Camera.Width;
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, newHeight);
        };

        barcodeScanner.StatusChanged += (sender, arg) => {
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (barcodeScanner == null)
        {
            barcodeScanner = new Scanner();
        }
        else
        {
            barcodeScanner.Update();
        }
    }

    public void Scan()
    {
        if (scannerbuttonText.text == "Scan")
        {
            scannerbuttonText.text = "Scanning";
            barcodeScanner.Scan((barType, barValue) =>
            {
                barcodeScanner.Stop();
                scannerbuttonText.text = "Scan";
                BarcodeData.Instance.tempBarcode = barValue;
                BarcodeData.Instance.barcodeText.text = barValue;
            });
        } else if (scannerbuttonText.text == "Scanning")
        {
            barcodeScanner.Stop();
            scannerbuttonText.text = "Scan";
        }

    }
}
