using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFlyText : MonoBehaviour
{
    float alpha = 1;
    float dissolveSpeed = 1f;
    float flySpeed = 10;

    public List<Image> dissolveImages = new List<Image>();
    public List<Text> dissolveText = new List<Text>();
    
    public void SetSprite(Sprite sprite)
    {
        if (dissolveImages.Count > 0)
        {
            dissolveImages[0].sprite = sprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        alpha -= Time.deltaTime * dissolveSpeed;
        transform.position = new Vector3(transform.position.x, transform.position.y + flySpeed * Time.deltaTime);
        foreach(Image image in dissolveImages)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
        }
        foreach (Text text in dissolveText)
        {

            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        }

        if (alpha <= 0)
            Destroy(this.gameObject);
    }
}
