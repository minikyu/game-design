using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
		public Vector3 p0, p1; // 前帧/本帧的位置
		public float length;   // 和上一节点的止动长度
};

public class Strand
{
	public int nodeStart, nodeEnd; // 此发束中，节点数组的起始和结束索引
	public Vector3 rootP;             // 发根的局部坐标（相对于头的变换）
};



public class SimulateHair : MonoBehaviour
{
	private LineRenderer lr;
	[SerializeField, Range(1, 100)] public int pointcount = 20;		//节点数量
	private  int strandcount = 1;									//头发数量		
	[SerializeField, Range(0.01f, 1f)] public float pointdis = 0.3f;//长度约束

	private  Vector3 a;		//加速度
	private Vector3 g;      //重力加速度
	public float m;			//头发质量
	public Vector3 Force;	//除重量之外的外力

	[SerializeField, Range(0.9f, 1f)] public float damping;	//阻尼系数
	public Vector3 headToWorld;								//脑袋坐标

	private List<Node> nodes;			//节点集
	private List<Strand> strands;		//发束集
	// Start is called before the first frame update
	void Start()
    {
		lr = gameObject.GetComponent<LineRenderer>();
		lr.positionCount = 20;
		
		headToWorld = new Vector3(0, 0, 0);
		nodes = new List<Node>();
		strands = new List<Strand>();

		Vector3[] vs = new Vector3[pointcount];
		for (int s = 0; s < strandcount; s++)
		{
			Strand  strand = new Strand();
			strand.nodeStart = s * pointcount;
			strand.nodeEnd = strand.nodeStart + pointcount - 1;
			strand.rootP = new Vector3(0, 0, s * 0.1f);
			strands.Add(strand);
			for (int i = strand.nodeStart; i <= strand.nodeEnd; i++)
			{
				Node node = new Node();
				node.p0 = headToWorld + new Vector3(i * pointdis, 0, 0)+ strand.rootP;
				node.p1 = node.p0;
				vs[i-s*pointcount] = node.p0;
				nodes.Add(node);
			}
			lr.SetPositions(vs);
		}

		Force = new Vector3(0, 0, 0);
		g = new Vector3(0.0f, -9.81f, 0.0f);
		a = g;
		m = 1;
		damping = 0.9f;
		
        
    }

	public Vector3 Verlet(Vector3 p0,Vector3 p1,float damping,Vector3 a,float dt)
	{
		Vector3 p2 = p1 + damping * (p1 - p0) + a * dt * dt;
		return p2;
	}

	public List<Vector3> lengthConstraint(Vector3 na,Vector3 nb,float len)
	{
		float dis = (na - nb).magnitude;
		Vector3 a = na + (nb - na) * (dis - pointdis) / dis / 2.0f;
		Vector3 b = nb - (nb - na) * (dis - pointdis) / dis / 2.0f;
		List<Vector3> res = new List<Vector3>();
		res.Add(a);
		res.Add(b);
		return res;
	}
	public void simulater( float damping, float dt)
	{
		for(int n = 0; n < nodes.Count; n++)
		{
			a = Force / m + g;
			Vector3 p2 = Verlet(nodes[n].p0, nodes[n].p1, damping, a, dt);
			nodes[n].p0 = nodes[n].p1;
			nodes[n].p1 = p2;
		}

		for(int s = 0; s < strands.Count; s++)
		{
			int start = strands[s].nodeStart;
			int end = strands[s].nodeEnd;
			for (int i = start ; i < end; i++)
			{
				Node na = nodes[i];
				Node nb = nodes[i + 1];

				// 碰撞检测和决议
				//nb.p1 = collideSphere(sphere, nb.p1)
				// 长度约束
				na.p1 = lengthConstraint(na.p1, nb.p1, nb.length)[0];
				nb.p1 = lengthConstraint(na.p1, nb.p1, nb.length)[1];

				nodes[start].p1 = headToWorld + strands[s].rootP;


			}
		}

	}


// Update is called once per frame
	void Update()
    {
		simulater(damping, Time.deltaTime);
		Vector3[] vs = new Vector3[pointcount];
		for (int s = 0; s < strandcount; s++)
		{
			for (int i = strands[s].nodeStart; i <= strands[s].nodeEnd; i++)
			{
				
				vs[i - s * pointcount] = nodes[i].p0;
			}
			lr.SetPositions(vs);
		}

	}
}
