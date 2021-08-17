using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualNovelManager : MonoBehaviour
{
    List<string> data = new List<string>();
    private float timerCountDown = 0.8f;
    private int progress = 0;
    private void Start()
    {
        if (GlobalVariables.instance != null)
        {
            LoadChapterFile(GlobalVariables.instance.charac, GlobalVariables.instance.tittle);
        }
        else
        {
            LoadChapterFile("Hakan", "Start");
        }

    }
    private void Update()
    { 
        if (timerCountDown > 0)
        {
            timerCountDown -= Time.deltaTime;
        }
        else
        {
            ChapterController.instance.HandleLine(data[progress]);
            progress++;
            timerCountDown = 0.8f;
        }
    }
    private void LoadChapterFile(string character, string fileName)
    {
        string file = Resources.Load<TextAsset>("Data/Dialogue/" + character + "/" + fileName).text;
        data = new List<string>(file.Split('\n'));
        progress = 0;
        ChapterController.instance.cachedLastSpeaker = "";
    }
}
