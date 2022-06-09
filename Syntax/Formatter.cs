
using System.Text.RegularExpressions;

namespace ExpressionSearch.Syntax {
	public static class Formatter {

		/*
		/// <summary> Reads each input of the stirng using the given format.
		/// Returns null if the format was not found. </summary>
		public static string[]? ReadFormat(string format, string s) {
			//count number of input
			bool exist = true;
			int inputCount = 0;
			while(exist) {
				exist = format.Contains("{" + inputCount + "}");
				if(exist) inputCount++;
			}

			//create regex
			string regex = format;

			for(int i = 0; i < inputCount; i++) {
				regex = regex.Replace("{" + i + "}", "(?<" + i + ">.*)");
			}

			//exstracts inputs
			Match matches = Regex.Match(s, regex);

			string[] inputStrings = new string[inputCount];
			for(int i = 0; i < inputStrings.Length; i++) {
				inputStrings[i] = matches.Groups[i].Value;
			}

			return inputStrings;
		}
		*/

		/// <summary> Reads each input of the stirng using the given format. 
		/// Returns null if the format was not found. </summary>
		public static string[]? ReadFormat(string format, string s) {
			//split format into formatting strings
			string[] formStrings = format.Split("%s");

			//use the formatting string to find occurence
			int currentIndex = 0;
			int firstBeginIndex = -1;

			string[] strings = new string[formStrings.Length - 1];

			for(int i = 0; i < formStrings.Length - 1; i++) {

				//getting end index and begin index
				int beginIndex = GetIndex(formStrings[i], s, currentIndex, i == 0, true, true);
				int endIndex = GetIndex(formStrings[i + 1], s, beginIndex, i == formStrings.Length - 2, false, true);

				if(endIndex == -1 || beginIndex == -1) {
					if(firstBeginIndex == -1 || firstBeginIndex == beginIndex - formStrings[i].Length) return null;

					currentIndex = firstBeginIndex + 1;
					i = -1;
					continue;
				}

				if(i == 0) firstBeginIndex = beginIndex - formStrings[i].Length;

				strings[i] = s.Substring(beginIndex, endIndex - beginIndex + 1);
				currentIndex = endIndex;
			}

			return strings;
		}

		/// <summary> Reads each input of the stirng using the given format. 
		/// Returns null if the format was not found. </summary>
		public static string[]? LastReadFormat(string format, string s) {

			//split format into formatting strings
			string[] formStrings = format.Split("%s");

			//use the formatting string to find occurence
			int currentIndex = s.Length - 1;
			int firstEndIndex = s.Length;

			string[] strings = new string[formStrings.Length - 1];

			for(int i = formStrings.Length - 2; i >= 0; i--) {

				//getting end index and begin index
				int endIndex = GetIndex(formStrings[i + 1], s, currentIndex, i == formStrings.Length - 2, false, false);
				int beginIndex = GetIndex(formStrings[i], s, endIndex, i == 0, true, false);

				if(endIndex == -1 || beginIndex == -1) {
					if(firstEndIndex == s.Length || firstEndIndex == endIndex + 1) return null;

					currentIndex = firstEndIndex - 1;
					i = -1;
					continue;
				}

				if(i == formStrings.Length - 2) firstEndIndex = endIndex + 1;

				strings[i] = s.Substring(beginIndex, endIndex - beginIndex + 1);
				currentIndex = beginIndex;
			}

			return strings;
		}


		private static int GetIndex(string formString, string s, int currentIndex, bool isAtIter, bool isBegin, bool leftRight) {
			if(currentIndex == -1) return -1;

			int index = leftRight ? s.IndexOf(formString, currentIndex) : s.LastIndexOf(formString, currentIndex);
			int foundIndex = index + (isBegin ? formString.Length : -1);
			if(index == -1 || (formString.Length != 0 && isAtIter &&
				(isBegin ? foundIndex != formString.Length : foundIndex + formString.Length != s.Length - 1))) return -1;
			if(formString.Length == 0) {
				if(isBegin && !leftRight) foundIndex = isAtIter ? 0 : currentIndex;
				if(!isBegin && leftRight) foundIndex = !isAtIter ? currentIndex : s.Length - 1;
			}
			return foundIndex;
		}

		public static string? ReadRegexValue(string regex, string s){

			//removes right and left whitespace
			int leftSpace = 0;
			while(leftSpace<s.Length && (s[leftSpace] == ' ' || s[leftSpace] == '\t' || s[leftSpace] == '\n')) leftSpace++;
			int rightSpace = s.Length - 1;
			while(rightSpace >= 0 && (s[rightSpace] == ' ' || s[rightSpace] == '\t' || s[rightSpace] == '\n')) rightSpace--;
			s = s.Substring(leftSpace, rightSpace - leftSpace + 1);

			//determines the given
			Match match = Regex.Match(s, regex);
			if(match.Success && match.Value.Length == s.Length) return match.Value;

			return null;
		}

	}
}
