using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderDraw : MonoBehaviour
{

    private RenderTexture renderTexture;
    public Material prueColormaterial;
    public Material skyboxmaterial;
    public Mesh CubeMesh;
    public Transform[] Cube;
    private Mesh m_mesh;
    private Vector4[] corners = new Vector4[4];
    private Mesh fullScreenMesh 
    {
        get
        {
            if (m_mesh != null)
            {
                return m_mesh;
            }
            m_mesh = new Mesh();
            m_mesh.vertices = new Vector3[]
            {
                new Vector3(-1,-1,0),
                new Vector3(-1,1,0),
                new Vector3(1,1,0),
                new Vector3(1,-1,0)
            };
            m_mesh.uv = new Vector2[]
            {
                new Vector2(0,1),
                new Vector2(0,0),
                new Vector2(1,0),
                new Vector2(1,1)

            };
            m_mesh.SetIndices(new int[] { 0, 1, 2, 3 }, MeshTopology.Quads,0);
            return m_mesh;
        }
    }
    void Start()
    {
        renderTexture = new RenderTexture(Screen.width, Screen.height,24);
    }

    // Update is called once per frame

    private void OnPostRender()
    {
        Camera camera = Camera.current;
        Graphics.SetRenderTarget(renderTexture);
        GL.Clear(true, true, Color.gray);
        prueColormaterial.color = new Color(0, 0.5f, 1);
        prueColormaterial.SetPass(0);
        foreach (var i in Cube) 
        { 
        Graphics.DrawMeshNow(CubeMesh, i.localToWorldMatrix);
        }
        DrawSkyBox(camera);

        Graphics.Blit(renderTexture, camera.targetTexture);
    }
    /// <summary>
    /// 绘制天空盒
    /// </summary>
    /// <param name="camera"></param>
    private void DrawSkyBox(Camera camera) 
    {
        corners[0] = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.farClipPlane));
        corners[1] = camera.ViewportToWorldPoint(new Vector3(1, 0, camera.farClipPlane));
        corners[2] = camera.ViewportToWorldPoint(new Vector3(0, 1, camera.farClipPlane));
        corners[3] = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.farClipPlane));
        skyboxmaterial.SetVectorArray("_corners", corners);
        skyboxmaterial.SetPass(0);
        Graphics.DrawMeshNow(fullScreenMesh, Matrix4x4.identity);
    }
}
