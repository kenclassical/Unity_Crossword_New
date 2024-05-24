using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;
using System.Text.RegularExpressions;

public class Randomitem : MonoBehaviour
{
    public Button randomButton;
    public GameObject Hand;
    [SerializeField] public TMP_Text textButton;
    public bool buttonCheck = true;
    public charimage charImageScript;
    private DeckChater deckChaterInstance;
    private Color ColorAlpha50;
    public Color ColorAlphaButton;
    public Color ColorAlphaText;
    private WordCheckGrid wordCheckGrid;
    private DeckChater deckChater;
    public List<string> wordList;

    public List<string> resultList;

    private DumbWod dumbWord;

    //SQL
    private MySqlConnection connection;
    private string connectionString = "Server=10.50.16.95;Database=userandpassword;User=root;Password='';SslMode=none;";
    void Awake()
    {
        connection = new MySqlConnection(connectionString);
        connection.Open();
        wordCheckGrid = FindAnyObjectByType<WordCheckGrid>();
        randomButton = GetComponent<Button>();
        deckChaterInstance = FindObjectOfType<DeckChater>();
        deckChater = FindObjectOfType<DeckChater>();
        dumbWord = FindObjectOfType<DumbWod>();
        wordList = new List<string>();
        ColorAlpha50.a = 0.5f;
        ColorAlphaButton = new Color(255f/255f, 212f/255f, 103f/255f);
        ColorAlphaText = new Color(255f/255f, 255f/255f, 255f/255f);
    }

    void Start()
    {
        randomButton.onClick.AddListener(() => {
            if(buttonCheck)
            {
                StateRandom();
            }
        });
    }

    private void StateRandom()
    {
        wordCheckGrid.BackToHand();
        SelectRandom();
        if(Hand != null && Hand.transform.childCount > 0){
            foreach (Transform child in Hand.transform)
            {  
                Destroy(child.gameObject);
            }
            randomButton.image.color = ColorAlpha50;
            textButton.color = ColorAlpha50;
            buttonCheck = false;
            for (int i = 0; i < 7; i++)
            {
                Instantiate(deckChaterInstance.CradToHand, Hand.transform.position, Hand.transform.rotation, Hand.transform);
            }
            foreach(Transform child in Hand.transform){
                child.GetComponent<Image>().raycastTarget = true;
            }
            if(!dumbWord.OnAndOff){
                dumbWord.Del();
            }
        }
    }

