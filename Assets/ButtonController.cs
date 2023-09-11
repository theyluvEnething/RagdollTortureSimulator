using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    private Color failColor = new Color(255, 0, 0, 255);
    private Color successColor = new Color(0, 255, 0, 255);
    private Color selectColor = new Color(255, 255, 0, 255);

    public void BuyItemByIndex(int index)
    {
        if (index < 0)
        {
            Debug.LogError("Wrong Index for Item was given to the button.");
            return;
        }

        GameManager.Item item = (GameManager.Item)index;
        int success = GameManager.Instance.BuyItem(item);

        switch (success)
        {
            case 0:
                StartCoroutine(ButtonPressed(failColor));
                break;
            case 1:
                StartCoroutine(ButtonPressed(successColor));
                break;
            case 2:
                StartCoroutine(ButtonPressed(selectColor));
                break;
        }
    }
    private IEnumerator ButtonPressed(Color _color)
    {
        Button button = GetComponent<Button>();
        if (button)
        {
            ColorBlock originalColor = button.colors;
            ColorBlock buttonColor = button.colors;
            buttonColor.normalColor = _color;
            buttonColor.highlightedColor = _color;
            buttonColor.pressedColor = _color;
            buttonColor.selectedColor = _color;
            buttonColor.disabledColor = _color;
            button.colors = buttonColor;

            yield return new WaitForSeconds(0.1f);

            button.colors = originalColor;
        }
    }
}
