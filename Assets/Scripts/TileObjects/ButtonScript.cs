using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ButtonScript : MonoBehaviour
{
    public TargetData data;
    public TextMeshProUGUI hitChance;
    public void init(TargetData data)
    {
        this.data = data;
        hitChance.text = "" + data.getHit() + "%";
    }


}
