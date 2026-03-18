using Cinemachine;
using UnityEngine;

namespace br.com.bonus630.thefrog.Utils
{
    public static class CameraUtils
    {
        public static CinemachineVirtualCamera GetActiveVirtualCamera2()
        {
            return CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera as CinemachineVirtualCamera;
        }
        public static Vector2 GetTopRight(CinemachineVirtualCamera vcam, float zPlane = 0f)
        {
            if (vcam.m_Lens.Orthographic)
            {
                float halfHeight = vcam.m_Lens.OrthographicSize;
                float halfWidth = halfHeight * vcam.m_Lens.Aspect;

                float maxY = vcam.transform.position.y + halfHeight;
                float maxX = vcam.transform.position.x + halfWidth;

                return new Vector2(maxX, maxY);
            }
            else
            {
                Vector3 topRight = Camera.main.ViewportToWorldPoint(
                    new Vector3(1f, 1f, Mathf.Abs(zPlane - Camera.main.transform.position.z))
                );
                return new Vector2(topRight.x, topRight.y);
            }
        }
        public static Vector2 GetTopLeft(CinemachineVirtualCamera vcam, float zPlane = 0f)
        {
            if (vcam.m_Lens.Orthographic)
            {
                float halfHeight = vcam.m_Lens.OrthographicSize;
                float halfWidth = halfHeight * vcam.m_Lens.Aspect;

                float maxY = vcam.transform.position.y + halfHeight;
                float maxX = vcam.transform.position.x - halfWidth;

                return new Vector2(maxX, maxY);
            }
            else
            {
                Vector3 topRight = Camera.main.ViewportToWorldPoint(
                    new Vector3(0f, 1f, Mathf.Abs(zPlane - Camera.main.transform.position.z))
                );
                return new Vector2(topRight.x, topRight.y);
            }
        }
        public static Vector2 GetBottomLeft(CinemachineVirtualCamera vcam, float zPlane = 0f)
        {
            if (vcam.m_Lens.Orthographic)
            {
                float halfHeight = vcam.m_Lens.OrthographicSize;
                float halfWidth = halfHeight * vcam.m_Lens.Aspect;

                float minY = vcam.transform.position.y - halfHeight;
                float minX = vcam.transform.position.x - halfWidth;

                return new Vector2(minX, minY);
            }
            else
            {
                Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(
                    new Vector3(0f, 0f, Mathf.Abs(zPlane - Camera.main.transform.position.z))
                );
                return new Vector2(bottomLeft.x, bottomLeft.y);
            }
        }
        public static CameraBounds2D GetCameraBounds(CinemachineVirtualCamera vcam, float zPlane = 0f)
        {
            CameraBounds2D bounds = new();
            Camera cam = Camera.main;

            if (vcam.m_Lens.Orthographic)
            {
                float halfHeight = vcam.m_Lens.OrthographicSize;
                float halfWidth = halfHeight * vcam.m_Lens.Aspect;
                Vector2 center = vcam.transform.position;

                bounds.topRight = center + new Vector2(halfWidth, halfHeight);
                bounds.topLeft = center + new Vector2(-halfWidth, halfHeight);
                bounds.bottomRight = center + new Vector2(halfWidth, -halfHeight);
                bounds.bottomLeft = center + new Vector2(-halfWidth, -halfHeight);

                bounds.center = center;
                bounds.size = new Vector2(halfWidth * 2f, halfHeight * 2f);
            }
            else
            {
                // Distância do plano Z da câmera
                float distance = zPlane - cam.transform.position.z;

                Vector3 bottomLeft3D = cam.ViewportToWorldPoint(new Vector3(0f, 0f, distance));
                Vector3 topRight3D = cam.ViewportToWorldPoint(new Vector3(1f, 1f, distance));

                bounds.bottomLeft = new Vector2(bottomLeft3D.x, bottomLeft3D.y);
                bounds.topRight = new Vector2(topRight3D.x, topRight3D.y);
                bounds.topLeft = new Vector2(bounds.bottomLeft.x, bounds.topRight.y);
                bounds.bottomRight = new Vector2(bounds.topRight.x, bounds.bottomLeft.y);

                bounds.center = (bounds.topRight + bounds.bottomLeft) * 0.5f;
                bounds.size = new Vector2(bounds.topRight.x - bounds.bottomLeft.x,
                                          bounds.topRight.y - bounds.bottomLeft.y);
            }

            return bounds;
        }

        public static Vector2 GetTopRight()
        {
            return GetTopRight(GetActiveVirtualCamera2(), 0f);
        }
        public static Vector2 GetBottomLeft()
        {
            return GetBottomLeft(GetActiveVirtualCamera2(), 0f);
        }

        public static Vector2 GetTopLeft()
        {
            return GetTopLeft(GetActiveVirtualCamera2(), 0f);
        }
        public static CameraBounds2D GetBounds2D()
        {
            return GetCameraBounds(GetActiveVirtualCamera2(), 0f);
        }
    }
    public struct CameraBounds2D
    {
        public Vector2 topLeft { get;  set; }
        public Vector2 topRight { get;  set; }
        public Vector2 bottomLeft { get;  set; }
        public Vector2 bottomRight { get;  set; }

        public Vector2 center { get;  set; }
        public Vector2 size { get;  set; }

        // Construtor: com quatro cantos
        public CameraBounds2D(Vector2 topLeft, Vector2 topRight, Vector2 bottomLeft, Vector2 bottomRight)
        {
            this.topLeft = topLeft;
            this.topRight = topRight;
            this.bottomLeft = bottomLeft;
            this.bottomRight = bottomRight;

            this.center = (topRight + bottomLeft) * 0.5f;
            this.size = new Vector2(topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
        }

        // Construtor: com center e size
        public CameraBounds2D(Vector2 center, Vector2 size)
        {
            this.center = center;
            this.size = size;

            Vector2 halfSize = size * 0.5f;
            this.topLeft = center + new Vector2(-halfSize.x, halfSize.y);
            this.topRight = center + new Vector2(halfSize.x, halfSize.y);
            this.bottomLeft = center + new Vector2(-halfSize.x, -halfSize.y);
            this.bottomRight = center + new Vector2(halfSize.x, -halfSize.y);
        }
        public bool Contains(Vector2 point)=>
             point.x >= topLeft.x && point.x <= topRight.x && point.y <= topLeft.y && point.y >= bottomLeft.y;
        
        // Construtor: só tamanho, assume centro na origem
        public CameraBounds2D(Vector2 size)
            : this(Vector2.zero, size)
        { }

        public override string ToString()
        {
            return $"Bounds - Left:{topLeft.x} Right:{topRight.x} Top:{topRight.y} Bottom:{bottomLeft.y}";
        }
        
    }
}
