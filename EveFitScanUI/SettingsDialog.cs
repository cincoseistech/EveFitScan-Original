using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using EveFitScan.Core.Catalog;
using SdeConverter;

namespace EveFitScanUI;

public class SettingsDialog : Form
{
	private readonly Action _onCatalogUpdated;

	private CancellationTokenSource _updateCts;

	private IContainer components = null;

	private Button m_ButtonOk;

	private CheckBox m_AlwaysOnTop;

	private CheckBox m_GetPrices;

	private Label m_LabelPriceProvider;

	private ComboBox m_PriceProvider;

	private Label m_LabelJaniceApiKey;

	private TextBox m_JaniceApiKey;

	private CheckBox m_Highlight;

	private CheckBox m_ActivateOnFitUpdate;

	private Label m_LabelCatalog;

	private Label m_CatalogStatus;

	private Button m_ButtonUpdateSde;

	private Label m_UpdateProgress;

	public SettingsDialog(Action onCatalogUpdated = null)
	{
		_onCatalogUpdated = onCatalogUpdated;
		InitializeComponent();
	}

	private void SettingsDialog_Load(object sender, EventArgs e)
	{
		m_AlwaysOnTop.Checked = ConfigHelper.Instance.AlwaysOnTop;
		m_GetPrices.Checked = ConfigHelper.Instance.GetPrices;
		m_Highlight.Checked = ConfigHelper.Instance.Highlight;
		m_ActivateOnFitUpdate.Checked = ConfigHelper.Instance.ActivateOnFitUpdate;
		string priceProvider = ConfigHelper.Instance.PriceProvider;
		int num = m_PriceProvider.Items.IndexOf(priceProvider);
		m_PriceProvider.SelectedIndex = ((num >= 0) ? num : 0);
		m_JaniceApiKey.Text = ConfigHelper.Instance.JaniceApiKey;
		RefreshCatalogStatus();
	}

	private void RefreshCatalogStatus()
	{
		int activeBuildNumber = CatalogLoader.GetActiveBuildNumber();
		string value = (CatalogLoader.IsUsingUserCatalog() ? "user-updated" : "embedded");
		m_CatalogStatus.Text = ((activeBuildNumber > 0) ? $"Active build: {activeBuildNumber} ({value})" : "Active build: unknown");
	}

	private void m_ButtonOk_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void m_AlwaysOnTop_CheckedChanged(object sender, EventArgs e)
	{
		ConfigHelper.Instance.AlwaysOnTop = m_AlwaysOnTop.Checked;
	}

	private void m_GetPrices_CheckedChanged(object sender, EventArgs e)
	{
		ConfigHelper.Instance.GetPrices = m_GetPrices.Checked;
	}

