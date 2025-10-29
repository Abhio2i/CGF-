using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.IO;
using UnityEditor;
using Assets.Code.ShareData;
namespace camera.Record
{
    public class CameraStreaming : MonoBehaviour
    {
        #region variables
        [SerializeField] GameObject video_1;
        [SerializeField] GameObject video_2;
        [SerializeField] GameObject video_3;
        [SerializeField] GameObject video_4;
        [SerializeField] string[] videoName_1;
        [SerializeField] string[] videoName_2;
        [SerializeField] string[] videoName_3;
        [SerializeField] string[] videoName_4;
        [SerializeField] Slider videoController;
        VideoPlayer videoPlayer_1;
        VideoPlayer videoPlayer_2;
        VideoPlayer videoPlayer_4;
        VideoPlayer videoPlayer_3;
        string path1;
        string path2;
        string path3;
        string path4;

        int tempCount;
        bool isMoving = true;

       [SerializeField] bool cam2,cam3, cam4;

        [SerializeField]float frameRate;
        float maxFrames;
        #endregion
        #region unity functions
        private void Awake()
        {
            videoPlayer_1 = video_1.GetComponent<VideoPlayer>();
            videoPlayer_2 = video_2.GetComponent<VideoPlayer>();
            videoPlayer_3 = video_3.GetComponent<VideoPlayer>();
            videoPlayer_4 = video_4.GetComponent<VideoPlayer>();
            path1 = Application.streamingAssetsPath + "/Captures/MainCam";
            path2 = Application.streamingAssetsPath + "/Captures/TopCam";
            path3 = Application.streamingAssetsPath + "/Captures/LeftCam";
            path4 = Application.streamingAssetsPath + "/Captures/RightCam";
            videoController.value = 0;

            if (Directory.Exists(path1))
            {
                videoName_1 = Directory.GetFiles(path1, "*.mp4");
            }
            if (Directory.Exists(path2))
            {
                videoName_2 = Directory.GetFiles(path2, "*.mp4");
            }
            if (Directory.Exists(path3))
            {
                videoName_3 = Directory.GetFiles(path3, "*.mp4");
            }
            if (Directory.Exists(path4))
            {
                videoName_4 = Directory.GetFiles(path4, "*.mp4");
            }
            try
            {
                if (videoName_1.Length > 0)
            {
                videoPlayer_1.url =videoName_1[0];
                videoPlayer_1.Prepare();
            }
                if (videoName_2.Length > 0)
                {
                    videoPlayer_2.url = videoName_2[0]; //videoName_2[0];
                    videoPlayer_2.Prepare();
                }
                else
                {
                    cam2 = true;
                }
                if (videoName_3.Length > 0)
                {
                    videoPlayer_3.url = videoName_3[0];
                    videoPlayer_3.Prepare();
                }
                else
                {
                    cam3 = true;
                }
                if (videoName_4.Length > 0)
                {
                    videoPlayer_4.url = videoName_4[0];
                    videoPlayer_4.Prepare();
                }
                else
                {
                    cam4=true;
                }
            }
            catch {  }

            videoController.onValueChanged.AddListener(delegate { ChangeTempValue(); });
            OnPause();
        }
        public long rewindTime;
        private void FixedUpdate()
        {
            if (videoPlayer_1.isPrepared & (videoPlayer_2.isPrepared||cam2) & (videoPlayer_3.isPrepared||cam3) & (videoPlayer_4.isPrepared||cam4))
            {
               
                frameRate = videoPlayer_1.frameRate;
                maxFrames = videoPlayer_1.frameCount;
                videoController.maxValue = maxFrames;
                if (isMoving && !isPause)
                {
                    StartCoroutine(CameraStreamingRoutine());
                    isMoving = false;
                }
            }

        }
        #endregion
        #region required functions
        IEnumerator CameraStreamingRoutine()
        {
            if (rewindTime == maxFrames - 1)
                rewindTime = 1;
            videoController.value = rewindTime;
            yield return null;

            if (rewindTime < maxFrames - 1)
            {
                rewindTime++;
                isMoving = true;
            }


        }
        [SerializeField] bool isPause;
        public void OnPause() //pause
        {
            isPause = true;
            videoPlayer_1.Pause();
            videoPlayer_2.Pause();
            videoPlayer_3.Pause();
            videoPlayer_4.Pause();
        }
        public void ChangeTempValue() //slider value changing
        {
            rewindTime = (int)videoController.value;
            if (!isPause)
            {
                videoPlayer_1.frame = rewindTime;
                videoPlayer_2.frame = rewindTime;
                videoPlayer_3.frame = rewindTime;
                videoPlayer_4.frame = rewindTime;
            }
            isMoving = true;
        }
        public void OnPlay() //on play
        {
            ChangeTempValue();

            isPause = false;
        }
        
        private void OnEnable()
        {
            
            #if UNITY_EDITOR
                AssetDatabase.Refresh();
            #endif
            
        }
#endregion
    }
}