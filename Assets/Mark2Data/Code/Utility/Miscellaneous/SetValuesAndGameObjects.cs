using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetValuesAndGameObjects :MonoBehaviour
{
    public   float allySpeed;
    public   float neutralSpeed;
    public   float adversarySpeed;
    public string enemyTag;
    public string enemyMissileTag;
    public string playerMissileTag;
    public string allyTag;
    public string allyMissileTag;
    public string playerTag;
    public string neutralTag="Neutral";
    public   GameObject bullet;
    public   GameObject missile;
    public   GameObject flare;
    public int enemyMissileCount;
    public int playerMissileCount;
    public int allyMissileCount;
    public int enemyFlareCount;
    public int playerFlareCount;
    public int allyFlareCount;
}
