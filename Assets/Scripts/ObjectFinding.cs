using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectFinding : MonoBehaviour
{
    public Transform parent;
    public TextMeshProUGUI txt;
    public Circle circle;
    int current = 0;
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Instantiate(circle, new(0, 0, 0), Quaternion.identity, parent);
            current++;
            txt.text = "Amount: " + current.ToString();
        }
    }
}
