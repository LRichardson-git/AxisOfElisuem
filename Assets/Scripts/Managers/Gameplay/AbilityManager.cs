using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{

    Ray ray;
    Camera cam;
    RaycastHit hit;
    public bool active;
    Ability currentAbility;
    InputManager _input;
    public GameObject buttonPrefab;
    List<buttonAbility> spawnedButtons;
    public static AbilityManager Instance;
    Vector3 origin;
    public List<GameObject> smokeCloud;
    public GameObject Shooiting_View;




    private void Start()
    {
        cam = Camera.main;
        _input = InputManager.Instance;
        spawnedButtons = new List<buttonAbility>();
        Instance = this;
        smokeCloud = new List<GameObject>();
    }

    public void activate(Ability ability)
    {


        deactivate();

        currentAbility = ability;
        currentAbility.setup();
        active = true;
        Shooiting_View.gameObject.SetActive(true);
        origin = ObjectSelector.Instance.getSelectedUnit().transform.position;
        Shooting_View_Controller.Instance.UpdateInfo(ability);
    }

    public void addsmoke(GameObject smoke)
    {
        smokeCloud.Add(smoke);
    }

    public void removeSmoke(GameObject smoke)
    {
        smokeCloud.Remove(smoke);
    }

    void Update()
    {

        if (!active || currentAbility == null)
            return;


        if (_input.cancelAction)
        {
            currentAbility.deActivate();
            deactivate();
            Shooiting_View.SetActive(false);
            return;
        }
        
        ray = cam.ScreenPointToRay(_input.MousePos);

        if (Physics.Raycast(ray, out hit) && Vector3.Distance(origin, hit.point) <= currentAbility.Range * 10) 
        {
            currentAbility.Target(hit.point);
            if (_input.leftClick)
            {
                Shooiting_View.SetActive(false);
                currentAbility.Execute(hit.point);
                deactivate();
                
            }


        }

   
        

    }


    public void pressButton(int i)
    {

        Debug.Log(i-1);
        Debug.Log(spawnedButtons.Count);
        Debug.Log(spawnedButtons[i - 1].name);

        if (i !> spawnedButtons.Count -1)
            spawnedButtons[i - 1].activate();


    }



    public void deactivate()
    {
        currentAbility = null;
        active = false;
        
    }


    public void createButtons(Unit unit)
    {
        
        // Remove any previously spawned buttons
        foreach (buttonAbility button in spawnedButtons)
        {
            Destroy(button.gameObject);
        }
        spawnedButtons.Clear();

        if (unit.Abilities.Count <= 0) return;

        float buttonWidth = buttonPrefab.GetComponent<RectTransform>().rect.width;
        float buttonSpacing = buttonWidth * 0.15f;

        //spawn based on centre
        Vector2 spawnPosition = new Vector2((Screen.width / 2) - ((buttonWidth + buttonSpacing) * unit.Abilities.Count), 70);


        for (int i = 0; i < unit.Abilities.Count; i++)
        {
            // Adjust the spawn position based spacing
            spawnPosition += new Vector2(buttonWidth + buttonSpacing, 0);

            buttonAbility button = Instantiate(buttonPrefab, spawnPosition, Quaternion.identity, transform).GetComponent<buttonAbility>();
            spawnedButtons.Add(button);
            button.init(unit.Abilities[i]);
            button.setnum(i + 1);
        }


    }


}
