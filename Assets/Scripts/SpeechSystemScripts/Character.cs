using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Character
{ 
    public string characterName;
    [HideInInspector]public RectTransform root;
    public bool isMultiLayerCharacter { get { return renderers.renderder == null; } }

    DialogueSystem dialogue;
    public void Say(string speech)
    {
        dialogue.Say(speech, characterName);
    }


    public Character(string _name)
    {
        CharacterManager cm = CharacterManager.instance;
        GameObject prefab = Resources.Load("Prefabs/VNPrefabs/Character["+_name+"]") as GameObject;
        GameObject go = GameObject.Instantiate(prefab, cm.characterPanel);
        root = go.GetComponent<RectTransform>();
        characterName = _name;
        renderers.renderder = go.GetComponentInChildren<RawImage>();
        if (isMultiLayerCharacter)
        {
            renderers.bodyRender = go.transform.Find("bodyLayer").GetComponent<Image>();
            renderers.expresionRender = go.transform.Find("expressionLayer").GetComponent<Image>();
        }
        dialogue = DialogueSystem.instance;
    }
    [System.Serializable]
    public class Renderers
    {
        public RawImage renderder;
        public Image bodyRender;
        public Image expresionRender;

    }
    public Renderers renderers = new Renderers();
}
    