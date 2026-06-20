using UnityEngine;
using UnityEngine.SceneManagement;

namespace Maxy.GameFramework.Common.Tool
{
    public static class MTool
    {
        /// <summary>
        /// float百分比音量转到分贝
        /// </summary>
        /// <returns></returns>
        public static float ToDB(this float target) => Mathf.Log10(Mathf.Clamp(target, 0.0001f, 10f)) * 20;

        /// <summary>
        /// -180~180转到0~360
        /// </summary>
        public static float TranslateAngle(float x)
        {
            x -= 180;
            if (x < 0)
                return x + 180;
            else return x - 180;
        }

        /// <summary>
        /// 将旋转值规范到0~360
        /// </summary>
        public static float NormalizeAngle(float angle)
        {
            angle %= 360;

            while (angle < 0)
                angle += 360;

            return angle;
        }

        public static Vector2 GetMoveInput(bool onlyWASD = true)
        {
            if (!onlyWASD)
            {
                return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            }

            var x = (Input.GetKey(KeyCode.D) ? 1 : 0) + (Input.GetKey(KeyCode.A) ? -1 : 0);
            var y = (Input.GetKey(KeyCode.W) ? 1 : 0) + (Input.GetKey(KeyCode.S) ? -1 : 0);

            return new Vector2(x, y);
        }

        public static Vector2 GetMoveInputSmoothly()
        {
            var x = Input.GetAxis("Horizontal");
            var y = Input.GetAxis("Vertical");

            return new Vector2(x, y);
        }

        public static void ShowCursor() { Cursor.lockState = CursorLockMode.None; }

        public static void HideCursor() { Cursor.lockState = CursorLockMode.Locked; }

        public static void SetTargetFrameRate(int frameRate) { Application.targetFrameRate = frameRate; }

        public static string TranslateDirectionToEngString(Vector2 direction, bool preferX = false)
        {
            if (direction == Vector2.zero) return "Down";

            if (preferX && Mathf.Approximately(Mathf.Abs(direction.x), Mathf.Abs(direction.y)))
            {
                if (direction.x > 0) return "Right";
                if (direction.x < 0) return "Left";
            }

            //当Y值比X值大
            if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
            {
                if (direction.y > 0) return "Up";
                if (direction.y < 0) return "Down";
            }

            if (direction.x > 0) return "Right";
            return "Left";
        }

        public static void LookAt2D(Transform self, Vector3 target)
        {
            if (self == null) return;

            var dir = target - self.position;
            dir.z = 0;
            //方向转换为反正切值，再转换为角度
            var targetAngle = NormalizeAngle(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f);

            var euler = self.rotation.eulerAngles;
            var targetRot = Quaternion.Euler(euler.x, euler.y, targetAngle);

            self.rotation = targetRot;
        }

        public static bool LookAt2D(Transform self, Vector3 target, float smoothRate)
        {
            if (self == null) return false;

            var dir = target - self.position;
            dir.z = 0;
            //方向转换为反正切值，再转换为角度
            var targetAngle = NormalizeAngle(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f);

            var euler = self.eulerAngles;
            var targetRot = Quaternion.Euler(euler.x, euler.y, targetAngle);

            if (Mathf.Abs(euler.z - targetAngle) > 1f)
            {
                self.rotation = Quaternion.RotateTowards(self.rotation, targetRot, smoothRate * Time.deltaTime * 100);
                return false;
            }

            self.rotation = targetRot;

            return true;
        }

        public static void LookAt2DOnlyX(Transform self, Transform target)
        {
            if (self == null || target == null) return;

            var dir = target.transform.position - self.position;
            dir.z = 0;
            //方向转换为反正切值，再转换为角度
            var targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            self.localScale = new Vector3(targetAngle > 90 || targetAngle < -90 ? -1 : 1, self.localScale.y, self.localScale.z);
        }

        public static Transform DetectObjectCircle2D(Vector2 selfPos, float Range, int layer)
        {
            var result = Physics2D.OverlapCircle(selfPos, Range, layer);

            if (!result)
                return null;
            return result.transform;
        }

        /// <summary>
        /// 计算射线与 Y=height 的虚拟平面的交点
        /// </summary>
        public static bool GetRayPlaneIntersection(Ray ray, float height, out Vector3 hitPoint)
        {
            hitPoint = Vector3.zero;

            // 射线方向的Y分量为0 → 射线平行于平面，无交点
            if (Mathf.Approximately(ray.direction.y, 0))
                return false;

            // 计算参数t：t = (height - 射线原点Y) / 射线方向Y
            float t = (height - ray.origin.y) / ray.direction.y;

            // t<0 → 交点在相机后方，无效
            if (t < 0)
                return false;

            // 计算交点
            hitPoint = ray.origin + ray.direction * t;
            return true;
        }

