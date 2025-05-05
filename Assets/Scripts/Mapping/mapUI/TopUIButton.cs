using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopUIButton : MonoBehaviour
{
    public bool selected;
    public bool pressed;

    [SerializeField]
    private Sprite inactiveSprite;
    [SerializeField]
    private Sprite highlightedSprite;
    [SerializeField]
    private Sprite activeSprite;
    [SerializeField]
    Button button;

    SpriteState unpressedState;
    SpriteState pressedState;

    [Header("Image")]
    public Image image;
    public mapUI.mapScreens screen;

    public Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();
        pressedState.selectedSprite = activeSprite;
        pressedState.highlightedSprite = activeSprite;
        unpressedState.selectedSprite = inactiveSprite;
        unpressedState.highlightedSprite = highlightedSprite;
    }
    public void buttonPress()
    {
        mapUI.instance.buttonPressed(screen);
        activate();
    }
    public void deselect()
    {
        image.sprite = inactiveSprite;
        unpressedState.highlightedSprite = highlightedSprite;
        resetStates();
        pressed = false;
        if(unpressedState.highlightedSprite != null)
            button.spriteState = unpressedState;
    }

    public void highlight()
    {
        image.sprite = highlightedSprite;

        //unpressedState = button.spriteState;
    }
    public void activate()
    {
        unpressedState = button.spriteState;
        button.spriteState = pressedState;
        image.sprite = activeSprite;
        pressed = true;
    }

    public void resetStates()
    {
        if (pressed)
            return;
        pressedState.highlightedSprite = activeSprite;
        pressedState.selectedSprite = activeSprite;
        unpressedState.highlightedSprite = highlightedSprite;
        unpressedState.selectedSprite = inactiveSprite;
        //unpressedState.pressedSprite = inactiveSprite;
    }

    private void Update()
    {
        if (pressed)
            button.image.sprite = activeSprite;
        if(!pressed)
        {
            unpressedState.highlightedSprite = highlightedSprite;
        }
    }

    public void leftButton()
    {
        mapUI.instance.pressLeftButton();
    }
    public void rightButton()
    {
        mapUI.instance.pressRightButton();
    }
}
