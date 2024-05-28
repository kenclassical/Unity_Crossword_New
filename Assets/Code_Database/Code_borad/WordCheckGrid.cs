using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using System.Linq;
using Photon.Pun;
using MySql.Data.MySqlClient;
using TMPro;

public class WordCheckGrid : MonoBehaviour
{
    public enum Table
    {
        Horizontal, Vertical, None
    }
    public Table TableGrid = Table.None;
    public bool isFirstTurn = true;
    public string ShowWord;

    //EndGame
    private EndGame endGame;

    //Score
    public TMP_Text ScorePlayer1;
    public TMP_Text ScorePlayer2;

    //Draw
    private DeckChater deckChaterInstance;
    public GameObject Hand;

    //Image Grid
    public Sprite StandardImage;
    public Sprite StarStart;
    public Sprite DL;
    public Sprite DW;
    public Sprite TL;
    public Sprite TW;

    public byte NumberOfRows = 9;
    public byte NumberOfColumns = 9;

    // UIGrid
    public UIGrid FieldUIGrid;
    public DorpTable[,] Grid; 

    // DorpTable
    public DorpTable TableD;
    [SerializeField] public List<DorpTable> CurrentTiles;
    private List<DorpTable> _wordsFound;
    private List<DorpTable> _WordTiles = new List<DorpTable>();

    //SQL
    private MySqlConnection connection;
    private string connectionString;
    private PhotonView PV;

    //EndTurn
    private EndTurn endTurn;

