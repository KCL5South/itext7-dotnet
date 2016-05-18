/*
$Id$

This file is part of the iText (R) project.
Copyright (c) 1998-2016 iText Group NV
Authors: Bruno Lowagie, Paulo Soares, et al.

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License version 3
as published by the Free Software Foundation with the addition of the
following permission added to Section 15 as permitted in Section 7(a):
FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
ITEXT GROUP. ITEXT GROUP DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
OF THIRD PARTY RIGHTS

This program is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU Affero General Public License for more details.
You should have received a copy of the GNU Affero General Public License
along with this program; if not, see http://www.gnu.org/licenses or write to
the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
Boston, MA, 02110-1301 USA, or download the license from the following URL:
http://itextpdf.com/terms-of-use/

The interactive user interfaces in modified source and object code versions
of this program must display Appropriate Legal Notices, as required under
Section 5 of the GNU Affero General Public License.

In accordance with Section 7(b) of the GNU Affero General Public License,
a covered work must retain the producer line in every PDF that is created
or manipulated using iText.

You can be released from the requirements of the license by purchasing
a commercial license. Buying such a license is mandatory as soon as you
develop commercial activities involving the iText software without
disclosing the source code of your own applications.
These activities include: offering paid services to customers as an ASP,
serving PDFs on the fly in a web application, shipping iText with a closed
source product.

For more information, please contact iText Software Corp. at this
address: sales@itextpdf.com
*/
using System;
using System.Collections.Generic;
using iTextSharp.IO;
using iTextSharp.IO.Font.Cmap;
using iTextSharp.IO.Font.Otf;
using iTextSharp.IO.Util;

namespace iTextSharp.IO.Font
{
	public class CidFont : FontProgram
	{
		private int pdfFontFlags;

		private ICollection<String> compatibleCmaps;

		internal CidFont(String fontName, ICollection<String> cmaps)
		{
			compatibleCmaps = cmaps;
			InitializeCidFontNameAndStyle(fontName);
			IDictionary<String, Object> fontDesc = CidFontProperties.GetAllFonts()[fontNames.
				GetFontName()];
			if (fontDesc == null)
			{
				throw new IOException("no.such.predefined.font.1").SetMessageParams(fontName);
			}
			InitializeCidFontProperties(fontDesc);
		}

		internal CidFont(String fontName, ICollection<String> cmaps, IDictionary<String, 
			Object> fontDescription)
		{
			InitializeCidFontNameAndStyle(fontName);
			InitializeCidFontProperties(fontDescription);
			compatibleCmaps = cmaps;
		}

		public virtual bool CompatibleWith(String cmap)
		{
			if (cmap.Equals(PdfEncodings.IDENTITY_H) || cmap.Equals(PdfEncodings.IDENTITY_V))
			{
				return true;
			}
			else
			{
				return compatibleCmaps != null && compatibleCmaps.Contains(cmap);
			}
		}

		public override int GetKerning(Glyph glyph1, Glyph glyph2)
		{
			return 0;
		}

		public override int GetPdfFontFlags()
		{
			return pdfFontFlags;
		}

		public override bool IsFontSpecific()
		{
			return false;
		}

		private void InitializeCidFontNameAndStyle(String fontName)
		{
			String nameBase = GetBaseName(fontName);
			if (nameBase.Length < fontName.Length)
			{
				fontNames.SetFontName(fontName);
				fontNames.SetStyle(fontName.Substring(nameBase.Length));
			}
			else
			{
				fontNames.SetFontName(fontName);
			}
		}

		private void InitializeCidFontProperties(IDictionary<String, Object> fontDesc)
		{
			fontIdentification.SetPanose((String)fontDesc["Panose"]);
			fontMetrics.SetItalicAngle(System.Convert.ToInt32((String)fontDesc["ItalicAngle"]
				));
			fontMetrics.SetCapHeight(System.Convert.ToInt32((String)fontDesc["CapHeight"]));
			fontMetrics.SetTypoAscender(System.Convert.ToInt32((String)fontDesc["Ascent"]));
			fontMetrics.SetTypoDescender(System.Convert.ToInt32((String)fontDesc["Descent"]));
			fontMetrics.SetStemV(System.Convert.ToInt32((String)fontDesc["StemV"]));
			pdfFontFlags = System.Convert.ToInt32((String)fontDesc["Flags"]);
			String fontBBox = (String)fontDesc["FontBBox"];
			StringTokenizer tk = new StringTokenizer(fontBBox, " []\r\n\t\f");
			int llx = System.Convert.ToInt32(tk.NextToken());
			int lly = System.Convert.ToInt32(tk.NextToken());
			int urx = System.Convert.ToInt32(tk.NextToken());
			int ury = System.Convert.ToInt32(tk.NextToken());
			fontMetrics.UpdateBbox(llx, lly, urx, ury);
			registry = (String)fontDesc["Registry"];
			String uniMap = GetCompatibleUniMap(registry);
			if (uniMap != null)
			{
				IntHashtable metrics = (IntHashtable)fontDesc["W"];
				CMapCidUni cid2Uni = FontCache.GetCid2UniCmap(uniMap);
				avgWidth = 0;
				foreach (int cid in cid2Uni.GetCids())
				{
					int uni = cid2Uni.Lookup(cid);
					int width = metrics.ContainsKey(cid) ? metrics.Get(cid) : DEFAULT_WIDTH;
					Glyph glyph = new Glyph(cid, width, uni);
					avgWidth += glyph.GetWidth();
					codeToGlyph[cid] = glyph;
					unicodeToGlyph[uni] = glyph;
				}
				FixSpaceIssue();
				if (codeToGlyph.Count != 0)
				{
					avgWidth /= codeToGlyph.Count;
				}
			}
		}

		private static String GetCompatibleUniMap(String registry)
		{
			String uniMap = "";
			foreach (String name in CidFontProperties.GetRegistryNames()[registry + "_Uni"])
			{
				uniMap = name;
				if (name.EndsWith("H"))
				{
					break;
				}
			}
			return uniMap;
		}
	}
}