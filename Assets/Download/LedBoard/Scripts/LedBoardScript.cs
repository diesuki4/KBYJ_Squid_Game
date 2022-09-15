using System.Collections.Generic;
using UnityEngine;

public enum Move { Right_Down, Left_Up };
public enum LedMesh { Cube, Sphere}

/// <summary>
/// Main LedBoard script
/// </summary>
public class LedBoardScript : MonoBehaviour
{
    /// <summary>
    /// Led prefab
    /// </summary>
    public GameObject PrefabLed;
    /// <summary>
    /// Letter prefab
    /// </summary>
    public GameObject PrefabLetterField;

    /// <summary>
    /// "On" led material
    /// </summary>
    public Material PrefOnMaterial;
    /// <summary>
    /// "Off" led material
    /// </summary>
    public Material PrefOffMaterial;

    [SerializeField] public string LedText;
    [SerializeField] public LedMesh LedType = LedMesh.Cube;
    [SerializeField] public int distLetFielsH = 1;
    [SerializeField] public int distLetFielsV = 0;
    [SerializeField] public bool isHorizontal = true;
    [SerializeField] public bool isBlink = false;
    [SerializeField] public float DeltaBlinkTime = 0.5f;
    [SerializeField] public bool isMove = false;
    [SerializeField] public Move MoveTo = Move.Right_Down;
    [SerializeField] public float DeltaMoveTime = 0.5f;
    [SerializeField] public Color LedColor = Color.yellow;
    [SerializeField] public Color BoardColor = Color.black;
    [SerializeField] public Vector3 LedScale = new Vector3(0.8f, 0.8f, 0.8f);

    /// <summary>
    /// List of board letters
    /// </summary>
    public List<char> BoardLetters = new List<char>();
    /// <summary>
    /// List of board letter`s field
    /// </summary>
    public List<LetterFieldScript> LetFieldsList = new List<LetterFieldScript>();
    /// <summary>
    /// All leds on board
    /// </summary>
    public List<LedScript> AllLeds = new List<LedScript>();
    /// <summary>
    /// "On" leds on board
    /// </summary>
    public List<LedScript> OnLeds = new List<LedScript>();
    /// <summary>
    /// "Off" leds of board
    /// </summary>
    public List<LedScript> OffLeds = new List<LedScript>();

    private int BoardWidth;
    private int BoardHeight;

    private const int constheight = 7;
    private const int constwidth = 5;
    private int HeightLetter => constheight + distLetFielsV;
    private int WidthLetter => constwidth + distLetFielsH;
    private int LedCount => HeightLetter * WidthLetter;
    private Vector3 CurrentLetFieldPos;

    private void Awake()
    {
        BoardConstructor();
    }

    /// <summary>
    /// Create LedBoard
    /// </summary>
    public void BoardConstructor()
    {

        //Find leds
        LedScript[] findedleds = GetComponentsInChildren<LedScript>();
        LetterFieldScript[] findedfields = GetComponentsInChildren<LetterFieldScript>();

        //Delete leds
        foreach (LedScript item in findedleds)
        {
            DestroyImmediate(item.gameObject);
        }
        foreach (LetterFieldScript letters in findedfields)
        {
            DestroyImmediate(letters.gameObject);
        }

        //Clear lists
        if (LetFieldsList.Count != 0)
            LetFieldsList.Clear();
        if (AllLeds.Count != 0)
            AllLeds.Clear();
        if (OnLeds.Count != 0)
            OnLeds.Clear();
        if (OffLeds.Count != 0)
            OffLeds.Clear();

        CurrentLetFieldPos = new Vector3();

        switch (LedType)
        {
            case LedMesh.Cube:
                {
                    PrefabLed = (GameObject)Resources.Load("Led");
                }
                break;
            case LedMesh.Sphere:
                {
                    PrefabLed = (GameObject)Resources.Load("Led_Sphere");
                }
                break;
            default:
            break;
        }

        //Create new leds and letter fields from string
        BoardLetters = new List<char>(LedText.ToCharArray());
        int index = 0;
        foreach (char letter in BoardLetters)
        {
            CreateLetterField(letter, index);
            index++;
        }

        GetAllLeds();
    }

