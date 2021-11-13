using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Mainmenu : MonoBehaviour
{

    [SerializeField]
    private TMP_InputField inputFieldName,inputFieldNumber;



    SavedData data;
    // Start is called before the first frame update
    void Start()
    {
        data = SavedData.Instance;
        SavedData.Instance.LoadData();
        FindObjectOfType<Customizer>().GetAppereance();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, 1);
    }
    public void PlayScene()
    {
        SavedData.Instance.SaveDate();
        SceneManager.LoadScene(1);
    }
    public void TypedName()
    {
        SavedData.Instance.SetData(Color.clear, Color.clear, inputFieldName.text, -1);
        Customize();
    }
    public void TypedNumber()
    {
        int value;
        if (int.TryParse(inputFieldNumber.text, out value))
        {
            if(value < 1000 && value >=0)
                SavedData.Instance.SetData(Color.clear, Color.clear, null, value);
        }
        Customize();
    }
    public void GetBlue()
    {
        data.SetData(Color.blue, Color.gray, null, -1);
        Customize();
    }
    public void GetRed()
    {
        data.SetData(Color.red, Color.white, null, -1);
        Customize();
    }
    public void GetOrange()
    {
        data.SetData(Color.yellow, Color.black, null, -1);
        Customize();
    }
    private void Customize()
    {
        FindObjectOfType<Customizer>().GetAppereance();
        SavedData.Instance.SaveDate();
    }
}
