using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// 1. Added the "GooseModdingAPI" project as a reference.
// 2. Compile this.
// 3. Create a folder with this DLL in the root, and *no GooseModdingAPI DLL*
using GooseShared;
using SamEngine;
using System.Reflection;

namespace DefaultMod
{
    public class ModEntryPoint : IMod
    {
        String costume;
        Image costumeBMP;
        void IMod.Init()
        {
            //Read costume from ini file
            costume = "sanic";

            // Subscribe to Value Initialization InjectionPoint
            InjectionPoints.PreTickEvent += OneTimeInitialization;
        }

        public void OneTimeInitialization(GooseEntity g)
        {
            Console.WriteLine("Goose OneTimeInitialization");

            if (costume == "sanic")
            {
                Console.WriteLine("Goose Value Intialization - Costume Sanic");

                g.parameters.WalkSpeed = 80 * 2.5f;
                g.parameters.RunSpeed = 200 * 2.5f;
                g.parameters.ChargeSpeed = 400 * 2.5f; Sonicgoose

                g.currentSpeed = 80 * 2.5f;

                g.renderData.brushGooseOutline = new SolidBrush(Color.FromArgb(1,34,107));
                g.renderData.brushGooseWhite = new SolidBrush(Color.FromArgb(31,88,216)) ;

                costumeBMP = Sonicgoose.Properties.Resources.sonichair;

                InjectionPoints.PostRenderEvent += SanicPostRender;
            }

            if (costume == "ghost")
            {
                Console.WriteLine("Goose Value Intialization - Costume Ghost");

                g.renderData.brushGooseOutline = new SolidBrush(Color.Transparent);
                g.renderData.brushGooseWhite = new SolidBrush(Color.Transparent);
            }

            InjectionPoints.PreTickEvent -= OneTimeInitialization;
        }


        public void SanicPostRender(GooseEntity g, Graphics graph)
        {
            var direction = g.direction + 90f; ;
            var headPoint = g.rig.neckHeadPoint;

            var vertBase = (Rig.HeadRadius1 / 2) * .2f;

            var baseOffset = Rig.HeadRadius1 * 2f / 2;
            var vertOffset = (((float)costumeBMP.Height) / (float)costumeBMP.Width) * baseOffset * 2;

            var set = new[] {
                new Vector2(-baseOffset, vertBase + vertOffset),
                new Vector2(baseOffset, vertBase + vertOffset),
                new Vector2(-baseOffset, vertBase)
            };

            float? sin = null, cos = null;
            var asPoints = set.Select(v => Rotate(v, direction, ref sin, ref cos))
                              .Select(v => v + headPoint)
                              .Select(ToPoint).ToArray();

            graph.DrawImage(costumeBMP, asPoints);
        }

        private static Vector2 Rotate(Vector2 point, float degrees, ref float? sin, ref float? cos)
        {
            if (sin == null) sin = Sin(degrees);
            if (cos == null) cos = Cos(degrees);
            return new Vector2(point.x * cos.Value - point.y * sin.Value,
                               point.y * cos.Value + point.x * sin.Value);
        }

        private static float Sin(float deg)
            => (float)Math.Sin(deg * (Math.PI / 180d));
        private static float Cos(float deg)
            => (float)Math.Cos(deg * (Math.PI / 180d));

        private static Point ToPoint(Vector2 vector)
            => new Point((int)vector.x, (int)vector.y);
    }
}