using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SimpleTextEditor.Enums;
using SimpleTextEditor.Helpers;
using SimpleTextEditor.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace SimpleTextEditor
{
	public partial class TextEditor
	{
		[Parameter]
		public List<SimpleTextBlock> Blocks { get; set; }
		[Parameter]
		public EventCallback<List<SimpleTextBlock>> BlocksChanged { get; set; }	
		private int _blockIndex { get; set; }
		private ElementReference inputBoxRef { get; set; }
		private bool isMouseDown { get; set; } = false;
		private CursorSelectionState _cursorSelectionState { get; set; }
		private CursorSelectionState _lastCharacterSelection { get; set; }
		private string _alphaNumericCharacterPattern = "^[a-zA-Z0-9.,@£$%!?&*^()\\[\\]\\/{} ]$";
		private bool _focused = false;
		private bool bold { get; set; } = false;
		private bool underline { get; set; } = false;
		private bool italic { get; set; } = false;

		protected override void OnInitialized()
		{
			_cursorSelectionState = new CursorSelectionState();
			_cursorSelectionState.SetCursorPos(0, Blocks[0].Characters.Count() - 1);
		}
		private void FocusAndSetCursorPos(int blockIndex, int charIndex)
		{
			Focus();
			_cursorSelectionState.SetCursorPos(blockIndex, charIndex);
			isMouseDown = false;
		}
		private async Task KeyDown(KeyboardEventArgs e)
		{
			switch (true)
			{
				case bool _ when Regex.IsMatch(e.Key, _alphaNumericCharacterPattern):
					DeleteSelection();
					var blockObj = Blocks[_cursorSelectionState.CursorBlock];
					var charObj = new SimpleTextCharacter { Content = e.Key };
					blockObj.Characters.Insert(_cursorSelectionState.CursorChar + 1, charObj);
					_cursorSelectionState.SetCursorPos(Blocks.IndexOf(blockObj), blockObj.Characters.IndexOf(charObj));
					break;
				case bool _ when e.Key == "Enter":
					DeleteSelection();
					var newBlock = new SimpleTextBlock();
					Blocks.Insert(_cursorSelectionState.StartBlock + 1, newBlock);
					var charsToMove = Blocks[_cursorSelectionState.StartBlock].Characters.Where(fb => Blocks[_cursorSelectionState.StartBlock].Characters.IndexOf(fb) > _cursorSelectionState.StartChar);
					Blocks[_cursorSelectionState.StartBlock + 1].Characters.AddRange(charsToMove);
					Blocks[_cursorSelectionState.StartBlock].Characters.RemoveAll(fb => charsToMove.Contains(fb));
					_cursorSelectionState.SetCursorPos(_cursorSelectionState.StartBlock + 1, -1);
					break;
				case bool _ when e.Key == "Backspace":
					if (_cursorSelectionState.NoActiveSelection)
					{
						if (_cursorSelectionState.CursorChar < 0)
						{
							if (_cursorSelectionState.CursorBlock > 0)
							{
								var currentBlockChars = Blocks[_cursorSelectionState.CursorBlock].Characters;
								Blocks.Remove(Blocks[_cursorSelectionState.CursorBlock]);
								_cursorSelectionState.SetCursorPos(_cursorSelectionState.CursorBlock - 1, Blocks[_cursorSelectionState.CursorBlock - 1].Characters.Count() - 1);
								Blocks[_cursorSelectionState.CursorBlock].Characters.AddRange(currentBlockChars);
							}
							break;
						}
						_cursorSelectionState.StartBlock = _cursorSelectionState.CursorBlock;
						_cursorSelectionState.StartChar = _cursorSelectionState.CursorChar - 1;
					}

					DeleteSelection();
					break;
				case bool _ when e.Key == "ArrowLeft":
					if (_cursorSelectionState.CursorChar < 0)
					{
						if (_cursorSelectionState.CursorBlock > 0)
						{
							_cursorSelectionState.SetCursorPos(_cursorSelectionState.CursorBlock - 1, Blocks[_cursorSelectionState.CursorBlock - 1].Characters.Count() - 1);
						}
						break;
					}
					_cursorSelectionState.SetCursorPos(_cursorSelectionState.CursorBlock, _cursorSelectionState.CursorChar - 1);
					break;
				case bool _ when e.Key == "ArrowRight":
					if (_cursorSelectionState.CursorChar == Blocks[_cursorSelectionState.CursorBlock].Characters.Count() - 1)
					{
						if (_cursorSelectionState.CursorBlock < Blocks.Count() - 1)
						{
							_cursorSelectionState.SetCursorPos(_cursorSelectionState.CursorBlock + 1, -1);
						}
						break;
					}
					_cursorSelectionState.SetCursorPos(_cursorSelectionState.CursorBlock, _cursorSelectionState.CursorChar + 1);
					break;
			}
			OnContentChanged();
		}

		private void DeleteSelection()
		{
			if (_cursorSelectionState.StartBlock < _cursorSelectionState.EndBlock)
			{
				Blocks[_cursorSelectionState.StartBlock].Characters.RemoveRange(_cursorSelectionState.StartChar + 1, Blocks[_cursorSelectionState.StartBlock].Characters.Count() - _cursorSelectionState.StartChar - 1);
				Blocks[_cursorSelectionState.StartBlock].Characters.AddRange(Blocks[_cursorSelectionState.EndBlock].Characters.TakeLast(Blocks[_cursorSelectionState.EndBlock].Characters.Count() - _cursorSelectionState.EndChar - 1));
				var blocksToRemove = new List<SimpleTextBlock>();
				for (var i = _cursorSelectionState.StartBlock + 1; i <= _cursorSelectionState.EndBlock; i++)
				{
					blocksToRemove.Add(Blocks[i]);
				}
				Blocks.RemoveAll(b => blocksToRemove.Contains(b));
			}
			else
			{
				Blocks[_cursorSelectionState.StartBlock].Characters.RemoveRange(_cursorSelectionState.StartChar + 1, _cursorSelectionState.EndChar - _cursorSelectionState.StartChar);
			}
			_cursorSelectionState.SetCursorPos(_cursorSelectionState.StartBlock, _cursorSelectionState.StartChar);

		}

		private async Task SelectionStart(int block, int index)
		{
			Console.WriteLine($"Selection Start | CursorBlock: {_cursorSelectionState.CursorBlock}, CursorChar: {_cursorSelectionState.CursorChar}");
			Focus();
			isMouseDown = true;
			_cursorSelectionState.SetCursorPos(block, index);
		}
		private async Task SelectionAdd(int block, int index)
		{
			Console.WriteLine($"Selection Add | CursorBlock: {_cursorSelectionState.CursorBlock}, CursorChar: {_cursorSelectionState.CursorChar}");
			if (isMouseDown)
			{
				_cursorSelectionState.CursorBlock = block;
				_cursorSelectionState.CursorChar = index;
			}
		}
		public void SelectionEnd()
		{
			Console.WriteLine($"Selection End | CursorBlock: {_cursorSelectionState.CursorBlock}, CursorChar: {_cursorSelectionState.CursorChar}");
			if (isMouseDown)
			{
				UpdateFormatButtons();
			}
			isMouseDown = false;
		}
		public void Focus()
		{
			inputBoxRef.FocusAsync();
			_focused = true;
		}
		public void LostFocus()
		{
			Console.WriteLine($"LostFocus | Block Index: {_blockIndex} CursorBlock: {_cursorSelectionState.CursorBlock}, CursorChar: {_cursorSelectionState.CursorChar}");
			_focused = false;
			_lastCharacterSelection = JsonSerializer.Deserialize<CursorSelectionState>(JsonSerializer.Serialize(_cursorSelectionState));
			_cursorSelectionState.SetCursorPos(_blockIndex, 0);
			isMouseDown = false;
		}
		private void UpdateFormatButtons()
		{
			bold = SelectionFormatContains(CharacterFormatEnum.B);
			italic = SelectionFormatContains(CharacterFormatEnum.I);
			underline = SelectionFormatContains(CharacterFormatEnum.U);
		}

		private bool SelectionFormatContains(CharacterFormatEnum formatFlag)
		{
			Console.WriteLine($"CursorBlock: {_cursorSelectionState.CursorBlock}, CursorChar: {_cursorSelectionState.CursorChar}");
			Console.WriteLine($"Block: {_cursorSelectionState.StartBlock}, Char: {_cursorSelectionState.StartChar}");
			if(Blocks[_cursorSelectionState.StartBlock].Characters.Count() == 0)
			{
				return false;
			}
			var startChar = _cursorSelectionState.StartChar < 0 ? 0 : _cursorSelectionState.StartChar;
			var firstSelectedBlock = Blocks[_cursorSelectionState.StartBlock].Characters[startChar];
			if (firstSelectedBlock == null)
				return false;
			return (firstSelectedBlock.Format & formatFlag) != 0;
		}

		private void FormatItem(CharacterFormatEnum formatFlag)
		{
			Focus();
			_cursorSelectionState = JsonSerializer.Deserialize<CursorSelectionState>(JsonSerializer.Serialize(_lastCharacterSelection));
			var flagAdded = false;
			for (var i = _cursorSelectionState.StartBlock; i <= _cursorSelectionState.EndBlock; i++)
			{
				var startChar = i == _cursorSelectionState.StartBlock ? _cursorSelectionState.StartChar : 0;
				var endChar = i == _cursorSelectionState.EndBlock ? _cursorSelectionState.EndChar : Blocks[i].Characters.Count() - 1;

				for (var j = startChar; j <= endChar; j++)
				{
					if (i == _cursorSelectionState.StartBlock && j == startChar)
					{
						if ((Blocks[i].Characters[j < 0 ? 0 : j].Format & formatFlag) == 0)
						{
							flagAdded = true;
						}
						else
						{
							flagAdded = false;
						}
					}
					else
					{
						if (flagAdded)
						{
							Blocks[i].Characters[j < 0 ? 0 : j].Format |= formatFlag;
						}
						else
						{
							Blocks[i].Characters[j < 0 ? 0 : j].Format &= ~formatFlag;
						}
					}
				}
			}
			OnContentChanged();
		}

		private void SetBlockType(BlockTypeEnum blockType)
		{
			Focus();
			_cursorSelectionState = JsonSerializer.Deserialize<CursorSelectionState>(JsonSerializer.Serialize(_lastCharacterSelection));
			Blocks[_cursorSelectionState.CursorBlock].BlockType = blockType;
			OnContentChanged();
		}

		private void OnContentChanged()
		{
			BlocksChanged.InvokeAsync(Blocks);
			StateHasChanged();
		}
	}
}