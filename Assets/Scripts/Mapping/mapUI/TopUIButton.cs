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
    public void buttonPress()
    {
        mapUI.instance.buttonPressed(screen);
        activate();
    }
    public void deselect()
    {
        image.sprite = inactiveSprite;
        pressed = false;
        if(unpressedState.highlightedSprite != null)
            button.spriteState = unpressedState;
    }

    public void highlight()
    {
        image.sprite = highlightedSprite;
        unpressedState = button.spriteState;
    }
    public void activate()
    {
        unpressedState = button.spriteState;
        pressedState.selectedSprite = activeSprite;
        pressedState.highlightedSprite = activeSprite;
        button.spriteState = pressedState;

        image.sprite = activeSprite;
        pressed = true;
    }

    private void Update()
    {
        if (pressed)
            button.image.sprite = activeSprite;
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
