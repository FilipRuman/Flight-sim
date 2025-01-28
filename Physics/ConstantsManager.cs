using Godot;
using System;

public static class ConstantsManager
{
    //   i use this script to save constants that are widely used like gravity
    // ones that are specific to 1 script go to the script that uses it 

    public const float gravity = 9.80665f; /* gravity in m/s^2 - let's assume this doesn't change with altitude  */
}
