using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Button nextButton;
    
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;
    
    public Image portraitImage;
    public Image dialogueArea;
    
    public string jsonFileName;

    private DialogueQueue<DialogueData.DialogueLine> dialogueQueue = new DialogueQueue<DialogueData.DialogueLine>();

    void Start()
    {
        LoadDialogue();
        ShowDialogue();
    }

    void LoadDialogue()
    {
        //Checks if the assigned jsonFileName exists in the path
        string path = Path.Combine(Application.streamingAssetsPath, jsonFileName);

        if (File.Exists(path))
        {
            //Reads and converts all JSON data
            string jsonText = File.ReadAllText(path);
            
            //Assigns all the data to dialogueData
            DialogueData.DialogueContainer dialogueData = JsonUtility.FromJson<DialogueData.DialogueContainer>(jsonText);

            // Enqueue all dialogues lines into the queue
            foreach (var dialogue in dialogueData.dialogues)
            {
                dialogueQueue.Enqueue(dialogue);
            }
        }
        else
        {
            Debug.LogError("Failed to load dialogue JSON");
        }
    }

    void ShowDialogue()
    {
        if (!dialogueQueue.IsEmpty())
        {
            var currentLine = dialogueQueue.Dequeue();

            speakerText.text = currentLine.speaker;
            dialogueText.text = currentLine.lines[0];

            // Load and set portrait
            string spritePath = $"Art/Sprites/{currentLine.portrait}";
            Sprite loadedSprite = Resources.Load<Sprite>(spritePath);
            if (loadedSprite != null)
            {
                portraitImage.sprite = loadedSprite;
            }
            else
            {
                Debug.LogError($"Failed to load sprite at: {spritePath}.");
            }

            // Play sound effect if available
            if (!string.IsNullOrEmpty(currentLine.sfxName))
            {
                SFXManager.Instance.PlayGlobalSound(currentLine.sfxName, 1f);
            }
        }
        else
        {
            EndDialogue();
        }
    }


    //Method to display the next line of dialogue to the next button
    public void NextLine()
    {
        SFXManager.Instance.PlaySoundOnObject("Button", this.gameObject, 0.5f);
        if (!dialogueQueue.IsEmpty())
        {
            ShowDialogue();
        }
        else
        {
            EndDialogue();
        }
    }

    //Method that Turns off the dialogue relateds UI when the last line has been dequeued
    private void EndDialogue()
    {
        nextButton.gameObject.SetActive(false);
        speakerText.gameObject.SetActive(false);
        dialogueText.gameObject.SetActive(false);
        portraitImage.gameObject.SetActive(false);
        dialogueArea.gameObject.SetActive(false);
        Debug.Log("Dialogue finished.");
    }
}
