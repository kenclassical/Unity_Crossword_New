using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System;
using Photon.Pun;

public class DorpTable : MonoBehaviour,IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public string CurrentLetter;
    public int Points;
    public string TempLetter;

    public Sprite CharImageGrid;

    public Text ScoreForWord;

    public bool HasLetter;
    public bool HaveLetter;
    public int Row;
    public int Column;
    public int LetterMultiplier = 1;
    public int WordMultiplier = 1;

    private WordCheckGrid parent;

    void Start()
    {
        parent = transform.parent.gameObject.GetComponent<WordCheckGrid>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (eventData.pointerDrag == null)
            return;

        Drag d = eventData.pointerDrag.GetComponent<Drag>();
        if (d != null) {
            d.placeHolderParent = this.transform;
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (eventData.pointerDrag == null)
            return;

        Drag d = eventData.pointerDrag.GetComponent<Drag>();
        if (d != null && d.placeHolderParent == this.transform) {
            d.placeHolderParent = d.parentToReTurnTo;
        }
        if(!HaveLetter){
            HasLetter = false;
            CurrentLetter = "";
            Points = 0;
            CharImageGrid = null;
            eventData.pointerDrag.GetComponent<Image>().color = Color.white;
            parent.CurrentTiles.Remove(this);
            if (parent.CurrentTiles.Count == 1 || parent.CurrentTiles.Count == 0)
                parent.TableGrid = WordCheckGrid.Table.None;
            if (parent.isFirstTurn)
            {
                parent.TableGrid = WordCheckGrid.Table.None;
            }
        }
    }

    public void OnDrop(PointerEventData eventData) {
        Drag d = eventData.pointerDrag.GetComponent<Drag>();
        if (!HasLetter)
        {
            d.parentToReTurnTo = this.transform;
            d.transform.position = this.transform.position;
            if (parent.TableGrid == WordCheckGrid.Table.None ||
                (parent.TableGrid == WordCheckGrid.Table.Horizontal && Row == parent.CurrentTiles[0].Row) ||
                (parent.TableGrid == WordCheckGrid.Table.Vertical && Column == parent.CurrentTiles[0].Column))
            {
                parent.CurrentTiles.Add(this);
                if (parent.CurrentTiles.Count == 2)
                {
                    if (parent.CurrentTiles[0].Row == Row) parent.TableGrid = WordCheckGrid.Table.Horizontal;
                    else if (parent.CurrentTiles[0].Column == Column) parent.TableGrid = WordCheckGrid.Table.Vertical;
                    else
                    {
                        parent.CurrentTiles.RemoveAt(1);
                        return;
                    }
                }
                HasLetter = true;
                CurrentLetter = eventData.pointerDrag.GetComponent<charimage>().charatletter;
                Points = eventData.pointerDrag.GetComponent<charimage>().scorecharat;
                CharImageGrid = eventData.pointerDrag.GetComponent<charimage>().charImage;
                eventData.pointerDrag.GetComponent<Image>().color = Color.green;
                if(parent.CheckWords()){
                    parent.pointsShow = parent.Points();
                    ShowPointsShow();
                }
            }
        }
    }

    public void DeleteDrop(){
        if (transform.childCount > 0)
        {
            GameObject draggedObject = transform.GetChild(0).gameObject;
            Destroy(draggedObject);
        }
    }

    private void ShowPointsShow() {
        string pointsToShow = "+" + "(" + parent.pointsShow.ToString() + ")";
        
        if (PhotonNetwork.IsMasterClient) {
            string currentText = parent.ScorePlayer1.text;
            
            int index = currentText.IndexOf('+');
            if (index != -1) {
                currentText = currentText.Substring(0, index).Trim();
            }
            
            parent.ScorePlayer1.text = currentText + " " + pointsToShow;
        } else {
            string currentText = parent.ScorePlayer2.text;
            
            // Remove any existing pointsToShow
            int index = currentText.IndexOf('+');
            if (index != -1) {
                currentText = currentText.Substring(0, index).Trim();
            }
            
            parent.ScorePlayer2.text = currentText + " " + pointsToShow;
        }
    }
}
