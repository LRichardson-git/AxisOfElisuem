
using UnityEngine;
using TMPro;
public sealed class MainView : View
{
    [SerializeField]
    private TextMeshProUGUI TurnText;

    public static MainView Instance { get; private set; }

    private void Start()
    {
        Instance = this;
        TurnText.text = "Enemy Activity";
    }

    



}
