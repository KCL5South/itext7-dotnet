/*
* $Id: be2ae88ae871e34a9ae7179420b89c99dff576ca $
*
* This file is part of the iText (R) project.
* Copyright (c) 2014-2015 iText Group NV
* Authors: Bruno Lowagie, Paulo Soares, et al.
*
* This program is free software; you can redistribute it and/or modify
* it under the terms of the GNU Affero General Public License version 3
* as published by the Free Software Foundation with the addition of the
* following permission added to Section 15 as permitted in Section 7(a):
* FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
* ITEXT GROUP. ITEXT GROUP DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
* OF THIRD PARTY RIGHTS
*
* This program is distributed in the hope that it will be useful, but
* WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
* or FITNESS FOR A PARTICULAR PURPOSE.
* See the GNU Affero General Public License for more details.
* You should have received a copy of the GNU Affero General Public License
* along with this program; if not, see http://www.gnu.org/licenses or write to
* the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
* Boston, MA, 02110-1301 USA, or download the license from the following URL:
* http://itextpdf.com/terms-of-use/
*
* The interactive user interfaces in modified source and object code versions
* of this program must display Appropriate Legal Notices, as required under
* Section 5 of the GNU Affero General Public License.
*
* In accordance with Section 7(b) of the GNU Affero General Public License,
* a covered work must retain the producer line in every PDF that is created
* or manipulated using iText.
*
* You can be released from the requirements of the license by purchasing
* a commercial license. Buying such a license is mandatory as soon as you
* develop commercial activities involving the iText software without
* disclosing the source code of your own applications.
* These activities include: offering paid services to customers as an ASP,
* serving PDFs on the fly in a web application, shipping iText with a closed
* source product.
*
* For more information, please contact iText Software Corp. at this
* address: sales@itextpdf.com
*
*
* This class is based on the C# open source freeware library Clipper:
* http://www.angusj.com/delphi/clipper.php
* The original classes were distributed under the Boost Software License:
*
* Freeware for both open source and commercial applications
* Copyright 2010-2014 Angus Johnson
* Boost Software License - Version 1.0 - August 17th, 2003
*
* Permission is hereby granted, free of charge, to any person or organization
* obtaining a copy of the software and accompanying documentation covered by
* this license (the "Software") to use, reproduce, display, distribute,
* execute, and transmit the Software, and to prepare derivative works of the
* Software, and to permit third-parties to whom the Software is furnished to
* do so, all subject to the following:
*
* The copyright notices in the Software and this entire statement, including
* the above license grant, this restriction and the following disclaimer,
* must be included in all copies of the Software, in whole or in part, and
* all derivative works of the Software, unless such copies or derivative
* works are solely in the form of machine-executable object code generated by
* a source language processor.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE, TITLE AND NON-INFRINGEMENT. IN NO EVENT
* SHALL THE COPYRIGHT HOLDERS OR ANYONE DISTRIBUTING THE SOFTWARE BE LIABLE
* FOR ANY DAMAGES OR OTHER LIABILITY, WHETHER IN CONTRACT, TORT OR OTHERWISE,
* ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
* DEALINGS IN THE SOFTWARE.
*/
namespace com.itextpdf.kernel.pdf.canvas.parser.clipper
{
	public abstract class IClipper
	{
		public enum ClipType
		{
			INTERSECTION,
			UNION,
			DIFFERENCE,
			XOR
		}

		public enum Direction
		{
			RIGHT_TO_LEFT,
			LEFT_TO_RIGHT
		}

		public enum EndType
		{
			CLOSED_POLYGON,
			CLOSED_LINE,
			OPEN_BUTT,
			OPEN_SQUARE,
			OPEN_ROUND
		}

		public enum JoinType
		{
			BEVEL,
			ROUND,
			MITER
		}

		public enum PolyFillType
		{
			EVEN_ODD,
			NON_ZERO,
			POSITIVE,
			NEGATIVE
		}

		public enum PolyType
		{
			SUBJECT,
			CLIP
		}

		public interface IZFillCallback
		{
			void ZFill(Point.LongPoint bot1, Point.LongPoint top1, Point.LongPoint bot2, Point.LongPoint
				 top2, Point.LongPoint pt);
		}

		public const int REVERSE_SOLUTION = 1;

		public const int STRICTLY_SIMPLE = 2;

		public const int PRESERVE_COLINEAR = 4;

		//InitOptions that can be passed to the constructor ...
		public abstract bool AddPath(Path pg, IClipper.PolyType polyType, bool Closed);

		public abstract bool AddPaths(Paths ppg, IClipper.PolyType polyType, bool closed);

		public abstract void Clear();

		public abstract bool Execute(IClipper.ClipType clipType, Paths solution);

		public abstract bool Execute(IClipper.ClipType clipType, Paths solution, IClipper.PolyFillType
			 subjFillType, IClipper.PolyFillType clipFillType);

		public abstract bool Execute(IClipper.ClipType clipType, PolyTree polytree);

		public abstract bool Execute(IClipper.ClipType clipType, PolyTree polytree, IClipper.PolyFillType
			 subjFillType, IClipper.PolyFillType clipFillType);
	}

	public static class IClipperConstants
	{
	}
}