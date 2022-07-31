using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDrawing
{
    public class Helpers
    {
        public class CustomShapes
        {
            public static Pen ObjectPen { get; private set; }
            public static Pen ObjectBackgroundPen { get; private set; }

            /// <summary>
            /// Cylinder in isometric projection using basic shapes from System.Drawing
            /// For more info about isometric projection, visit:
            /// <see cref="https://en.wikipedia.org/wiki/Isometric_projection"/>
            /// </summary>
            public class IsometricCylinder : CustomShapes
            {
                /// <summary>
                /// Just a basic constructor to init everything needed.
                /// </summary>
                static IsometricCylinder()
                {
                    ObjectPen = new Pen(new SolidBrush(Color.FromArgb(255, 214, 16)));
                    ObjectBackgroundPen = new Pen(new SolidBrush(Color.FromArgb(114, 96, 6)));
                    ObjectBackgroundPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                }
        
                /// <summary>
                /// Just a quick magic math trick to get distance between two points.
                /// </summary>
                /// <param name="P1">Point 1</param>
                /// <param name="P2">Point 2</param>
                /// <returns></returns>
                private static double GetDistanceBetweenPoints(Point P1, Point P2)
                {
                    // whoah, magic!
                    return Math.Sqrt(Math.Pow(P1.X - P2.X, 2) + Math.Pow(P1.Y - P2.Y, 2));
                }

                /// <summary>
                /// Method where you can specify the look of your cylinder!
                /// </summary>
                /// <param name="objectPen">Pen that will be used to draw main(front) side of cylinder.</param>
                /// <param name="objectBackgroundPen">Pen that will be used to draw back(dashed) side of cylinder.</param>
                /// <param name="dashedBackgroundPen">Want a dotted line behind the cylinder?</param>
                public static void SetLook(Pen objectPen, Pen objectBackgroundPen, bool dashedBackgroundPen = true)
                {
                    ObjectPen = objectPen;
                    ObjectBackgroundPen = objectBackgroundPen;
                    ObjectBackgroundPen.DashStyle = dashedBackgroundPen ? System.Drawing.Drawing2D.DashStyle.Dash : System.Drawing.Drawing2D.DashStyle.Solid;
                }

                /// <summary>
                /// Creates an isometric cylinder with specified parameters.
                /// For more info about isometric projection, visit:
                /// <see cref="https://en.wikipedia.org/wiki/Isometric_projection"/>
                /// </summary>
                /// <param name="g">Graphics where the cylinder will be drawn.</param>
                /// <param name="canvas">Just to determine the center point of your canvas, where you'd like to draw.</param>
                /// <param name="r">Defines a radius of your shiny cylinder.</param>
                /// <param name="h">Defines a height of your masculine cylinder.</param>
                public static void DrawIsometricCylinder(Graphics g, Size canvas, int r, int h)
                {
                    // determine the center point
                    Point center = new Point(canvas.Width / 2, canvas.Height / 2);

                    // quick check if cylinder's not too big for mommy
                    r = r < canvas.Width / 4 ? r : canvas.Width / 4;
                    h = h < canvas.Height / 2 ? h : canvas.Height / 2;

                    // make sure it is actually centered in the canvas
                    // (no wrong holes anymore!)
                    Point O1 = center;
                    if (h > r) O1.Offset(0, -h); else O1.Offset(0, -r);

                    /// <seealso cref="cylinderlegend.jpg"/>
                    Point A = new Point(O1.X - r * 2, O1.Y + r);
                    Point B = new Point(O1.X + r * 2, O1.Y + r);

                    Point O2 = O1;
                    O2.Offset(0, 2 * r);

                    Point C = A;
                    C.Offset(0, h);

                    Point D = B;
                    D.Offset(0, h);

                    Point O7 = O2;
                    O7.Offset(0, h);

                    var distanceAB = GetDistanceBetweenPoints(A, B);
                    var distanceO1O2 = GetDistanceBetweenPoints(O1, O2);

                    Point E = new Point(A.X + (int)(distanceAB / 8), A.Y);
                    Point F = new Point(B.X - (int)(distanceAB / 8), B.Y);

                    Point G = E;
                    G.Offset(0, h);

                    Point H = F;
                    H.Offset(0, h);

                    int ellipseWidth = (int)(distanceO1O2 - distanceO1O2 / 4);

                    // that should be point D.. I don't remember anymore.
                    Point recPtD = new Point(E.X, O1.Y + (int)(distanceO1O2 / 8));

                    // idk, it looks a little better..
                    // if your pc can't handle that, just comment it out!
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    // finally drawing!
                    g.DrawArc(ObjectBackgroundPen, new Rectangle(recPtD.X, recPtD.Y + h, F.X - E.X, ellipseWidth), 180, 180);
                    g.DrawEllipse(ObjectPen, new Rectangle(recPtD.X, recPtD.Y, F.X - E.X, ellipseWidth));
                    g.DrawArc(ObjectPen, new Rectangle(recPtD.X, recPtD.Y + h, F.X - E.X, ellipseWidth), 0, 180);
                    g.DrawLine(ObjectPen, E, G);
                    g.DrawLine(ObjectPen, F, H);
                }
            }
        }
    }
}