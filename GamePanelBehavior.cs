using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class GamePanelBehavior : MonoBehaviour
{

    public GameObject CardPrefab;
    public GameObject CardPanel;

    internal Context context;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Game started!");
        LoadImages();
        for (int i = 0; i < 6; i++)
        {
            var instance = Instantiate(CardPrefab, CardPanel.transform);
            instance.GetComponent<Image>().sprite = _BackImages[0];
            var frontimage = GetFrontImage();
            CardProperty cardProperty = new CardProperty(instance, _BackImages[0], frontimage);
            _CardProperties.Add(instance.GetInstanceID(), cardProperty);
            Debug.Log(cardProperty.ToString());
        }
        context = Context.GetInstance();
    }

    internal Context GetContext()
    {
        return context;
    }

    private Dictionary<int, CardProperty> _CardProperties = new Dictionary<int, CardProperty>();
    internal CardProperty ReturnCardProperty(GameObject instance)
    {
        return _CardProperties[instance.GetInstanceID()];
    }

    private List<int> _Index = new List<int>() { 0, 0, 1, 1, 2, 2 };

    private Sprite GetFrontImage()
    {
        int random = UnityEngine.Random.Range(0, _Index.Count);
        var frontImage = _FrontImages[_Index[random]];
        _Index.RemoveAt(random);
        return frontImage;
    }

    private Sprite[] _BackImages;
    private Sprite[] _FrontImages;

    private void LoadImages()
    {
        _BackImages = Resources.LoadAll<Sprite>("Images\\Back");
        _FrontImages = Resources.LoadAll<Sprite>("Images\\Front");
        foreach (var front in _FrontImages)
        {
            Debug.Log($"Image loaded: {front.name}");
        }
        foreach (var back in _BackImages)
        {
            Debug.Log($"Image loaded: {back.name}");
        }
    }
  
}

public class Context : State
{
    private static Context _Instance;


    private Context()
    {
        currentState = NoCards;
    }

    public static Context GetInstance()
    {
        if (_Instance == null)
        {
            _Instance = new Context();
        }
        return _Instance;
    }

    internal CardBehavior firstInstance = null;
    internal CardBehavior secondInstance = null;

    internal State NoCards = new StateNoCards();
    internal State OneCard = new StateOneCard();
    internal State TwoCards = new StateTwoCards();
    internal State currentState;

    public void OnClick(CardBehavior instance)
    {
        currentState.OnClick(instance);
    }

    public void AnimationEnd(CardBehavior instance)
    {
        currentState.AnimationEnd(instance);
    }

    public void Validation()
    {
        currentState.Validation();
    }
}

public interface State
{
    void OnClick(CardBehavior instance);


    void AnimationEnd(CardBehavior instance);

    void Validation();
}

class StateNoCards : State
{
    bool _isAnimationRunning = false;


    public void OnClick(CardBehavior instance)
    {
        if(!_isAnimationRunning)
        {
            _isAnimationRunning = true;
            instance.PlayAnimation();
            instance.PlayAudio();
            Context.GetInstance().firstInstance = instance;
        }
    }

    public void AnimationEnd(CardBehavior instance)
    {
        Context.GetInstance().currentState = Context.GetInstance().OneCard;
        Context.GetInstance().Validation();
        bool _isAnimationRunning = false;
    }

    public void Validation()
    {
       
    }
}

class StateOneCard : State
{
    bool _isAnimationRunning = false;

    public void OnClick(CardBehavior instance)
    {
        if (!_isAnimationRunning && Context.GetInstance().firstInstance.GetInstanceID() != instance.GetInstanceID());
        {
            _isAnimationRunning = true;
            instance.PlayAnimation();
            instance.PlayAudio();
            Context.GetInstance().secondInstance = instance;
        }
    }

    public void AnimationEnd(CardBehavior instance)
    {
        Context.GetInstance().currentState = Context.GetInstance().TwoCards;
        Context.GetInstance().Validation();
        bool _isAnimationRunning = false;
    }

    public void Validation()
    { 
    }
}

class StateTwoCards : State
{
    public void OnClick(CardBehavior instance)
    {
       
    }


    bool IsAnimationFinished_1 = false;
    bool IsAnimationFinished_2 = false;

    public void AnimationEnd(CardBehavior instance)
    { 
        if (Context.GetInstance().firstInstance.GetInstanceID() == instance.GetInstanceID())
        {
            IsAnimationFinished_1 = true;
        }
        if (Context.GetInstance().secondInstance.GetInstanceID() == instance.GetInstanceID())
        {
            IsAnimationFinished_2 = true;
        }
        if(IsAnimationFinished_1 && IsAnimationFinished_2)
        {
            IsAnimationFinished_1 = false;
            IsAnimationFinished_2 = false;
            Context.GetInstance().currentState = Context.GetInstance().NoCards;
            Context.GetInstance().firstInstance = null;
            Context.GetInstance().secondInstance = null;
        }
    }

    public void Validation()
    {
        CardBehavior firstCard = Context.GetInstance().firstInstance;
        CardBehavior secondCard = Context.GetInstance().secondInstance;
        if(firstCard.GetCurrentSprite().name == secondCard.GetCurrentSprite().name)
        {
            Context.GetInstance().firstInstance.Dispose();
            Context.GetInstance().secondInstance.Dispose();
            Context.GetInstance().currentState = Context.GetInstance().NoCards;
            Context.GetInstance().firstInstance = null;
            Context.GetInstance().secondInstance = null;
        }
        else
        {
            firstCard.PlayAnimation();
            secondCard.PlayAnimation();
        }
    }
}
