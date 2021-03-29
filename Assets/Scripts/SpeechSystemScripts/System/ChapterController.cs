using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
public class ChapterController : MonoBehaviour
{
    List<string> data = new List<string>();
    int progress = 0;
    private string chapterName = GlobalVariables.instance.chapterTxt;
    public static ChapterController instance { get; private set;}
    float timerCountDown = 0.8f;

    // ChapterPachacuti_start
    private void Start()
    {
        LoadChapterFile(chapterName);
    }
    private void Update()
    {

        if (timerCountDown>0)
        {
            timerCountDown -= Time.deltaTime;
        }
        else
        {
            HandleLine(data[progress]);
            progress++;
            timerCountDown = 0.8f;
        }
        /*
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            HandleLine(data[progress]);
            progress++;
        }
        */
    }
    public void LoadChapterFile(string fileName)
    {
        //data = FileManager.LoadFile(FileManager.savPath + "Resources/Data/Dialogue/"+fileName);
        string file = Resources.Load<TextAsset>("Data/Dialogue/" + fileName).text;
        data = new List<string>(file.Split('\n'));
        progress = 0;
        cachedLastSpeaker = "";
    }
    void HandleLine(string line)
    {
        string[] dialogueAndActions = line.Split('"');

        if (dialogueAndActions.Length == 3)
        {
            HandleDialogue(dialogueAndActions[0], dialogueAndActions[1]);
            HandleEventsFromLine(dialogueAndActions[2]);
        }
        else
        {
            HandleEventsFromLine(dialogueAndActions[0]);
        }
    }
    string cachedLastSpeaker = "";
    void HandleDialogue(string dialogueDetalis, string dialogue)
    {
        string speaker = cachedLastSpeaker;
        bool additive = dialogueDetalis.Contains("+");
        if (additive)
        {
            dialogueDetalis = dialogueDetalis.Remove(dialogueDetalis.Length-1);
        }
        if (dialogueDetalis.Length>0)
        {
            if(dialogueDetalis[dialogueDetalis.Length-1]==' ')
            {
                dialogueDetalis = dialogueDetalis.Remove(dialogueDetalis.Length - 1);
            }
            speaker = dialogueDetalis;
            cachedLastSpeaker = speaker;
        }
        if (speaker != "narrator")
        {
            CharacterVN character = CharacterManagerVN.instance.GetCharacter(speaker);
            character.Say(dialogue, additive);
        }
        else
        {
            DialogueSystem.instance.Say(dialogue, speaker, additive);
        }
    }

    void HandleEventsFromLine(string events)
    {
        string[] actions = events.Split(' ');
        foreach (var act in actions)
        {
            HandleAction(act);
        }
    }
    void HandleAction(string action)
    {
        //print("Handle actions [" + action + "]");
        string[] data = action.Split('(', ')');
        switch (data[0])
        {
            case "setBackground":
                Command_SetLayerImage(data[1], BCFC.instance.background);
                break;
            case "setCinematic":
                Command_SetLayerImage(data[1], BCFC.instance.cinematic);
                break;
            case "setForeground":
                Command_SetLayerImage(data[1], BCFC.instance.foreground);
                break;
            case "playSound":
                Command_PlaySound(data[1]);
                break;
            case "playMusic":
                Command_PlayMusic(data[1]);
                break;
            case "move":
                Command_MoveCharacter(data[1]);
                break;
            case "setPosition":
                Command_SetPosition(data[1]);
                break;
            case "setExpression":
                Command_ChangeExpression(data[1]);
                break;
            case "changeScene":
                Command_ChangeScene(data[1]);
                break;
            default:
                break;
        }
    }

    void Command_SetLayerImage(string data, BCFC.LAYER layer)
    {
        string texName = data.Contains(",") ? data.Split(',')[0] : data;
        Texture2D tex = texName == "null"? null : Resources.Load("Textures/VNAssets/Backdrops/" + texName) as Texture2D;
        float speed = 2f;
        bool smooth = false;

        if (data.Contains(","))
        {
            string[] parameters = data.Split(',');
            foreach (var p in parameters)
            {
                float fVal = 0;
                bool bVal = false;
                if (float.TryParse(p,out fVal))
                {
                    speed = fVal; continue;
                }
                if (bool.TryParse(p, out bVal))
                {
                    smooth = bVal;continue;
                }
            }
        }
        layer.TransitionToTexture(tex, speed, smooth);
    }
    void Command_PlaySound(string data)
    {
        AudioClip clip = Resources.Load("Audio/SFX/" + data) as AudioClip;
        if (clip!=null)
        {
            AudioManager.instance.PlaySFX(clip);
        }
        else
        {
            Debug.LogError("Clip does not exist - " + data);
        }

    }
    void Command_PlayMusic(string data)
    {
        AudioClip clip = Resources.Load("Audio/Music/" + data) as AudioClip;
        if (clip != null)
        {
            AudioManager.instance.PlaySong(clip);
        }
        else
        {
            Debug.LogError("Clip does not exist - " + data);
        }
    }
    void Command_MoveCharacter(string data)
    {
        string[] parameters = data.Split(',');
        string character = parameters[0];
        float locationX = float.Parse(parameters[1]);
        float locationY = float.Parse(parameters[2]);
        float speed = parameters.Length == 4 ? float.Parse(parameters[3]) : 1f ;
        bool smooth = parameters.Length == 5 ? bool.Parse(parameters[4]) : true;

        CharacterVN c = CharacterManagerVN.instance.GetCharacter(character);
        c.MoveTo(new Vector2(locationX, locationY),speed,smooth);
    }
    void Command_SetPosition(string data)
    {
        string[] parameters = data.Split(',');
        string character = parameters[0];
        float locationX = float.Parse(parameters[1]);
        float locationY = float.Parse(parameters[2]);

        CharacterVN c = CharacterManagerVN.instance.GetCharacter(character);
        c.SetPosition(new Vector2(locationX, locationY));
    }
    void Command_ChangeExpression(string data)
    {
        string[] parameters = data.Split(',');
        
        string character = parameters[0];
        string region = parameters[1];
        string expression = parameters[2];
        float speed = parameters.Length == 4 ? float.Parse(parameters[3]) : 1f;

        CharacterVN c = CharacterManagerVN.instance.GetCharacter(character);
        Sprite sprite = c.GetSprite(expression);

        if (region.ToLower() == "body")
        {
            c.TransitionBody(sprite, speed,true);
        }
        if (region.ToLower() == "face")
        {
            c.TransitionExpression(sprite, speed, true);
        }
    }
    void Command_ChangeScene(string data)
    {
        SceneManager.LoadScene(int.Parse(data));
    }

}
