using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Feed
{
    public class Emulator : MonoBehaviour
    {
        public static int currentFrame=0;
        public int frameNumber;
        
        [SerializeField]int maxFrames;

        [SerializeField] Slider sliderController;
        
        [SerializeField] RectTransform sliderHandle;
        
        [SerializeField]bool isPause;
        private void OnEnable()
        {
            currentFrame = 0;
        }
        public void SetMaxFrames()
        {
            sliderController.maxValue = PlayerPrefs.GetInt("MAX_Frames");
            maxFrames = (int)sliderController.maxValue;
        }
        public void FixedUpdate()
        {
            PlayerPrefs.SetInt("CurrentFrame", currentFrame);
            if (!isPause)
            {
                if(currentFrame==maxFrames-1)Reset();

                if (currentFrame < maxFrames)
                {
                    currentFrame++;
                    sliderController.value = currentFrame;
                }
            }
        }

        public void Pause()
        {
            isPause = true;
        }
        public void Play()
        {
            isPause = false;
        }
        public void Reset()
        {
            currentFrame = 0;
            FeedSceneManager.ResetScene();
        }
        public void ManuallySetFrame()
        {
            currentFrame = (int)sliderController.value;
        }
        public Vector3 EventPositions(int val)
        {
            sliderController.value = val;
            return sliderHandle.position;
        }
    }
}