/*
$Id: 5d6b6bbd04bda7a3019a5dcc8b72b90856c1c436 $

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
using com.itextpdf.io.util;

namespace com.itextpdf.io.font.cmap
{
	/// <author>psoares</author>
	public class CMapCidByte : AbstractCMap
	{
		private IDictionary<int, byte[]> map = new Dictionary<int, byte[]>();

		private readonly byte[] EMPTY = new byte[] {  };

		internal override void AddChar(String mark, CMapObject code)
		{
			if (code.IsNumber())
			{
				byte[] ser = DecodeStringToByte(mark);
				map[(int)code.GetValue()] = ser;
			}
		}

		public virtual byte[] Lookup(int cid)
		{
			byte[] ser = map[cid];
			if (ser == null)
			{
				return EMPTY;
			}
			else
			{
				return ser;
			}
		}

		public virtual IntHashtable GetReversMap()
		{
			IntHashtable code2cid = new IntHashtable(map.Count);
			foreach (int cid in map.Keys)
			{
				byte[] bytes = map[cid];
				int byteCode = 0;
				foreach (byte b in bytes)
				{
					byteCode <<= 8;
					byteCode += b & 0xff;
				}
				code2cid.Put(byteCode, cid);
			}
			return code2cid;
		}
	}
}