        /// <summary>
        /// 2D 专用：计算三阶贝塞尔曲线追踪点（自身前方为Y轴正方向）
        /// </summary>
        public static Vector3 CalculateBezierPoint2D(Transform self, Vector3 targetPos, float curveStrength = 3f)
        {
            Vector3 start = self.position;
            Vector3 end = targetPos;
            Vector3 toTarget = end - start;

            Vector3 forward = self.up;

            // 2D 左右垂直方向（基于 Y 轴朝前）
            Vector3 right = new Vector3(-forward.y, forward.x, 0);

            //自动判断目标在左侧还是右侧，决定曲线往哪边弯
            float side = Mathf.Sign(Vector3.Dot(toTarget, right));
            Vector3 finalRight = right * side;

            //贝塞尔曲线控制点（动态方向）
            Vector3 controlPoint1 = start + finalRight * curveStrength;
            Vector3 controlPoint2 = end - forward * curveStrength;

            //计算 t=0.5 曲线中点
            float t = 0.5f;
            float omt = 1 - t;

            Vector3 point =
                omt * omt * omt * start +
                3 * omt * omt * t * controlPoint1 +
                3 * omt * t * t * controlPoint2 +
                t * t * t * end;

            return point;
        }

        /// <summary>
        /// 计算抛物线初速度（支持垂直方向）
        /// </summary>
        /// <param name="startPoint">起点</param>
        /// <param name="endPoint">原始终点</param>
        /// <param name="apexHeight">顶点高度（相对于起点）</param>
        /// <param name="interferencePercent">干扰百分比（0-1）</param>
        /// <param name="gravity">重力加速度</param>
        /// <returns>初速度向量</returns>
        public static Vector2 CalculateVelocityOfParabola2D(
            Vector2 startPoint,
            Vector2 endPoint,
            float apexHeight,
            float interferencePercent = 0f,
            float gravity = 9.8f)
        {
            interferencePercent = Mathf.Clamp01(interferencePercent);

            // // 计算带干扰的终点
            // Vector2 interferedEndPoint = new Vector2(
            //     GetRandomWithPercent(endPoint.x, interferencePercent),
            //     GetRandomWithPercent(endPoint.y, interferencePercent)
            // );

            float deltaX = endPoint.x - startPoint.x;
            float deltaY = endPoint.y - startPoint.y;

            // 处理水平距离为0（垂直方向）的情况
            if (Mathf.Abs(deltaX) < 0.001f)
            {
                return CalculateVerticalVelocity(startPoint.y, endPoint.y, apexHeight, gravity);
            }

            // 普通斜抛情况
            float verticalVelocity = Mathf.Sqrt(2 * gravity * apexHeight);
            float timeToApex = verticalVelocity / gravity;

            float fallDistance = apexHeight - deltaY;
            if (fallDistance < 0.1f) fallDistance = 0.1f;

            float timeFromApexToEnd = Mathf.Sqrt(2 * fallDistance / gravity);
            float totalTime = timeToApex + timeFromApexToEnd;

            float horizontalVelocity = deltaX / totalTime;

            //根据干扰率，调整最终方向向量
            return new Vector2(
                GetRandomWithPercent(horizontalVelocity, interferencePercent),
                verticalVelocity
            );
        }

        /// <summary>
        /// 计算垂直方向的上抛速度（起点和终点在同一垂直线上时使用）
        /// </summary>
        private static Vector2 CalculateVerticalVelocity(float startY, float endY, float apexHeight, float gravity)
        {
            // 垂直方向的总位移
            float totalVerticalDelta = endY - startY;

            // 确保顶点高度高于起点（至少高一点）
            float actualApexHeight = Mathf.Max(apexHeight, 0.5f); // 最低0.5单位高度

            // 计算上升阶段：从起点到顶点
            float riseDistance = actualApexHeight;
            float verticalVelocity = Mathf.Sqrt(2 * gravity * riseDistance);

            // 计算下落时间
            //float fallTime = Mathf.Sqrt(2 * fallDistance / gravity);

            // 总时间（用于验证，垂直方向水平速度为0）
            // 这里返回仅包含垂直分量的速度，水平速度为0
            return new Vector2(0, verticalVelocity);
        }

        private static float GetRandomWithPercent(float originalValue, float percent)
        {
            if (percent <= 0) return originalValue;
            float maxOffset = originalValue * percent;
            return originalValue + Random.Range(-maxOffset, maxOffset);
        }

        public static void LoadScene(string name) { SceneManager.LoadScene(name); }
    }
}