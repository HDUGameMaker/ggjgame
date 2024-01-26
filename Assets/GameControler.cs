using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameControler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
public class Topic
{
    public string name;
    public List<Textstring> texts;
    public int id;
    public Topic nextTopic;
    public Topic leftTopic;
    public Topic rightTopic;
}
public class Textstring
{
    public float time;
    public string period;
}
public class Chosen
{
    public int topicId;
    public float startTime;
    public float endTime;
}
