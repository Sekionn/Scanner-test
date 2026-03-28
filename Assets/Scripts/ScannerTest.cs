using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BarcodeScanner;
using BarcodeScanner.Scanner;
using UnityEngine.UI;
using Wizcorp.Utils.Logger;
using System.Linq;

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
        barcodeScanner.OnReady += (sender, arg) =>
        {
            // Set Orientation & Texture
            CalculateBackgroundQuad();
        };

        barcodeScanner.StatusChanged += (sender, arg) =>
        {
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
        }
        else if (scannerbuttonText.text == "Scanning")
        {
            barcodeScanner.Stop();
            scannerbuttonText.text = "Scan";
        }

    }

    void CalculateBackgroundQuad()
    {
        Vector3 QuadScale;
        Camera cam = Camera.main;
        WebCamTexture webCamTexture = (WebCamTexture)barcodeScanner.Camera.Texture;
        webCamTexture.autoFocusPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        viewPort.texture = webCamTexture;
        Quaternion baseRotation = new Quaternion(0, 0, 90, 0);

        float screenRatio = (float)Screen.width / (float)Screen.height;
        float distance = cam.farClipPlane / 2f;
        float frustumHeight = .0048f * distance * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);

        viewPort.transform.localRotation = baseRotation * Quaternion.AngleAxis(-webCamTexture.videoRotationAngle, Vector3.forward);

        float TextureRatio = (float)(webCamTexture.width) / (float)(webCamTexture.height);
        if (screenRatio > TextureRatio)
        {
            float SH = screenRatio / TextureRatio;
            float TW = frustumHeight * -1f * SH;
            float TH = TW * (barcodeScanner.Camera.IsVerticalyMirrored() ? 1 : -1) * SH;
            QuadScale = new Vector3(TW, TH, 1f);
        }
        else
        {
            float SH = screenRatio / TextureRatio;
            float TW = TextureRatio * frustumHeight;
            float TH = TW * (barcodeScanner.Camera.IsVerticalyMirrored() ? 1 : -1) * SH;
            QuadScale = new Vector3(TW * (barcodeScanner.Camera.IsVerticalyMirrored() ? 1 : -1), TH, 1f);
        }

        viewPort.transform.localScale = QuadScale;
    }
}
