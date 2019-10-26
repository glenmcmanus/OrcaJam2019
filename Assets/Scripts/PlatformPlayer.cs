using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPlayer : MonoBehaviour
{
    Vector2 movement;


    // Update is called once per frame
    void Update()
    {
        movement = Vector2.zero;
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            movement = Vector2.left;
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            movement = Vector2.right;
        }

        if(Input.GetKey(KeyCode.UpArrow))
        {
            //jump
        }
        else if(Input.GetKey(KeyCode.DownArrow))
        {
            //crouch
        }
    }
}