    public int pointsShow;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        deckChaterInstance = FindObjectOfType<DeckChater>();
        endGame = FindObjectOfType<EndGame>();
    }
    void Start()
    {
        connectionString = "Server=192.168.1.163;Database=userandpassword;User=root;Password='';SslMode=none;";

        connection = new MySqlConnection(connectionString);
        connection.Open();
        CurrentTiles = new List<DorpTable>();
        _wordsFound = new List<DorpTable>();
        endTurn = FindObjectOfType<EndTurn>();
        FieldUIGrid.Initialize();
        PV.RPC("CreateTable", RpcTarget.AllBuffered);
    }

    // CreateTableImage
    [PunRPC]
    private void CreateTable(){
        Grid = new DorpTable[NumberOfRows, NumberOfColumns];
        for (var i = 0; i < NumberOfRows; i++)
        {
            for (var j = 0; j < NumberOfColumns; j++)
            {
                var newTile = Instantiate(TableD);
                newTile.transform.SetParent(gameObject.transform);
                newTile.Column = j;
                var render = newTile.GetComponent<Image>();
                render.sprite  = StandardImage;
                newTile.Row = i;
                Grid[i, j] = newTile;
                FieldUIGrid.AddElement(i, j, newTile.gameObject);
            }
        }
        Grid[4,4].GetComponent<Image>().sprite = StarStart;
        Grid[4,4].WordMultiplier = 2;
        ImageGrid();
        NumberGrid();
    }

    private void ImageGrid(){
        Grid[3,3].GetComponent<Image>().sprite = DL;
        Grid[3,5].GetComponent<Image>().sprite = DL;
        Grid[5,5].GetComponent<Image>().sprite = DL;
        Grid[5,3].GetComponent<Image>().sprite = DL;
        Grid[2,2].GetComponent<Image>().sprite = DL;
        Grid[2,6].GetComponent<Image>().sprite = DL;
        Grid[6,6].GetComponent<Image>().sprite = DL;
        Grid[6,2].GetComponent<Image>().sprite = DL;

        Grid[1,1].GetComponent<Image>().sprite = DW;
        Grid[1,7].GetComponent<Image>().sprite = DW;
        Grid[7,7].GetComponent<Image>().sprite = DW;
        Grid[7,1].GetComponent<Image>().sprite = DW;

        Grid[0,4].GetComponent<Image>().sprite = TL;
        Grid[4,8].GetComponent<Image>().sprite = TL;
        Grid[4,0].GetComponent<Image>().sprite = TL;
        Grid[8,4].GetComponent<Image>().sprite = TL;

        Grid[0,0].GetComponent<Image>().sprite = TW;
        Grid[8,0].GetComponent<Image>().sprite = TW;
        Grid[0,8].GetComponent<Image>().sprite = TW;
        Grid[8,8].GetComponent<Image>().sprite = TW;
    }

    private void NumberGrid(){
        Grid[3,3].LetterMultiplier = 2;
        Grid[3,5].LetterMultiplier = 2;
        Grid[5,5].LetterMultiplier = 2;
        Grid[5,3].LetterMultiplier = 2;
        Grid[2,2].LetterMultiplier = 2;
        Grid[2,6].LetterMultiplier = 2;
        Grid[6,6].LetterMultiplier = 2;
        Grid[6,2].LetterMultiplier = 2;

        Grid[1,1].WordMultiplier = 2;
        Grid[1,7].WordMultiplier = 2;
        Grid[7,7].WordMultiplier = 2;
        Grid[7,1].WordMultiplier = 2;

        Grid[0,4].LetterMultiplier = 3;
        Grid[4,8].LetterMultiplier = 3;
        Grid[4,0].LetterMultiplier = 3;
        Grid[8,4].LetterMultiplier = 3;

        Grid[0,0].WordMultiplier = 3;
        Grid[8,0].WordMultiplier = 3;
        Grid[0,8].WordMultiplier = 3;
        Grid[8,8].WordMultiplier = 3;
    }

    public void OnEndTurn(){
        if(CurrentTiles.Count > 0){
            if(CheckWords()){
                if(Grid[4,4].HasLetter == true){
                    foreach (var tile in CurrentTiles)
                    {
                        if (!tile.HaveLetter)
                        {
                            tile.DeleteDrop();
                            PV.RPC("GridOnline", RpcTarget.AllBuffered, tile.CurrentLetter, tile.Row, tile.Column, tile.CharImageGrid.name, tile.Points);
                        }
                    }
                    while (Hand.transform.childCount < 7)
                    {
                        Instantiate(deckChaterInstance.CradToHand, Hand.transform.position, Hand.transform.rotation, Hand.transform);
                    }
                    var points = Points();
                    PV.RPC("PointsOnline",RpcTarget.AllBuffered,points,endTurn.currentPlayerIndex);
                    endTurn.EndTurnPlayer(ShowWord);
                }
            }else{
                Debug.Log("false");
            }
        }else{
            TableGrid = Table.None;
            ShowWord = "";
            endTurn.EndTurnPlayer(ShowWord);
        }
    }

    [PunRPC]
    private void PointsOnline(int points,int index){
        endTurn.currentPlayerIndex = index;
        if(endTurn.currentPlayerIndex == 0){
            ClearPointsShow();
            ScorePlayer1.text = (int.Parse(ScorePlayer1.text) + points).ToString();
            CurrentTiles.Clear();
            TableGrid = Table.None;
        }else{
            ClearPointsShow();
            ScorePlayer2.text = (int.Parse(ScorePlayer2.text) + points).ToString();
            CurrentTiles.Clear();
            TableGrid = Table.None;
        }
        endGame.Score(int.Parse(ScorePlayer1.text),int.Parse(ScorePlayer2.text));
    }

    private void ClearPointsShow() {
        if (PhotonNetwork.IsMasterClient) {
            string currentText = ScorePlayer1.text;
            int index = currentText.IndexOf('+');
            if (index != -1) {
                currentText = currentText.Substring(0, index).Trim();
            }
            ScorePlayer1.text = currentText;
        } else {
            string currentText = ScorePlayer2.text;
            
            int index = currentText.IndexOf('+');
            if (index != -1) {
                currentText = currentText.Substring(0, index).Trim();
            }
            
            ScorePlayer2.text = currentText;
        }
    }

    public bool CheckWords()
    {
        var words = CreateWords();
        _wordsFound = words;
        var word = GetWord(words[0], words[1]);
        if (_WordTiles.Count != 0)
        {
            var index1 = word.IndexOf('_');
            var index2 = 0;
            if (_WordTiles.Count == 2)
                index2 = word.IndexOf('_', index1 + 1);
            var variants = GetAllWordVariants(word);
            SwitchDirection();
            foreach (var variant in variants)
            {
                _WordTiles[0].TempLetter = variant[index1].ToString();
                if (_WordTiles.Count == 2)
                    _WordTiles[1].TempLetter = variant[index2].ToString();
                var successful = true;
                for (var i = 3; i < words.Count; i += 2)
                {
                    word = GetWord(words[i - 1], words[i]);
                    if (!CheckWord(word))
                    {
                        successful = false;
                        break;
                    }
                }
                if (successful)
                {
                    SwitchDirection();
                    return true;
                }
            }
            SwitchDirection();
            return false;
        }
        else
        {
            var successful = CheckWord(word);
            var i = 3;
            SwitchDirection();
            while (successful && i < words.Count)
            {
                word = GetWord(words[i - 1], words[i]);
                successful = CheckWord(word);
                i += 2;
            }
            SwitchDirection();
            return successful;
        }
    }

    private List<DorpTable> CreateWords()
    {
        _WordTiles.Clear();
        var res = new List<DorpTable>();
        if (TableGrid == Table.None)
            TableGrid = Table.Horizontal;
        DorpTable start, end;
        CreateWord(CurrentTiles[0], out start, out end);
        if (start == end)
        {
            SwitchDirection();
            CreateWord(CurrentTiles[0], out start, out end);
        }
        res.Add(start);
        res.Add(end);
        SwitchDirection();
        foreach (var tile in CurrentTiles)
        {
            CreateWord(tile, out start, out end);
            if (start != end)
            {
                res.Add(start);
                res.Add(end);
            }
        }
        if (_WordTiles.Count == 2)
            _WordTiles = _WordTiles.OrderByDescending(t => t.Row).ThenBy(t => t.Column).Distinct().ToList();
        SwitchDirection();
        return res;
    }

    private void CreateWord(DorpTable start, out DorpTable wordStart, out DorpTable wordEnd)
    {
        if (TableGrid == Table.Vertical)
        {
            var j = start.Row;
            while (j < 9 && Grid[j, start.Column].HasLetter)
            {
                if (Grid[j, start.Column].CurrentLetter.Equals("*"))
                    if (!_WordTiles.Contains(Grid[j, start.Column]))
                        _WordTiles.Add(Grid[j, start.Column]);
                j++;
            }
            wordStart = Grid[j - 1, start.Column];
            j = start.Row;
            while (j >= 0 && Grid[j, start.Column].HasLetter)
            {
                if (Grid[j, start.Column].CurrentLetter.Equals("*"))
                    if (!_WordTiles.Contains(Grid[j, start.Column]))
                        _WordTiles.Add(Grid[j, start.Column]);
                j--;
            }
            wordEnd = Grid[j + 1, start.Column];
        }
        else
        {
            var j = start.Column;
            while (j >= 0 && Grid[start.Row, j].HasLetter)
            {
                if (Grid[start.Row, j].CurrentLetter.Equals("*"))
                    if (!_WordTiles.Contains(Grid[start.Row, j]))
                        _WordTiles.Add(Grid[start.Row, j]);
                j--;
            }
            wordStart = Grid[start.Row, j + 1];
            j = start.Column;
            while (j < 9 && Grid[start.Row, j].HasLetter)
            {
                if (Grid[j, start.Column].CurrentLetter.Equals("*"))
                    if (!_WordTiles.Contains(Grid[start.Row, j]))
                        _WordTiles.Add(Grid[start.Row, j]);
                j++;
            }
            wordEnd = Grid[start.Row, j - 1];
        }
    }

    private void SwitchDirection()
    {
        TableGrid = TableGrid == Table.Horizontal ? Table.Vertical : Table.Horizontal;
    }

    private string GetWord(DorpTable begin, DorpTable end)
    {
        if (TableGrid == Table.Vertical)
        {
            var sb = new StringBuilder();
            for (var j = begin.Row; j >= end.Row; j--)
            {
                if (!String.IsNullOrEmpty(Grid[j, begin.Column].TempLetter))
                    sb.Append(Grid[j, begin.Column].TempLetter);
                else if (Grid[j, begin.Column].CurrentLetter.Equals("*"))
                    sb.Append('_');
                else sb.Append(Grid[j, begin.Column].CurrentLetter);
            }
            return sb.ToString();
        }
        else
        {
            var sb = new StringBuilder();
            for (var j = begin.Column; j <= end.Column; j++)
            {
                if (!String.IsNullOrEmpty(Grid[begin.Row, j].TempLetter))
                    sb.Append(Grid[begin.Row, j].TempLetter);
                else if (Grid[begin.Row, j].CurrentLetter.Equals("*"))
                    sb.Append('_');
                else sb.Append(Grid[begin.Row, j].CurrentLetter);
            }
            return sb.ToString();
        }
    }

    private List<string> GetAllWordVariants(string word)
    {
        var sql = "SELECT * FROM vocabulary	WHERE English_word like \"" + word.ToLower() + "\"";
        var command = new MySqlCommand(sql, connection);
        var reader = command.ExecuteReader();
        if (reader.HasRows)
        {
            var res = new List<string>();
            while (reader.Read())
            {
                res.Add(reader.GetString(0));
            }
            reader.Close();
            return res;
        }
        reader.Close();
        return null;
    }

    private bool CheckWord(string word)
    {
        var sql = "SELECT count(*) FROM vocabulary WHERE English_word LIKE @word";
        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@word", word.ToLower());
            var inp = command.ExecuteScalar();
            if (Convert.ToInt32(inp) != 0)
            {
                ShowGetWord(word);
            }
            return Convert.ToInt32(inp) != 0;
        }
    }

    private void ShowGetWord(string word)
    {
        var sql = "SELECT * FROM vocabulary WHERE English_word LIKE @word";
        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@word", word.ToLower());
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    ShowWord = reader.GetString("English_word").ToUpper() + " = " + reader.GetString("Thai_word");
                }
                Debug.Log(ShowWord);
            }
        }
    }

    public int Points(){
        var result = 0;
        var wordAllMultiplier = 1;
        for (int i = 0; i < _wordsFound.Count - 1; i++){
            var num = 0;
            if(_wordsFound[i].Row == _wordsFound[i+1].Row){
                for (int j = _wordsFound[i].Column; j <= _wordsFound[i + 1].Column; j++){
                    var tile = Grid[_wordsFound[i].Row,j];
                    num += tile.Points * tile.LetterMultiplier;
                    wordAllMultiplier *= tile.WordMultiplier;
                    if(tile.HaveLetter == true){
                        if(tile.LetterMultiplier > 1){
                            tile.LetterMultiplier = 1;
                        }
                        tile.WordMultiplier = 1;
                    }
                }
            }else{
                for (int j = _wordsFound[i].Row; j >= _wordsFound[i + 1].Row; j--){
                    var tile = Grid[j,_wordsFound[i].Column];
                    num += tile.Points * tile.LetterMultiplier;
                    wordAllMultiplier *= tile.WordMultiplier;
                    if(tile.HaveLetter == true){
                        if(tile.LetterMultiplier > 1){
                            tile.LetterMultiplier = 1;
                        }
                        tile.WordMultiplier = 1;
                    }
                }
            }
            result += num;
        }
        return result * wordAllMultiplier;
    }

    [PunRPC]
    private void GridOnline(string letter, int row, int column, string charImageGrid, int Points){
        DorpTable tile = Grid[row, column];
        Sprite sprite = Resources.Load<Sprite>(charImageGrid);
        tile.GetComponent<Image>().sprite = sprite;
        tile.GetComponent<Image>().color = Color.white;
        tile.CurrentLetter = letter;
        tile.Points = Points;
        tile.HasLetter = true;
        tile.HaveLetter = true;
    }

    public void BackToHand(){
        for (int i = 0; i < NumberOfRows; i++)
        {
            for (int j = 0; j < NumberOfColumns; j++)
            {
                DorpTable tile = Grid[i, j];
                if (tile.transform.childCount > 0)
                {
                    GameObject draggedObject = tile.transform.GetChild(0).gameObject;
                    draggedObject.transform.SetParent(Hand.transform);
                    tile.HasLetter = false;
                    tile.CurrentLetter = "";
                    tile.Points = 0;
                    tile.CharImageGrid = null;
                }
            }
        }
        CurrentTiles.Clear();
    }
}