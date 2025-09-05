using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaPrsimSimple.Helpers
{

    public class LinearGradientBrushHelper
    {
        /// <summary>
        /// RotateAngle AttachedProperty definition
        /// indicates ....
        /// </summary>
        public static readonly AttachedProperty<double> RotateAngleProperty =
            AvaloniaProperty.RegisterAttached<LinearGradientBrushHelper, StyledElement, double>("RotateAngle", coerce: OnRotateAngleChanged);

        private static double OnRotateAngleChanged(AvaloniaObject @object, double arg2)
        {
            if (@object is Border border)
            {
                SetGradientRotation(border, border.BorderBrush as LinearGradientBrush, arg2);
            }
            return arg2;
        }

        /// <summary>
        /// Accessor for Attached property <see cref="RotateAngleProperty"/>.
        /// </summary>
        /// <param name="element">Target element</param>
        /// <param name="value">The value to set  <see cref="RotateAngleProperty"/>.</param>
        public static void SetRotateAngle(StyledElement element, double value) =>
            element.SetValue(RotateAngleProperty, value);

        /// <summary>
        /// Accessor for Attached property <see cref="RotateAngleProperty"/>.
        /// </summary>
        /// <param name="element">Target element</param>
        public static double GetRotateAngle(StyledElement element) =>
            element.GetValue(RotateAngleProperty);

        /// <summary>
        /// 设置渐变色的角度。
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="linearGradientBrush"></param>
        /// <param name="rotation"></param>
        /// <exception cref="Exception"></exception>
        public static void SetGradientRotation(Visual visual, LinearGradientBrush linearGradientBrush, double rotation)
        {
            var borderRect = new Rect(visual.Bounds.Size);
            SetGradientRotation(borderRect, linearGradientBrush, rotation);
        }

        /// <summary>
        /// 设置渐变色的角度。
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="linearGradientBrush"></param>
        /// <param name="rotation"></param>
        /// <exception cref="Exception"></exception>
        public static void SetGradientRotation(Rect borderRect, LinearGradientBrush linearGradientBrush, double rotation)
        {
            var m = Math.Tan(rotation);

            double Normalize(double rotation)
            {
                return rotation % (2 * Math.PI);
            }

            bool IsP90(double nrotation)
            {
                if (nrotation == (Math.PI / 2)) return true;
                return false;
            }
            bool IsN90(double nrotation)
            {
                if (nrotation == (Math.PI / 2 + Math.PI)) return true;
                return false;
            }
            bool IsP180(double nrotation)
            {
                return nrotation == Math.PI;
            }
            bool IsP0(double nrotation)
            {
                return nrotation == 0 || nrotation == Math.PI * 2;
            }

            double GetY(double x)
            {
                return m * (x - borderRect.Center.X) + borderRect.Center.Y;
            }

            double GetX(double y)
            {
                return (y - borderRect.Center.Y) / m + borderRect.Center.X;
            }

            Point GetFollowDirectionInsectionCore(double nrotation)
            {
                if (nrotation > 0 && nrotation < Math.PI / 2)
                {
                    var bottomY = borderRect.Height;
                    var bottomX = GetX(bottomY);

                    var rightX = borderRect.Width;
                    var rightY = GetY(rightX);

                    if (bottomY < rightY) return new Point(bottomX, bottomY);
                    else return new Point(rightX, rightY);
                }
                else if (nrotation > Math.PI / 2 && nrotation < Math.PI * 1)
                {
                    var bottomY = borderRect.Height;
                    var bottomX = GetX(bottomY);

                    var leftX = 0;
                    var leftY = GetY(leftX);

                    if (bottomY < leftY) return new Point(bottomX, bottomY);
                    else return new Point(leftX, leftY);
                }
                else if (nrotation > Math.PI * 1 && nrotation < Math.PI * 3d / 2d)
                {
                    var topY = 0;
                    var topX = GetX(topY);

                    var leftX = 0;
                    var leftY = GetY(leftX);

                    if (topY > leftY) return new Point(topX, topY);
                    else return new Point(leftX, leftY);
                }
                else if (nrotation > Math.PI * 3d / 2d && nrotation < Math.PI * 2)
                {
                    var topY = 0;
                    var topX = GetX(topY);

                    var rightX = borderRect.Width;
                    var rightY = GetY(rightX);

                    if (topY > rightY) return new Point(topX, topY);
                    else return new Point(rightX, rightY);
                }
                throw new Exception("GetFollowDirectionInsection 角度不在定义域内，定义域不包括 0 90 180 270");
            }

            Point GetReverseDirectionInsectionCore(double nrotation)
            {
                var vrotation = (nrotation + Math.PI) % (Math.PI * 2);
                return GetFollowDirectionInsectionCore(vrotation);
            }

            Point GetFollowDirectionInsection(double nrotation)
            {
                var point = GetFollowDirectionInsectionCore(nrotation);
                return new Point(point.X / borderRect.Width, point.Y / borderRect.Height);
            }
            Point GetReverseDirectionInsection(double nrotation)
            {
                var point = GetReverseDirectionInsectionCore(nrotation);

                return new Point(point.X / borderRect.Width, point.Y / borderRect.Height);
            }

            void SetPoint(Point startPoint, Point endPoint)
            {
                linearGradientBrush.StartPoint = new RelativePoint(startPoint, RelativeUnit.Relative);
                linearGradientBrush.EndPoint = new RelativePoint(endPoint, RelativeUnit.Relative);
            }

            var nrotation = Normalize(rotation);

            Point startPoint, endPoint;

            if (IsP90(nrotation))
            {
                startPoint = new Point(0.5, 0);
                endPoint = new Point(0.5, 1);
                SetPoint(startPoint, endPoint);
            }
            else if (IsN90(nrotation))
            {
                startPoint = new Point(0.5, 1);
                endPoint = new Point(0.5, 0);
                SetPoint(startPoint, endPoint);
            }
            else if (IsP0(nrotation))
            {
                startPoint = new Point(0, 0.5);
                endPoint = new Point(1, 0.5);
                SetPoint(startPoint, endPoint);
            }
            else if (IsP180(nrotation))
            {
                startPoint = new Point(1, 0.5);
                endPoint = new Point(0, 0.5);
                SetPoint(startPoint, endPoint);
            }
            else
            {
                // 关于起点和终点的配置，哪个是起点，哪个是终点？
                startPoint = GetReverseDirectionInsection(nrotation);
                endPoint = GetFollowDirectionInsection(nrotation);
                SetPoint(startPoint, endPoint);

            }

            //Debug.WriteLine($"{startPoint} {endPoint}");

        }

    }

}
