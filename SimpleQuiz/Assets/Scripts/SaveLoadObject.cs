using System.IO;
using UnityEngine;

public class SaveLoadObject
{
    public void SaveQuestionList(QuestionObject obj, string fileName)
    {
        string json = JsonUtility.ToJson(obj);
        File.WriteAllText(Application.dataPath + "/Saves/save_" + fileName + ".txt", json);
    }

    public Question[] LoadQuestionList(string fileName)
    {
        string loadString = File.ReadAllText(Application.dataPath + "/Saves/save_" + fileName + ".txt");

        QuestionObject loadObj = JsonUtility.FromJson<QuestionObject>(loadString);

        Question[] list = loadObj.list;
        return list;
    }
}
