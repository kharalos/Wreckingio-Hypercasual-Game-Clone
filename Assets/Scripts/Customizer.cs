using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Customizer : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer[] meshOverall, meshSecond;
    [SerializeField]
    private SkinnedMeshRenderer driverMesh;

    private Color firstColor, secondColor;

    [SerializeField]
    private TextMeshPro playerNumber, playerName;

    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private Image emojiScreen;

    [SerializeField]
    private bool inMenu;
    void Start()
    {
        if (inMenu) return;
        if (gameObject.tag == "Player")
        {
            if (SavedData.Instance == null) return;
            GetAppereance();
        }
        else
        {
            GenerateAppereance();
        }
        SetAppereance();
    }
    public void GetAppereance()
    {
        SavedData.Instance.GetData();
        firstColor = SavedData.Instance.firstColor;
        secondColor = SavedData.Instance.secondColor;
        playerNumber.text = SavedData.Instance.playerNumber.ToString();
        playerName.text = SavedData.Instance.playerName;
        SetAppereance();
    }
    private void GenerateAppereance()
    {
        playerName.text = null;
        const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";
        int charAmount = Random.Range(3, 15); //set those to the minimum and maximum length of your string
        for (int i = 0; i < charAmount; i++)
        {
            playerName.text += glyphs[Random.Range(0, glyphs.Length)];
        }

        firstColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        secondColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

        playerNumber.text = Random.Range(1, 999).ToString();
    }

    private void SetAppereance()
    {
        for (int i = 0; i < meshOverall.Length; i++)
        {
            meshOverall[i].material.color = firstColor;
        }
        for (int i = 0; i < meshSecond.Length; i++)
        {
            meshSecond[i].material.color = secondColor;
        }
        driverMesh.materials[0].color = firstColor;
        driverMesh.materials[1].color = secondColor;
        //playerName.color = new Color32(((byte)secondColor.r), ((byte)secondColor.g), ((byte)secondColor.b),((byte)Color.black.a));
    }
    public void Hit(float velocity)
    {
        if(velocity < 60)
        {
            StartCoroutine(EmojiAlpha(sprites[(int)Mathf.Abs(velocity / 10f)]));
        }
        else
        {
            StartCoroutine(EmojiAlpha(sprites[7]));
        }
    }
    private IEnumerator EmojiAlpha(Sprite emoji)
    {
        Debug.Log("Chosen emoji is: " + emoji.name);
        emojiScreen.sprite = emoji;
        var emojiColor = emojiScreen.color;

        for (float i = 0; i < 1; i+= 0.01f)
        {
            emojiColor.a = i;
            emojiScreen.color = emojiColor;
            yield return new WaitForSeconds(0.003f);
        }
        yield return new WaitForSeconds(0.5f);
        for (float i = 1; i > 0; i -= 0.01f)
        {
            emojiColor.a = i;
            emojiScreen.color = emojiColor;
            yield return new WaitForSeconds(0.003f);
        }
    }
    private void Update()
    {
        playerName.transform.LookAt(playerName.transform.position + Camera.main.transform.rotation * -Vector3.back,
            Camera.main.transform.rotation * -Vector3.down);
    }
}
