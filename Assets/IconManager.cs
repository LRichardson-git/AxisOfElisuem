using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconManager : MonoBehaviour
{

    public static IconManager instance;
    public Dictionary<string, Sprite> IconDicontioanry;
    private IconManager audioSource;

    public List<Sprite> iconList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        IconDicontioanry = new Dictionary<string, Sprite>();

        foreach (Sprite sprite in iconList)
        {
            AddIcon(sprite.name, sprite);
        }







    }

    public void AddIcon(string name, Sprite clip)
    {
        IconDicontioanry.Add(name, clip);
    }


    public Sprite getIcon(string name)
    {
        Debug.Log(name);
        if (IconDicontioanry.ContainsKey(name))
            return IconDicontioanry[name];
        else
            Debug.Log("IconNotFound");

        return IconDicontioanry["Fire"];
    }

}
