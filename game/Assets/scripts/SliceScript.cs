using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Collections.Generic;

public class TriSlicer
{
	public class Tri
	{
        public float z;
		public Vector2 v1;// = new Vector3[3]();
		public Vector2 v2;
		public Vector2 v3;
		public Vector2 uv1;
		public Vector2 uv2;
		public Vector2 uv3;
		public Vector2 vel;
	}

	Mesh mesh;
	float driftFactor = 0.1f;
	int maxPieces = 9001;

	//Vector2[] verts;
	//List<int[]> tris = new List<int[]>();
	public List<Tri> tris = null;

	//List<Vector2> trivels = new List<Vector2>();

	//Constructor, must initialize with a sacrificial mesh
	public TriSlicer(Mesh _mesh, float _driftFactor, int _maxPieces)
	{
		mesh = _mesh;
		this.driftFactor = _driftFactor;
		this.maxPieces = _maxPieces;
		//For now, this doesn't load the mesh into the 2d structure (could project or something later)
	}

	public override string ToString()
	{
		string ret = "TriSlicer! Tri count: " + this.tris.Count + " \n";
		foreach (Tri tri in this.tris)
		{
			ret += "Tri: " + tri.v1 + " " + tri.v2 + " " + tri.v3 + " vel: " + tri.vel + "\n";
		}
		return ret;
	}

	//Rebuilds actual mesh from the 2d structure within TriSlicer
	public void RefreshMesh()
	{
		if (this.mesh == null || this.tris == null || this.tris.Count == 0) return;

//		Vector3[] verts3 = new Vector3[verts.Length];
//		int i = 0;
//		foreach (Vector2 vert in verts)
//		{
//			verts3[i++] = new Vector3(vert[0],0,vert[1]);
//			//Debug.Log(verts3[i-1].ToString());
//		}
//		int[] tris3 = new int[tris.Count * 3];
//		i = 0;
//		foreach (int[] tri in tris)
//		{
//			tris3[i++] = tri[0];
//			tris3[i++] = tri[1];
//			tris3[i++] = tri[2];
//		}

		Vector3[] verts3 = new Vector3[tris.Count * 3];
		int[] tris3 = new int[tris.Count * 3];
		Vector2[] uvs3 = new Vector2[tris.Count * 3];
		int i = 0;
		foreach (Tri tri in tris)
		{
			verts3[i*3+0] = new Vector3(tri.v1[0], tri.z, tri.v1[1]);
			verts3[i*3+1] = new Vector3(tri.v2[0], tri.z, tri.v2[1]);
			verts3[i*3+2] = new Vector3(tri.v3[0], tri.z, tri.v3[1]);
			
			tris3[i*3+0] = i*3+0;
			tris3[i*3+1] = i*3+1;
			tris3[i*3+2] = i*3+2;

			uvs3[i*3+0] = tri.uv1;
			uvs3[i*3+1] = tri.uv2;
			uvs3[i*3+2] = tri.uv3;

			i++;
		}

		mesh.Clear();
		mesh.vertices = verts3;
		mesh.triangles = tris3;
		mesh.uv = uvs3;
		mesh.RecalculateBounds();
		//mesh.RecalculateNormals();

		//Debug.Log("verts len: " + mesh.vertices.Length);
		//Debug.Log("tris len: " + mesh.triangles.Length);
		//string aoeu = "";
		//foreach (Vector3 vec in mesh.vertices)
		//{
		//	aoeu += " - " + vec.ToString() + " - ";
		//}
		//Debug.Log(aoeu);
	}

	//Initialize the mesh with a plane
	public void InitPlane(float scale)
	{
		Tri t1 = new Tri();// = Tri;
		Tri t2 = new Tri();// = Tri;

		t1.v1 = new Vector3(-scale,-scale);
		t1.v2 = new Vector3(-scale,scale);
		t1.v3 = new Vector3(scale,scale);
		t1.uv1 = new Vector2(0,0);
		t1.uv2 = new Vector2(0,1);
		t1.uv3 = new Vector2(1,1);

		t2.v1 = new Vector3(scale,scale);
		t2.v2 = new Vector3(scale,-scale);
		t2.v3 = new Vector3(-scale,-scale);
		t2.uv1 = new Vector2(1,1);
		t2.uv2 = new Vector2(1,0);
		t2.uv3 = new Vector2(0,0);

		//t1.vel = new Vector2(-0.1f,0.1f);
		//t2.vel = new Vector2(0.1f,-0.1f);
		t1.vel = new Vector2(0,0);
		t2.vel = new Vector2(0,0);

		this.tris = new List<Tri>();
		tris.Add(t1);
		tris.Add(t2);


		//Vector2[] verts2 = {new Vector3(-1,-1), new Vector3(-1,1), new Vector3(1,1), new Vector3(1,-1)};
		//int[] tris2 = {0,1,2,2,3,0};

//		tris.Clear();
//		tris.Add(new int[]{0,1,2});
//		tris.Add(new int[]{2,3,0});
//
//		verts = verts2;
//
//		trivels = new List<Vector2>();
//		for (int i = 0; i < tris.Count; i++)
//		{
//			trivels.Add(new Vector2(0,0));
//		}

		//Debug.Log("verts len: " + verts.Length);
		//Debug.Log("tris len: " + tris.Count);

		RefreshMesh();
	}

