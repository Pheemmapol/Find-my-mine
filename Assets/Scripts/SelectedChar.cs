using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedChar : MonoBehaviour
{
    public CharacterDatabase characterDB;

    
    public Image artworkSprite;


    
    // Start is called before the first frame update
    void Start()
    {
        UpdateCharacter(Client.instance.NoChar);
    }
    
    public void UpdateCharacter(int selectedOption)
    {
        Character character = characterDB.GetCharacter(selectedOption);
        artworkSprite.sprite = character.characterSprite;
        Client.instance.NoChar = selectedOption;

    }
}
