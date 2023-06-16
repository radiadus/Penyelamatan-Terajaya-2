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
        StreamReader file = File.OpenText("Assets/Resources/NPC Dialogues.csv");
        string line = "";
        while ((line = file.ReadLine()) != null)
        {
            string[] data = line.Split(';');
            if (!dialogues.ContainsKey(data[0]))
            {
                dialogues.Add(data[0], new Dictionary<int, List<string>>());
            }
            List<string> pages = new List<string>();
            int index = 2;
            while (index < data.Length)
            {
                pages.Add(data[index]);
                index++;
            }
            dialogues[data[0]].Add(int.Parse(data[1]), pages);
        }
        file.Close();
        return dialogues;
    }

    public List<string> GetDialoguesByCodeAndId(string code, int id)
    {
        return dialogueBank.dialogues[code][id];
    }
}