	public void UpdateMovements(float deltaTime)
	{
		if (this.mesh == null || this.tris == null || this.tris.Count == 0) return;
		int i = 0;
		for (i = 0; i < tris.Count; i++)
		{
			Tri tri = tris[i];
			tri.v1 += deltaTime * tri.vel;
			tri.v2 += deltaTime * tri.vel;
			tri.v3 += deltaTime * tri.vel;
			tris[i] = tri;
		}
	}

	//returns t from A1, scaled to S1
	//A1 and A2 are starting points for a ray, S1 and S2 are the slopes
	public static float LineLineIntersectReturnT(Vector2 A1, Vector2 S1, Vector2 A2, Vector2 S2)
	{
		return (S2.y * (A2.x - A1.x) + S2.x * (A1.y - A2.y)) / ((S1.x * S2.y) - (S2.x * S1.y));
	}

	public static Vector2 PointNearestPointOnLine(Vector2 point, Vector2 linepoint, Vector2 linedir)
	{
		return linepoint + Vector2.Dot((point - linepoint), linedir) * linedir;
	}

	public void Slice(Vector2 a, Vector2 b)
	{
		if (this.tris.Count > maxPieces) return;
		//Debug.Log(this.tris.Count + " " + maxPieces);

		//Debug.Log("Slice begin. Tris len: " + tris.Count);

		Vector2 s = b-a;
		List<Tri> removethese = new List<Tri>();
		List<Tri> addthese = new List<Tri>();
		foreach (Tri tri in tris)
		{
			//slice this triangle

			//intersect edge 1
			Vector2 t1a = tri.v1;
			Vector2 t1s = tri.v2-tri.v1;
			float t1 = LineLineIntersectReturnT(t1a, t1s, a, s);
			bool isect1valid = t1 > 0 && t1 < 1;
			//intersect edge 2
			Vector2 t2a = tri.v2;
			Vector2 t2s = tri.v3-tri.v2;
			float t2 = LineLineIntersectReturnT(t2a, t2s, a, s);
			bool isect2valid = t2 > 0 && t2 < 1;
			//intersect edge 3
			Vector2 t3a = tri.v3;
			Vector2 t3s = tri.v1-tri.v3;
			float t3 = LineLineIntersectReturnT(t3a, t3s, a, s);
			bool isect3valid = t3 > 0 && t3 < 1;

			int numvalid = (isect1valid?1:0) + (isect3valid?1:0) + (isect2valid?1:0);
			if (numvalid == 2)
			{
				Vector2 isect1 = t1a + t1 * t1s;
				Vector2 isect2 = t2a + t2 * t2s;
				Vector2 isect3 = t3a + t3 * t3s;

				//figure out which vertex of the triangle is shared by the two intersected edges
				Vector2 common;
				Vector2 p1, p2, o1, o2;
				Vector2 commonuv, o1uv, o2uv;
				if (isect1valid && isect2valid)
				{
					common = tri.v2;
					p1 = isect1;
					p2 = isect2;
					o1 = tri.v1;
					o2 = tri.v3;
					commonuv = tri.uv2;
					o1uv = tri.uv1;
					o2uv = tri.uv3;
				}
				else if (isect2valid && isect3valid)
				{
					common = tri.v3;
					p1 = isect2;
					p2 = isect3;
					o1 = tri.v2;
					o2 = tri.v1;
					commonuv = tri.uv3;
					o1uv = tri.uv2;
					o2uv = tri.uv1;
				}
				else //if (isect3valid && isect1valid)
				{
					common = tri.v1;
					p1 = isect3;
					p2 = isect1;
					o1 = tri.v3;
					o2 = tri.v2;
					commonuv = tri.uv1;
					o1uv = tri.uv3;
					o2uv = tri.uv2;
				}

				//create new triangles
				Tri nt1 = new Tri();
				Tri nt2 = new Tri();
				Tri nt3 = new Tri();

				nt1.v1 = p1;
				nt1.v2 = common;
				nt1.v3 = p2;

				nt2.v1 = o1;
				nt2.v2 = p1;
				nt2.v3 = p2;

				nt3.v1 = p2;
				nt3.v2 = o2;
				nt3.v3 = o1;

				//UVs
				Vector2 o1_common = common - o1;
				Vector2 o2_common = common - o2;
				float p1_onto_o1_common = Vector2.Dot(p1-o1, o1_common.normalized) / o1_common.magnitude;
				Vector2 p1uv = o1uv + p1_onto_o1_common * (commonuv - o1uv);
				float p2_onto_o2_common = Vector2.Dot(p2-o2, o2_common.normalized) / o2_common.magnitude;
				Vector2 p2uv = o2uv + p2_onto_o2_common * (commonuv - o2uv);

				nt1.uv1 = p1uv;
				nt1.uv2 = commonuv;
				nt1.uv3 = p2uv;
				//Debug.Log("p1_onto_o1_common: " + p1_onto_o1_common);
				//Debug.Log("p2_onto_o2_common: " + p2_onto_o2_common);
				//Debug.Log("nt1 uvs: " + p1uv + " " + commonuv + " " + p2uv + " " + o1uv + " " + o2uv);
				//Debug.Log("nt1 pos: " + p1 + " " + common + " " + p2 + " " + o1 + " " + o2);

				nt2.uv1 = o1uv;
				nt2.uv2 = p1uv;
				nt2.uv3 = p2uv;
				
				nt3.uv1 = p2uv;
				nt3.uv2 = o2uv;
				nt3.uv3 = o1uv;

				//Give them velocities
				//Vector2 velcommon = (common - (p1 + (p2-p1) * 0.5f));
				//Vector2 nearest = PointNearestPointOnLine(common, a, s);
				//Vector2 velcommon = (common - nearest).normalized;
				Vector2 sPerp = new Vector2(-s.y, s.x);
				//Debug.Log(Vector2.Dot(s, sperp));
				Vector2 velcommon = sPerp.normalized;
				float veladjust = 1.0f;
				if (Vector2.Dot((common - a), sPerp) > 0)
				{
					veladjust = 1.0f;
				}
				else
				{
					veladjust = -1.0f;
				}
				nt1.vel = this.driftFactor * velcommon * veladjust + tri.vel;
				nt2.vel = this.driftFactor * -velcommon * veladjust + tri.vel;
				nt3.vel = nt2.vel;

                float z1 = Random.value * 0.1f;
                float z2 = Random.value * 0.1f;
                nt1.z = z1;
                nt2.z = z2;
                nt3.z = z2;

                addthese.Add(nt1);
				addthese.Add(nt2);
				addthese.Add(nt3);

				//tag old ones for removal
				removethese.Add(tri);

			}//if there was a valid slice made
		}//loop over all triangles to check slices

		//Remove tris tagged for removal
		foreach (Tri tri in removethese)
		{
			this.tris.Remove(tri);
		}

		//Add tris created by slice
		foreach (Tri tri in addthese)
		{
			this.tris.Add(tri);
		}

		//Debug.Log("Slice end. Tris len: " + tris.Count + " Removed: " + removethese.Count + " Added: " + addthese.Count);
		//Debug.Log(this.ToString());
	}//Slice function
}

