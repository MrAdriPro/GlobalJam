using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Configuration
{
    //Variables
    public static float mouse_SensX = 100f;
    public static float mouse_SensY = 100f;

    public static float cam_tiltAmount = 1f;
    public static float cam_tiltSmooth = 23f;
    public static float cam_smoothMovement = 10f;
    public static float fov = 70f;
    public static bool normalCam = false;

    public enum Exceptions 
    {
        None,
        MenuException
    }

    public static string ToString(int menu) 
    {
        string message = "";
        if (Exceptions.MenuException.Equals(menu)) 
        {
            message = "Error loading the menu.";
        }

        return message;
    }
}
