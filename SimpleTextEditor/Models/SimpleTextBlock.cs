using SimpleTextEditor.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTextEditor.Models
{
	public class SimpleTextBlock
	{
		public List<SimpleTextCharacter> Characters { get; set; } = new();
		public BlockTypeEnum BlockType { get; set; } = BlockTypeEnum.P;
	}
}