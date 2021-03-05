﻿using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Common.Shared.Min.Extensions;
using IO.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SRAM.Comparison.Helpers;
using SRAM.Comparison.Properties;
using WebApp.SoE.Extensions;

#pragma warning disable 8509

namespace WebApp.SoE.ViewModels
{
	/// <summary>Base Viewmodel for loading SoE S-RAM files</summary>
	public class SetOffsetValueViewModel : GetOffsetValueViewModel
	{
#nullable disable
		[Inject] internal IJSRuntime JsRuntime { get; set; }
#nullable restore
		
		private bool Changed { get; set; }

		public bool CanSave => Changed && CanSet && !IsSavestate;

		public async Task SetOffsetValueAsync()
		{
			try
			{
				await Task.CompletedTask;
				
				SramFile.ThrowIfNull(nameof(SramFile));

				IsError = false;
				SramFile.SetOffsetValue(Options.CurrentFileSaveSlot - 1, OffsetAddress, (byte)OffsetValue);
				var valueDisplayText = NumberFormatter.FormatDecHexBin((byte)OffsetValue);

				OutputMessage = Resources.StatusSetOffsetValueTemplate.InsertArgs(OffsetAddress, valueDisplayText).ColorText(Color.Green).ToMarkup();

				Changed = true;
			}
			catch (Exception ex)
			{
				OutputMessage = ex.GetColoredMessage();
				IsError = true;
			}
		}

		public async Task SaveAndDownloadAsync()
		{
			try
			{
				SramFile.ThrowIfNull(nameof(SramFile));

				IsError = false;
				var bytes = new byte[8192];
				SramFile.Save(new MemoryStream(bytes));

				await JsRuntime.StartDownloadAsync(CurrentFileName!, bytes);

				Changed = false;
			}
			catch (Exception ex)
			{
				OutputMessage = ex.GetColoredMessage();
				IsError = true;
			}
		}
	}
}
