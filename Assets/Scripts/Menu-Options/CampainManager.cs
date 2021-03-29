using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampainManager : MonoBehaviour
{
    public static CampainManager instance;
    private static string campainSelected;
    private Button[] buttonList;
    [SerializeField] private Button backBtn;
    [SerializeField] private GameObject gridLayout;
    [SerializeField] private CharacterList characterList = new CharacterList();
    private List<GameObject> characterOptions = new List<GameObject>();

    public void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        buttonList = gameObject.GetComponentsInChildren<Button>();
        for (int i = 0; i < buttonList.Length - 1; i++)
        {
            int index = i;
            buttonList[index].onClick.AddListener(() => ChangeCampainSelected(index));
        }
        backBtn.onClick.AddListener(() => DestroyAllInstantiate());
    }

    void ChangeCampainSelected(int index)
    {
        string[] temp = buttonList[index].name.Split(char.Parse("B"));
        campainSelected = temp[0];
        print(campainSelected);
        ReadJson("Data/Menu/Characters");
    }
    void ReadJson(string route)
    {
        TextAsset asset = Resources.Load(route) as TextAsset;
        if (asset != null)
        {
            characterList = JsonUtility.FromJson<CharacterList>(asset.text);
            InstantiateCharacterOption();
        }
        else
        {
            print("asset is null");
        }
    }
    void InstantiateCharacterOption()
    {
        foreach (Character charac in characterList.Characters)
        {
            if (charac.Campaign == campainSelected)
            {
                GameObject characterOption = Instantiate(Resources.Load("Prefabs/MenuPrefabs/CharacterOption")) as GameObject;
                characterOption.transform.SetParent(gridLayout.transform, false);
                characterOption.name = charac.CharacterName;
                characterOption.transform.Find("Character/ImageCharacter").gameObject.GetComponent<Image>().sprite = charac.Picture;
                characterOption.transform.Find("Character/TextBackground/NameCharacter").gameObject.GetComponent<Text>().text = charac.CharacterName;
                characterOption.transform.Find("Description/OrigenCharacter").gameObject.GetComponent<Text>().text = charac.Origin;
                characterOption.transform.Find("Description/AgeCharacter").gameObject.GetComponent<Text>().text = charac.Age.ToString();
                characterOption.transform.Find("Description/CampainCharacter").gameObject.GetComponent<Text>().text = charac.Campaign;
                characterOptions.Add(characterOption);
            }
        }

    }
    void DestroyAllInstantiate()
    {
        for (int i = 0; i < characterOptions.Count; i++)
        {
            Destroy(characterOptions[i]);
        }
    }
}
