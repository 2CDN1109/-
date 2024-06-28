using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizUIManager : MonoBehaviour
{
    public TMP_Text questionText;
    public TMP_Text choice1Text;
    public TMP_Text choice2Text;
    public TMP_Text choice3Text;
    public Image choice1Image;
    public Image choice2Image;
    public Image choice3Image;
    public Image cursorFrame;

    private Image[] choiceImages;
    private TMP_Text[] choiceTexts;
    private int selectedChoiceIndex = 1;

    void Start()
    {
        choiceImages = new Image[] { choice1Image, choice2Image, choice3Image };
        choiceTexts = new TMP_Text[] { choice1Text, choice2Text, choice3Text };
        UpdateChoiceSelection(); // 初期選択状態を更新
        Debug.Log("QuizUIManager initialized.");
    }

    public void DisplayQuestion(QuizQuestion question)
    {
        questionText.text = question.QuestionText;

        // 選択肢のテキストを設定
        choiceTexts[0].text = question.Choice1;
        choiceTexts[1].text = question.Choice2;
        choiceTexts[2].text = question.Choice3;

        LoadSprite(question.Choice1ImagePath, choice1Image);
        LoadSprite(question.Choice2ImagePath, choice2Image);
        LoadSprite(question.Choice3ImagePath, choice3Image);

        // 初期選択状態を更新

        UpdateChoiceSelection();
    }

    public void SelectNextChoice()
    {
        selectedChoiceIndex = (selectedChoiceIndex + 1) % 3;
        UpdateChoiceSelection();
    }

    public void SelectPreviousChoice()
    {
        selectedChoiceIndex = (selectedChoiceIndex - 1 + 3) % 3;
        UpdateChoiceSelection();
    }

    public int GetSelectedChoiceIndex()
    {
        return selectedChoiceIndex;
    }

    private void UpdateChoiceSelection()
    {
        // カーソルフレームを選択中の選択肢の位置に移動
        if (cursorFrame != null && choiceImages.Length > selectedChoiceIndex)
        {
            cursorFrame.transform.position = choiceImages[selectedChoiceIndex].transform.position;
            cursorFrame.gameObject.SetActive(true); // カーソルフレームを表示
        }
        else
        {
            Debug.LogError("Cursor frame or choiceImages is not set correctly.");
        }
    }

    private void LoadSprite(string path, Image image)
    {
        Sprite sprite = Resources.Load<Sprite>(path);
        if (sprite != null)
        {
            image.sprite = sprite;
        }
        else
        {
            Debug.LogError("Failed to load sprite at path: " + path);
        }
    }
}
