using System.Collections.Generic;

[System.Serializable]
public class Word
{
    public string word;
    public string explanation;
    public int char_count;
}

[System.Serializable]
public class WordList
{
    public List<Word> words;
}
