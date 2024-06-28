using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagesGenerator : MonoBehaviour
{
    public Sprite[] imagesSprites;

    public GameObject imageContainer;
    public Transform parent;

    public int numberImages;
    public ImageData Img;

  //  public GameObject imageSelectedScreen;

    private void Awake()
    {
        
       // imageSelectedScreen = ImageSelecrted.Instance.gameObject;
    }
    private void Start()
    {
        numberImages = imagesSprites.Length;

       for (int i = 0; i < numberImages; i++)
        {
            // Instantiate the image container
            GameObject imgContainer = Instantiate(imageContainer, parent) as GameObject;

            // Set the sprite of the child Image component
            imgContainer.transform.GetChild(0).GetComponent<Image>().sprite = imagesSprites[i];

            // Set up the button listener
            int index = i;  // Capture the current value of i
            imgContainer.GetComponent<Button>().onClick.AddListener(() => SetIMage(index));
        }
    }

    public void SetIMage(int indeex)
    {
        Img.sprite= imagesSprites[indeex];
        //   ImageSelecrted.Instance.gameObject.SetActive(true);
        GameController.instance.EnableSelectedScreen();
    }
}
