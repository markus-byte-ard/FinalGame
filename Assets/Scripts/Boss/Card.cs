using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {
    public CardData cardData = null;

    public Text titleText = null;
    public Text descriptionText = null;
    public Text costText = null;
    public Text damageText = null;
    public Image cardImage = null;
    public Image frameImage = null;
    public Image burnImage = null;

    public void Initialise() {
        if (cardData == null) {
            Debug.LogError("Card has no CardData");
            return;
        }
        titleText.text = cardData.cardTitle;
        descriptionText.text = cardData.description;
        cardImage.sprite = cardData.cardImage;
        frameImage.sprite = cardData.frameImage;
        costText.text = cardData.cost.ToString();
        damageText.text = cardData.damage.ToString();
    }
}
