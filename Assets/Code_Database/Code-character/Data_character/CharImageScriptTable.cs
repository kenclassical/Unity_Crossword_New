using System.Collections.Generic;
using UnityEngine;
public class CharImageScriptTable : MonoBehaviour
{
    public static List<CharatImagedata> charatList = new List<CharatImagedata>();

    void Awake() {
        charatList.Add (new CharatImagedata(0,"A",1,Resources.Load<Sprite>("A")));
        charatList.Add (new CharatImagedata(1,"B",3,Resources.Load<Sprite>("B")));
        charatList.Add (new CharatImagedata(2,"C",3,Resources.Load<Sprite>("C")));
        charatList.Add (new CharatImagedata(3,"D",2,Resources.Load<Sprite>("D")));
        charatList.Add (new CharatImagedata(4,"E",1,Resources.Load<Sprite>("E")));
        charatList.Add (new CharatImagedata(5,"F",4,Resources.Load<Sprite>("F")));
        charatList.Add (new CharatImagedata(6,"G",2,Resources.Load<Sprite>("G")));
        charatList.Add (new CharatImagedata(7,"H",4,Resources.Load<Sprite>("H")));
        charatList.Add (new CharatImagedata(8,"I",1,Resources.Load<Sprite>("I")));
        charatList.Add (new CharatImagedata(9,"J",8,Resources.Load<Sprite>("J")));
        charatList.Add (new CharatImagedata(10,"K",5,Resources.Load<Sprite>("K")));
        charatList.Add (new CharatImagedata(11,"L",1,Resources.Load<Sprite>("L")));
        charatList.Add (new CharatImagedata(12,"M",3,Resources.Load<Sprite>("M")));
        charatList.Add (new CharatImagedata(13,"N",1,Resources.Load<Sprite>("N")));
        charatList.Add (new CharatImagedata(14,"O",1,Resources.Load<Sprite>("O")));
        charatList.Add (new CharatImagedata(15,"P",3,Resources.Load<Sprite>("P")));
        charatList.Add (new CharatImagedata(16,"Q",10,Resources.Load<Sprite>("Q")));
        charatList.Add (new CharatImagedata(17,"R",1,Resources.Load<Sprite>("R")));
        charatList.Add (new CharatImagedata(18,"S",1,Resources.Load<Sprite>("S")));
        charatList.Add (new CharatImagedata(19,"T",1,Resources.Load<Sprite>("T")));
        charatList.Add (new CharatImagedata(20,"U",1,Resources.Load<Sprite>("U")));
        charatList.Add (new CharatImagedata(21,"V",4,Resources.Load<Sprite>("V")));
        charatList.Add (new CharatImagedata(22,"W",4,Resources.Load<Sprite>("W")));
        charatList.Add (new CharatImagedata(23,"X",8,Resources.Load<Sprite>("X")));
        charatList.Add (new CharatImagedata(24,"Y",4,Resources.Load<Sprite>("Y")));
        charatList.Add (new CharatImagedata(25,"Z",10,Resources.Load<Sprite>("Z")));
    }
}