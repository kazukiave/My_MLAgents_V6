using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoundingBox : MonoBehaviour
{
    private Vector3 max;
    private Vector3 min;

    /*
    private readonly float minSizeX = 3f;
    private readonly float minSizeY = 2.7f;
    private readonly float minSizeZ = 3f;
    */

    public Vector3 Max => max;

    public Vector3 Min => min;

    public Vector3 Center => (max + min) * 0.5f;

    public Vector3 Size => GetSize();


    //Referennce 式形式のメンバー
    //https://docs.microsoft.com/ja-jp/dotnet/csharp/programming-guide/statements-expressions-operators/expression-bodied-members
    //読みやすい
    public BoundingBox()
    {
        max = Vector3.zero;
        min = Vector3.zero;
    }
    public BoundingBox(Vector3 bbMax, Vector3 bbMin)
    {
        max = bbMax;
        min = bbMin;
    }



    private Vector3 GetSize()
    {
        return new Vector3(max.x - min.x, max.y - min.y, max.z - min.z);
    }

    /// <summary>
    /// AR Plan　のリストから部屋のBoundigBoxを作る時に使う
    /// </summary>
    /// <param name="transforms"></param>
    public void MakeBBox(IEnumerable<Transform> transforms)
    {
        float minX = 0;
        float maxX = 0;
        float minY = 0;
        float maxY = 0;
        float minZ = 0;
        float maxZ = 0;
    
        foreach (var trans in transforms)
        {
            var position = trans.transform.position;
            Vector3 pos = position;
            minX = pos.x < minX ? position.x : minX;
            maxX = pos.x > maxX ? position.x : maxX;
            minY = pos.y < minY ? position.y : minY;
            maxY = pos.y > maxY ? position.y : maxY;
            minZ = pos.z < minZ ? position.z : minZ;
            maxZ = pos.z > maxZ ? position.z : maxZ;
        }
        
        max = new Vector3(maxX, maxY, maxZ);
        min = new Vector3(minX, minY, minZ);
        
        //check in which direction the wall exists
        
        /*
        //check min size
        //壁が検出できてない方向に伸ばしたい(宿題）
        if (max.x - min.x < minSizeX)
        {
            max.x += (minSizeX * 0.5f);
            min.x -= (minSizeX * 0.5f);
        }

        if (max.y - min.y < minSizeY)
        {
            max.y += minSizeY;
        }

        if (max.z - min.z < minSizeZ)
        {
            max.z += (minSizeZ * 0.5f);
            min.z -= (minSizeZ * 0.5f);
        }
        */
    }

    public void MakeBBox(IEnumerable<Vector3> pts)
    {
        float minX = 0;
        float maxX = 0;
        float minY = 0;
        float maxY = 0;
        float minZ = 0;
        float maxZ = 0;

        foreach (var pt in pts)
        {
            var position = pt;
            Vector3 pos = position;
            minX = pos.x < minX ? position.x : minX;
            maxX = pos.x > maxX ? position.x : maxX;
            minY = pos.y < minY ? position.y : minY;
            maxY = pos.y > maxY ? position.y : maxY;
            minZ = pos.z < minZ ? position.z : minZ;
            maxZ = pos.z > maxZ ? position.z : maxZ;
        }

        max = new Vector3(maxX, maxY, maxZ);
        min = new Vector3(minX, minY, minZ);
    }

    /// <summary>
    /// Particle のレンダーからBBoxつくるとき
    /// </summary>
    /// <param name="renderer"></param>
    public void MakeBBox(Renderer renderer)
    {
        max = renderer.bounds.max;
        min = renderer.bounds.min;
    }

    public bool ToPlane(Vector3 pos)
    {
        var valArr = new float[3]{Size.x, Size.y, Size.z};
        var idxArr = new int[3]{0, 1, 2};
        Array.Sort(valArr,idxArr);//大きい順
    
        Debug.Log(valArr[0] + " " + valArr[1] + " " + valArr[2]);
        Debug.Log(idxArr[0] + " " + idxArr[1] + " " + idxArr[2]);
        Debug.Log(valArr.Min());
        
        if (valArr.Min()> 0.2f)
        {
            return false;
        }
        
        switch (idxArr[2])
        {
            case 0:
                max = new Vector3(pos.x, max.y, max.z);
                min = new Vector3(pos.x, min.y, min.z);
                break;
            
            case 1:
                max = new Vector3(max.x, pos.y, max.z);
                min = new Vector3(min.x, pos.y, min.z);
                break;
            
            case 2:
                max = new Vector3(max.x, max.y, pos.z);
                min = new Vector3(min.x, min.y, pos.z);
                break;
        }

        return true;
    }
}