    /// <summary>
    /// Create leds on letter field
    /// </summary>
    /// <param name="letter">letter</param>
    /// <param name="indexLetter">index letter</param>
    public void CreateLetterField(char letter, int indexLetter)
    {
        string[] bitlines = BitString(letter);

        //Create LetterField
        GameObject LetFieldObj = Instantiate(PrefabLetterField, GetComponent<Transform>());
        LetFieldObj.name = "Letter_" + letter.ToString();
        LetFieldObj.transform.localPosition = CurrentLetFieldPos;

        if (isHorizontal)
        {
            CurrentLetFieldPos += Vector3.right * WidthLetter;
        }
        else
        {
            CurrentLetFieldPos += Vector3.up * -HeightLetter;
        }

        LetterFieldScript lfs = LetFieldObj.GetComponent<LetterFieldScript>();
        LetFieldsList.Add(lfs);

        //Create Leds
        int irow = 0;
        int icol = 0;
        for (int i = 0; i < LedCount; i++)
        {
            GameObject iLedObj = Instantiate(PrefabLed, LetFieldObj.transform);
            iLedObj.name = "iLed" + i.ToString();
            iLedObj.transform.localScale = LedScale;
            lfs.ListLeds.Add(iLedObj.GetComponent<LedScript>());

            if (i % WidthLetter == 0)
            {
                irow -= 1;
                icol = 0;
            }
            icol += 1;

            iLedObj.transform.localPosition = new Vector3(icol, irow, 0.0f);
            LedScript iLedScript = iLedObj.GetComponent<LedScript>();

            iLedScript.IndexX = Mathf.Abs(icol) + WidthLetter * indexLetter;
            iLedScript.IndexY = Mathf.Abs(irow) + HeightLetter * indexLetter;

            bool isOne = bitlines[Mathf.Abs(irow) - 1].Substring(Mathf.Abs(icol) - 1, 1) == "1";
            iLedScript.isOne = isOne;
            iLedScript.ApplyMaterial();

            if (isOne)
            {
                OnLeds.Add(iLedObj.GetComponent<LedScript>());
            }
            else
            {
                OffLeds.Add(iLedObj.GetComponent<LedScript>());
            }
        }
        BoardWidth = Mathf.Abs(icol) + (WidthLetter * indexLetter);
        BoardHeight = Mathf.Abs(irow) + (HeightLetter * indexLetter);
    }

    /// <summary>
    /// String contains 0 or 1, 0 - is off led, 1 - is on led
    /// </summary>
    /// <param name="letter">letter</param>
    /// <returns></returns>
    public string[] BitString(char letter)
    {
        string[] bitlines = new string[HeightLetter];
        for (int i = 0; i < HeightLetter; i++)
        {
            if (i < HeightLetter - distLetFielsV)
            {
                FontToBit ftb = new FontToBit(letter, i);
                bitlines[i] = ftb.CreateBitLine(distLetFielsH);
            }
            else
            {
                bitlines[i] = new string('0', WidthLetter);
            }
        }
        return bitlines;
    }

    /// <summary>
    /// Get all leds from board
    /// </summary>
    public void GetAllLeds() => GetComponentsInChildren(AllLeds);

    /// <summary>
    /// Move leds on board
    /// </summary>
    public void MoveBoard()
    {
        foreach (LetterFieldScript lfs in LetFieldsList)
        {
            lfs.transform.DetachChildren();
        }
        foreach (LedScript led in AllLeds)
        {
            led.transform.parent = transform;
        }

        if (isHorizontal)
        {
            switch (MoveTo)
            {
                case Move.Right_Down:
                    {
                        foreach (LedScript item in AllLeds)
                        {
                            item.transform.localPosition += Vector3.right;
                            item.IndexX += 1;

                            if (item.IndexX > BoardWidth)
                            {
                                item.transform.localPosition = new Vector3(1.0f, item.transform.localPosition.y, item.transform.localPosition.z);
                                item.IndexX = 1;
                            }
                        }
                    }
                    break;
                case Move.Left_Up:
                    {
                        foreach (LedScript item in AllLeds)
                        {
                            item.transform.localPosition += Vector3.left;
                            item.IndexX -= 1;

                            if (item.IndexX <= 0)
                            {
                                item.transform.localPosition = new Vector3(BoardWidth, item.transform.localPosition.y, item.transform.localPosition.z);
                                item.IndexX = BoardWidth;
                            }
                        }
                    }
                    break;
                default:
                break;
            }

        }

        if (!isHorizontal)
        {
            switch (MoveTo)
            {
                case Move.Right_Down:
                    {
                        foreach (LedScript item in AllLeds)
                        {
                            item.transform.localPosition -= Vector3.up;
                            item.IndexY += 1;

                            if (item.IndexY > BoardHeight)
                            {
                                item.transform.localPosition = new Vector3(item.transform.localPosition.x, -1.0f, item.transform.localPosition.z);
                                item.IndexY = 1;
                            }
                        }
                    }
                    break;
                case Move.Left_Up:
                    {
                        foreach (LedScript item in AllLeds)
                        {
                            item.transform.localPosition -= Vector3.down;
                            item.IndexY -= 1;

                            if (item.IndexY <= 0)
                            {
                                item.transform.localPosition = new Vector3(item.transform.localPosition.x, -BoardHeight, item.transform.localPosition.z);
                                item.IndexY = BoardHeight;
                            }
                        }
                    }
                    break;
                default:
                break;
            }


        }

    }

    private float ledsec = 0.0f;
    private float movsec = 0.0f;

    /// <summary>
    /// Blinking leds
    /// </summary>
    /// <param name="deltaTime">Delta time</param>
    private void Blinking(float deltaTime)
    {
        ledsec += Time.deltaTime;
        if (deltaTime <= ledsec)
        {
            foreach (LedScript ls in OnLeds)
            {
                if (ls.isOne)
                {
                    ls.TurnOff();
                }
                else
                {
                    ls.TurnOn();
                }
            }
            ledsec = 0.0f;
        }
    }

    private void Update()
    {
        if (isBlink)
        {
            Blinking(DeltaBlinkTime);
        }
        if (isMove)
        {
            movsec += Time.deltaTime;
            if (movsec >= DeltaMoveTime)
            {
                MoveBoard();
                movsec = 0.0f;
            }
        }

        /*if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LedText = "test";
            BoardConstructor();
        }*/
    }
}