public class SliceScript : MonoBehaviour
{
	MeshFilter meshfilter;
	TriSlicer trislicer;

	public GameObject sliceCountText;
	Text sliceCountTextComponent;

	public GameObject bladeModeElement;

    public GameObject clickZone;

    public GameObject returnButton;

	//If this is primed, it detects slice mode activation
	public bool sliceModePrimed = false;
	//The next object to prime (if any) when slice mode is completed here
	public GameObject nextSliceObjectToPrime;

	//Slice mode settings
	public float sliceModeMaxIdleTime = 5.0f;
	public float sliceModeActivateTime = 3.0f;
	public AudioSource audioSourceSliceMode;
	public AudioSource audioSourceBackground;
	public float driftFactor = 0.1f;
	public int maxPieces = 9001;

	//Currently in slice mode?
	private bool sliceMode = false;
	//float sliceModeStartTime = 0.0f;
	float sliceModeActivityTime = 0.0f;
	float timeMouseDown = 0.0f;

	// Use this for initialization
	void Start ()
	{
		meshfilter = this.GetComponent<MeshFilter>();
		trislicer = new TriSlicer(meshfilter.mesh, this.driftFactor, maxPieces);

		trislicer.InitPlane(Mathf.Abs(this.transform.lossyScale.x)*5f);
		this.transform.localScale = new Vector3(1,1,1);

		if (this.sliceCountText)
		{
			this.sliceCountTextComponent = this.sliceCountText.GetComponent<Text>();
		}
	}

