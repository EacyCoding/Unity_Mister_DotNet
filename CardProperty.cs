using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardProperty
{
    internal Sprite BackImage;
    internal Sprite FrontImage;
    internal GameObject Instance;


    public CardProperty(GameObject instance, Sprite backimage, Sprite frontimage)
    {
        this.BackImage = backimage;
        this.FrontImage = frontimage;
        this.Instance = instance;
    }

    public override string ToString()
    {
        return $"InstanceID: {Instance.GetInstanceID()}, Backimage: {BackImage.name}, Frontimage: {FrontImage.name}";
    }
}
