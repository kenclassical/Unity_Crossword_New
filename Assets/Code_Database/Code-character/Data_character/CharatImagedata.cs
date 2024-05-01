using System;
using UnityEngine;

[Serializable]
public class CharatImagedata
{
    public int id;
    public string charatletter;
    public Sprite charImage;
    public int scorecharat;
    public CharatImagedata(int Id,string Charatletter,int Scorecharat,Sprite CharImage){
        id = Id;
        charatletter = Charatletter;
        charImage = CharImage;
        scorecharat =Scorecharat;
    }

}
