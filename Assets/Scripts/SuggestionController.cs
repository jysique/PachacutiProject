using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuggestionController : MonoBehaviour
{
    public static SuggestionController instance { get; private set; }
    private List<string> data = new List<string>();
    private int progress = 0;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadChapterFile(string fileName)
    {
        string file = Resources.Load<TextAsset>("Data/Dialogue/Tutorial/" + fileName).text;
        data = new List<string>(file.Split('\n'));
        progress = 0;
        ChapterController.instance.cachedLastSpeaker = "";
    }
}
