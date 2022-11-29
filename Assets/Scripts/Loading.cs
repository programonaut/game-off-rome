using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    public Transform pointer;
    public float perc = 0;
    public float speed = .3f;
    void Update()
    {
        perc += speed * Time.deltaTime;
        if (perc > 1) {
            perc = 0;
        }
        pointer.localRotation = Quaternion.Euler(0, 0, perc * -360);
    }
}
