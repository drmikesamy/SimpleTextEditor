using SimpleTextEditor.Enums;
using SimpleTextEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace SimpleTextEditor.Helpers
{
	public static class Extensions
	{
		public static string ToHtml(this List<SimpleTextBlock> blocks)
		{
			var htmlContent = "";
			foreach (var block in blocks)
			{
				htmlContent += $"<{block.BlockType.ToString().ToLower()}>";
				CharacterFormatEnum lastFormat = CharacterFormatEnum.None;

				var placeHolderFormatBlock = new SimpleTextCharacter{ Content = "*" };
				block.Characters.Add(placeHolderFormatBlock);

				foreach (var character in block.Characters)
				{

					if ((lastFormat & CharacterFormatEnum.I) == 0 && (character.Format & CharacterFormatEnum.I) != 0)
					{
						htmlContent += $"<i>";
					}
					if ((lastFormat & CharacterFormatEnum.U) == 0 && (character.Format & CharacterFormatEnum.U) != 0)
					{
						htmlContent += $"<u>";
					}
					if ((lastFormat & CharacterFormatEnum.B) == 0 && (character.Format & CharacterFormatEnum.B) != 0)
					{
						htmlContent += $"<b>";
					}
					if ((lastFormat & CharacterFormatEnum.B) != 0 && ((character.Format & CharacterFormatEnum.B) == 0))
					{
						htmlContent += $"</b>";
					}
					if ((lastFormat & CharacterFormatEnum.U) != 0 && ((character.Format & CharacterFormatEnum.U) == 0))
					{
						htmlContent += $"</u>";
					}
					if ((lastFormat & CharacterFormatEnum.I) != 0 && ((character.Format & CharacterFormatEnum.I) == 0))
					{
						htmlContent += $"</i>";
					}

					if (character != placeHolderFormatBlock)
					{
						htmlContent += $"{character.Content}";
					}
					lastFormat = character.Format;
				}

				block.Characters.Remove(placeHolderFormatBlock);

				htmlContent += $"</{block.BlockType.ToString().ToLower()}>";
			}
			return htmlContent;
		}
		public static string ToJson(this List<SimpleTextBlock> blocks)
		{
			return JsonSerializer.Serialize(blocks);
		}
		public static List<SimpleTextBlock> FromJson(this string serialisedBlocks)
		{
			try
			{
				return JsonSerializer.Deserialize<List<SimpleTextBlock>>(serialisedBlocks);
			}
			catch
			{
				return new List<SimpleTextBlock> { new SimpleTextBlock() };
			}
		}
	}
}