	private void m_PriceProvider_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (m_PriceProvider.SelectedItem != null)
		{
			ConfigHelper.Instance.PriceProvider = m_PriceProvider.SelectedItem.ToString();
		}
	}

	private void m_JaniceApiKey_TextChanged(object sender, EventArgs e)
	{
		ConfigHelper.Instance.JaniceApiKey = m_JaniceApiKey.Text;
	}

	private void m_Highlight_CheckedChanged(object sender, EventArgs e)
	{
		ConfigHelper.Instance.Highlight = m_Highlight.Checked;
	}

	private void m_ActivateOnFitUpdate_CheckedChanged(object sender, EventArgs e)
	{
		ConfigHelper.Instance.ActivateOnFitUpdate = m_ActivateOnFitUpdate.Checked;
	}

	private async void m_ButtonUpdateSde_Click(object sender, EventArgs e)
	{
		if (_updateCts != null)
		{
			return;
		}
		DialogResult confirm = MessageBox.Show(this, "Download the latest EVE static data and rebuild the ship/module catalog?\n\nThis may take a few minutes. The current fit will be cleared when the update finishes.", "Update catalog", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
		if (confirm != DialogResult.Yes)
		{
			return;
		}
		_updateCts = new CancellationTokenSource();
		m_ButtonUpdateSde.Enabled = false;
		m_ButtonOk.Enabled = false;
		m_UpdateProgress.Text = "Starting…";
		Progress<string> progress = new Progress<string>(delegate(string msg)
		{
			if (!base.IsDisposed)
			{
				m_UpdateProgress.Text = ShortenStatus(msg);
			}
		});
		try
		{
			CatalogUpdateResult result = await CatalogUpdater.UpdateAsync(CatalogPaths.SdeCacheDir, CatalogPaths.CatalogMsgpack, CatalogPaths.CatalogMeta, skipDownload: false, progress, _updateCts.Token).ConfigureAwait(continueOnCapturedContext: true);
			CatalogLoader.UserCatalogPath = CatalogPaths.CatalogMsgpack;
			_onCatalogUpdated?.Invoke();
			RefreshCatalogStatus();
			m_UpdateProgress.Text = $"Updated to build {result.BuildNumber} ({result.ShipCount} ships, {result.ModuleCount} modules).";
			MessageBox.Show(this, $"Catalog updated to SDE build {result.BuildNumber}.", "Update complete", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		catch (OperationCanceledException)
		{
			m_UpdateProgress.Text = "Update cancelled.";
		}
		catch (Exception ex2)
		{
			Exception ex3 = ex2;
			m_UpdateProgress.Text = "Update failed.";
			MessageBox.Show(this, "Catalog update failed:\n\n" + ex3.Message, "Update failed", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			_updateCts.Dispose();
			_updateCts = null;
			if (!base.IsDisposed)
			{
				m_ButtonUpdateSde.Enabled = true;
				m_ButtonOk.Enabled = true;
			}
		}
	}

	private static string ShortenStatus(string message)
	{
		if (string.IsNullOrEmpty(message))
		{
			return "";
		}
		if (message.StartsWith("Downloading SDE from ", StringComparison.Ordinal))
		{
			return "Downloading latest SDE…";
		}
		if (message.StartsWith("Saved ", StringComparison.Ordinal) && message.Contains("sde-latest"))
		{
			return "Download complete; extracting…";
		}
		if (message.Length > 120)
		{
			return message.Substring(0, 117) + "…";
		}
		return message;
	}

	protected override void OnFormClosing(FormClosingEventArgs e)
	{
		if (_updateCts != null)
		{
			e.Cancel = true;
			MessageBox.Show(this, "Please wait for the catalog update to finish.", "Update in progress", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		else
		{
			base.OnFormClosing(e);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.m_ButtonOk = new System.Windows.Forms.Button();
		this.m_AlwaysOnTop = new System.Windows.Forms.CheckBox();
		this.m_GetPrices = new System.Windows.Forms.CheckBox();
		this.m_LabelPriceProvider = new System.Windows.Forms.Label();
		this.m_PriceProvider = new System.Windows.Forms.ComboBox();
		this.m_LabelJaniceApiKey = new System.Windows.Forms.Label();
		this.m_JaniceApiKey = new System.Windows.Forms.TextBox();
		this.m_Highlight = new System.Windows.Forms.CheckBox();
		this.m_ActivateOnFitUpdate = new System.Windows.Forms.CheckBox();
		this.m_LabelCatalog = new System.Windows.Forms.Label();
		this.m_CatalogStatus = new System.Windows.Forms.Label();
		this.m_ButtonUpdateSde = new System.Windows.Forms.Button();
		this.m_UpdateProgress = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.m_AlwaysOnTop.AutoSize = true;
		this.m_AlwaysOnTop.Location = new System.Drawing.Point(20, 16);
		this.m_AlwaysOnTop.Name = "m_AlwaysOnTop";
		this.m_AlwaysOnTop.Size = new System.Drawing.Size(127, 17);
		this.m_AlwaysOnTop.TabIndex = 1;
		this.m_AlwaysOnTop.Text = "Toggle always on top";
		this.m_AlwaysOnTop.UseVisualStyleBackColor = true;
		this.m_AlwaysOnTop.CheckedChanged += new System.EventHandler(m_AlwaysOnTop_CheckedChanged);
		this.m_GetPrices.AutoSize = true;
		this.m_GetPrices.Checked = true;
		this.m_GetPrices.CheckState = System.Windows.Forms.CheckState.Checked;
		this.m_GetPrices.Location = new System.Drawing.Point(20, 44);
		this.m_GetPrices.Name = "m_GetPrices";
		this.m_GetPrices.Size = new System.Drawing.Size(74, 17);
		this.m_GetPrices.TabIndex = 2;
		this.m_GetPrices.Text = "Get prices";
		this.m_GetPrices.UseVisualStyleBackColor = true;
		this.m_GetPrices.CheckedChanged += new System.EventHandler(m_GetPrices_CheckedChanged);
		this.m_LabelPriceProvider.AutoSize = true;
		this.m_LabelPriceProvider.Location = new System.Drawing.Point(20, 76);
		this.m_LabelPriceProvider.Name = "m_LabelPriceProvider";
		this.m_LabelPriceProvider.Size = new System.Drawing.Size(74, 13);
		this.m_LabelPriceProvider.TabIndex = 3;
		this.m_LabelPriceProvider.Text = "Price provider";
		this.m_PriceProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.m_PriceProvider.FormattingEnabled = true;
		this.m_PriceProvider.Items.AddRange(new object[3] { "Goonpraisal", "Fuzzwork", "Janice" });
		this.m_PriceProvider.Location = new System.Drawing.Point(20, 92);
		this.m_PriceProvider.Name = "m_PriceProvider";
		this.m_PriceProvider.Size = new System.Drawing.Size(320, 21);
		this.m_PriceProvider.TabIndex = 4;
		this.m_PriceProvider.SelectedIndexChanged += new System.EventHandler(m_PriceProvider_SelectedIndexChanged);
		this.m_LabelJaniceApiKey.AutoSize = true;
		this.m_LabelJaniceApiKey.Location = new System.Drawing.Point(20, 124);
		this.m_LabelJaniceApiKey.Name = "m_LabelJaniceApiKey";
		this.m_LabelJaniceApiKey.Size = new System.Drawing.Size(200, 13);
		this.m_LabelJaniceApiKey.TabIndex = 5;
		this.m_LabelJaniceApiKey.Text = "Janice API key (required for Janice)";
		this.m_JaniceApiKey.Location = new System.Drawing.Point(20, 140);
		this.m_JaniceApiKey.Name = "m_JaniceApiKey";
		this.m_JaniceApiKey.Size = new System.Drawing.Size(320, 20);
		this.m_JaniceApiKey.TabIndex = 6;
		this.m_JaniceApiKey.UseSystemPasswordChar = true;
		this.m_JaniceApiKey.TextChanged += new System.EventHandler(m_JaniceApiKey_TextChanged);
		this.m_Highlight.AutoSize = true;
		this.m_Highlight.Checked = true;
		this.m_Highlight.CheckState = System.Windows.Forms.CheckState.Checked;
		this.m_Highlight.Location = new System.Drawing.Point(20, 176);
		this.m_Highlight.Name = "m_Highlight";
		this.m_Highlight.Size = new System.Drawing.Size(126, 17);
		this.m_Highlight.TabIndex = 7;
		this.m_Highlight.Text = "Highlight full tank / fit";
		this.m_Highlight.UseVisualStyleBackColor = true;
		this.m_Highlight.CheckedChanged += new System.EventHandler(m_Highlight_CheckedChanged);
		this.m_ActivateOnFitUpdate.AutoSize = true;
		this.m_ActivateOnFitUpdate.Checked = true;
		this.m_ActivateOnFitUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
		this.m_ActivateOnFitUpdate.Location = new System.Drawing.Point(20, 204);
		this.m_ActivateOnFitUpdate.Name = "m_ActivateOnFitUpdate";
		this.m_ActivateOnFitUpdate.Size = new System.Drawing.Size(166, 17);
		this.m_ActivateOnFitUpdate.TabIndex = 8;
		this.m_ActivateOnFitUpdate.Text = "Activate window on fit update";
		this.m_ActivateOnFitUpdate.UseVisualStyleBackColor = true;
		this.m_ActivateOnFitUpdate.CheckedChanged += new System.EventHandler(m_ActivateOnFitUpdate_CheckedChanged);
		this.m_LabelCatalog.AutoSize = true;
		this.m_LabelCatalog.Location = new System.Drawing.Point(20, 240);
		this.m_LabelCatalog.Name = "m_LabelCatalog";
		this.m_LabelCatalog.Size = new System.Drawing.Size(120, 13);
		this.m_LabelCatalog.TabIndex = 9;
		this.m_LabelCatalog.Text = "Ship / module catalog";
		this.m_CatalogStatus.AutoEllipsis = true;
		this.m_CatalogStatus.Location = new System.Drawing.Point(20, 258);
		this.m_CatalogStatus.Name = "m_CatalogStatus";
		this.m_CatalogStatus.Size = new System.Drawing.Size(320, 18);
		this.m_CatalogStatus.TabIndex = 10;
		this.m_CatalogStatus.Text = "Build: …";
		this.m_ButtonUpdateSde.Location = new System.Drawing.Point(20, 284);
		this.m_ButtonUpdateSde.Name = "m_ButtonUpdateSde";
		this.m_ButtonUpdateSde.Size = new System.Drawing.Size(320, 28);
		this.m_ButtonUpdateSde.TabIndex = 11;
		this.m_ButtonUpdateSde.Text = "Download latest SDE / update catalog";
		this.m_ButtonUpdateSde.UseVisualStyleBackColor = true;
		this.m_ButtonUpdateSde.Click += new System.EventHandler(m_ButtonUpdateSde_Click);
		this.m_UpdateProgress.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.m_UpdateProgress.Location = new System.Drawing.Point(20, 320);
		this.m_UpdateProgress.Name = "m_UpdateProgress";
		this.m_UpdateProgress.Size = new System.Drawing.Size(320, 56);
		this.m_UpdateProgress.TabIndex = 12;
		this.m_UpdateProgress.Text = "";
		this.m_ButtonOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
		this.m_ButtonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
		this.m_ButtonOk.Location = new System.Drawing.Point(140, 396);
		this.m_ButtonOk.Name = "m_ButtonOk";
		this.m_ButtonOk.Size = new System.Drawing.Size(80, 28);
		this.m_ButtonOk.TabIndex = 0;
		this.m_ButtonOk.Text = "Ok";
		this.m_ButtonOk.UseVisualStyleBackColor = true;
		this.m_ButtonOk.Click += new System.EventHandler(m_ButtonOk_Click);
		base.AcceptButton = this.m_ButtonOk;
		base.AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		base.ClientSize = new System.Drawing.Size(360, 440);
		base.ControlBox = false;
		base.Controls.Add(this.m_UpdateProgress);
		base.Controls.Add(this.m_ButtonUpdateSde);
		base.Controls.Add(this.m_CatalogStatus);
		base.Controls.Add(this.m_LabelCatalog);
		base.Controls.Add(this.m_ActivateOnFitUpdate);
		base.Controls.Add(this.m_Highlight);
		base.Controls.Add(this.m_JaniceApiKey);
		base.Controls.Add(this.m_LabelJaniceApiKey);
		base.Controls.Add(this.m_PriceProvider);
		base.Controls.Add(this.m_LabelPriceProvider);
		base.Controls.Add(this.m_GetPrices);
		base.Controls.Add(this.m_AlwaysOnTop);
		base.Controls.Add(this.m_ButtonOk);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(376, 479);
		base.Name = "SettingsDialog";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Settings";
		base.Load += new System.EventHandler(SettingsDialog_Load);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
