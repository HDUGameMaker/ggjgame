using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartAction : MonoBehaviour
{
    public Image Image; // RawImage component
    public Vector2 speed = new Vector2(150f, 150f); // Speed of movement

    private RectTransform rectTransform; // RectTransform of the RawImage
    private Vector2 direction = Vector2.one; // Initial direction of movement

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = Image.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move the RawImage
        rectTransform.anchoredPosition += direction * speed * Time.deltaTime;

        // Get the size of the canvas
        Vector2 canvasSize = rectTransform.parent.GetComponent<RectTransform>().sizeDelta;

        // Get the size of the RawImage
        Vector2 rawImageSize = rectTransform.sizeDelta;

        // Check for collision with the canvas boundaries and bounce back
        if (rectTransform.anchoredPosition.x + rawImageSize.x / 2 > canvasSize.x / 2 || rectTransform.anchoredPosition.x - rawImageSize.x / 2 < -canvasSize.x / 2)
        {
            direction.x = -direction.x;
        }
        if (rectTransform.anchoredPosition.y + rawImageSize.y / 2 > canvasSize.y / 2 || rectTransform.anchoredPosition.y - rawImageSize.y / 2 < -canvasSize.y / 2)
        {
            direction.y = -direction.y;
        }
    }
}
