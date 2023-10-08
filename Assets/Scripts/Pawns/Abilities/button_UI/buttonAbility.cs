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
    public void init(Ability ability)
    {
        this.ability = ability;
        this.gameObject.GetComponent<Button>().onClick.AddListener(activate);
    }

    public void activate()
    {
        AbilityManager.Instance.activate(ability);
        Debug.Log("Activing: " + ability.Name);

    }

    private void OnMouseUpAsButton()
    {
        activate();
    }

    public void setnum(int i)
    {
        Number.text = i.ToString();
    }

}
