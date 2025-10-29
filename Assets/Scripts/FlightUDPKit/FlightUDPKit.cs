using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public class FlightUDPKit : MonoBehaviour
{
    public int sendingPort = 5000;
    public string sendingIp = "";
    private UdpClient udpClient;
    public Transform target;
    public Rigidbody rb;
    public bool send = false;
    // Start is called before the first frame update
    void Start()
    {
        udpClient = new UdpClient();
        udpClient.EnableBroadcast = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (send)
        encode();
    }

    void encode()
    {
        string stringdata = "";
        byte hexValue = 0x09;

        // Convert byte to ASCII character
        char asciiChar = (char)hexValue;
        string join = asciiChar + "";
        float originalValue = 0.0f;
        //pos x
        originalValue = target.position.x;
        stringdata += originalValue.ToString("0.000") + join;
        //pos y
        originalValue = target.position.y;
        stringdata += originalValue.ToString("0.000") + join;
        //pos z
        originalValue = target.position.z;
        stringdata += originalValue.ToString("0.000") + join;

        //rot x
        originalValue = target.eulerAngles.x;
        stringdata += originalValue.ToString("0.000") + join;
        //rot y
        originalValue = target.eulerAngles.y;
        stringdata += originalValue.ToString("0.000") + join;
        //rot z
        originalValue = target.eulerAngles.z;
        stringdata += originalValue.ToString("0.000") + join;

        //rb vel x
        originalValue = rb.velocity.x;
        stringdata += originalValue.ToString("0.000") + join;
        //rb vel y
        originalValue = rb.velocity.y;
        stringdata += originalValue.ToString("0.000") + join;
        //rb vel z
        originalValue = rb.velocity.z;
        stringdata += originalValue.ToString("0.000") + join;

        //rb ang x
        originalValue = rb.angularVelocity.x;
        stringdata += originalValue.ToString("0.000") + join;
        //rb ang y
        originalValue = rb.angularVelocity.y;
        stringdata += originalValue.ToString("0.000") + join;
        //rb ang z
        originalValue = rb.angularVelocity.z;
        stringdata += originalValue.ToString("0.000") + join;


        if (stringdata.Length == 0)
        {
            stringdata += "0" + join;
        }
        else
        {
            stringdata = stringdata.Replace("NaN", "0").Replace("NaN", "0").Replace("NaN", "0").Replace("NaN", "0").Replace("NaN", "0").Replace("NaN", "0");
        }

        byte[] byteArra = Encoding.ASCII.GetBytes(stringdata);
        //string asciiString = Encoding.ASCII.GetString(dat);
        Debug.Log(stringdata);
        SendByteMessage(byteArra);
    }

    void SendByteMessage(byte[] data)
    {
        IPAddress specificIPAddress = IPAddress.Parse(sendingIp);
        // Send the UDP message to the broadcast address on the specified port
        udpClient.Send(data, data.Length, new IPEndPoint(specificIPAddress, sendingPort));
    }

    void OnDestroy()
    {
        // Close the UDP client
        if (udpClient != null)
        {
            // isbound = false;
            udpClient.Close();
        }
    }
}
