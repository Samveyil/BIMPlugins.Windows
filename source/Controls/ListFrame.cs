using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BIMPlugins.Windows.Controls
{
    /// <summary>
    /// The frame around a list: 1 px in the primary colour with rounded corners. The list inside is clipped to the
    /// rounding, so that the square ends of its scroll bars do not cover the frame's corners.
    /// </summary>
    internal sealed class ListFrame : Border
    {
        public ListFrame()
        {
            BorderThickness = new Thickness(1);
            CornerRadius = new CornerRadius(3);
            SetResourceReference(BorderBrushProperty, "BIMPlugins.PrimaryBrush");
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var size = base.ArrangeOverride(finalSize);

            if (Child != null)
            {
                // The inner edge of the frame is rounded by the corner's radius less the frame's thickness.
                var radius = Math.Max(0, CornerRadius.TopLeft - BorderThickness.Left);
                var clip = new RectangleGeometry(new Rect(Child.RenderSize), radius, radius);
                clip.Freeze();
                Child.Clip = clip;
            }

            return size;
        }
    }
}
