#region Script Info
//inp: Plane Current position.
//out: Based of Functions this Script is calculate the current latitude altitude and longitude of Plane.
#endregion
using UnityEngine;

public class LatLong : MonoBehaviour
{
    #region Global Perimeters
    public float mapMidX = 0;
    public float mapMaxX = 0;
    public float mapMidY = 0;
    public float mapMaxY = 0;

    public float unitymapx = 0;
    public float unitymapy = 0;
    #endregion

    #region Local Perimeters
    [SerializeField] private float longitudevalue;
    [SerializeField] private float lattitudevalue;

    [SerializeField] private float mapxval = 0;
    [SerializeField] private float mapyval = 0;
    #endregion

    #region Local Functions
    private void Start()
    {
        //longitude
        var actualX = mapMaxX - mapMidX;
        var unityX = unitymapx;
        longitudevalue = (unityX / actualX);

        //lattitude
        var actualY = mapMaxY - mapMidY;
        var unityY = unitymapy;
        lattitudevalue = (unityY / actualY);

    }
    #endregion
    public float temp;
    #region Global Function
    public Vector2 CalculateLatLong(float posZ, float posX)
    {
        //latitude
        //mapyval = mapMidY + (cube.transform.position.z / System.Math.Abs(lattitudevalue));
        mapyval = mapMidY + (posZ / System.Math.Abs(lattitudevalue));

        //longitude
        //mapxval = mapMidX + (cube.transform.position.x / System.Math.Abs(longitudevalue));
        mapxval = mapMidX + (posX / System.Math.Abs(longitudevalue));

        return new Vector2(mapyval, mapxval);
    }
    public Vector2 LatLongToVector2(Vector2 _latLong)
    {
        float x = (_latLong.x - mapMidX) * -longitudevalue;
        float y = (_latLong.y - mapMidY) * lattitudevalue;
        return new Vector2(x, y);
    }
    #endregion

}