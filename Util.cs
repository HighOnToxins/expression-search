using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionSearch {
	public static class Util {


		public static string AddFormat(string format) {

			int index = 0;
			int num = 0;
			while(true) {

				index = format.IndexOf("%s", index);

				if(index == -1) break;

				format = format.Substring(0, index) + "{" + num + "}" + format.Substring(index + 2);
				num++;
				index += 3;
			}

			return format;
		}

	}
}
