using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsTest : MonoBehaviour
{
    public GameObject[] bullettypes;
    double spawntimer = 0;
    double roundtimer = 0;
    int pattern = 0;
    public bool inprogress = false;
    public GameObject lastbullet;

    private float spawninterval = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        //inprogress = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (inprogress)
        {
            spawntimer += Time.deltaTime;
            roundtimer += Time.deltaTime;

            //bullet pattern #0
            if (pattern == 0)
            {
                //generates a new bullet after a certain amount of time
                if (spawntimer > spawninterval)
                {
                    int dir = Random.Range(0, 4);
                    float xypos = Random.Range(-1.4f, 1.4f);
                    float x, y;

                    if (dir == 0) //up. bullets generate at xypos x, -1.5 y
                    {
                        x = xypos;
                        y = -2f;
                    }
                    else if (dir == 1) //right. bullets generate at -1.5 x, xypos y
                    {
                        x = -2f;
                        y = xypos;
                    }
                    else if (dir == 2) //down. bullets generate at xypos x, 1.5 y
                    {
                        x = xypos;
                        y = 2f;
                    }
                    else//left. bullets generate at 1.5 x, xypos y
                    {
                        x = 2f;
                        y = xypos;
                    }
                    GenerateBullet(0, x, y);

                    //change direction of generated bullet
                    lastbullet.GetComponent<BulletSimple>().dir = dir;

                    //reset spawntimer
                    spawntimer -= spawninterval;
                }
                //exits the current pattern after a certain amount of time
                if (roundtimer > 10)
                {
                    inprogress = false;
                    spawntimer = 0;
                    roundtimer = 0;
                }
            }
        }
    }

    void GenerateBullet(int id, float x, float y) //xypos is the position in either the x or y direction, as determined by dir (direction)
    {
        //error prevention
        if (id < 0 || id > bullettypes.Length) { return; }

        Vector3 pos = new Vector3(x, y, 0);
        Quaternion rot = new Quaternion(0, 0, 0, 0);
        lastbullet = Instantiate(bullettypes[id],pos,rot);
    }
}
