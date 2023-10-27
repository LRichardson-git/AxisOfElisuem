using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health : MonoBehaviour
{
    public List<GameObject> Health;
    private void Start()
    {
        Health = new List<GameObject>();

        foreach (Transform trans in transform)
        {
            Health.Add(trans.gameObject);
        }

    }


    public void setSize(int hp)
    {


        for (int i =0; i < Health.Count ; i++)
        {
            if (i > hp - 1)
            {
                Health[i].SetActive(false); 
            }
            else
            {
                Health[i].SetActive(true); 
            }
        }

    }

}
