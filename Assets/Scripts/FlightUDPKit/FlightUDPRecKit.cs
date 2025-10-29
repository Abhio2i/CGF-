using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public class FlightUDPRecKit : MonoBehaviour
{
    public int sendingPort = 5000;
    public string sendingIp = "192.168.1.100";
    public bool RecUdpPacket = false;
    public int recePort = 10000;
    public string reciveIp = "192.168.1.100";

    private byte[] recByte;
    private UdpClient udpClient;
    private System.Threading.Thread receiveThread;
    public bool isRunning = true;
    public bool isbound = false;
    public bool send = false;
    string[] array = { };
    public Transform target;
    public Rigidbody rb;

    // Interpolation
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private Vector3 targetVelocity;
    private Vector3 targetAngularVelocity;
    private bool newDataReceived = false;

    public float interpolationSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void activate(bool value)
    {
        send = value;
        StartReceive();
    }

    public void setSendPort(string value)
    {
        sendingPort = int.Parse(value);
    }

    public void setRecPort(string value)
    {
        recePort = int.Parse(value);
    }

    public void setIp(string ip)
    {
        sendingIp = ip;
        reciveIp = ip;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (isbound && array.Length >0)
        //{
        //    target.position = new Vector3(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]));
        //    target.eulerAngles = new Vector3(float.Parse(array[3]), float.Parse(array[4]), float.Parse(array[5]));
        //    rb.velocity = new Vector3(float.Parse(array[6]), float.Parse(array[7]), float.Parse(array[8]));
        //    rb.angularVelocity = new Vector3(float.Parse(array[9]), float.Parse(array[10]), float.Parse(array[11]));
        //    array = new string[] { };
        //}
        // ReceiveData  array = message.Split(asciiChar);
        if (send)
            encode();

        if (array.Length >= 12)
        {
            send = false;
            targetPosition = new Vector3(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]));
            targetRotation = Quaternion.Euler(float.Parse(array[3]), float.Parse(array[4]), float.Parse(array[5]));
            targetVelocity = new Vector3(float.Parse(array[6]), float.Parse(array[7]), float.Parse(array[8]));
            targetAngularVelocity = new Vector3(float.Parse(array[9]), float.Parse(array[10]), float.Parse(array[11]));

            newDataReceived = true; 
        }

        if (isbound && newDataReceived)
        {
            // 1. Interpolate Position 
            // LERP (Linear Interpolation) 
            target.position = Vector3.Lerp(target.position, targetPosition, Time.fixedDeltaTime * interpolationSpeed);

            // 2. Interpolate Rotation 
            target.rotation = Quaternion.Slerp(target.rotation, targetRotation, Time.fixedDeltaTime * interpolationSpeed);

            // 3. Extrapolate Velocity 
            // FixedUpdate 
            rb.velocity = targetVelocity;
            rb.angularVelocity = targetAngularVelocity;

            // * newDataReceived 
        }

    }

    void StartReceive()
    {
        udpClient = new UdpClient();
        udpClient.EnableBroadcast = true;
        // Create a local endpoint to bind the UdpClient to
        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, recePort);

        // Bind the UdpClient to the local endpoint
        udpClient.Client.Bind(localEndPoint);
        isbound = true;
        if (receiveThread == null)
        {
            // Start a new thread to listen for incoming UDP messages
            receiveThread = new System.Threading.Thread(new System.Threading.ThreadStart(ReceiveData));
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }
    }

    // In FlightUDPRecKit.cs

    // ... (existing code)

    void OnDestroy()
    {
        // 1. Set the flag to false to stop the loop
        isRunning = false;
        isbound = false;

        // 2. Close the UDP client to unblock the ReceiveData thread
        if (udpClient != null)
        {
            udpClient.Close();
            // This will cause the ReceiveData thread to throw a SocketException or ObjectDisposedException
        }

        // 3. Wait for the thread to exit (optional, but good practice)
        if (receiveThread != null && receiveThread.IsAlive)
        {
            receiveThread.Join(500); // Wait up to 500ms for thread to finish
            // If the thread is still alive after Join, it might be safer to abandon it 
            // than to use Thread.Abort(), which causes the error you saw.
        }
    }

    void ReceiveData()
    {
        while (isRunning)
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, recePort);

            if (isbound) // Redundant check, but harmless
            {
                try
                {
                    byte[] data = udpClient.Receive(ref remoteEndPoint);

                    recByte = data;
                    RecUdpPacket = true;
                    byte hexValue = 0x09;

                    // Convert byte to ASCII character
                    char asciiChar = (char)hexValue;
                    // Assuming 'recByte' is your byte array
                    string message = Encoding.UTF8.GetString(recByte);
                    string packetData = BitConverter.ToString(data);
                    array = message.Split(asciiChar);
                    
                    //Debug.Log("Received message from " + remoteEndPoint.Address + " : len " + data.Length);
                    Debug.Log("Received message from " + remoteEndPoint.Address + " : len " + float.Parse(array[0]));
                }
                // Handle the expected exceptions when the UdpClient is closed while Receive is running
                catch (ObjectDisposedException)
                {
                    // This is the clean exit exception when udpClient.Close() is called.
                    // Break the loop if we're supposed to be stopping.
                    if (!isRunning)
                        break;
                }
                catch (SocketException e)
                {
                    // 10004 (Interrupted) or 10057 (Not connected) are common on close/shutdown.
                    if (!isRunning)
                    {
                        // Expected exception during shutdown, just exit.
                        break;
                    }
                    Debug.LogError($"Socket error: {e}");
                }
                catch (Exception e)
                {
                    // Now this catch block only handles *truly* unexpected errors.
                    Debug.LogError($"Unexpected error: {e}");

                }
            }
        }
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

}
