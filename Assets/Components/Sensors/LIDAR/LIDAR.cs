using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LIDAR : MonoBehaviour {
    public float MinRange = 0.02f;          // Minimum range (m)
    public float MaxRange = 5.6f;           // Max range (m)
    public float ScanRate = 100.0f;         // Time between each scan (msec)
    public float ScanAngle = 240.0f;        // The scope of the scan (degrees)
    public float ScanResolution = 0.36f;    // Angle between each scan point (degrees)
    public int edgeIterations = 5;          // The number of loops in searching for an edge
    public float edgeDistThreshold;         // HHow far do we want the edges to move before we recast

    // Masks
    public LayerMask obstacleMask;          // Material that the rays collide with

    public MeshFilter ScanMeshFilter;       // Scan mesh
    private Mesh ScanMesh;

    private float startAngle;
    private float endAngle;

    public List<Vector3> EdgePoints = new List<Vector3>();  // Points that we have identified as being the edge

    // Start is called before the first frame update
    void Start() {
        startAngle = ScanAngle / 2.0f * -1.0f;
        endAngle = startAngle * -1.0f;

        ScanMesh = new Mesh();
        ScanMesh.name = "Scan Mesh";
        ScanMeshFilter.mesh = ScanMesh;

        edgeDistThreshold = ScanResolution / 2;
    }


    // Update is called once per frame
    void Update() {

    }

    /*
    ================================
    DrawRange()
        This is the main function for the LIDAR where the LOS circle is drawn
        and it calls the other functions to ascertain the raycasts. 
    ================================
     */
    void DrawRange() {
        EdgePoints.Clear();
        int stepCount = Mathf.RoundToInt(ScanAngle * (1.0f / ScanResolution));
        float stepAngleSize = ScanAngle / stepCount;
        List<Vector3> scanPoints = new List<Vector3>();
        RayCastInfo oldRayCast = new RayCastInfo();

        for (int i = 0; i <= stepCount; i++) {
            float angle = transform.eulerAngles.y - ScanAngle / 2
                          + stepAngleSize * i;
            RayCastInfo newRayCast = RayCast(angle);
            if (i > 0) {
                bool edgeDistThresholExeeded =
                    Mathf.Abs(oldRayCast.dist - newRayCast.dist)
                    > edgeDistThreshold;
                if (oldRayCast.hit != newRayCast.hit || (oldRayCast.hit && newRayCast.hit && edgeDistThresholExeeded)) {
                    // Find the edgeb etween the two raycasts
                    EdgeInfo edge = FindEdge(oldRayCast, newRayCast);
                    if (edge.pointA != Vector3.zero) {
                        scanPoints.Add(edge.pointA);
                        if (Mathf.Round(Vector3.Distance(transform.position, edge.pointA)) < MaxRange) {
                            EdgePoints.Add(edge.pointA);
                        }
                    }
                    if (edge.pointB != Vector3.zero) {
                        scanPoints.Add(edge.pointB);
                        if (Mathf.Round(Vector3.Distance(transform.position, edge.pointB)) < MaxRange) {
                            EdgePoints.Add(edge.pointB);
                        }
                    }
                }
            }
        } // End for

        // Start setting up the mesh
        int vCount = scanPoints.Count + 1;             // How many vertices? Points + origin
        Vector3[] vertices = new Vector3[vCount];       // Create vertices array from the prev. calc
        int[] triangles = new int[(vCount - 2) * 3];    // Triangles have 3 points each

        vertices[0] = Vector3.zero;                     // Set origin as 0
        for (int i = 0; i < vCount - 1; i++) {
            vertices[i + 1] = transform.InverseTransformPoint(scanPoints[i]) + Vector3.forward;

            if (i < vCount - 2) {
                triangles[i * 3] = 0;                   // Firstr triangle point is origin
                triangles[i * 3 + 1] = i + 1;           // goes clockwise
                triangles[i * 3 + 2] = i + 2;           // ...
            }
        }

        // Draw the mesh
        ScanMesh.Clear();
        ScanMesh.vertices = vertices;
        ScanMesh.triangles = triangles;
        ScanMesh.RecalculateNormals();


    }
    /*
    ================================
    RayCast()
        This function basically does the raycast and then creates a RayCastInfo 
        object so we can use the raycast information in a meaningful way.
    ================================
     */
    private RayCastInfo RayCast(float globalAngle) {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, MaxRange, obstacleMask)) {
            return new RayCastInfo(true, hit.point, hit.distance, globalAngle);
        } else {
            return new RayCastInfo(false, transform.position + dir * MaxRange, MaxRange, globalAngle);
        }
    }

    /*
    ================================
    DirFromAngle()
        Takesin an angle and returns the direction of the angle as a vector. 
        Trigonometry usually considers 0 degrees at east going anti-clockwise 
        (i.e. 90 is north) wile Unity uses 0 degrees at north and goes
        clockwise. Unity angle can be alculated by (90 - x) where x is he angle 
        in degrees for trigonometry. 
        e.g.    If given a trig angle as x being 90, unity angle = 90 - 90, or 0
                If given a trig angle as 180, unity is 90 - 180 or -90, or 270
        Since sin(90-x) = cos(x), when using an object in unity, we can swap 
        sine and cosine
    ================================
     */
    private Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if (!angleIsGlobal) {
            angleInDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    /*
    ============================================================================
    FindEdge()
        Iteratively looks for the edge of an obstacle. THe higher the
        edgeIterations, the more refined we can make that edge. Takes two
        RayCastInfo parameters as the minimum and maximum of the ray cast. 
        The function works by egetting the two ray casts where one is a hit and 
        the other is a miss, creating a new ray cast down the middle and then 
        repeating between a higt and a missd cast until the threshold or
        iterations limit is reached. 
    ============================================================================
     */
    private EdgeInfo FindEdge(RayCastInfo minRayCast, RayCastInfo maxRayCast) {
        float minAngle = minRayCast.angle;
        float maxAngle = maxRayCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        RayCastInfo newRayCast = new RayCastInfo();

        for (int i = 0; i < edgeIterations; i++) {
            float angle = (minAngle + maxAngle) / 2;
            newRayCast = RayCast(angle);
            bool edgeDistThresholExeeded = Mathf.Abs(minRayCast.dist - newRayCast.dist) > edgeDistThreshold;
            if (newRayCast.hit == minRayCast.hit && !edgeDistThresholExeeded) {
                // Replace the minimum with the new ray cast we have created
                minAngle = angle;
                minPoint = newRayCast.point;
            } else {
                // Replace the max as the new ray ast we have created
                maxAngle = angle;
                maxPoint = newRayCast.point;
            }
        }

        Debug.DrawLine(transform.position, minPoint, Color.blue);
        Debug.DrawLine(transform.position, maxPoint, Color.blue);

        if (newRayCast.hit && Mathf.Round(newRayCast.dist) < MaxRange) {
            EdgePoints.Add(minPoint);
        } else if (!newRayCast.hit && Mathf.Round(newRayCast.dist) < MaxRange) {
            EdgePoints.Add(maxPoint);
        }
        return new EdgeInfo(minPoint, maxPoint);

    }
    /*
    ============================================================================
    RayCastInfo
        This struct stores information about a single raycast
    ============================================================================
     */
    public struct RayCastInfo {
        public bool hit;                    // Does it hit something?
        public Vector3 point;               // The edge of the raycast
        public float dist;                  // Distance from origin
        public float angle;                 // Angle from origin

        public RayCastInfo(bool _hit, Vector3 _point, float _dist, float _angle) {
            hit = _hit;
            point = _point;
            dist = _dist;
            angle = _angle;
        }
    }
    /*
    ============================================================================
    EdgeInfo
        This struct stores information about the binary points in trying to
        resolve an edge iteration.
    ============================================================================
     */
    public struct EdgeInfo {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB) {
            pointA = _pointA;
            pointB = _pointB;
        }

        void OnDrawGizmos() {
            // for (int i = 0; i < EdgePoints.Count; i++) {
            //     Gizmos.DrawWireSphere(EdgePoints[i], 0.5f);
            // }
        }
    }

}
