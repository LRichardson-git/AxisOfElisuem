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
    List<GameObject> spawnedButtons;
    public static AbilityManager Instance;
    Vector3 origin;
    public List<GameObject> smokeCloud;
    public GameObject Shooiting_View;




    private void Start()
    {
        cam = Camera.main;
        _input = InputManager.Instance;
        spawnedButtons = new List<GameObject>();
        Instance = this;
        smokeCloud = new List<GameObject>();
    }

    public void activate(Ability ability)
    {
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

        //Debug.Log(active);
        if (!active || currentAbility == null)
            return;


        if (_input.cancelAction)
        {
            currentAbility.deActivate();
            deactivate();
            return;
        }
        


        ray = cam.ScreenPointToRay(_input.MousePos);

        if (Physics.Raycast(ray, out hit) && Vector3.Distance(origin, hit.point) <= currentAbility.Range * 10) 
        {
            currentAbility.Target(hit.point);
            if (_input.leftClick)
            {
                currentAbility.Execute(hit.point);
                deactivate();
            }


        }

   
        

    }


    

    public void deactivate()
    {
        currentAbility = null;
        active = false;
        Shooiting_View.SetActive(false);
    }


    public void createButtons(Unit unit)
    {
        // Remove any previously spawned buttons
        foreach (GameObject button in spawnedButtons)
        {
            Destroy(button);
        }
        spawnedButtons.Clear();

        if (unit.Abilities.Count <= 0) return;
        // Calculate the position of the bottom right corner of the screen
        float buttonWidth = buttonPrefab.GetComponent<RectTransform>().rect.width;
        float buttonSpacing = buttonWidth * 0.15f;

        //spawn based on centre
        Vector2 spawnPosition = new Vector2((Screen.width / 2) - ((buttonWidth + buttonSpacing) * unit.Abilities.Count), 70);


        // Get the width of the button prefab
        

        // Calculate the spacing between buttons
        // Adjust this value as needed

        // Spawn new buttons based on the number of objects
        for (int i = 0; i < unit.Abilities.Count; i++)
        {
            // Adjust the spawn position based on the button width and spacing
            spawnPosition += new Vector2(buttonWidth + buttonSpacing, 0);

            GameObject button = Instantiate(buttonPrefab, spawnPosition, Quaternion.identity, transform);
            spawnedButtons.Add(button);
            button.GetComponent<buttonAbility>().init(unit.Abilities[i]);
        }


    }

    
   




}
