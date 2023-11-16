using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShowHitDmg : MonoBehaviour
{
    public CameraControler cameraa;
    public bool hit;
    public TextMeshPro dmgHitText;
    public GameObject graphic;
    SpriteRenderer holder;
    SpriteRenderer diamond;
    Color invis;
    int y = 2;
    public int speedTOgo;
    float timee = 3.5f;
    //bool active = false; //avoid weird bug
    private void Start()
    {
        cameraa = CameraControler.LocalInstance;
        
        if (!hit)
        {
            y = 0;
            dmgHitText.color = Color.red;
            timee = 1.5f;
        }
        invis.a = 0;
        holder = graphic.GetComponent<SpriteRenderer>();
        diamond = graphic.GetComponentInChildren<SpriteRenderer>();
        graphic.SetActive(false);
        Vector3 temp = new Vector3(graphic.transform.localPosition.x, graphic.transform.localPosition.y + y, graphic.transform.localPosition.z);
        graphic.transform.localPosition = temp;
    }

    private void FixedUpdate()
    {
        transform.LookAt(cameraa.transform.position);


    }

    public void ShowDmghit(int dmgHit, int ID)
    {
        graphic.SetActive(true);
        Unit unit = null;
        foreach (Unit unitp in UnitManager.Instance.GetUnitList())
            if (unitp.getID() == ID)
            {
                unit = unitp;
                break;
            }

        if (unit == null)
            Debug.Log("Null");

        CameraControler.LocalInstance.SetCameraUnit(unit.transform.position, 200);

        

        
        
        StartCoroutine(FadeIn());

        if (hit)
            dmgHitText.text = "%" + dmgHit;
        else
            dmgHitText.text = dmgHit + " *";

        transform.position = unit.transform.position;

    }

    IEnumerator FadeIn()
    {

        holder.color = invis;
        diamond.color = invis;
        
        Color TempColor = holder.color;
        Color textColor = dmgHitText.color;
        dmgHitText.color = invis;
        textColor.a = 0;
        while (TempColor.a < 1f)
        {
            TempColor.a += Time.deltaTime / 0.5f;
            textColor.a += Time.deltaTime / 0.5f;

            if (TempColor.a > 1f)
                TempColor.a = 1.0f;

            holder.color = TempColor;
            diamond.color = TempColor;
            dmgHitText.color = textColor;
            yield return null;
        }

            Invoke("goInvis", timee);

    }

    void goInvis() { graphic.SetActive(false); }

}
