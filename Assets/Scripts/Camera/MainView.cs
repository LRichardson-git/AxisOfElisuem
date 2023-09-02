
using UnityEngine;
using TMPro;
public sealed class MainView : View
{
    [SerializeField]
    private TextMeshProUGUI TurnText;

    [SerializeField]
    private TextMeshProUGUI AmmoText;

    private void Update()
    {
        if (!initialized) return;
        //safety stuff
        Player player = Player.LocalInstance;
        if (player == null) return;
      
        //display current pawns health
        TurnText.text = $"Turn: {player.turn}";

        

    }



}
