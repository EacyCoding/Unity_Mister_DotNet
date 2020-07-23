using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class CardBehavior : MonoBehaviour
{
    
    public void OnClick()
    {
        Debug.Log("Click on Card.");
        GetComponentInParent<GamePanelBehavior>().GetContext().OnClick(this);
    }

    internal void Dispose()
    {
        Destroy(this.gameObject);
    }

    internal void PlayAudio()
    {
        gameObject.GetComponent<AudioSource>().Play();
    }

    internal void PlayAnimation()
    {
        gameObject.GetComponent<PlayableDirector>().Play();
    }

    public void MiddleAnimation()
    {
        ChangeImage();
    }

    public void EndAnimation()
    {
        gameObject.GetComponent<AudioSource>().Stop();
        GetComponentInParent<GamePanelBehavior>().GetContext().AnimationEnd(this);
    }

    internal Sprite GetCurrentSprite()
    {
        CardProperty cardProperty = GetComponentInParent<GamePanelBehavior>().ReturnCardProperty(this.gameObject);
        return gameObject.GetComponent<Image>().sprite;
    }

    private void ChangeImage()
    {
        CardProperty cardProperty = GetComponentInParent<GamePanelBehavior>().ReturnCardProperty(this.gameObject);
        Sprite currentSprite = gameObject.GetComponent<Image>().sprite;
        if (currentSprite == cardProperty.BackImage)
        {
            gameObject.GetComponent<Image>().sprite = cardProperty.FrontImage;
        } 
        else
        {
            gameObject.GetComponent<Image>().sprite = cardProperty.BackImage;
        }
    }

}
