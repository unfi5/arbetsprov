using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Damage : MonoBehaviour
{
    private int CurrentHealth;
    private float DelayTime = 0.2f;
    private Image Image;
    private Color tempcolor;
    public float Fade;
    private float FadeTime = 0.5f;
    private float ImageAlpha = 0;

    private void Start()
    {
        Image = GetComponent<Image>();
        Image.color = new Vector4 (Image.color.r, Image.color.g, Image.color.b, 0.0f);

        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        CurrentHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Health;
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentHealth != 0)
        {

            if (CurrentHealth > GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Health)
            {
                CurrentHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Health;

                ImageAlpha = 1;
                Fade = 1;
                Image = GetComponent<Image>();
                tempcolor = Image.color;
                tempcolor.a = 1;
                StartCoroutine(Delay());
            }

            Image.color = new Vector4(Image.color.r, Image.color.g, Image.color.b, Fade);
        }

        else
        {
            Image.color = new Vector4(Image.color.r, Image.color.g, Image.color.b, 1.0f);
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(DelayTime);

        StartCoroutine(FadeTimer());
    }

    private IEnumerator FadeTimer()
    {
        while (Fade > 0)
        {
            yield return new WaitForSeconds(FadeTime);
            Fade -= FadeTime;
        }
    }

}
