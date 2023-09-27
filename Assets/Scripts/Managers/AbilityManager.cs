using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{

    Ray ray;
    Camera cam;
    RaycastHit hit;
    bool active;
    Ability currentAbility;
    InputManager _input;
    public GameObject buttonPrefab;
    List<GameObject> spawnedButtons;
    public static AbilityManager Instance;
    bool skip1frame;
    private void Start()
    {
        cam = Camera.main;
        _input = InputManager.Instance;
        spawnedButtons = new List<GameObject>();
        Instance = this;
    }

    public void activate(Ability ability)
    {
        currentAbility = ability;

        //delayed cause wanna use left click but button insta registers
        Invoke("activ", 0.5f);
        
    }


    public void activ() { active = true; }

    void Update()
    {

        //Debug.Log(active);
        if (!active || currentAbility == null)
            return;


        ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            currentAbility.Target(hit.point);
            if (InputManager.Instance.leftClick)
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
