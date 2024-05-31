using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObjectStruct[] selections;
    public GameObjectStruct[] hGates;
    public GameObjectStruct[] xGates;
    public GameObjectStruct[] cxGates;

    public GameObject[] lasers;

    public GateType[,] entries;

    List<int[]> statePool = new List<int[]>();

    int currentRow = 0;
    int currentCol = 0;

    int bitsCount, cyclesCount;

    private void Start()
    {
        bitsCount = selections.Length;
        cyclesCount = selections[0].cols.Length;

        InitEntryArray();
    }

    private void EnableSelection()
    {
        for (int s = 0; s < selections.Length; s++)
        {
            for (int k = 0; k < selections[0].cols.Length; k++)
            {
                selections[s].cols[k].SetActive(false);
            }
        }
        selections[currentRow].cols[currentCol].SetActive(true);
    }

    public void EnableHGate()
    {
        xGates[currentRow].cols[currentCol].SetActive(false);
        cxGates[currentRow].cols[currentCol].SetActive(false);
        hGates[currentRow].cols[currentCol].SetActive(!hGates[currentRow].cols[currentCol].activeSelf);
    }

    public void EnableXGate()
    {
        hGates[currentRow].cols[currentCol].SetActive(false);
        cxGates[currentRow].cols[currentCol].SetActive(false);
        xGates[currentRow].cols[currentCol].SetActive(!xGates[currentRow].cols[currentCol].activeSelf);
    }

    public void EnableCXGate()
    {
        hGates[currentRow].cols[currentCol].SetActive(false);
        xGates[currentRow].cols[currentCol].SetActive(false);
        cxGates[currentRow].cols[currentCol].SetActive(!cxGates[currentRow].cols[currentCol].activeSelf);
    }

    public void MoveRight()
    {
        currentCol = (currentCol + 1) % selections[0].cols.Length;
    }

    public void MoveLeft()
    {
        currentCol = (currentCol - 1) % selections[0].cols.Length;

        if (currentCol < 0)
            currentCol += selections[0].cols.Length;
    }

    public void MoveUp()
    {
        currentRow = (currentRow - 1) % selections.Length;

        if (currentRow < 0)
            currentRow += selections.Length;
    }

    public void MoveDown()
    {
        currentRow = (currentRow + 1) % selections.Length;
    }

    public void EnableLaser(int index)
    {
        lasers[index].SetActive(true);
        lasers[index].GetComponent<Rigidbody2D>().velocity = new Vector2(0, 10);
    }

    public void InitEntryArray()
    {
        entries = new GateType[bitsCount, cyclesCount];
        for (int i = 0; i < bitsCount; i++)
        {
            for (int j = 0; j < cyclesCount; j++)
            {
                entries[i, j] = GateType.Empty;
            }
        }
    }

    public void FillEntryArray()
    {
        for (int i = 0; i < bitsCount; i++)
        {
            for (int j = 0; j < cyclesCount; j++)
            {
                if (xGates[i].cols[j].activeSelf)
                {
                    entries[i, j] = GateType.X;
                }
                else if (hGates[i].cols[j].activeSelf)
                {
                    entries[i, j] = GateType.H;
                }
                else if (cxGates[i].cols[j].activeSelf)
                {
                    entries[i, j] = GateType.CX;
                }
                else
                {
                    entries[i, j] = GateType.Empty;
                }
            }
        }
    }

    public void FillStatePool()
    {
        // init with adding one entry: 000..00

        int[] initialBits = Enumerable.Repeat(0, bitsCount).ToArray();
        statePool.Clear();
        statePool.Add(initialBits);

        for (int i = 0; i < cyclesCount; i++)
        {
            for (int j = 0; j < bitsCount; j++)
            {
                if (entries[j,i] == GateType.X)
                {
                    for (int k = 0; k < statePool.Count; k++)
                    {
                        statePool[k][j] = statePool[k][j] == 0 ? 1 : 0;
                    }
                }
                else if (entries[j,i] == GateType.H)
                {
                    int count = statePool.Count;
                    
                    for (int k = 0; k < count; k++)
                    {
                        int[] newBits = new int[bitsCount];
                        for (int t = 0; t < bitsCount; t++)
                        {
                            if (t == j)
                            {
                                newBits[t] = statePool[k][t] == 0 ? 1 : 0;
                            }
                            else
                            {
                                newBits[t] = statePool[k][t];
                            }
                        }
                        
                        statePool.Add(newBits);
                    }
                }
                else if (entries[j,i] == GateType.CX)
                {
                    for (int k = 0; k < statePool.Count; k++)
                    {
                        if (statePool[k][j] == 1)
                        {
                            statePool[k][(j + 1) % bitsCount] = statePool[k][(j + 1) % bitsCount] == 0 ? 1 : 0;
                        }
                    }
                }
            }
        }

    }

    public int CalculateIndex(int[] state)
    {
        int result = 0;
        for (int j = 0; j < state.Length; j++)
        {
            result += ((int)Mathf.Pow(2, state.Length - 1 - j) * state[j]);
        }

        return result;
    }

    public void ResetLasers()
    {
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].transform.position = new Vector2(lasers[i].transform.position.x, 2.1f);
            lasers[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            lasers[i].SetActive(false);
        }
    }
    
    public void Fire()
    {
        ResetLasers();
        FillEntryArray();
        FillStatePool();

        if (statePool.Count > 0)
        {
            
            if (statePool.Count <= 2)
            {
                for (int i = 0; i < statePool.Count; i++)
                {
                    EnableLaser(CalculateIndex(statePool[i]));
                }
            }
            else
            {
                int randCount = Random.Range(1, 4);
                for (int i = 0; i < randCount; i++)
                {
                    int randState = Random.Range(0, statePool.Count);
                    EnableLaser(CalculateIndex(statePool[randState]));
                }
            }

        }

    }

   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            MoveDown();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            MoveUp();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            MoveRight();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            MoveLeft();
        }

        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            EnableXGate();
        }

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            EnableHGate();
        }

        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            EnableCXGate();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Fire();
        }

        EnableSelection();
    }


    [System.Serializable]
    public struct GameObjectStruct
    {
        public GameObject[] cols;
    }

    public enum GateType
    {
        Empty = 0,
        X = 1, 
        H = 2,
        CX = 3,
        XC = 4
    }
}
