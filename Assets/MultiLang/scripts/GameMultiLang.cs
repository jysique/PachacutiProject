using System;
using UnityEngine;
using System.Collections.Generic;


// Created by: Hamza Herbou        (mobile games developer)
// email     : hamza95herbou@gmail.com


public class GameMultiLang : MonoBehaviour
{
	public static GameMultiLang instance;
	
	public static Dictionary<String, String> Fields;
	public static Dictionary<String, String> Fields_Events;

	[SerializeField] string defaultLang = "en";
	//[SerializeField] GameObject canvas;

	void Awake ()
	{
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}

		LoadLanguage ();
	}


	void LoadLanguage ()
	{
		if (Fields == null)
			Fields = new Dictionary<string, string> ();
		if (Fields_Events == null)
			Fields_Events = new Dictionary<string, string>();

		Fields.Clear ();
		Fields_Events.Clear();
		string lang = PlayerPrefs.GetString ("_language", defaultLang);


		if (PlayerPrefs.GetInt ("_language_index", -1) == -1)
			PlayerPrefs.SetInt ("_language_index", 0);

		string allTexts = (Resources.Load (@"Languages/" + lang) as TextAsset).text; //without (.txt)
		string allTexts_events = (Resources.Load(@"Languages/events-" + lang) as TextAsset).text; //without (.txt)

		string[] lines = allTexts.Split (new string[] { "\r\n", "\n" }, StringSplitOptions.None);
		string key, value;

		for (int i = 0; i < lines.Length; i++) {
			if (lines [i].IndexOf ("=") >= 0 && !lines [i].StartsWith ("#")) {
				key = lines [i].Substring (0, lines [i].IndexOf ("="));
				value = lines [i].Substring (lines [i].IndexOf ("=") + 1,
					lines [i].Length - lines [i].IndexOf ("=") - 1).Replace ("\\n", Environment.NewLine);
				value = value.Replace("&", "\n");
				Fields.Add (key, value);
			}
		}

		string[] lines_events = allTexts_events.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
		string key_events, value_events;

		for (int i = 0; i < lines_events.Length; i++)
		{
			if (lines_events[i].IndexOf("=") >= 0 && !lines_events[i].StartsWith("#"))
			{
				key_events = lines_events[i].Substring(0, lines_events[i].IndexOf("="));
				value_events = lines_events[i].Substring(lines_events[i].IndexOf("=") + 1,
					lines_events[i].Length - lines_events[i].IndexOf("=") - 1).Replace("\\n", Environment.NewLine);
				value_events = value_events.Replace("&", "\n");
				Fields_Events.Add(key_events, value_events);
			}
		}
	}


	public static string GetTraductionEvents(string key)
	{
		if (!Fields_Events.ContainsKey(key))
		{
			Debug.LogError("There is no key with name: [" + key + "] in your text files");
			return null;
		}

		return Fields_Events[key];
	}
	public static string GetTraduction (string key)
	{
		if (!Fields.ContainsKey (key)) {
			Debug.LogError ("There is no key with name: [" + key + "] in your text files");
			return null;
		}

		return Fields [key];
	}

	public static string GetTraductionReverse(string value)
	{
		if (!Fields.ContainsValue(value))
		{
			Debug.LogError("There is no value with name: [" + value + "] in your text files");
			return null;
		}
		string a = "";
        foreach (var item in Fields)
        {
            if (item.Value==value)
            {
				a = item.Key;
            }
        }
		return a;
		//return Fields[key];
	}

	public void TranslateAllText()
    {
		int i = 0;
		LoadLanguage();
		TextMeshProTranslator[] translates = Resources.FindObjectsOfTypeAll<TextMeshProTranslator>();
		foreach (TextMeshProTranslator text in translates)
		{
			i++;
			text.Translate();
		}
//		print("i|" + i);
	}

}
