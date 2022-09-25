using PathCreation;
using PathCreation.Examples;
using UnityEngine;

[ExecuteInEditMode]
public class ObjectPlacer : PathSceneTool
{

    public GameObject prefab;
    public GameObject holder;
    public float spacing = 3;
    public LayerMask layerMask;
    public float heightOffset;

    public Vector3 normalOffset;

    const float minSpacing = .1f;

    void Generate()
    {
        if (pathCreator != null && prefab != null && holder != null)
        {
            DestroyObjects();

            VertexPath path = pathCreator.path;

            spacing = Mathf.Max(minSpacing, spacing);
            float dst = 0;

            while (dst < path.length)
            {
                Vector3 point = path.GetPointAtDistance(dst);

                RaycastHit hit;
                if(Physics.Raycast(point,transform.TransformDirection(-Vector3.up), out hit, 500, layerMask))
                {
                    point.y = hit.point.y + heightOffset;
                }

                //Quaternion rot = path.GetRotationAtDistance(dst);

                var hitNormal = hit.normal;

                Quaternion rot = Quaternion.LookRotation(new Vector3(hitNormal.x + normalOffset.x, hitNormal.y + normalOffset.y, hitNormal.z + normalOffset.z));
                Instantiate(prefab, point, rot, holder.transform);
                dst += spacing;
            }
        }
    }

    void DestroyObjects()
    {
        int numChildren = holder.transform.childCount;
        for (int i = numChildren - 1; i >= 0; i--)
        {
            DestroyImmediate(holder.transform.GetChild(i).gameObject, false);
        }
    }

    protected override void PathUpdated()
    {
        if (pathCreator != null)
        {
            Generate();
        }
    }
}
