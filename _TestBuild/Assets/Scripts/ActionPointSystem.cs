using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPoint : MonoBehaviour
{
    public int APPoints = 0;
    public int maxAPpoints = 100;


    // set AP points to max at start
    void Start()
    {
        APPoints = maxAPpoints;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Increment AP by one 
    public void IncreaseAP()
    {
        if(APPoints < maxAPpoints)
            APPoints++;
    }
    // Overload - Increase by a specific amount
    public void IncreaseAP(int amount)
    {
        if((APPoints += amount) < maxAPpoints && amount > 0)
            APPoints += amount;
    }

    // Set the AP to a specific value
    public void setAP(int value)
    {
        //bounds checking
        if(value > 0 && value < maxAPpoints)
            APPoints = value;
    }

    // Get the AP
    public int getAP()
    {
        return APPoints;
    }

    // Reset the AP to max
    public void resetAP()
    {
        APPoints = maxAPpoints;
    }


}

