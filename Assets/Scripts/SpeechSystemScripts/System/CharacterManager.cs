using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;
    public RectTransform characterPanel;

    public List<CharacterVN> characters = new List<CharacterVN>();

    public Dictionary<string, int> characterDictionary = new Dictionary<string, int>();

    private void Awake()
    {
        instance = this;
    }

    public CharacterVN GetCharacter(string characterName, bool createCharacterIfDoesntExist = true, bool enableCreatedCharacterOnStart = true)
    {
        int index = -1;
        if(characterDictionary.TryGetValue(characterName, out index))
        {
            return characters[index];
        }
        else if (createCharacterIfDoesntExist)
        {
            return CreateCharacter(characterName, enableCreatedCharacterOnStart);
        }

        return null;
    }
    public CharacterVN CreateCharacter(string characterName , bool enableOnStart = true)
    {
        CharacterVN newCharacter = new CharacterVN(characterName, enableOnStart);
        characterDictionary.Add(characterName, characters.Count);
        characters.Add(newCharacter);

        return newCharacter;
    }
    public class CHARACTERPOSITIONS
    {
        public Vector2 bottomLeft = new Vector2(0, 0);
        public Vector2 topRight = new Vector2(1f, 1f);
        public Vector2 center = new Vector2(0.5f, 0.5f);
        public Vector2 bottomRight = new Vector2(1f, 0.5f);
        public Vector2 topLeft = new Vector2(0f, 1f);
    }
    public static CHARACTERPOSITIONS characterPosition = new CHARACTERPOSITIONS();

    public class CHARACTEREXPRESSION
    {
        public int normal = 0;
        public int shy = 1;
        public int normalangle = 2;
        public int cojoinedFingers = 3;
    }
    public static CHARACTEREXPRESSION characterExpression = new CHARACTEREXPRESSION();
}