	private bool dragging = false;
	Vector2 dragstart;
	Vector2 dragend;
	private bool letUpMouseAtLeastOnce = false;
	// Update is called once per frame
	void Update()
	{
		//If slice mode is not primed, there is nothing to do from here on!
		if (!this.sliceModePrimed) return;

		if (!letUpMouseAtLeastOnce)
		{
			if (!Input.GetMouseButton(0))
			{
				letUpMouseAtLeastOnce = true;
				return;
			}
			return;
		}

		// If the mouse button is not down, reset the time to activate slice mode
		if (!Input.GetMouseButton(0))
		{
			timeMouseDown = Time.time;
		}

		// Check if the mouse has been held down long enough to activate slice mode
		if (Input.GetMouseButton(0) && !sliceMode)
		{
			if ((Time.time - timeMouseDown ) > sliceModeActivateTime)
			{
				ActivateSliceMode();

                //Disable cookie script so the click count stops going up
                //this.gameObject.GetComponent<CookieScript>().enabled = false;

                //Disable click zone so clicking doesn't work while in blade mode
                this.clickZone.SetActive(false);
            }
		}

		//If slice mode is not activated, there is nothing to do from here on!
		if (!sliceMode) return;

		//hack to avoid zbuffer issues
		if (this.transform.position.z > -0.5)
		{
			this.transform.position += new Vector3(0,0,-0.001f);
		}

		if (!dragging && Input.GetMouseButton(0))
		{
			//Debug.Log("drag");
			//Vector3 clickpoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//dragstart = clickpoint;

			Ray clickray = Camera.main.ScreenPointToRay(Input.mousePosition);
			dragstart = clickray.GetPoint((Camera.main.transform.position - this.transform.position).magnitude);

			dragging = true;
		}

		if (dragging && !Input.GetMouseButton(0))
		{
			//Debug.Log("Done drag");
			//Vector3 clickpoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//dragend = clickpoint;

			Ray clickray = Camera.main.ScreenPointToRay(Input.mousePosition);
			dragend = clickray.GetPoint((Camera.main.transform.position - this.transform.position).magnitude);

			dragging = false;
			Vector2 offset = this.transform.position;
			trislicer.Slice(dragstart-offset,dragend-offset);

			this.sliceModeActivityTime = Time.time;
		}

		//Update moving slices
		trislicer.UpdateMovements(Time.deltaTime);
		//Slice?

		//Rebuild actual mesh from 2d slice structure 
		trislicer.RefreshMesh();

		//If there is an object to display the number of slices, then display it!
		if (this.sliceCountText)
		{
			if (trislicer.tris.Count > 0)
			{
				if (trislicer.tris.Count < 9000)
				{
					sliceCountTextComponent.fontSize = 80;
					sliceCountTextComponent.text = "" + trislicer.tris.Count;
				}
				else
				{
					sliceCountTextComponent.fontSize = 27;
					sliceCountTextComponent.text = "OVER NINE\nTHOUSAND?!\nTHERE'S NO WAY\nTHAT CAN BE\nRIGHT!!!!";
				}
			}
			else
			{
				sliceCountTextComponent.text = "";
			}
		}

		//If slice mode has timed out, end it!
		if (this.sliceModeActivityTime != 0.0f && (Time.time - this.sliceModeActivityTime > this.sliceModeMaxIdleTime))
		{
			DeactivateSliceMode();
		}
	}

	public void ActivateSliceMode()
	{
		this.sliceMode = true;
		//this.slicemodestarttime = Time.time;
		this.sliceModeActivityTime = Time.time;
		audioSourceSliceMode.Play();
		audioSourceBackground.Stop();
		this.bladeModeElement.SetActive(true);
		for (int i = 0; i < this.bladeModeElement.transform.childCount; i++)
		{
			this.bladeModeElement.transform.GetChild(i).gameObject.SetActive(true);
		}
	}

	public void DeactivateSliceMode()
	{
		//deactivate blademode element
		this.bladeModeElement.SetActive(false);
		for (int i = 0; i < this.bladeModeElement.transform.childCount; i++)
		{
			this.bladeModeElement.transform.GetChild(i).gameObject.SetActive(false);
		}
		
		//Reset the music to what it was before
		audioSourceSliceMode.Stop();
		audioSourceBackground.Play();
		
		//Disable slice mode priming and slice mode and clean up.
		this.sliceModePrimed = false;
		this.sliceMode = false;
		this.trislicer.tris.Clear();
		if (this.sliceCountTextComponent)
		{
			this.sliceCountTextComponent.text = "";
		}

        //Prime the next slice object
        if (this.nextSliceObjectToPrime)
        {
            this.nextSliceObjectToPrime.GetComponent<SliceScript>().sliceModePrimed = true;
        }
        else
        {
            this.returnButton.SetActive(true);
        }
		
		//This may not be necessary, but I guess it probably stops scripts from running, which may save resources
		this.gameObject.SetActive(false);
		//Debug.Log("Slice object removed!");
	}

    public void returnToMainMenu()
    {
        SceneManager.LoadScene("mainmenu_mgr");
    }
}
