using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueData : MonoBehaviour
{
    [Serializable]
    public class DialogueLine
    {
        public string speaker;
        public string portrait;
        public string[] lines;
        public string sfxName;
    }

    [Serializable]
    public class DialogueContainer
    {
        public List<DialogueLine> dialogues;
    }
}
