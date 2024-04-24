using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MatchResult
{
    public List<Object> connectedObject;
    public MatchDirection direction;
}

public enum MatchDirection
{
    Vertical,
    Horizontal,
    LongVertical,
    LongHorizontal,
    Super,
    None
}

public class ObjectBoard : MonoBehaviour
{

    [Header("Config")]
    [SerializeField] private int _width = 6;
    [SerializeField] private int _height = 8;

    [SerializeField] private float _spacingX;
    [SerializeField] private float _spacingY;

    [SerializeField] private ObjectBoard Board;

    [SerializeField] private GameObject _objectPrefab;
    [SerializeField] private ObjectConfig[] _objectConfigs;

    [SerializeField] private Node[,] _objectBoard;

    public ArrayLayout _arrayLayout;

    public List<GameObject> ObjectToDestroy = new();

    [SerializeField] private Object _selectedObject;

    [SerializeField] private bool _isProcessingMove;

    [SerializeField] List<Object> _objectToRemove = new();

    private float _delay = 0.2f;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null && hit.collider.gameObject.GetComponent<Object>())
            {

                if (_isProcessingMove)
                {
                    return;
                }

                Object obj = hit.collider.gameObject.GetComponent<Object>();
               // Debug.Log("I have a clicked a potion it is: " + obj.gameObject);

                SelectObject(obj);
            }
        }
    }

    public void Constract()
    {

    }

    private void Init()
    {
        DestroyObjects();
        _objectBoard = new Node[_width, _height];

        _spacingX = (float)(_width - 1) / 2;
        _spacingY = (float)((_height - 1) / 2) +1;

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                Vector2 position = new Vector2(x - _spacingX, y - _spacingY);

                if (_arrayLayout.rows[y].row[x] == true)
                {
                    _objectBoard[x, y] = new Node(false, null);
                }
                else
                {
                    GameObject newObj = Instantiate(_objectPrefab, position, Quaternion.identity); // in pool
                    newObj.transform.SetParent(transform);

                    Object obj = newObj.GetComponent<Object>();

                    int randomIndex = Random.Range(0, _objectConfigs.Length);
                    obj.Constract(_objectConfigs[randomIndex]);

                    obj.SetIndex(x, y);
                    _objectBoard[x, y] = new Node(true, newObj);
                    ObjectToDestroy.Add(newObj);
                }
            }
        }

        if (CheckBoard(false) == true)
        {
            Debug.Log("We have matches let's re-create the board");
            Init();
        }
        else
        {
            Debug.Log("There are no matches, it's time to start the game!");
        }
    }

    private void DestroyObjects()
    {
        if (ObjectToDestroy != null)
        {
            foreach (GameObject obj in ObjectToDestroy)
            {
                Destroy(obj);
            }
        }
        ObjectToDestroy.Clear();
    }

    public bool CheckBoard(bool takeAction)
    {
        Debug.Log("Checking Board");
        bool hasMatched = false;

        List<Object> objectsToRemove = new();

        foreach(Node nodeObject in _objectBoard)
        {
            if (nodeObject.Object != null)
            {
                nodeObject.Object.GetComponent<Object>().SetMatched(false);
            }
        }

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (_objectBoard[x, y].IsUsable)
                {
                    Object obj = _objectBoard[x, y].Object.GetComponent<Object>();

                    if (obj.IsMatched == false)
                    {
                        MatchResult matchedObjects = IsConnected(obj);

                        if (matchedObjects.connectedObject.Count >= 3)
                        {
                            MatchResult superMatchedObjects = SuperMatch(matchedObjects);

                            objectsToRemove.AddRange(superMatchedObjects.connectedObject);

                            foreach (Object pot in superMatchedObjects.connectedObject)
                            {
                                obj.SetMatched(true);
                            }

                            hasMatched = true;
                        }
                    }
                }
            }
        }

        if(takeAction == true)
        {
            foreach (Object objectToRemove in objectsToRemove)
            {
                objectToRemove.SetMatched(false);
            }

            RemoveAndRefill(objectsToRemove);

            if (CheckBoard(false) == true)
            {
                CheckBoard(true);
            }
        }

        return hasMatched;
    }

    private void RemoveAndRefill(List<Object> objectsToRemove)
    {
        foreach(Object obj in objectsToRemove)
        {
            int xIndex = obj.XIndex;
            int yIndex = obj.YIndex;

            Destroy(obj.gameObject);

            _objectBoard[xIndex, yIndex] = new Node(true, null);
        }

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {

                if (_objectBoard[x,y].Object == null)
                {
                    Debug.Log("The location x: " + x + " y" + "is emprt, attempting to refill it.");
                    RefillObject(x, y);
                }
            }
        }
    }

    private void RefillObject(int x, int y)
    {
        int yOffset = 1;

        while (y + yOffset < _height && _objectBoard[x, y + yOffset].Object == null)
        {
            Debug.Log("Object above me is null");
            yOffset++;
        }

        if (y + yOffset < _height && _objectBoard[x, y + yOffset].Object != null)
        {
            Object objectAbove = _objectBoard[x, y + yOffset].Object.GetComponent<Object>();

            Vector3 targetPos = new Vector3(x - _spacingX, y - _spacingY, objectAbove.transform.position.z);
            Debug.Log("");

            objectAbove.MoveToTarget(targetPos);

            objectAbove.SetIndex(x, y);

            _objectBoard[x, y] = _objectBoard[x, y + yOffset];

            _objectBoard[x, y + yOffset] = new Node(true, null);
        }

        if (y + yOffset == _height)
        {
            Debug.Log("");
            SpawnObjectAtTop(x);
        }
    }

    private void SpawnObjectAtTop(int x)
    {
        int index = FindIndexOfLowestNull(x);

        int locationToMove = 8 - index;
        Debug.Log("");

        GameObject newObj = Instantiate(_objectPrefab, new Vector2(x - _spacingX, _height - _spacingY), Quaternion.identity); // in pool
        newObj.transform.SetParent(transform);

        Object obj = newObj.GetComponent<Object>();

        int randomIndex = Random.Range(0, _objectConfigs.Length);
        obj.Constract(_objectConfigs[randomIndex]);

        obj.SetIndex(x, index);

        _objectBoard[x, index] = new Node(true, newObj);

        Vector3 targetPosition = new Vector3(newObj.transform.position.x, newObj.transform.position.y - locationToMove, newObj.transform.position.z);
        obj.MoveToTarget(targetPosition);
    }

    private int FindIndexOfLowestNull(int x)
    {
        int lowestNull = 99;

        for (int y = 7; y >= 0; y--)
        {
            if (_objectBoard[x, y].Object == null)
            {
                lowestNull = y;
            }
        }

        return lowestNull;
    }

    #region Cascading Objects



    #endregion

    private MatchResult SuperMatch(MatchResult matchedResults)
    {
        if (matchedResults.direction == MatchDirection.Horizontal || matchedResults.direction == MatchDirection.LongHorizontal)
        {

            foreach (Object obj in matchedResults.connectedObject)
            {
                List<Object> extraConnectedObjects = new List<Object>();

                CheckDirection(obj, new Vector2Int(0, 1), extraConnectedObjects);
                CheckDirection(obj, new Vector2Int(0, -1), extraConnectedObjects);

                if (extraConnectedObjects.Count >= 2)
                {
                    Debug.Log("I have a super Horizontal match");
                    extraConnectedObjects.AddRange(matchedResults.connectedObject);

                    return new MatchResult {
                        connectedObject = extraConnectedObjects,
                        direction = MatchDirection.Super,
                    };

                }
            }

            return new MatchResult()
            {
                connectedObject = matchedResults.connectedObject,
                direction = matchedResults.direction,
            };
        }
        else if (matchedResults.direction == MatchDirection.Vertical || matchedResults.direction == MatchDirection.LongVertical)
        {

            foreach (Object obj in matchedResults.connectedObject)
            {
                List<Object> extraConnectedObjects = new List<Object>();

                CheckDirection(obj, new Vector2Int(1, 0), extraConnectedObjects);
                CheckDirection(obj, new Vector2Int(-1, 0), extraConnectedObjects);

                if (extraConnectedObjects.Count >= 2)
                {
                    Debug.Log("I have a super Vertical match");
                    extraConnectedObjects.AddRange(matchedResults.connectedObject);

                    return new MatchResult
                    {
                        connectedObject = extraConnectedObjects,
                        direction = MatchDirection.Super,
                    };

                }
            }

            return new MatchResult()
            {
                connectedObject = matchedResults.connectedObject,
                direction = matchedResults.direction,
            };
        }

        return null;
    }

    MatchResult IsConnected(Object obj)
    {
        List<Object> connectedObject = new();
        ObjectType objectType = obj.Type;

        connectedObject.Add(obj);

        CheckDirection(obj, new Vector2Int(1, 0), connectedObject);
        CheckDirection(obj, new Vector2Int(-1, 0), connectedObject);

        if (connectedObject.Count == 3)
        {
            Debug.Log("I have a normal horizontal match, the color of my match is: " + connectedObject[0].Type);

            return new MatchResult
            {
                connectedObject = connectedObject,
                direction = MatchDirection.Horizontal
            };
        }
        //checking for more than 3 (Long horizontal Match)
        else if (connectedObject.Count > 3)
        {
            Debug.Log("I have a Long horizontal match, the color of my match is: " + connectedObject[0].Type);

            return new MatchResult
            {
                connectedObject = connectedObject,
                direction = MatchDirection.LongHorizontal
            };
        }
        //clear out the connectedpotions
        connectedObject.Clear();
        //readd our initial potion
        connectedObject.Add(obj);

        //check up
        CheckDirection(obj, new Vector2Int(0, 1), connectedObject);
        //check down
        CheckDirection(obj, new Vector2Int(0, -1), connectedObject);

        //have we made a 3 match? (Vertical Match)
        if (connectedObject.Count == 3)
        {
            Debug.Log("I have a normal vertical match, the color of my match is: " + connectedObject[0].Type);

            return new MatchResult
            {
                connectedObject = connectedObject,
                direction = MatchDirection.Vertical
            };
        }
        //checking for more than 3 (Long Vertical Match)
        else if (connectedObject.Count > 3)
        {
            Debug.Log("I have a Long vertical match, the color of my match is: " + connectedObject[0].Type);

            return new MatchResult
            {
                connectedObject = connectedObject,
                direction = MatchDirection.LongVertical
            };
        }
        else
        {
            return new MatchResult
            {
                connectedObject = connectedObject,
                direction = MatchDirection.None
            };
        }
    }

    private void CheckDirection(Object obj, Vector2Int direction, List<Object> connectedObjects)
    {
        ObjectType objectType = obj.Type;

        int x = obj.XIndex + direction.x;
        int y = obj.YIndex + direction.y;

        while (x >= 0 && x < _width && y >= 0 && y < _height)
        {
            if (_objectBoard[x,y].IsUsable == true)
            {
                Object neighbourObject = _objectBoard[x,y].Object.GetComponent<Object>();

                if (neighbourObject.IsMatched == false && neighbourObject.Type == objectType) 
                {
                    connectedObjects.Add(neighbourObject);

                    x += direction.x;
                    y += direction.y;
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }
    }

    #region Swapping 

    public void SelectObject(Object obj)
    {
        if (_selectedObject == null)
        {
           // Debug.Log(obj);
            _selectedObject = obj;
        }
        else if (_selectedObject == obj)
        {
            _selectedObject = null;
        }
        else if (_selectedObject != obj)
        {
            SwapObject(_selectedObject, obj);
            _selectedObject = null;
        }
    }

    private void SwapObject(Object currentObject, Object targetObject)
    {
        if (IsAdjacent(currentObject, targetObject) == false)
        {
            return;
        }

        DoSwap(currentObject, targetObject);

        _isProcessingMove = true;

        StartCoroutine(ProcessMatches(currentObject, targetObject));
    }

    private void DoSwap(Object currentObject, Object targetObject)
    {
        GameObject temp = _objectBoard[currentObject.XIndex, currentObject.YIndex].Object;

        _objectBoard[currentObject.XIndex, currentObject.YIndex].Object = _objectBoard[targetObject.XIndex, targetObject.YIndex].Object;
        _objectBoard[targetObject.XIndex, targetObject.YIndex].Object = temp;

        int tempXIndex = currentObject.XIndex;
        int tempYIndex = currentObject.YIndex;

        currentObject.SetIndex(targetObject.XIndex, targetObject.YIndex);
        targetObject.SetIndex(tempXIndex, tempYIndex);

        currentObject.MoveToTarget(_objectBoard[targetObject.XIndex, targetObject.YIndex].Object.transform.position);
        targetObject.MoveToTarget(_objectBoard[currentObject.XIndex, currentObject.YIndex].Object.transform.position);
    }

    private IEnumerator ProcessMatches(Object currentObject, Object targetObject)
    {
        yield return new WaitForSeconds(_delay); 

        bool hasMatch = CheckBoard(true); //

        if (hasMatch == false)
        {
            DoSwap(currentObject, targetObject);
        }

        _isProcessingMove = false;
    }

    private bool IsAdjacent(Object currentObject, Object targetObject)
    {
        return Mathf.Abs(currentObject.XIndex - targetObject.XIndex) + Mathf.Abs(currentObject.YIndex - targetObject.YIndex) == 1;
    }
    

    #endregion
}
