//Author: Arman Ghazanchyan
//Created date: 01/27/2007
//Last updated: 01/28/2007
using System;

using System.Drawing;
using System.Drawing.Drawing2D;

namespace LSLEditor.Editor
{
	class RoundCorners
	{
		///<summary>
		/// Rounds a rectangle corners' and returns the graphics path.
		/// </summary>
		/// <param name="rec">A rectangle whose corners should be rounded.</param>
		/// <param name="r">The radius of the rounded corners. This value should be 
		/// bigger then 0 and less or equal to the (a half of the smallest value 
		/// of the rectangle’s width and height).</param>
		/// <param name="exclude_TopLeft">A value that specifies if the top-left 
		/// corner of the rectangle should be rounded. If the value is True 
		/// then the corner is not rounded otherwise it is.</param>
		/// <param name="exclude_TopRight">A value that specifies if the top-right 
		/// corner of the rectangle should be rounded. If the value is True 
		/// then the corner is not rounded otherwise it is.</param>
		/// <param name="exclude_BottomRight">A value that specifies if the bottom-right 
		/// corner of the rectangle should be rounded. If the value is True 
		/// then the corner is not rounded otherwise it is.</param>
		/// <param name="exclude_BottomLeft">A value that specifies if the bottom-left 
		/// corner of the rectangle should be rounded. If the value is True 
		/// then the corner is not rounded otherwise it is.</param>
		public static GraphicsPath RoundedRectangle(Rectangle rec, int r)
		{
			return RoundedRectangle(rec,r,false,false,false,false);
		}

		public static GraphicsPath RoundedRectangle(Rectangle rec, int r,
		bool exclude_TopLeft,
		bool exclude_TopRight,
		bool exclude_BottomRight,
		bool exclude_BottomLeft)
		{
			GraphicsPath path = new GraphicsPath();
			int s = r * 2;
			//If 's' is less than or equal to zero, 
			//then return a simple rectangle.
			if (s <= 0)
			{
				path.StartFigure();
				path.AddLine(rec.Right, rec.Y, rec.Right, rec.Y);
				path.AddLine(rec.Right, rec.Bottom, rec.Right, rec.Bottom);
				path.AddLine(rec.X, rec.Bottom, rec.X, rec.Bottom);
				path.AddLine(rec.X, rec.Y, rec.X, rec.Y);
				path.CloseAllFigures();
				return path;
			}
			//If 's' is bigger than the smallest value of the size, 
			//then assign the value to 's'.
			if (rec.Height <= rec.Width)
			{
				if (s > rec.Height)
					s = rec.Height;
			}
			else
			{
				if (s > rec.Width)
					s = rec.Width;
			}

			path.StartFigure();
			//Set top-right corner.
			if (!exclude_TopRight)
				path.AddArc(rec.Right - s, rec.Y, s, s, 270, 90);
			else
				path.AddLine(rec.Right, rec.Y, rec.Right, rec.Y);

			//Set bottom-right corner.
			if (!exclude_BottomRight)
				path.AddArc(rec.Right - s, rec.Bottom - s, s, s, 0, 90);
			else
				path.AddLine(rec.Right, rec.Bottom, rec.Right, rec.Bottom);

			//Set bottom-left corner.
			if (!exclude_BottomLeft)
				path.AddArc(rec.X, rec.Bottom - s, s, s, 90, 90);
			else
				path.AddLine(rec.X, rec.Bottom, rec.X, rec.Bottom);

			//Set top-left corner.
			if (!exclude_TopLeft)
				path.AddArc(rec.X, rec.Y, s, s, 180, 90);
			else
				path.AddLine(rec.X, rec.Y, rec.X, rec.Y);

			path.CloseAllFigures();
			return path;
		}

		/// <summary>
		/// Rounds the corners of the newly created rectangle-shape region and returns the region.
		/// </summary>
		/// <param name="rSize">The size of the region.</param>
		/// <param name="r">The radius of the rounded corners. This value should be 
		/// bigger then 0 and less or equal to the (a half of the smallest value 
		/// of the region’s width and height).</param>
		/// <param name="exclude_TopLeft">A value that specifies if the top-left 
		/// corner of the region should be rounded. If the value is True 
		/// then the corner is not rounded otherwise it is.</param>
		/// <param name="exclude_TopRight">A value that specifies if the top-right 
		/// corner of the region should be rounded. If the value is True 
		/// then the corner is not rounded otherwise it is.</param>
		/// <param name="exclude_BottomRight">A value that specifies if the bottom-right 
		/// corner of the region should be rounded. If the value is True 
		/// then the corner is not rounded otherwise it is.</param>
		/// <param name="exclude_BottomLeft">A value that specifies if the bottom-left 
		/// corner of the region should be rounded. If the value is True 
		/// then the corner is not rounded otherwise it is.</param>
		public static Region RoundedRegion(Size rSize, int r)
		{
			return RoundedRegion(rSize, r, false, false, false, false);
		}

		public static Region RoundedRegion(Size rSize, int r,
		bool exclude_TopLeft,
		bool exclude_TopRight,
		bool exclude_BottomRight,
		bool exclude_BottomLeft)
		{
			int s = r * 2;
			GraphicsPath path = new GraphicsPath();
			//If 's' is less than or equal to zero, 
			//then return a simple rectangle.
			if (s <= 0)
			{
				path.StartFigure();
				path.AddLine(rSize.Width, 0, rSize.Width, 0);
				path.AddLine(rSize.Width, rSize.Height, rSize.Width, rSize.Height);
				path.AddLine(0, rSize.Height, 0, rSize.Height);
				path.AddLine(0, 0, 0, 0);
				path.CloseAllFigures();
				return new Region(path);
			}
			//If 's' is bigger than the smallest value of the size, 
			//then assign the value to 's'.
			if (rSize.Height < rSize.Width)
			{
				if (s > rSize.Height)
					s = rSize.Height;
			}
			else
			{
				if (s > rSize.Width)
					s = rSize.Width;
			}
			path.StartFigure();
			//Set top-right corner.
			if (!exclude_TopRight)
				path.AddArc(rSize.Width - s, 0, s - 1, s - 1, 270, 90);
			else
				path.AddLine(rSize.Width, 0, rSize.Width, 0);
			//Set bottom-right corner.
			if (!exclude_BottomRight)
			{
				path.AddLine(rSize.Width, r, rSize.Width, rSize.Height - r);
				path.AddArc(rSize.Width - s, rSize.Height - s, s - 1, s - 1, 0, 90);
			}
			else
				path.AddLine(rSize.Width, rSize.Height, rSize.Width, rSize.Height);

			//Set bottom-left corner.
			if (!exclude_BottomLeft)
			{
				path.AddLine(rSize.Width - r, rSize.Height, r, rSize.Height);
				path.AddArc(0, rSize.Height - s, s - 1, s - 1, 90, 90);
			}
			else
				path.AddLine(0, rSize.Height, 0, rSize.Height);

			//Set top-left corner.
			if (!exclude_TopLeft)
				path.AddArc(0, 0, s - 1, s - 1, 180, 90);
			else
				path.AddLine(0, 0, 0, 0);

			path.CloseAllFigures();
			return new Region(path);
		}
	}
}
