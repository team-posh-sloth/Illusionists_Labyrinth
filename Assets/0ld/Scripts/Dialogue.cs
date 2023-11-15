using UnityEngine; using TMPro;

namespace Old_Labyrinth
{
    public class Dialogue : Interactable
    {
        [SerializeField] GameObject textBubble;

        [SerializeField][Multiline(3)][TextArea(1, 3)] string[] text;

        int textState;

        protected override void Detect() { textBubble.GetComponent<Animator>().Play("popup"); setTextState(); } // textBubble pops up when detected (setText to default state [textState = 0])

        protected override void Undetect() { textBubble.GetComponent<Animator>().Play("popdown"); setTextState(-1); } // textBubble pops down when undetected (setText to inactive state n[textState = -1])

        protected override void Watch() { textBubble.transform.LookAt(new Vector3(Camera.main.transform.position.x, textBubble.transform.position.y, Camera.main.transform.position.z)); } // textBubble looks at camera (XZ only)

        protected override void Interact() { if (textState < 0) { Detect(); } else { textState++; setTextState(textState); } } // Interacting with textBubble increases textState

        void setTextState(int i = 0) { if (i <= 0) { textState = i; } if (i > text.Length - 1) { Undetect(); } else if (i > -1) { textBubble.GetComponentInChildren<TMP_Text>().text = text[i]; } } //Handles textState (-1 is inactive; 0+ sets text index)

    }
}