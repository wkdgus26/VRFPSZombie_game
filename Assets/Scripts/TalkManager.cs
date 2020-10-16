using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour {

    Dictionary<int, string[]> talkData;


	void Awake ()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
	}
	
	// Update is called once per frame
	void GenerateData()
    {
        talkData.Add(100, new string[] { "이런,", "길을 잘못들엇군" });
        talkData.Add(200, new string[] { "저기로", "가면 되겠군" });
    }

    public string GetTalk(int id, int talkIndex)
    {
        return talkData[id][talkIndex];

    }
}
