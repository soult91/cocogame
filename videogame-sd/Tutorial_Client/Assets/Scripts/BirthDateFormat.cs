using TMPro;
using UnityEngine;
 
public class BirthDateFormat : MonoBehaviour {
 
    bool allSlashAdded;
 
    public void OnValueChanged() {

        string input = GameObject.Find("CreateBirthdate").GetComponent<TMP_InputField>().text;
        if (!allSlashAdded && (input.Length == 2 || input.Length == 5))
        {
            allSlashAdded = input.Length == 5;
 
            input += "/";
        }
    }
}