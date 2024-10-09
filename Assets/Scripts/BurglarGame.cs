
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class BurglarGame : MonoBehaviour
{   
    [SerializeField] private Text firstKeyText;
    [SerializeField] private Text secondKeyText;
    [SerializeField] private Text thirdKeyText;
    [SerializeField] private Text drillButtonText;
    [SerializeField] private Text hammerButtonText;
    [SerializeField] private Text masterKeyButtonText;

    [SerializeField] private int[] drillArray = {1, -1, 0};
    [SerializeField] private int[] hammerArray = {-1, 2, -1};
    [SerializeField] private int[] masterKeyArray = {-1, 1, 1};

    [SerializeField] private int[] passwordArray = new int[3];
    [SerializeField] private int[] correctPasswordArray = new int[3];

    [SerializeField] private float timer = 90.0f;
    [SerializeField] private Text timerText;

    [SerializeField] private GameObject resultPanel;
    [SerializeField] private Text resultText;

    private bool winCheck = false;


    private int[] GenerateSecretPassword(){
        int[] passwordArray = new int[3];
        for(int i = 0; i < passwordArray.Length; i++){
            passwordArray[i] = Random.Range(0,10);
        }

        return passwordArray;
    }

    private int[] GeneratePassword(int[] secretArray){
        int[] passwordArray = new int[3];

        for(int i = 0; i < passwordArray.Length; i++){
            passwordArray[i] = secretArray[i];
        }

        int first = Random.Range(0, 10);
        int second = Random.Range(0, 10);
        int third = Random.Range(0, 10);

        for (int j = 0; j < first; j++){
            passwordArray = UseInstrument(passwordArray, drillArray, -1);
        }
        for (int j = 0; j < second; j++){
            passwordArray = UseInstrument(passwordArray, hammerArray, -1);
        }
        for (int j = 0; j < third; j++){
            passwordArray = UseInstrument(passwordArray, masterKeyArray, -1);
        }


        Debug.Log($"---------ПОДСКАЗКА ВЕКА---------\n{first} раз используй Дрель\n{second} раз используй Молоток\n{third} раз используй Отмычку\n---------ПОДСКАЗКА ВЕКА---------");
         
        return passwordArray;
        
    }

    private void DisplayPassword(int[] passwordArray){
        firstKeyText.text = passwordArray[0].ToString();
        secondKeyText.text = passwordArray[1].ToString();
        thirdKeyText.text = passwordArray[2].ToString();
    }

    private int[] UseInstrument(int[] passwordArray, int[] instrumentArray, int iter){
        for(int i = 0; i < passwordArray.Length; i++){
            passwordArray[i] = (passwordArray[i] + iter * instrumentArray[i]) % 10;
            if (passwordArray[i] < 0) passwordArray[i] += 10;            
        }
        return passwordArray;
    }
    private string FormatingText(int[] array, string text){
        string result = "";
        for (int i = 0; i < array.Length; i++){
            if (array[i] > 0) result += "+";
            result += array[i].ToString();
            if (i < array.Length - 1) result += " | ";
        }
        result += "\n" + text;
        return result;
    }
        
    private void InstrumentSettings(){
        drillButtonText.text = FormatingText(drillArray, "Дрель"); 
        hammerButtonText.text = FormatingText(hammerArray, "Молоток");
        masterKeyButtonText.text = FormatingText(masterKeyArray, "Отмычка");
    }

    public void OnClickCheckPassword(){
        bool check = true;
        for (int i = 0; i < passwordArray.Length; i++){
            if (passwordArray[i] != correctPasswordArray[i]){
                check = false;
                break;
            }
        }

        if (check){
            winCheck = true;
        }
        else {
            timer -= 3;
            Debug.Log("Потеря 3х секунд");
        }
    }

    public void OnClickUseDrill(){
        passwordArray = UseInstrument(passwordArray, drillArray, 1);
    }

    public void OnClickUseHammer(){
        passwordArray = UseInstrument(passwordArray, hammerArray, 1);
    }

    public void OnClickUseMasterKey(){
        passwordArray = UseInstrument(passwordArray, masterKeyArray, 1);
    }

    public void OnClickRestart(){
        winCheck = false;
        timer = 90.0f;
        timerText.text = timer.ToString("F1");
        timerText.color = Color.white;
        resultPanel.SetActive(false);

        Start();
    }

    void Start()
    {
        Debug.Log("---------НАЧАЛО ИГРЫ---------");
        InstrumentSettings();
        correctPasswordArray = GenerateSecretPassword();
        passwordArray = GeneratePassword(correctPasswordArray);
        Debug.Log($"Начальный пароль: {passwordArray[0]} {passwordArray[1]} {passwordArray[2]}");
        Debug.Log($"Правильный пароль: {correctPasswordArray[0]} {correctPasswordArray[1]} {correctPasswordArray[2]}");
    }

    void Update()
    {   
        if (!winCheck){
            timer -= Time.deltaTime;
            timerText.text = timer.ToString("F1");
            if (timer <= 15){
            timerText.color = Color.red;
            }
            if (timer <= 0){
                timer = 0;
                resultPanel.SetActive(true);
                resultText.text = "ПОРАЖЕНИЕ";
            }
        }
        else {
            resultPanel.SetActive(true);
            resultText.text = "ПОБЕДА";
        }

        DisplayPassword(passwordArray);
    }
}
