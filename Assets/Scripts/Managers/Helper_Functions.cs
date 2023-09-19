using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper_Functions 
{
    public static RaycastHit2D getWorldMouse()
    {
        Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero);
        return hit;
    }
}
