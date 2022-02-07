using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    [SerializeField] [Multiline(8)] [TextArea(1, 8)] string text;

    Color ogColor;

    void Start()
    {
        ogColor = gameObject.GetComponent<Renderer>().material.color;
    }

    public void Detection()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.yellow;
    }

    public void Undetection()
    {
        gameObject.GetComponent<Renderer>().material.color = ogColor;
    }

    public void Interaction()
    {
        print(text);
    }

}
