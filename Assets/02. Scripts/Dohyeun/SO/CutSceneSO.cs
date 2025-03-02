using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CutSceneSO", menuName = "Scriptable Objects/CutSceneSO")]

public class CutSceneSO : ScriptableObject
{
    // 한 개 Item 당 한 개의 이벤트
    public CutSceneData[] CutsceneDatas;
}
[Serializable]
public class CutSceneData
{
    public string Id;

    // 한 개 Item 당 BG1, Texts 구성
    public OneCutSceneData[] OneCutscenes;
}
[Serializable]
public class OneCutSceneData
{
    public Sprite Background;
    public Vector2 TextPosition;
    public string[] Texts;

}

// 나중에 아래 아이디어 참고해서 개선

//public class CutSceneSO : ScriptableObject
//{
//    public CutSceneData[] CutSceneDatas;
//}
//[Serializable]
//public class CutSceneData // 이벤트 당 한 개의 CutSceneData
//{
//    public string Id;
//    public OneClickData[] OneClickDatas; // 전부 다 큐에 넣어
//}
//[Serializable]
//public class OneClickData // 
//{
//    public Sprite Background;
//    public OneClickSet[] sprites;
//}
//[Serializable]
//public class SpriteSet : OneClickSet
//{
//    public Sprite Sprite;
//}
//[Serializable]
//public class TextSet : OneClickSet
//{
//    public string Text;
//}
//[Serializable]
//public class OneClickSet
//{
//    public Vector2 Position;
//}