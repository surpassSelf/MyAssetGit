using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


public class UIGridList : MonoBehaviour {

    public delegate void OnReposition();
    public OnReposition onReposition;

    public enum ArrangeMentType
    {
        Horizontal,
        Vertical,
    }

    public GameObject mControlTemp;

    [HideInInspector]
    [SerializeField]
    private int nMaxCount;
    public int mnMaxCount
    {
        get { return nMaxCount; }
        set {
            if (nMaxCount == value) return;
            nMaxCount = value;
            RebuildCells();
        }
    }

    [HideInInspector]
    [SerializeField]
    private int nMaxLine;
    public int mnMaxLine
    {
        get { return nMaxLine; }
        set {
            if (nMaxLine == value) return;
            nMaxLine = value;
            RebuildCells();
        }
    }
    /// <summary>
    /// 子物体
    /// </summary>
    [HideInInspector]
    [SerializeField]
    private int nCellWidth;
    public int mnCellWidth
    {
        set {
            if (nCellWidth == value) return;
            nCellWidth = value;
            RebuildCells();
        }
        get { return nCellWidth; }
    }

    [HideInInspector]
    [SerializeField]
    private int nCellHeight;
    public int mnCellHeight
    {
        set {
            if (nCellHeight == value) return;
            nCellHeight = value;
            RebuildCells();
        }
        get { return nCellHeight; }
    }

    [HideInInspector]
    [SerializeField]
    private UIWidget.Pivot pivot;
    public UIWidget.Pivot mPivot
    {
        set {
            if (pivot == value) return;
            pivot = value;
            RebuildCells();
        }
        get { return pivot; }
    }

    public bool IsSmmothTween = false;

    public List<GameObject> mLisControlList = new List<GameObject>();
    private List<GameObject> mLisRemoveGo = new List<GameObject>();

    [HideInInspector]
    [SerializeField]
    private ArrangeMentType arrangeType = ArrangeMentType.Horizontal;
    public ArrangeMentType mArrangeType
    {
        set {
            if (arrangeType == value) return;
            arrangeType = value;
            RebuildCells();
        }
        get { return arrangeType; }
    }

    private void Start()
    {
        RebuildCells();
    }

    private void RebuildCells()
    {
        if (mControlTemp == null) return;

        int oldMaxCount = mLisControlList.Count;
        if (mnMaxCount < oldMaxCount)
        {
            for (int i = oldMaxCount - 1; i >= mnMaxCount; i--)
            {
                GameObject go = mLisControlList[i];
                mLisControlList.RemoveAt(i);

                if (Application.isPlaying)
                {
                    mLisRemoveGo.Add(go);
                    go.SetActive(false);
                }
                else
                {
                    DestroyImmediate(go);
                }
            }
        }
        else
        {
            for (int i = oldMaxCount; i < mnMaxCount; i++)
            {
                GameObject go;
                if (mLisRemoveGo.Count > 0)
                {
                    go = mLisRemoveGo[0];
                    mLisRemoveGo.RemoveAt(0);
                }
                else
                {
                    go = GameObject.Instantiate(mControlTemp);
                }

                go.name = mControlTemp.name + i;
                go.transform.SetParent(this.transform);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = Vector3.zero;
                mLisControlList.Add(go);
                go.SetActive(true);
            }
        }

        if (mControlTemp != null) mControlTemp.SetActive(!Application.isPlaying);
        ResetPosition(mLisControlList);
    }

    private void ResetPosition(List<GameObject> listGo)
    {
        int nX = 0;
        int nY = 0;
        int maxX = 0;
        int maxY = 0;

        for (int i = 0; i < listGo.Count; i++)
        {
            Transform tran = listGo[i].transform;

            float z = tran.localPosition.z;
            Vector3 vec3 = mArrangeType == ArrangeMentType.Horizontal ?
                new Vector3(nCellWidth * nX, -nCellHeight * nY, z) :
                new Vector3(nCellWidth * nY, -nCellHeight * nX, z);

            tran.localPosition = vec3;

            maxX = Mathf.Max(maxX, nX);
            maxY = Mathf.Max(maxY, nY);

            if (++nX >= mnMaxLine && mnMaxLine > 0)
            {
                nX = 0;
                ++nY;
            }
        }

        if (mPivot != UIWidget.Pivot.TopLeft)
        {
            Vector2 off = NGUIMath.GetPivotOffset(mPivot);

            float x, y;
            if (mArrangeType == ArrangeMentType.Horizontal)
            {
                x = Mathf.Lerp(0f, maxX * mnCellWidth, off.x);
                y = Mathf.Lerp(-maxY * mnCellHeight, 0f, off.y);
            }
            else
            {
                x = Mathf.Lerp(0f, maxY * mnCellWidth, off.x);
                y = Mathf.Lerp(-maxX * mnCellHeight, 0f, off.y);
            }

            for (int i = 0; i < listGo.Count; i++)
            {
                Transform tran = listGo[i].transform;
                Vector3 vec3 = new Vector3(tran.localPosition.x - x, tran.localPosition.y - y, tran.localPosition.z);

                if (IsSmmothTween)
                {
                    SpringPosition.Begin(tran.gameObject, vec3, 15f);
                }
                else
                {
                    SpringPosition spring = tran.GetComponent<SpringPosition>();
                    if (spring != null) Destroy(spring);
                        
                    tran.localPosition = vec3;
                }
            }
        }

        if (onReposition != null) onReposition();
    }
}
