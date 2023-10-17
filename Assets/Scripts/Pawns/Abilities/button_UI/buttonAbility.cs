using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class buttonAbility : MonoBehaviour
{
    Ability ability;
    [SerializeField]
    private TextMeshProUGUI Number;
    
    public TextMeshProUGUI uses;

    public Image icon;
    public void init(Ability ability)
    {
        this.ability = ability;
        this.gameObject.GetComponent<Button>().onClick.AddListener(activate);
        
        if (ability.uses < 100)
            uses.text = ability.uses.ToString();
    }

    public void activate()
    {
        AbilityManager.Instance.activate(ability);


    }

    private void OnMouseUpAsButton()
    {
        activate();
    }

    public void setnum(int i)
    {
        Number.text = i.ToString();
    }
    public void SetIcon(Sprite sprite)
    {
        icon.sprite = sprite;
    }
}
