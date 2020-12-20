﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using Microsoft.AspNetCore.Components;

namespace WebServer.SoE.Extensions
{
	/// <summary>
	/// Various HTML extensions for strings
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>Sets a fore color of a string</summary>
		public static string ColorText(this string? source, Color color) => $"<span style='color:{color.Name}'>{source}</span>";

		/// <summary>Sets a background color of a string</summary>
		public static string BgColorText(this string? source, Color color) => $"<span style='background-color:{color.Name}'>{source}</span>";

		/// <summary>Adds a tooltop to a string</summary>
		public static string AddTooltip(this string source, string title) => $"<span title='{title}'>{source}</span>";

		/// <summary>Replaces spaces by non breakable spaces</summary>
		public static MarkupString ReplaceSpaces(this string source) => source.Replace(" ", "&nbsp;").ToMarkup();
		
		/// <summary>Creates a markup string</summary>
		public static MarkupString ToMarkup(this string source) => (MarkupString)source;

		public static string ReplaceHtmlLinebreaks(this MarkupString source) => source.ToString().ReplaceHtmlLinebreaks();
		public static string ReplaceHtmlLinebreaks([NotNull] this string source) => source
			.Replace("<br>", Environment.NewLine);
	}
}