    private void SelectRandom(){
        bool hasWordPlaced = false;
        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < 9; j++) {
                DorpTable dorpTable = wordCheckGrid.Grid[i,j];
                if(dorpTable.HaveLetter){
                    HaveLetterVertical(j);
                    HaveLetterHorizontal(i);
                    hasWordPlaced = true;
                }
            }
        }
        if(hasWordPlaced){
            SelectRandomSQL();   
        }else{
            deckChater.SelectStart();
        }
    }
    
    private void HaveLetterVertical(int column){
        string GetWord = "";
        for (int i = 8; i >= 0 ; i--){
            DorpTable dorpTable = wordCheckGrid.Grid[i,column];
            if(dorpTable.HaveLetter){
                GetWord += dorpTable.CurrentLetter;
            }else if(i < 8 && column + 1 < wordCheckGrid.Grid.GetLength(1) && wordCheckGrid.Grid[i,column+1] && wordCheckGrid.Grid[i,column+1].HaveLetter
                || i > 0 && column - 1 >= 0 && wordCheckGrid.Grid[i,column-1] && wordCheckGrid.Grid[i,column-1].HaveLetter){
                GetWord += "/";
            }else{
                GetWord += "_";
            }
        }
        if(GetWord != ""){
            if(!wordList.Contains(GetWord)){
                wordList.Add(GetWord);
            }
        }
    }

    private void HaveLetterHorizontal(int row){ 
        string GetWord = "";
        for (int i = 0; i < 9 ; i++){
            DorpTable dorpTable = wordCheckGrid.Grid[row,i];
            if(dorpTable.HaveLetter){
                GetWord += dorpTable.CurrentLetter;
            }else if(row < 8 && wordCheckGrid.Grid[row+1,i] && wordCheckGrid.Grid[row+1,i].HaveLetter 
                || row > 0 && wordCheckGrid.Grid[row-1,i] && wordCheckGrid.Grid[row-1,i].HaveLetter){
                GetWord += "/";
            }else{
                GetWord += "_";
            }
        }
        if(GetWord != ""){
            if(!wordList.Contains(GetWord)){
                wordList.Add(GetWord);
            }
        }
    }

    private void SelectRandomSQL(){
        resultList = new List<string>();
        List<string> wordsToRemove = new List<string>();
        dumbWord.AllWordRandom = new List<string>();
        foreach (string word in wordList)
        {
            int startLetter = 0;
            int endLetter = 0;
            string likeStart = "";
            string likeEnd = "";
            string modifiedWords = CreateWord(word, ref startLetter, ref endLetter);
            StringBuilder sqlBuilder = new StringBuilder("SELECT English_word FROM vocabulary WHERE ");

            if (CheckPattern_(modifiedWords)){
                int startNumber = 0;
                int endNumber = 0;

                StringBuilder startWords = new StringBuilder();
                StringBuilder endWords = new StringBuilder();

                for (int i = 0; i < modifiedWords.Length - 2; i++)
                {
                    if (modifiedWords[i] == '_')
                    {
                        endNumber++;
                    }
                    else if (modifiedWords[i] == '/')
                    {
                        break;
                    }
                }

                for (int i = modifiedWords.Length - 1; i >= 2; i--)
                {
                    if (modifiedWords[i] == '_')
                    {
                        startNumber++;
                    }
                    else if (modifiedWords[i] == '/')
                    {
                        break;
                    }
                }
                startWords.Append(modifiedWords[0]);
                endWords.Insert(0, modifiedWords[modifiedWords.Length - 1]);
    
                for(int i = 0; i <= startLetter;i++){
                    if (i > 0){
                       likeStart += "_"; 
                    }
                    for(int j = endNumber; j >= 0; j--){
                       if (j > 0){
                            likeEnd += "_";
                            sqlBuilder.AppendFormat("English_word LIKE '{0}{1}{2}' OR ", likeStart, startWords, likeEnd);
                        } 
                    }
                    likeEnd = "";
                }
                
                likeStart = "";
                for(int i = 0; i <= startNumber;i++){
                    if (i > 0){
                       likeStart += "_"; 
                    }
                    for(int j = endLetter; j >= 0; j--){
                       if (j > 0){
                            likeEnd += "_";
                            sqlBuilder.AppendFormat("English_word LIKE '{0}{1}{2}' OR ", likeStart, endWords, likeEnd);
                        } 
                    }
                    likeEnd = "";
                }

                likeStart = "";
                for (int i = 0; i <= startLetter; i++){
                    if (i > 0){
                        likeStart += "_";
                    }
                    for (int j = endLetter; j >= 0; j--){
                        if (j > 0){
                            likeEnd += "_";
                            sqlBuilder.AppendFormat("English_word LIKE '{0}{1}{2}' OR ", likeStart, modifiedWords, likeEnd);
                        } else if (i > 0 && j == 0){
                            sqlBuilder.AppendFormat("English_word LIKE '{0}{1}' OR ", likeStart, modifiedWords);
                        }
                    }
                    likeEnd = "";
                }

                string sql = sqlBuilder.ToString();
                sql = sql.Substring(0, sql.Length - 4);

                if(sql != "SELECT English_word FROM vocabulary WH"){
                    using (var cmd = new MySqlCommand(sql, connection)){
                        using (var reader = cmd.ExecuteReader()){
                            while (reader.Read()){
                                string englishWord = reader.GetString(0);
                                resultList.Add(englishWord);
                                dumbWord.AllWordRandom.Add(englishWord.ToLower());
                            }
                        }
                    }
                    wordsToRemove.Add(word);
                }
            }else{
                if (startLetter == 0 && endLetter == 0)
                {
                    continue;
                }
                for (int i = 0; i <= startLetter; i++){
                    if (i > 0){
                        likeStart += "_";
                    }
                    for (int j = endLetter; j >= 0; j--){
                        if (j > 0){
                            likeEnd += "_";
                            sqlBuilder.AppendFormat("English_word LIKE '{0}{1}{2}' OR ", likeStart, modifiedWords, likeEnd);
                        } else if (i > 0 && j == 0){
                            sqlBuilder.AppendFormat("English_word LIKE '{0}{1}' OR ", likeStart, modifiedWords);
                        }
                    }
                    likeEnd = "";
                }
                
                string sql = sqlBuilder.ToString();
                sql = sql.Substring(0, sql.Length - 4);
                
                if(sql != "SELECT English_word FROM vocabulary WH"){
                    using (var cmd = new MySqlCommand(sql, connection)){
                        using (var reader = cmd.ExecuteReader()){
                            while (reader.Read()){
                                string englishWord = reader.GetString(0);
                                resultList.Add(englishWord);
                                dumbWord.AllWordRandom.Add(englishWord.ToLower());
                            }
                        }
                    }
                    wordsToRemove.Add(word);
                }
            }
        }

        foreach (string wordToRemove in wordsToRemove){
            wordList.Remove(wordToRemove);
        }
        deckChater.RandomStart(resultList);
        
    }

    private string CreateWord(string word,ref int startLetter,ref int endLetter){
        StringBuilder stringBuilder = new StringBuilder(word);
        for (int i = 0; i < stringBuilder.Length; i++)
        {
            if (stringBuilder[i] == '_')
            {
                startLetter++;
                stringBuilder.Remove(i, 1);
                i--;
            }
            else if(stringBuilder[i] == '/')
            {
                startLetter = 0;
                stringBuilder.Remove(i, 1);
                i--;
            }
            else if (stringBuilder[i] != '_' && stringBuilder[i] != '/')
            {
                break;
            }
        }

        for (int i = stringBuilder.Length - 1; i >= 0; i--)
        {
            if (stringBuilder[i] == '_')
            {
                endLetter++;
                stringBuilder.Remove(i, 1);
            }
            else if(stringBuilder[i] == '/')
            {
                endLetter = 0;
                stringBuilder.Remove(i, 1);
            }
            else if (stringBuilder[i] != '_' && stringBuilder[i] != '/')
            {
                break;
            }
        }

        return stringBuilder.ToString();
    }

    private bool CheckPattern_(string input)
    {
        string pattern = @".{1,9}[_/]{1,9}.{1,9}";
        return Regex.IsMatch(input, pattern);
    }
}

