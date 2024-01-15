using SimpleTextEditor.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTextEditor.Models
{
	public class SimpleTextCharacter
	{
		public string Content { get; set; } = "";
		public CharacterFormatEnum Format { get; set; } = CharacterFormatEnum.None;
	}
}