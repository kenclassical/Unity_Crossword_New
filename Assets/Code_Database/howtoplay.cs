using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class howtoplay : MonoBehaviour
{
    public Sprite Image1;
    public Sprite Image2;
    public Sprite Image3;
    public Sprite Image4;
    public Image imageAdd;
    public GameObject HowToPlayShow;
    public GameObject B;
    public GameObject N;
    private int num = 1;
    public void OnClcikHowToPlay(){
        HowToPlayShow.SetActive(true);
        B.SetActive(false);
        imageAdd.sprite = Image1;
    }

    public void Next(){
        num++;
        if(num == 1){
            imageAdd.sprite = Image1;
            B.SetActive(false);
        }else if(num == 2){
            imageAdd.sprite = Image2;
            B.SetActive(true);
        }else if(num == 3){
            imageAdd.sprite = Image3;
            N.SetActive(true);
        }else if(num == 4){
            imageAdd.sprite = Image4;
            N.SetActive(false);
        }
    }

    public void Exit(){
        HowToPlayShow.SetActive(false);
    }

    public void Back(){
        num--;
        if(num == 1){
            imageAdd.sprite = Image1;
            B.SetActive(false);
        }else if(num == 2){
            imageAdd.sprite = Image2;
            B.SetActive(true);
        }else if(num == 3){
            imageAdd.sprite = Image3;
            N.SetActive(true);
        }else if(num == 4){
            imageAdd.sprite = Image4;
            N.SetActive(false);
        }
    }
}
