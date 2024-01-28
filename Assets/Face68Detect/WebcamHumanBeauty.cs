using DlibFaceLandmarkDetector;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rect = UnityEngine.Rect;
using TMPro;
 
public class WebcamHumanBeauty : MonoBehaviour
{
    #region webcam params
    [SerializeField, TooltipAttribute("Set the name of the device to use.")]
    public string requestedDeviceName = null;
    [SerializeField, TooltipAttribute("Set the width of WebCamTexture.")]
    public int requestedWidth = 320;
    [SerializeField, TooltipAttribute("Set the height of WebCamTexture.")]
    public int requestedHeight = 240;

    public float smileMark;
    public RawImage WebcamHumanBeautyRaw;
    private WebCamTexture webCamTexture;
    private WebCamDevice webCamDevice;
    #endregion
 
    #region fps
    [SerializeField, TooltipAttribute("Set FPS of WebCamTexture.")]
    public int requestedFPS = 30;
    #endregion
 
    #region dlib plugin params
    private FaceLandmarkDetector faceLandmarkDetector; // dlib 实例
    private string dlibShapePredictorFileName = "sp_human_face_68.dat"; // 训练模型
    private string dlibShapePredictorFilePath; // 训练模型文件
    private Color32[] colors;
    private Texture2D texture;
    private Mat webcamMat; // 摄像头 Mat
    private Mat faceRectMat; // 脸部区域 Mat
    
    private Texture2D testRawT2D;
    private OpenCVForUnity.CoreModule.Rect faceRect;
    #endregion
    
    public TextMeshProUGUI confidenceText;
 
    private void Start()
    {
        // fps 监视器
        dlibShapePredictorFilePath = Utils.getFilePath(dlibShapePredictorFileName);
        Run();
 
        //webcamMat = new Mat(texture.height, texture.width, CvType.CV_8UC4);
    }
 
    private void Update()
    {
        Color32[] colors = GetColors();
        if (colors != null)
        {
            faceLandmarkDetector.SetImage<Color32>(colors, texture.width, texture.height, 4, true);
            List<Rect> detectResult = faceLandmarkDetector.Detect();
            foreach (var rect in detectResult)
            {
                //Debug.Log("face : " + rect);
                //detect landmark points
                List<Vector2> landmarks = faceLandmarkDetector.DetectLandmark(rect);
                smileMark = CalculateSmileDegree(landmarks);
                //draw landmark points
                faceLandmarkDetector.DrawDetectLandmarkResult<Color32>(colors, texture.width, texture.height, 2, true, 0, 255, 0, 255);
                confidenceText.text = smileMark.ToString();
            }
            //draw face rect
            faceLandmarkDetector.DrawDetectResult<Color32>(colors, texture.width, texture.height, 4, true, 255, 0, 0, 255, 2);
 
            texture.SetPixels32(colors);
            texture.Apply(false);
 
            //if (detectResult.Count > 0)
            //{
            //    faceRect = new OpenCVForUnity.CoreModule.Rect(0, 0, 600, 500);
            //    faceRect.x = Convert.ToInt32(detectResult[0].x);
            //    faceRect.y = Convert.ToInt32(detectResult[0].y);
            //    faceRect.width = Convert.ToInt32(detectResult[0].width);
            //    faceRect.height = Convert.ToInt32(detectResult[0].height);
            //    Debug.Log("face : " + faceRect);
            //    testRawT2D = new Texture2D(600, 500);
            //    //testRawT2D = new Texture2D(faceRect.width, faceRect.height);
            //    //faceRectMat = new Mat(webcamMat, faceRect);
            //    //Utils.matToTexture2D(faceRectMat, testRawT2D);
            //    //TestRaw.texture = testRawT2D;
            //    //TestRaw.SetNativeSize();
            //}
 
 
 
            //Utils.texture2DToMat(texture, webcamMat);
 
        }
    }
 
    private float CalculateSmileDegree(List<Vector2> landmarks)
    {
        // 获取嘴巴的特征点
        Vector2 mouthLeft = landmarks[48]; // 点49
        Vector2 mouthRight = landmarks[54]; // 点55
        Vector2 mouthTop = landmarks[50]; // 点51
        Vector2 mouthBottom = landmarks[56]; // 点57
        Vector2 nose = landmarks[33]; // 点34

        // 计算嘴巴的宽度和高度
        float mouthWidth = Vector2.Distance(mouthLeft, mouthRight);
        float mouthNose = Vector2.Distance(nose ,mouthTop);
        float mouthHeight = Vector2.Distance(mouthTop, mouthBottom);

        // 计算笑容程度
        float smileDegree = (mouthHeight + mouthWidth)/mouthNose;

        return smileDegree;
    }
    
    // 创建dlib 实例 初始化
    private void Run()
    {
        faceLandmarkDetector = new FaceLandmarkDetector(dlibShapePredictorFilePath);
 
        Initialize();
    }
 
    // 携程打开摄像头
    private void Initialize()
    {
        StartCoroutine(_Initialize());
    }
 
    // 摄像头携程初始化
    private IEnumerator _Initialize()
    {
        // Creates the camera
        if (!String.IsNullOrEmpty(requestedDeviceName))
        {
            int requestedDeviceIndex = -1;
            if (Int32.TryParse(requestedDeviceName, out requestedDeviceIndex))
            {
                if (requestedDeviceIndex >= 0 && requestedDeviceIndex < WebCamTexture.devices.Length)
                {
                    webCamDevice = WebCamTexture.devices[requestedDeviceIndex];
                    webCamTexture = new WebCamTexture(webCamDevice.name, requestedWidth, requestedHeight, requestedFPS);
                }
            }
            else
            {
                for (int cameraIndex = 0; cameraIndex < WebCamTexture.devices.Length; cameraIndex++)
                {
                    if (WebCamTexture.devices[cameraIndex].name == requestedDeviceName)
                    {
                        webCamDevice = WebCamTexture.devices[cameraIndex];
                        webCamTexture = new WebCamTexture(webCamDevice.name, requestedWidth, requestedHeight, requestedFPS);
                        break;
                    }
                }
            }
            if (webCamTexture == null)
                Debug.Log("Cannot find camera device " + requestedDeviceName + ".");
            else
                Debug.Log("Camera device " + requestedDeviceName + " is selected");
        }
 
        if (webCamTexture == null)
        {
            if (WebCamTexture.devices.Length > 0)
            {
                webCamDevice = WebCamTexture.devices[0];
                webCamTexture = new WebCamTexture(webCamDevice.name, requestedWidth, requestedHeight, requestedFPS);
            }
            else
            {
                Debug.LogError("Camera device does not exist.");
                yield break;
            }
        }
 
 
        WebcamHumanBeautyRaw.texture = webCamTexture;
        // Starts the camera
        webCamTexture.Play();
 
        //webCamTexture.GetPixels32 创建Color32要放在Play() 后面
        //https://docs.unity3d.com/ScriptReference/WebCamTexture.GetPixels32.html
        colors = new Color32[webCamTexture.width * webCamTexture.height];
        texture = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.RGBA32, false);
        WebcamHumanBeautyRaw.texture = texture;
 
    }
 
    // 返回摄像头像素值
    private Color32[] GetColors()
    {
        webCamTexture.GetPixels32(colors);
 
        return colors;
    }
 
    // 销毁释放
    private void OnDestroy()
    {
        if (webCamTexture != null)
        {
            webCamTexture.Stop();
            WebCamTexture.Destroy(webCamTexture);
            webCamTexture = null;
        }
 
        if (faceLandmarkDetector != null)
            faceLandmarkDetector.Dispose();
    }
}