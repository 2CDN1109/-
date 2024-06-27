[System.Serializable]
public class QuizQuestion
{
    public int QuestionNumber;
    public string QuestionText;
    public string Choice1;
    public string Choice2;
    public string Choice3;
    public int CorrectChoiceIndex; // 0-based index
    public string Choice1ImagePath;
    public string Choice2ImagePath;
    public string Choice3ImagePath;
}
