using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class GameManager : MonoBehaviour
{
    private Transform gameTransform;
    [SerializeField] private Transform piecePrefab;
    public Material material;
    public ImageData img; // Assuming this is a custom class holding a sprite
    private List<Transform> pieces;
    private int emptyLocation;
    private int size;
    private bool shuffling = false;
    private bool isGameCompleted = false; // Flag to track if the game is completed

    public bool usePowerUp = false;
    private Transform firstPressedPiece = null; // To keep track of the first pressed piece when power-up is active


    public void SetUsePowerUp()
    {
        usePowerUp = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        pieces = new List<Transform>();
        size = img.size;
        //gameTransform =Instantiate(gameObject,new Vector3(0.0199999996f, -1.05999994f, -5.25f),Quaternion.identity).transform;
        // Ensure the texture is readable

        gameTransform=GameController.instance.GameBoard.transform;
        if (img.sprite.texture.isReadable)
        {
            Texture2D newTexture = SpriteToTexture2D(img.sprite);
            if (material != null)
            {
                // Change the texture using the shader property name "_MainTex"
                material.SetTexture("_MainTex", newTexture);
                Debug.Log("Texture changed successfully.");
            }
        }
        else
        {
            Debug.LogError("Sprite texture is not readable. Enable Read/Write in the texture import settings.");
        }

        CreateGamePieces(0.01f);
    }

    public void InitisaliseGame()
    {
        pieces = new List<Transform>();
        size = img.size;

        // Ensure the texture is readable
        if (img.sprite.texture.isReadable)
        {
            Texture2D newTexture = SpriteToTexture2D(img.sprite);
            if (material != null)
            {
                // Change the texture using the shader property name "_MainTex"
                material.SetTexture("_MainTex", newTexture);
                Debug.Log("Texture changed successfully.");
            }
        }
        else
        {
            Debug.LogError("Sprite texture is not readable. Enable Read/Write in the texture import settings.");
        }

        CreateGamePieces(0.01f);
    }

    private void CreateGamePieces(float gapThickness)
    {
        // This is the width of each tile.
        float width = 1f / size;
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                Transform piece = Instantiate(piecePrefab, gameTransform);
                pieces.Add(piece);
                // Pieces will be in a game board going from -1 to +1.
                piece.localPosition = new Vector3(-1 + (2 * width * col) + width,
                                                  1 - (2 * width * row) - width,
                                                  0);
                piece.localScale = ((2 * width) - gapThickness) * Vector3.one;
                piece.name = $"{(row * size) + col}";

              //  // Add a TextMeshPro component to display the number
              //  GameObject textObj = new GameObject("TextMeshPro");
              //  textObj.transform.SetParent(piece);
              //  TextMesh textMeshPro = textObj.AddComponent<TextMesh>();
              //  textMeshPro.text = $"{(row * size) + col}";
              ////  textMeshPro.alignment = TextAlignmentOptions.Center;
              //  textMeshPro.fontSize = 24; // Adjust as needed for better visibility
              //  textMeshPro.transform.localScale = new Vector3(0.14f, 0.14f, 0.14f);

                // Adjust the TextMeshPro object's RectTransform
                //RectTransform textRect = textObj.GetComponent<RectTransform>();
                // textRect.sizeDelta = piece.localScale * 100; // Adjust as needed for better visibility
               // textMeshPro.transform.position = Vector3.zero;

                // We want an empty space in the bottom right.
                if (row == size - 1 && col == size - 1)
                {
                    emptyLocation = (size * size) - 1;
                    piece.gameObject.SetActive(false);
                }
                else
                {
                    // We want to map the UV coordinates appropriately, they are 0->1.
                    float gap = gapThickness / 2;
                    Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                    Vector2[] uv = new Vector2[4];
                    // UV coord order: (0, 1), (1, 1), (0, 0), (1, 0)
                    uv[0] = new Vector2((width * col) + gap, 1 - ((width * (row + 1)) - gap));
                    uv[1] = new Vector2((width * (col + 1)) - gap, 1 - ((width * (row + 1)) - gap));
                    uv[2] = new Vector2((width * col) + gap, 1 - ((width * row) + gap));
                    uv[3] = new Vector2((width * (col + 1)) - gap, 1 - ((width * row) + gap));
                    // Assign our new UVs to the mesh.
                    mesh.uv = uv;

                    // Apply the material with the texture to the piece
                    piece.GetComponent<Renderer>().material = material;
                }
            }
        }
    }

    // Helper method to convert a Sprite to Texture2D
    private Texture2D SpriteToTexture2D(Sprite sprite)
    {
        // Create a new Texture2D with the same dimensions as the sprite
        Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        // Get the pixels from the sprite's texture
        Color[] pixels = sprite.texture.GetPixels((int)sprite.textureRect.x, (int)sprite.textureRect.y, (int)sprite.textureRect.width, (int)sprite.textureRect.height);
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }

    // Update is called once per frame
    void Update()
    {
        // Check for completion.
        if (!shuffling && !isGameCompleted && CheckCompletion())
        {
            isGameCompleted = true;
            shuffling = true;
            StartCoroutine(WaitShuffle(0.5f));
        }

        // On click send out ray to see if we click a piece.
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                for (int i = 0; i < pieces.Count; i++)
                {
                    if (pieces[i] == hit.transform)
                    {
                        if (usePowerUp)
                        {
                            if (firstPressedPiece == null)
                            {
                                firstPressedPiece = pieces[i];
                                SoundManager.instance.PlayPannelPopSound();
                            }
                            else
                            {
                                SwapPieces(firstPressedPiece, pieces[i]);
                                SoundManager.instance.PlayPieceSound();
                                firstPressedPiece = null;
                                usePowerUp = false;
                            }
                            break;
                        }

                        // Check each direction to see if valid move.
                        // We break out on success so we don't carry on and swap back again.

                        // Vertical Check
                        if (SwapIfValid(i, -size, size)) { SoundManager.instance.PlayPieceSound(); break; }
                        if (SwapIfValid(i, +size, size)) { SoundManager.instance.PlayPieceSound(); break; }
                        // Horizontal Check
                        if (SwapIfValid(i, -1, 0)) { SoundManager.instance.PlayPieceSound(); break; }
                        if (SwapIfValid(i, +1, size - 1)) { SoundManager.instance.PlayPieceSound(); break; }
                    }
                }
            }
        }
    }

    // colCheck is used to stop horizontal moves wrapping.
    private bool SwapIfValid(int i, int offset, int colCheck)
    {
        if (((i % size) != colCheck) && ((i + offset) == emptyLocation))
        {
            // Swap them in game state.
            (pieces[i], pieces[i + offset]) = (pieces[i + offset], pieces[i]);
            // Swap their transforms.
            (pieces[i].localPosition, pieces[i + offset].localPosition) = ((pieces[i + offset].localPosition, pieces[i].localPosition));
            // Update empty location.
            emptyLocation = i;
            return true;
        }
        return false;
    }

    private void SwapPieces(Transform piece1, Transform piece2)
    {
        Vector3 tempPosition = piece1.localPosition;
        piece1.localPosition = piece2.localPosition;
        piece2.localPosition = tempPosition;

        int index1 = pieces.IndexOf(piece1);
        int index2 = pieces.IndexOf(piece2);

        pieces[index1] = piece2;
        pieces[index2] = piece1;
    }

    // We name the pieces in order so we can use this to check completion.
    private bool CheckCompletion()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].name != $"{i}")
            {
                return false;
            }
        }
        GameController.instance.ShowGameOver();
        return true;
    }

    private IEnumerator WaitShuffle(float duration)
    {
        yield return new WaitForSeconds(duration);
        Shuffle();
        shuffling = false;
    }

    // Brute force shuffling.
    private void Shuffle()
    {
        int count = 0;
        int last = 0;
        while (count < (size * size * size))
        {
            // Pick a random location.
            int rnd = Random.Range(0, size * size);
            // Only thing we forbid is undoing the last move.
            if (rnd == last) { continue; }
            last = emptyLocation;
            // Try surrounding spaces looking for valid move.
            if (SwapIfValid(rnd, -size, size))
            {
                count++;
            }
            else if (SwapIfValid(rnd, +size, size))
            {
                count++;
            }
            else if (SwapIfValid(rnd, -1, 0))
            {
                count++;
            }
            else if (SwapIfValid(rnd, +1, size - 1))
            {
                count++;
            }
        }
    }
}
