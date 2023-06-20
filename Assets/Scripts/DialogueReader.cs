using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogueReader : MonoBehaviour
{
    public static DialogueReader Instance { get; private set; }
    public DialogueBank dialogueBank;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        dialogueBank.dialogues = FetchDialogues();

    }

    private Dictionary<string, Dictionary<int, List<string>>> FetchDialogues()
    {
        Dictionary<string, Dictionary<int, List<string>>> dialogues = new Dictionary<string, Dictionary<int, List<string>>>();
        TextAsset text = Resources.Load<TextAsset>("NPC_Dialogues_2");
        string textData = text.text;
        string[] lines = textData.Split(Environment.NewLine);
        foreach (string line in lines)
        {
            string trimmed = line.Trim();
            string[] data = trimmed.Split(';');
            if (!dialogues.ContainsKey(data[0]))
            {
                dialogues.Add(data[0], new Dictionary<int, List<string>>());
            }
            List<string> pages = new List<string>();
            int index = 2;
            while (index < data.Length && data[index] != "")
            {
                pages.Add(data[index]);
                index++;
            }
            dialogues[data[0]].Add(int.Parse(data[1]), pages);
        }
        return dialogues;
    }

    public List<string> GetDialoguesByCodeAndId(string code, int id)
    {
        return dialogueBank.dialogues[code][id];
    }
}
