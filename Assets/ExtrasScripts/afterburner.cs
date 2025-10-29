using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class afterburner : MonoBehaviour
{
    public SilantroController controller;
    public List<servo> afterburner_plates = new List<servo>();
    public Light light;
    public Slider leftThrottleSlide;
    public Slider rightThrottleSlide;
    public Slider leftThrustTemp;
    public Slider rightThrustTemp;

    private ParticleSystem pa;

    void Start()
    {
     pa = GetComponent<ParticleSystem>();   
    }

    // Update is called once per frame
    void Update()
    {
        if(controller != null)
        {
            foreach(var servo in afterburner_plates)
            {
                servo.set((controller.input.rawThrottleInput*-10f)+3f);
            }
            rightThrottleSlide.value = controller.input.rawThrottleInput*10;
            leftThrottleSlide.value = controller.input.rawThrottleInput*10;
            leftThrustTemp.value = Mathf.Pow((controller.totalThrustGenerated / 100000f), 2f)*8;
            rightThrustTemp.value = Mathf.Pow((controller.totalThrustGenerated / 100000f), 2f) * 8;

            light.intensity = (controller.input.rawThrottleInput*4f)+0.2f;
            pa.startLifetime = (Mathf.Pow((controller.totalThrustGenerated /100000f),2f)*0.22f*controller.input.rawThrottleInput)+0.05f;
            pa.startSpeed = (Mathf.Pow((controller.totalThrustGenerated / 100000f), 2f) * 21f * controller.input.rawThrottleInput) + 1f;
        }
    }
}
