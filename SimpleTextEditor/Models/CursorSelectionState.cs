﻿using SimpleTextEditor.Enums;
namespace SimpleTextEditor.Models
{
	public class CursorSelectionState
	{
		private int _cursorBlock;
		private int _cursorChar;
		public int StartBlock { get; set; }
		public int StartChar { get; set; }
		public int EndBlock { get; set; }
		public int EndChar { get; set; }
		public int AnchorBlock { get; set; }
		public int AnchorChar { get; set; }
		public int CursorBlock
		{
			get { return _cursorBlock; }
			set
			{
				_cursorBlock = value;
				StartBlock = Math.Min(AnchorBlock, _cursorBlock);
				EndBlock = Math.Max(AnchorBlock, _cursorBlock);
			}
		}
		public int CursorChar
		{
			get { return _cursorChar; }
			set
			{
				_cursorChar = value;
				if (AnchorBlock != CursorBlock)
				{
					if (CursorBlock == StartBlock)
					{
						StartChar = _cursorChar;
					}
					if (CursorBlock == EndBlock)
					{
						EndChar = _cursorChar;
					}
				}
				else
				{
					StartChar = Math.Min(AnchorChar, _cursorChar);
					EndChar = Math.Max(AnchorChar, _cursorChar);
				}

			}
		}
		public void SetCursorPos(int blockIndex, int charIndex)
		{
			AnchorBlock = blockIndex;
			AnchorChar = charIndex;
			CursorBlock = blockIndex;
			CursorChar = charIndex;
		}
		public bool NoActiveSelection => AnchorBlock == CursorBlock && AnchorChar == CursorChar;
		public bool IsSelected(int blockIndex, int charIndex)
		{
			if ((blockIndex > StartBlock && blockIndex < EndBlock)
			|| (blockIndex == StartBlock && EndBlock > StartBlock && charIndex > StartChar)
			|| (blockIndex == EndBlock && EndBlock > StartBlock && charIndex <= EndChar)
			|| (StartBlock == EndBlock && blockIndex == EndBlock && charIndex <= EndChar && charIndex > StartChar))
			{
				return true;
			}
			return false;
		}
	}
}
