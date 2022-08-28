using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealthDisplay : MonoBehaviour
{
    private Transform bar;

    [SerializeField]
    private TextMeshPro valueText;
    public PlayerController player;
    private void Start()
    {
        bar = transform.Find("Bar");
        bar.localScale = new Vector3(1f, 1f, 0f);
        SetColor(Color.red);
        valueText.GetComponent<TextMeshPro>().text = "" + player.maxHealth;
    }
    private void Update()
    {
        valueText.GetComponent<TextMeshPro>().text = "" + player.health;
    }
    public void SetColor(Color color)
    {
        bar.Find("BarSprite").GetComponent<SpriteRenderer>().color = color;
    }
    public void SetSize(float sizeNormalized)
    {
        sizeNormalized /= 10;
        bar.localScale = new Vector3(sizeNormalized, 1f);
    }
}
