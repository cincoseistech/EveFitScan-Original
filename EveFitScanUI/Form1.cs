using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using EveFitScan.Core;
using EveFitScan.Core.Catalog;
using EveFitScanUI.Pricing;
using HtmlAgilityPack;

namespace EveFitScanUI;

public class Form1 : Form
{
	private static int m_coldLeft = 528;

	private static int m_coldLeftSTK = m_coldLeft - 55;

	private static int m_hotLeft = 768;

	private static int m_hotLeftSTK = m_hotLeft - 140;

	private const string m_DownloadPageURL = "https://bitbucket.org/Donna_Hale_Eve/fitscan_eve/downloads/";

	private const int WMCLIPBOARDUPDATE = 797;

	private bool m_bFirstFire = true;

	private string m_LastCopy = string.Empty;

	private FitScanProcessor m_FitScanProcessor = null;

	private HistoryManager m_HistoryManager = null;

	private GankShips m_gankShips = null;

	private bool m_bCaptureClipboard = true;

	private IContainer components = null;

	private MenuStrip menuStrip1;

	private ToolStripMenuItem fileToolStripMenuItem;

	private ToolStripMenuItem exitToolStripMenuItem;

	private ToolStripMenuItem helpToolStripMenuItem;

	private ToolStripMenuItem aboutToolStripMenuItem;

	private ToolStripMenuItem licenseToolStripMenuItem;

	private ToolStripMenuItem sourceToolStripMenuItem;

	private Button m_ButtonResetFit;

	private ComboBox m_ComboBoxShipType;

	private Button m_ButtonCopyEFT;

	private RichTextBox m_TextBoxShieldHP;

	private Label label1;

	private Label label2;

	private Label label3;

	private RichTextBox m_TextBoxArmorHP;

	private RichTextBox m_TextBoxHullHP;

	private RichTextBox m_TextBoxShieldResistsCold;

	private RichTextBox m_TextBoxShieldResistsHot;

	private RichTextBox m_TextBoxArmorResistsCold;

	private RichTextBox m_TextBoxArmorResistsHot;

	private RichTextBox m_TextBoxHullResistsCold;

	private RichTextBox m_TextBoxHullResistsHot;

	private Label label4;

	private Label label5;

	private RichTextBox m_TextBoxEHPMjolnirCold;

	private RichTextBox m_TextBoxEHPMjolnirHot;

	private Label label6;

	private Label label7;

	private RichTextBox m_TextBoxEHPNovaHot;

	private RichTextBox m_TextBoxEHPNovaCold;

	private Label label8;

	private RichTextBox m_TextBoxEHPAntimatterHot;

	private RichTextBox m_TextBoxEHPAntimatterCold;

	private Label label9;

	private RichTextBox m_TextBoxEHPVoidHot;

	private RichTextBox m_TextBoxEHPVoidCold;

	private Label label10;

	private RichTextBox m_TextBoxEHPMultifreqHot;

	private RichTextBox m_TextBoxEHPMultifreqCold;

	private Label label11;

	private RichTextBox m_TextBoxEHPEMPHot;

	private RichTextBox m_TextBoxEHPEMPCold;

	private Label label12;

	private RichTextBox m_TextBoxEHPFusionHot;

	private RichTextBox m_TextBoxEHPFusionCold;

	private Label label13;

	private RichTextBox m_TextBoxEHPPhasedPlasmaHot;

	private RichTextBox m_TextBoxEHPPhasedPlasmaCold;

	private Label label14;

	private RichTextBox m_TextBoxEHPHailHot;

	private RichTextBox m_TextBoxEHPHailCold;

	private RichTextBox m_FitText;

	private CheckBox m_checkBoxPassive;

	private BackgroundWorker m_BackgroundWorkerPrices;

	private RichTextBox m_ValueHullText;

	private RichTextBox m_ValueFittingsText;

	private RichTextBox m_ValueTotalText;

	private RichTextBox m_ValueCanDropText;

	private Label label15;

	private Label label16;

	private Label label17;

	private Label label18;

	private BackgroundWorker m_BackgroundWorkerUpdate;

	private CheckBox m_checkBoxADCActive;

	private ToolStripMenuItem settingsToolStripMenuItem;

	private ComboBox m_History;

	private CheckBox m_checkBoxSTK;

	private ComboBox m_comboBoxSysSecurity;

	private Label labelSysSecurity;

	private Label labelDPS;

	private Label labelRoF;

	private Label labelSTK;

	private ToolStripMenuItem resetDPSRoFToolStripMenuItem;

	private RichTextBox m_textBox_DPS_Mjolnir;

	private RichTextBox m_textBox_RoF_Mjolnir;

	private RichTextBox m_textBox_DPS_Nova;

	private RichTextBox m_textBox_DPS_Antimatter;

	private RichTextBox m_textBox_DPS_Void;

	private RichTextBox m_textBox_DPS_Multifrequency;

	private RichTextBox m_textBox_DPS_EMP;

	private RichTextBox m_textBox_DPS_Fusion;

	private RichTextBox m_textBox_DPS_Phased_Plasma;

	private RichTextBox m_textBox_DPS_Hail;

	private RichTextBox m_textBox_RoF_Nova;

	private RichTextBox m_textBox_RoF_Antimatter;

	private RichTextBox m_textBox_RoF_Void;

	private RichTextBox m_textBox_RoF_Multifrequency;

	private RichTextBox m_textBox_RoF_EMP;

	private RichTextBox m_textBox_RoF_Fusion;

	private RichTextBox m_textBox_RoF_Phased_Plasma;

	private RichTextBox m_textBox_RoF_Hail;

	private RadioButton m_radioPassive;

	private RadioButton m_radioCold;

	private RadioButton m_radioHot;

	private ToolStripMenuItem resetDPSRoFcanFlyThenAll4ToolStripMenuItem;

	private Panel panel1;

	private Panel panel2;

	private Panel panel3;

	private RichTextBox m_textBox_RoF_VoidL;

	private RichTextBox m_textBox_DPS_VoidL;

	private Label label19;

	private RichTextBox m_TextBoxEHPVoidLHot;

	private RichTextBox m_TextBoxEHPVoidLCold;

	private Panel panel4;

	private Label labelSeconds;

	private ToolTip toolTip1;

	private Label labelBR;

	private Button button1;

	private Button button2;

	private CheckBox m_checkBoxManualEHP;

	private RichTextBox m_richTextBoxManualEHP;

	private bool m_InsideUpdate = false;

	private bool m_bIgnoreIndexChanges = false;

	private bool m_bInsideIndexChange = false;

	private Mutex m_Guard = new Mutex();

	private Dictionary<string, int> m_ItemsWithUnknownPrices = new Dictionary<string, int>();

	private List<string> m_ComboBoxItems = new List<string>();

	private BindingSource m_BindingSource = new BindingSource();

	private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
	{
		SettingsDialog settingsDialog = new SettingsDialog(delegate
		{
			if (m_FitScanProcessor != null)
			{
				m_FitScanProcessor.ReloadCatalog(m_checkBoxPassive.Checked, m_checkBoxADCActive.Checked);
				RefreshShipCombo();
			}
		});
		DialogResult dialogResult = settingsDialog.ShowDialog(this);
		settingsDialog.Dispose();
		if (dialogResult == DialogResult.OK)
		{
			base.TopMost = ConfigHelper.Instance.AlwaysOnTop;
		}
		HighlightFit();
	}

	private void m_checkBoxPassive_CheckedChanged(object sender, EventArgs e)
	{
		ConfigHelper.Instance.PassiveTank = m_checkBoxPassive.Checked;
		m_FitScanProcessor.SetPassive(m_checkBoxPassive.Checked);
		if (m_checkBoxPassive.Checked)
		{
			m_checkBoxADCActive.Enabled = false;
		}
		else
		{
			m_checkBoxADCActive.Enabled = true;
		}
	}

	private void m_checkBoxSTK_CheckedChanged(object sender, EventArgs e)
	{
		ConfigHelper.Instance.STK = m_checkBoxSTK.Checked;
		m_FitScanProcessor.SetSTK(m_checkBoxSTK.Checked);
		ConfigSTKDisplay();
	}

	private void ConfigSTKDisplay()
	{
		if (m_checkBoxSTK.Checked)
		{
			label10.Text = "Multifreq.";
			label13.Text = "Phased Pl.";
			m_TextBoxEHPAntimatterHot.Left = m_hotLeftSTK;
			m_TextBoxEHPEMPHot.Left = m_hotLeftSTK;
			m_TextBoxEHPFusionHot.Left = m_hotLeftSTK;
			m_TextBoxEHPHailHot.Left = m_hotLeftSTK;
			m_TextBoxEHPMjolnirHot.Left = m_hotLeftSTK;
			m_TextBoxEHPMultifreqHot.Left = m_hotLeftSTK;
			m_TextBoxEHPNovaHot.Left = m_hotLeftSTK;
			m_TextBoxEHPPhasedPlasmaHot.Left = m_hotLeftSTK;
			m_TextBoxEHPVoidHot.Left = m_hotLeftSTK;
			m_TextBoxEHPVoidLHot.Left = m_hotLeftSTK;
			m_TextBoxEHPVoidLHot.Visible = true;
			m_TextBoxEHPAntimatterCold.Left = m_coldLeftSTK;
			m_TextBoxEHPEMPCold.Left = m_coldLeftSTK;
			m_TextBoxEHPFusionCold.Left = m_coldLeftSTK;
			m_TextBoxEHPHailCold.Left = m_coldLeftSTK;
			m_TextBoxEHPMjolnirCold.Left = m_coldLeftSTK;
			m_TextBoxEHPMultifreqCold.Left = m_coldLeftSTK;
			m_TextBoxEHPNovaCold.Left = m_coldLeftSTK;
			m_TextBoxEHPPhasedPlasmaCold.Left = m_coldLeftSTK;
			m_TextBoxEHPVoidCold.Left = m_coldLeftSTK;
			m_TextBoxEHPVoidLCold.Left = m_coldLeftSTK;
			m_TextBoxEHPVoidLCold.Visible = true;
			m_comboBoxSysSecurity.Visible = true;
			labelSysSecurity.Visible = true;
			labelDPS.Visible = true;
			labelRoF.Visible = true;
			labelSTK.Visible = true;
			label19.Visible = true;
			panel4.Visible = true;
			labelSeconds.Visible = true;
			m_textBox_DPS_Mjolnir.Visible = true;
			m_textBox_DPS_Nova.Visible = true;
			m_textBox_DPS_Antimatter.Visible = true;
			m_textBox_DPS_Void.Visible = true;
			m_textBox_DPS_VoidL.Visible = true;
			m_textBox_DPS_Multifrequency.Visible = true;
			m_textBox_DPS_EMP.Visible = true;
			m_textBox_DPS_Fusion.Visible = true;
			m_textBox_DPS_Phased_Plasma.Visible = true;
			m_textBox_DPS_Hail.Visible = true;
			m_textBox_RoF_Mjolnir.Visible = true;
			m_textBox_RoF_Nova.Visible = true;
			m_textBox_RoF_Antimatter.Visible = true;
			m_textBox_RoF_Void.Visible = true;
			m_textBox_RoF_VoidL.Visible = true;
			m_textBox_RoF_Multifrequency.Visible = true;
			m_textBox_RoF_EMP.Visible = true;
			m_textBox_RoF_Fusion.Visible = true;
			m_textBox_RoF_Phased_Plasma.Visible = true;
			m_textBox_RoF_Hail.Visible = true;
			m_checkBoxPassive.Visible = false;
			m_radioCold.Visible = true;
			m_radioHot.Visible = true;
			m_radioPassive.Visible = true;
			m_checkBoxManualEHP.Visible = true;
			m_richTextBoxManualEHP.Visible = true;
			m_TextBoxArmorResistsHot.Visible = false;
			m_TextBoxHullResistsHot.Visible = false;
			m_TextBoxShieldResistsHot.Visible = false;
			label5.Visible = false;
			label4.Visible = false;
			if (m_FitScanProcessor.PassiveTank)
			{
				m_radioPassive.Checked = true;
			}
			else if (m_radioPassive.Checked)
			{
				m_radioCold.Checked = true;
			}
		}
		else
		{
			label10.Text = "Multifrequency";
			label13.Text = "Phased Plasma";
			m_TextBoxEHPAntimatterHot.Left = m_hotLeft;
			m_TextBoxEHPEMPHot.Left = m_hotLeft;
			m_TextBoxEHPFusionHot.Left = m_hotLeft;
			m_TextBoxEHPHailHot.Left = m_hotLeft;
			m_TextBoxEHPMjolnirHot.Left = m_hotLeft;
			m_TextBoxEHPMultifreqHot.Left = m_hotLeft;
			m_TextBoxEHPNovaHot.Left = m_hotLeft;
			m_TextBoxEHPPhasedPlasmaHot.Left = m_hotLeft;
			m_TextBoxEHPVoidHot.Left = m_hotLeft;
			m_TextBoxEHPVoidLHot.Left = m_hotLeft;
			m_TextBoxEHPVoidLHot.Visible = false;
			m_TextBoxEHPAntimatterCold.Left = m_coldLeft;
			m_TextBoxEHPEMPCold.Left = m_coldLeft;
			m_TextBoxEHPFusionCold.Left = m_coldLeft;
			m_TextBoxEHPHailCold.Left = m_coldLeft;
			m_TextBoxEHPMjolnirCold.Left = m_coldLeft;
			m_TextBoxEHPMultifreqCold.Left = m_coldLeft;
			m_TextBoxEHPNovaCold.Left = m_coldLeft;
			m_TextBoxEHPPhasedPlasmaCold.Left = m_coldLeft;
			m_TextBoxEHPVoidCold.Left = m_coldLeft;
			m_TextBoxEHPVoidLCold.Left = m_coldLeft;
			m_TextBoxEHPVoidLCold.Visible = false;
			m_comboBoxSysSecurity.Visible = false;
			labelSysSecurity.Visible = false;
			labelDPS.Visible = false;
			labelRoF.Visible = false;
			labelSTK.Visible = false;
			label19.Visible = false;
			panel4.Visible = false;
			labelSeconds.Visible = false;
			m_textBox_DPS_Mjolnir.Visible = false;
			m_textBox_DPS_Nova.Visible = false;
			m_textBox_DPS_Antimatter.Visible = false;
			m_textBox_DPS_Void.Visible = false;
			m_textBox_DPS_VoidL.Visible = false;
			m_textBox_DPS_Multifrequency.Visible = false;
			m_textBox_DPS_EMP.Visible = false;
			m_textBox_DPS_Fusion.Visible = false;
			m_textBox_DPS_Phased_Plasma.Visible = false;
			m_textBox_DPS_Hail.Visible = false;
			m_textBox_RoF_Mjolnir.Visible = false;
			m_textBox_RoF_Nova.Visible = false;
			m_textBox_RoF_Antimatter.Visible = false;
			m_textBox_RoF_Void.Visible = false;
			m_textBox_RoF_VoidL.Visible = false;
			m_textBox_RoF_Multifrequency.Visible = false;
			m_textBox_RoF_EMP.Visible = false;
			m_textBox_RoF_Fusion.Visible = false;
			m_textBox_RoF_Phased_Plasma.Visible = false;
			m_textBox_RoF_Hail.Visible = false;
			m_checkBoxPassive.Visible = true;
			m_radioCold.Visible = false;
			m_radioHot.Visible = false;
			m_radioPassive.Visible = false;
			m_checkBoxManualEHP.Visible = false;
			m_richTextBoxManualEHP.Visible = false;
			m_TextBoxArmorResistsHot.Visible = true;
			m_TextBoxHullResistsHot.Visible = true;
			m_TextBoxShieldResistsHot.Visible = true;
			label5.Visible = true;
			label4.Visible = true;
			m_checkBoxPassive.Checked = m_FitScanProcessor.PassiveTank;
		}
	}

	private void m_checkBoxADCActive_CheckedChanged(object sender, EventArgs e)
	{
		ConfigHelper.Instance.ADCActive = m_checkBoxADCActive.Checked;
		m_FitScanProcessor.SetADCActive(m_checkBoxADCActive.Checked);
	}

	private void exitToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void resetDPSRoFToolStripMenuItem_Click(object sender, EventArgs e)
	{
		ConfigHelper.Instance.ResetDpsRoF();
		Load_DPS_RoF();
		m_FitScanProcessor.SetSTK(bSTK: true);
	}

	private void resetDPSRoFcanFlyThenAll4ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		ConfigHelper.Instance.ResetDpsRoFScrub();
		Load_DPS_RoF();
		m_FitScanProcessor.SetSTK(bSTK: true);
	}

	private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
	{
		string text = "EveFitScan Version " + Assembly.GetEntryAssembly().GetName().Version.ToString() + "\n2017 Donna Hale <donna.hale.eve@gmail.com>\nShips to Kill mod (c) 2019 Vulkyn\n\nComments/Suggestions/Complaints can be posted in the appropriate \nthread on the Goonfleet Forums or sent to me via Jabber.";
		MessageBox.Show(text, "About", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
	}

	private void licenseToolStripMenuItem_Click(object sender, EventArgs e)
	{
		string text = "EveFitScan (C) 2017 Donna Hale <donna.hale.eve@gmail.com>\n\nShips to Kill mod by Vulkyn (C) 2019.\n\nEVE Online and the EVE logo are the registered trademarks of CCP hf.\nAll rights are reserved worldwide. All other trademarks are the\nproperty of their respective owners. EVE Online, the EVE logo, EVE\nand all associated logos and designs are the intellectual property\nof CCP hf. All artwork, screenshots, characters, vehicles,\nstorylines, world facts or other recognizable features of the\nintellectual property relating to these trademarks are likewise the\nintellectual property of CCP hf. CCP hf. does not endorse, and is\nnot in any way affiliated with, this software. CCP is in no way\nresponsible for the content or functioning of this software, nor\ncan it be liable for any damage arising from the use of this software.\n\nA non-exclusive, non-transferrable, limited time license to use this\nsoftware and associated source code, has been granted to you as a \nmember of one of the following organizations or its allies:\n\n* Goonswarm\n* Miniluv\n\nThis license exists only for as long as you continue to be a member\nof one of these groups or its allies, and will terminate at the time\nyou are no longer a member of those entities and/or its allies.\n\nThis license may also be terminated at any time for any and/or no\nreason by any or all of the copyright holders.\n\nTHE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY\nKIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE\nWARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR\nPURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE\nAUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,\nDAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF\nCONTRACT, TORT OR  OTHERWISE, ARISING FROM, OUT OF OR IN\nCONNECTION WITH THE SOFTWARE OR THE USE OR OTHER\nDEALINGS IN THE SOFTWARE.";
		MessageBox.Show(text, "License", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
	}

	private void sourceToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Process.Start("https://bitbucket.org/Donna_Hale_Eve/fitscan_eve/overview");
	}

	private void m_ButtonResetFit_Click(object sender, EventArgs e)
	{
		m_FitScanProcessor.ResetFit(m_checkBoxPassive.Checked, m_checkBoxADCActive.Checked);
	}

	private void m_ButtonCopyEFT_Click(object sender, EventArgs e)
	{
		bool bCaptureClipboard = m_bCaptureClipboard;
		m_bCaptureClipboard = false;
		Clipboard.SetText(m_FitScanProcessor.EFTFit);
		m_bCaptureClipboard = bCaptureClipboard;
	}

	private void BackgroundWorkerUpdate_DoWork(object sender, DoWorkEventArgs e)
	{
		HtmlWeb htmlWeb = new HtmlWeb
		{
			UsingCache = false
		};
		HtmlAgilityPack.HtmlDocument htmlDocument = htmlWeb.Load("https://bitbucket.org/Donna_Hale_Eve/fitscan_eve/downloads/");
		HtmlNodeCollection htmlNodeCollection = htmlDocument.DocumentNode.SelectNodes("//table[@id='uploaded-files']//tr//td[@class='name']/a");
		Regex regex = new Regex("EveFitScan_build_(\\d+)\\.(\\d+)\\.(\\d+)\\.(\\d+)\\.zip", RegexOptions.IgnoreCase);
		List<Version> list = new List<Version>();
		foreach (HtmlNode item in (IEnumerable<HtmlNode>)htmlNodeCollection)
		{
			string innerText = item.InnerText;
			Match match = regex.Match(innerText);
			if (!match.Success || match.Groups.Count != 5)
			{
				continue;
			}
			List<int> list2 = new List<int>();
			for (int i = 1; i <= 4; i++)
			{
				string s = match.Groups[i].ToString();
				int result = 0;
				if (int.TryParse(s, out result) && result >= 0)
				{
					list2.Add(result);
				}
			}
			if (list2.Count == 4)
			{
				list.Add(new Version(list2[0], list2[1], list2[2], list2[3]));
			}
		}
		if (list.Count > 0)
		{
			list.Sort();
			e.Result = list[list.Count - 1];
		}
		else
		{
			e.Result = new Version(0, 0, 0, 0);
		}
	}

	private void BackgroundWorkerUpdate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		if (e.Cancelled || e.Error != null)
		{
			return;
		}
		string text = e.Result.GetType().ToString();
		Version version = (Version)e.Result;
		Version version2 = Assembly.GetEntryAssembly().GetName().Version;
		if (version > version2)
		{
			string text2 = "You are currently running version " + version2?.ToString() + "." + Environment.NewLine + Environment.NewLine + "However, there is a newer version available: " + version?.ToString() + "." + Environment.NewLine + Environment.NewLine + "Would you like to download it now?";
			DialogResult dialogResult = MessageBox.Show(text2, "Newer version available", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
			if (dialogResult == DialogResult.Yes)
			{
				Process.Start("https://bitbucket.org/Donna_Hale_Eve/fitscan_eve/downloads/");
				Close();
			}
		}
	}

	public Form1()
	{
		InitializeComponent();
		Rectangle virtualScreen = SystemInformation.VirtualScreen;
		if (ConfigHelper.Instance.WindowPositionX < virtualScreen.X || ConfigHelper.Instance.WindowPositionX >= virtualScreen.Width || ConfigHelper.Instance.WindowPositionY < virtualScreen.Y || ConfigHelper.Instance.WindowPositionY >= virtualScreen.Height)
		{
			base.StartPosition = FormStartPosition.CenterScreen;
			return;
		}
		base.StartPosition = FormStartPosition.Manual;
		base.Location = new Point(ConfigHelper.Instance.WindowPositionX, ConfigHelper.Instance.WindowPositionY);
	}

	protected override void WndProc(ref Message m)
	{
		base.WndProc(ref m);
		if (m.Msg != 797)
		{
			return;
		}
		if (m_bFirstFire)
		{
			m_bFirstFire = false;
		}
		else if (m_bCaptureClipboard && m_FitScanProcessor != null && Clipboard.ContainsText())
		{
			string text = Clipboard.GetText();
			if (!string.IsNullOrEmpty(text) && !(text == m_LastCopy))
			{
				m_LastCopy = text;
				m_FitScanProcessor.NewPaste(text, m_checkBoxPassive.Checked, m_checkBoxADCActive.Checked);
			}
		}
	}

	private void Form1_Load(object sender, EventArgs e)
	{
		if (ConfigHelper.Instance.AlwaysOnTop)
		{
			base.TopMost = true;
		}
		NativeMethods.AddClipboardFormatListener(base.Handle);
		CatalogLoader.UserCatalogPath = CatalogPaths.CatalogMsgpack;
		m_FitScanProcessor = new FitScanProcessor();
		m_FitScanProcessor.EventShipFitChanged += OnShipFitChanged;
		m_FitScanProcessor.EventShipTankChanged += OnShipTankChanged;
		m_FitScanProcessor.EventNewItemsWithUnknownPrices += OnNewItemsWithUnknownPrices;
		m_FitScanProcessor.EventFitValueChanged += OnFitValueChanged;
		m_gankShips = new GankShips();
		ConfigHelper.Instance.RepairDpsRoF();
		m_HistoryManager = new HistoryManager();
		m_BindingSource = new BindingSource();
		RefreshShipCombo();
		m_checkBoxPassive.Checked = ConfigHelper.Instance.PassiveTank;
		m_FitScanProcessor.SetPassive(ConfigHelper.Instance.PassiveTank);
		m_comboBoxSysSecurity.SelectedIndex = ConfigHelper.Instance.SysSecurity;
		m_checkBoxSTK.Checked = ConfigHelper.Instance.STK;
		m_FitScanProcessor.SetSTK(ConfigHelper.Instance.STK);
		LoadPassiveColdHot();
		Load_DPS_RoF();
		ConfigSTKDisplay();
		m_BackgroundWorkerUpdate.RunWorkerAsync();
	}

	private void Form1_FormClosing(object sender, FormClosingEventArgs e)
	{
		ConfigHelper.Instance.WindowPositionX = base.Location.X;
		ConfigHelper.Instance.WindowPositionY = base.Location.Y;
		NativeMethods.RemoveClipboardFormatListener(base.Handle);
	}

	private void m_ComboBoxSysSecurity_SelectedIndexChanged(object sender, EventArgs e)
	{
		ConfigHelper.Instance.SysSecurity = m_comboBoxSysSecurity.SelectedIndex;
		labelSeconds.Text = m_gankShips.GetSeconds(m_comboBoxSysSecurity.Text);
		m_FitScanProcessor.SetSTK(bSTK: true);
	}

	private void LoadPassiveColdHot()
	{
		if (ConfigHelper.Instance.PassiveColdHot == "Passive")
		{
			m_radioPassive.Checked = true;
		}
		else if (ConfigHelper.Instance.PassiveColdHot == "Hot")
		{
			m_radioHot.Checked = true;
		}
		else
		{
			m_radioCold.Checked = true;
		}
		m_checkBoxManualEHP.Checked = ConfigHelper.Instance.Is_Manual_EHP;
		m_richTextBoxManualEHP.Enabled = m_checkBoxManualEHP.Checked;
		m_richTextBoxManualEHP.Text = ConfigHelper.Instance.Manual_EHP.ToString();
		m_radioCold.Enabled = !m_checkBoxManualEHP.Checked;
		m_radioHot.Enabled = !m_checkBoxManualEHP.Checked;
		m_radioPassive.Enabled = !m_checkBoxManualEHP.Checked;
		if (m_checkBoxManualEHP.Checked)
		{
			m_checkBoxADCActive.Enabled = false;
		}
	}

	private void m_textBox_DPS_Mjolnir_ValueChanged(object sender, EventArgs e)
	{
		int result = 0;
		if (int.TryParse(m_textBox_DPS_Mjolnir.Text, out result) && result != 0)
		{
			ConfigHelper.Instance.DPS_Mjolnir = result;
		}
		m_FitScanProcessor.SetSTK(bSTK: true);
	}

	private void m_textBox_DPS_Nova_ValueChanged(object sender, EventArgs e)
	{
		int result = 0;
		if (int.TryParse(m_textBox_DPS_Nova.Text, out result) && result != 0)
		{
			ConfigHelper.Instance.DPS_Nova = result;
		}
		m_FitScanProcessor.SetSTK(bSTK: true);
	}

	private void m_textBox_DPS_Antimatter_ValueChanged(object sender, EventArgs e)
	{
		int result = 0;
		if (int.TryParse(m_textBox_DPS_Antimatter.Text, out result) && result != 0)
		{
			ConfigHelper.Instance.DPS_Antimatter = result;
		}
		m_FitScanProcessor.SetSTK(bSTK: true);
	}

	private void m_textBox_DPS_Void_ValueChanged(object sender, EventArgs e)
	{
		int result = 0;
		if (int.TryParse(m_textBox_DPS_Void.Text, out result) && result != 0)
		{
			ConfigHelper.Instance.DPS_Void = result;
		}
		m_FitScanProcessor.SetSTK(bSTK: true);
	}

	private void m_textBox_DPS_VoidL_ValueChanged(object sender, EventArgs e)
	{
		int result = 0;
		if (int.TryParse(m_textBox_DPS_VoidL.Text, out result) && result != 0)
		{
			ConfigHelper.Instance.DPS_VoidL = result;
		}
		m_FitScanProcessor.SetSTK(bSTK: true);
	}

	private void m_textBox_DPS_Multifrequency_ValueChanged(object sender, EventArgs e)
	{
		int result = 0;
		if (int.TryParse(m_textBox_DPS_Multifrequency.Text, out result) && result != 0)
		{
			ConfigHelper.Instance.DPS_Multifrequency = result;
		}
		m_FitScanProcessor.SetSTK(bSTK: true);
	}

	private void m_textBox_DPS_EMP_ValueChanged(object sender, EventArgs e)
	{
		int result = 0;
		if (int.TryParse(m_textBox_DPS_EMP.Text, out result) && result != 0)
		{
			ConfigHelper.Instance.DPS_EMP = result;
		}
		m_FitScanProcessor.SetSTK(bSTK: true);
	}

	private void m_textBox_DPS_Fusion_ValueChanged(object sender, EventArgs e)
	{
		int result = 0;
		if (int.TryParse(m_textBox_DPS_Fusion.Text, out result) && result != 0)
		{
			ConfigHelper.Instance.DPS_Fusion = result;
		}
		m_FitScanProcessor.SetSTK(bSTK: true);
	}

	private void m_textBox_DPS_Phased_Plasma_ValueChanged(object sender, EventArgs e)
	{
		int result = 0;
		if (int.TryParse(m_textBox_DPS_Phased_Plasma.Text, out result) && result != 0)
		{
			ConfigHelper.Instance.DPS_Phased_Plasma = result;
		}
		m_FitScanProcessor.SetSTK(bSTK: true);
	}

	private void m_textBox_DPS_Hail_ValueChanged(object sender, EventArgs e)
	{
		int result = 0;
		if (int.TryParse(m_textBox_DPS_Hail.Text, out result) && result != 0)
		{
			ConfigHelper.Instance.DPS_Hail = result;
		}
		m_FitScanProcessor.SetSTK(bSTK: true);
	}

	private void m_textBox_RoF_Mjolnir_ValueChanged(object sender, EventArgs e)
	{
		double result = 0.0;
		if (double.TryParse(m_textBox_RoF_Mjolnir.Text, out result) && result != 0.0)
		{
			ConfigHelper.Instance.RoF_Mjolnir = result;
		}
		m_FitScanProcessor.SetSTK(bSTK: true);
	}

	private void m_textBox_RoF_Nova_ValueChanged(object sender, EventArgs e)
	{
		double result = 0.0;
		if (double.TryParse(m_textBox_RoF_Nova.Text, out result) && result != 0.0)
		{
			ConfigHelper.Instance.RoF_Nova = result;
		}
		m_FitScanProcessor.SetSTK(bSTK: true);
	}

	private void m_textBox_RoF_Antimatter_ValueChanged(object sender, EventArgs e)
	{
		double result = 0.0;
		if (double.TryParse(m_textBox_RoF_Antimatter.Text, out result) && result != 0.0)
		{
			ConfigHelper.Instance.RoF_Antimatter = result;
		}
		m_FitScanProcessor.SetSTK(bSTK: true);
	}

	private void m_textBox_RoF_Void_ValueChanged(object sender, EventArgs e)
	{
		double result = 0.0;
		if (double.TryParse(m_textBox_RoF_Void.Text, out result) && result != 0.0)
		{
			ConfigHelper.Instance.RoF_Void = result;
		}
		m_FitScanProcessor.SetSTK(bSTK: true);
	}

	private void m_textBox_RoF_VoidL_ValueChanged(object sender, EventArgs e)
	{
		double result = 0.0;
		if (double.TryParse(m_textBox_RoF_VoidL.Text, out result) && result != 0.0)
		{
			ConfigHelper.Instance.RoF_VoidL = result;
		}
		m_FitScanProcessor.SetSTK(bSTK: true);
	}

	private void m_textBox_RoF_Multifrequency_ValueChanged(object sender, EventArgs e)
	{
		double result = 0.0;
		if (double.TryParse(m_textBox_RoF_Multifrequency.Text, out result) && result != 0.0)
		{
			ConfigHelper.Instance.RoF_Multifrequency = result;
		}
		m_FitScanProcessor.SetSTK(bSTK: true);
	}

	private void m_textBox_RoF_EMP_ValueChanged(object sender, EventArgs e)
	{
		double result = 0.0;
		if (double.TryParse(m_textBox_RoF_EMP.Text, out result) && result != 0.0)
		{
			ConfigHelper.Instance.RoF_EMP = result;
		}
		m_FitScanProcessor.SetSTK(bSTK: true);
	}

	private void m_textBox_RoF_Fusion_ValueChanged(object sender, EventArgs e)
	{
		double result = 0.0;
		if (double.TryParse(m_textBox_RoF_Fusion.Text, out result) && result != 0.0)
		{
			ConfigHelper.Instance.RoF_Fusion = result;
		}
		m_FitScanProcessor.SetSTK(bSTK: true);
	}

	private void m_textBox_RoF_Phased_Plasma_ValueChanged(object sender, EventArgs e)
	{
		double result = 0.0;
		if (double.TryParse(m_textBox_RoF_Phased_Plasma.Text, out result) && result != 0.0)
		{
			ConfigHelper.Instance.RoF_Phased_Plasma = result;
		}
		m_FitScanProcessor.SetSTK(bSTK: true);
	}

	private void m_textBox_RoF_Hail_ValueChanged(object sender, EventArgs e)
	{
		double result = 0.0;
		if (double.TryParse(m_textBox_RoF_Hail.Text, out result) && result != 0.0)
		{
			ConfigHelper.Instance.RoF_Hail = result;
		}
		m_FitScanProcessor.SetSTK(bSTK: true);
	}

	private void Load_DPS_RoF()
	{
		m_textBox_DPS_Mjolnir.Text = ConfigHelper.Instance.DPS_Mjolnir.ToString();
		m_textBox_DPS_Nova.Text = ConfigHelper.Instance.DPS_Nova.ToString();
		m_textBox_DPS_Antimatter.Text = ConfigHelper.Instance.DPS_Antimatter.ToString();
		m_textBox_DPS_Void.Text = ConfigHelper.Instance.DPS_Void.ToString();
		m_textBox_DPS_VoidL.Text = ConfigHelper.Instance.DPS_VoidL.ToString();
		m_textBox_DPS_Multifrequency.Text = ConfigHelper.Instance.DPS_Multifrequency.ToString();
		m_textBox_DPS_EMP.Text = ConfigHelper.Instance.DPS_EMP.ToString();
		m_textBox_DPS_Fusion.Text = ConfigHelper.Instance.DPS_Fusion.ToString();
		m_textBox_DPS_Phased_Plasma.Text = ConfigHelper.Instance.DPS_Phased_Plasma.ToString();
		m_textBox_DPS_Hail.Text = ConfigHelper.Instance.DPS_Hail.ToString();
		m_textBox_RoF_Mjolnir.Text = ConfigHelper.Instance.RoF_Mjolnir.ToString();
		m_textBox_RoF_Nova.Text = ConfigHelper.Instance.RoF_Nova.ToString();
		m_textBox_RoF_Antimatter.Text = ConfigHelper.Instance.RoF_Antimatter.ToString();
		m_textBox_RoF_Void.Text = ConfigHelper.Instance.RoF_Void.ToString();
		m_textBox_RoF_VoidL.Text = ConfigHelper.Instance.RoF_VoidL.ToString();
		m_textBox_RoF_Multifrequency.Text = ConfigHelper.Instance.RoF_Multifrequency.ToString();
		m_textBox_RoF_EMP.Text = ConfigHelper.Instance.RoF_EMP.ToString();
		m_textBox_RoF_Fusion.Text = ConfigHelper.Instance.RoF_Fusion.ToString();
		m_textBox_RoF_Phased_Plasma.Text = ConfigHelper.Instance.RoF_Phased_Plasma.ToString();
		m_textBox_RoF_Hail.Text = ConfigHelper.Instance.RoF_Hail.ToString();
	}

	private void m_radioPassive_CheckedChanged(object sender, EventArgs e)
	{
		if (m_radioPassive.Checked)
		{
			ConfigHelper.Instance.PassiveColdHot = "Passive";
			m_FitScanProcessor.SetPassive(bPassive: true);
			m_checkBoxADCActive.Enabled = false;
		}
		else if (m_radioHot.Checked)
		{
			ConfigHelper.Instance.PassiveColdHot = "Hot";
			m_FitScanProcessor.SetPassive(bPassive: false);
			m_checkBoxADCActive.Enabled = true;
		}
		else
		{
			ConfigHelper.Instance.PassiveColdHot = "Cold";
			m_FitScanProcessor.SetPassive(bPassive: false);
			m_checkBoxADCActive.Enabled = true;
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		Clipboard.Clear();
		Clipboard.SetDataObject("Expanded Cargohold II\nExpanded Cargohold II\nExpanded Cargohold II");
	}

	private void button2_Click(object sender, EventArgs e)
	{
		Clipboard.Clear();
		Clipboard.SetDataObject("Reinforced Bulkheads II\nReinforced Bulkheads II\nReinforced Bulkheads II");
	}

	private void m_checkBoxManualEHP_CheckedChanged(object sender, EventArgs e)
	{
		m_richTextBoxManualEHP.Enabled = m_checkBoxManualEHP.Checked;
		ConfigHelper.Instance.Is_Manual_EHP = m_checkBoxManualEHP.Checked;
		m_radioCold.Enabled = !m_checkBoxManualEHP.Checked;
		m_radioHot.Enabled = !m_checkBoxManualEHP.Checked;
		m_radioPassive.Enabled = !m_checkBoxManualEHP.Checked;
		if (m_checkBoxManualEHP.Checked)
		{
			m_checkBoxADCActive.Enabled = false;
		}
		else
		{
			m_checkBoxADCActive.Enabled = !m_radioPassive.Checked;
		}
		OnShipTankChangedSTK();
	}

	private void m_richTextBoxManualEHP_TextChanged(object sender, EventArgs e)
	{
		if (int.TryParse(m_richTextBoxManualEHP.Text, out var result) && result > 0)
		{
			ConfigHelper.Instance.Manual_EHP = result;
		}
		OnShipTankChangedSTK();
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
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EveFitScanUI.Form1));
		this.menuStrip1 = new System.Windows.Forms.MenuStrip();
		this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.resetDPSRoFcanFlyThenAll4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.resetDPSRoFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.licenseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.sourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.m_ButtonResetFit = new System.Windows.Forms.Button();
		this.m_ComboBoxShipType = new System.Windows.Forms.ComboBox();
		this.m_ButtonCopyEFT = new System.Windows.Forms.Button();
		this.m_TextBoxShieldHP = new System.Windows.Forms.RichTextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.m_TextBoxArmorHP = new System.Windows.Forms.RichTextBox();
		this.m_TextBoxHullHP = new System.Windows.Forms.RichTextBox();
		this.m_TextBoxShieldResistsCold = new System.Windows.Forms.RichTextBox();
		this.m_TextBoxShieldResistsHot = new System.Windows.Forms.RichTextBox();
		this.m_TextBoxArmorResistsCold = new System.Windows.Forms.RichTextBox();
		this.m_TextBoxArmorResistsHot = new System.Windows.Forms.RichTextBox();
		this.m_TextBoxHullResistsCold = new System.Windows.Forms.RichTextBox();
		this.m_TextBoxHullResistsHot = new System.Windows.Forms.RichTextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.m_TextBoxEHPMjolnirCold = new System.Windows.Forms.RichTextBox();
		this.m_TextBoxEHPMjolnirHot = new System.Windows.Forms.RichTextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.m_TextBoxEHPNovaHot = new System.Windows.Forms.RichTextBox();
		this.m_TextBoxEHPNovaCold = new System.Windows.Forms.RichTextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.m_TextBoxEHPAntimatterHot = new System.Windows.Forms.RichTextBox();
		this.m_TextBoxEHPAntimatterCold = new System.Windows.Forms.RichTextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.m_TextBoxEHPVoidHot = new System.Windows.Forms.RichTextBox();
		this.m_TextBoxEHPVoidCold = new System.Windows.Forms.RichTextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.m_TextBoxEHPMultifreqHot = new System.Windows.Forms.RichTextBox();
		this.m_TextBoxEHPMultifreqCold = new System.Windows.Forms.RichTextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.m_TextBoxEHPEMPHot = new System.Windows.Forms.RichTextBox();
		this.m_TextBoxEHPEMPCold = new System.Windows.Forms.RichTextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.m_TextBoxEHPFusionHot = new System.Windows.Forms.RichTextBox();
		this.m_TextBoxEHPFusionCold = new System.Windows.Forms.RichTextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.m_TextBoxEHPPhasedPlasmaHot = new System.Windows.Forms.RichTextBox();
		this.m_TextBoxEHPPhasedPlasmaCold = new System.Windows.Forms.RichTextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.m_TextBoxEHPHailHot = new System.Windows.Forms.RichTextBox();
		this.m_TextBoxEHPHailCold = new System.Windows.Forms.RichTextBox();
		this.m_FitText = new System.Windows.Forms.RichTextBox();
		this.m_checkBoxPassive = new System.Windows.Forms.CheckBox();
		this.m_BackgroundWorkerPrices = new System.ComponentModel.BackgroundWorker();
		this.m_ValueHullText = new System.Windows.Forms.RichTextBox();
		this.m_ValueFittingsText = new System.Windows.Forms.RichTextBox();
		this.m_ValueTotalText = new System.Windows.Forms.RichTextBox();
		this.m_ValueCanDropText = new System.Windows.Forms.RichTextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.m_BackgroundWorkerUpdate = new System.ComponentModel.BackgroundWorker();
		this.m_checkBoxADCActive = new System.Windows.Forms.CheckBox();
		this.m_History = new System.Windows.Forms.ComboBox();
		this.m_checkBoxSTK = new System.Windows.Forms.CheckBox();
		this.m_comboBoxSysSecurity = new System.Windows.Forms.ComboBox();
		this.labelSysSecurity = new System.Windows.Forms.Label();
		this.labelDPS = new System.Windows.Forms.Label();
		this.labelRoF = new System.Windows.Forms.Label();
		this.labelSTK = new System.Windows.Forms.Label();
		this.m_textBox_DPS_Mjolnir = new System.Windows.Forms.RichTextBox();
		this.m_textBox_RoF_Mjolnir = new System.Windows.Forms.RichTextBox();
		this.m_textBox_DPS_Nova = new System.Windows.Forms.RichTextBox();
		this.m_textBox_DPS_Antimatter = new System.Windows.Forms.RichTextBox();
		this.m_textBox_DPS_Void = new System.Windows.Forms.RichTextBox();
		this.m_textBox_DPS_Multifrequency = new System.Windows.Forms.RichTextBox();
		this.m_textBox_DPS_EMP = new System.Windows.Forms.RichTextBox();
		this.m_textBox_DPS_Fusion = new System.Windows.Forms.RichTextBox();
		this.m_textBox_DPS_Phased_Plasma = new System.Windows.Forms.RichTextBox();
		this.m_textBox_DPS_Hail = new System.Windows.Forms.RichTextBox();
		this.m_textBox_RoF_Nova = new System.Windows.Forms.RichTextBox();
		this.m_textBox_RoF_Antimatter = new System.Windows.Forms.RichTextBox();
		this.m_textBox_RoF_Void = new System.Windows.Forms.RichTextBox();
		this.m_textBox_RoF_Multifrequency = new System.Windows.Forms.RichTextBox();
		this.m_textBox_RoF_EMP = new System.Windows.Forms.RichTextBox();
		this.m_textBox_RoF_Fusion = new System.Windows.Forms.RichTextBox();
		this.m_textBox_RoF_Phased_Plasma = new System.Windows.Forms.RichTextBox();
		this.m_textBox_RoF_Hail = new System.Windows.Forms.RichTextBox();
		this.m_radioPassive = new System.Windows.Forms.RadioButton();
		this.m_radioCold = new System.Windows.Forms.RadioButton();
		this.m_radioHot = new System.Windows.Forms.RadioButton();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.m_textBox_RoF_VoidL = new System.Windows.Forms.RichTextBox();
		this.m_textBox_DPS_VoidL = new System.Windows.Forms.RichTextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.m_TextBoxEHPVoidLHot = new System.Windows.Forms.RichTextBox();
		this.m_TextBoxEHPVoidLCold = new System.Windows.Forms.RichTextBox();
		this.panel4 = new System.Windows.Forms.Panel();
		this.labelSeconds = new System.Windows.Forms.Label();
		this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
		this.labelBR = new System.Windows.Forms.Label();
		this.button1 = new System.Windows.Forms.Button();
		this.button2 = new System.Windows.Forms.Button();
		this.m_checkBoxManualEHP = new System.Windows.Forms.CheckBox();
		this.m_richTextBoxManualEHP = new System.Windows.Forms.RichTextBox();
		this.menuStrip1.SuspendLayout();
		base.SuspendLayout();
		this.menuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
		this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.fileToolStripMenuItem, this.helpToolStripMenuItem, this.settingsToolStripMenuItem });
		this.menuStrip1.Location = new System.Drawing.Point(0, 0);
		this.menuStrip1.Name = "menuStrip1";
		this.menuStrip1.Size = new System.Drawing.Size(968, 24);
		this.menuStrip1.TabIndex = 2;
		this.menuStrip1.Text = "menuStrip1";
		this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.resetDPSRoFcanFlyThenAll4ToolStripMenuItem, this.resetDPSRoFToolStripMenuItem, this.exitToolStripMenuItem });
		this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
		this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
		this.fileToolStripMenuItem.Text = "File";
		this.resetDPSRoFcanFlyThenAll4ToolStripMenuItem.Name = "resetDPSRoFcanFlyThenAll4ToolStripMenuItem";
		this.resetDPSRoFcanFlyThenAll4ToolStripMenuItem.Size = new System.Drawing.Size(281, 22);
		this.resetDPSRoFcanFlyThenAll4ToolStripMenuItem.Text = "Reset DPS / RoF (lvl4 support, lvl3 spec)";
		this.resetDPSRoFcanFlyThenAll4ToolStripMenuItem.Click += new System.EventHandler(resetDPSRoFcanFlyThenAll4ToolStripMenuItem_Click);
		this.resetDPSRoFToolStripMenuItem.Name = "resetDPSRoFToolStripMenuItem";
		this.resetDPSRoFToolStripMenuItem.Size = new System.Drawing.Size(281, 22);
		this.resetDPSRoFToolStripMenuItem.Text = "Reset DPS / RoF (all 5~~~)";
		this.resetDPSRoFToolStripMenuItem.Click += new System.EventHandler(resetDPSRoFToolStripMenuItem_Click);
		this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
		this.exitToolStripMenuItem.Size = new System.Drawing.Size(281, 22);
		this.exitToolStripMenuItem.Text = "Exit";
		this.exitToolStripMenuItem.Click += new System.EventHandler(exitToolStripMenuItem_Click);
		this.helpToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
		this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.aboutToolStripMenuItem, this.licenseToolStripMenuItem, this.sourceToolStripMenuItem });
		this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
		this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
		this.helpToolStripMenuItem.Text = "Help";
		this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
		this.aboutToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
		this.aboutToolStripMenuItem.Text = "About";
		this.aboutToolStripMenuItem.Click += new System.EventHandler(aboutToolStripMenuItem_Click);
		this.licenseToolStripMenuItem.Name = "licenseToolStripMenuItem";
		this.licenseToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
		this.licenseToolStripMenuItem.Text = "License";
		this.licenseToolStripMenuItem.Click += new System.EventHandler(licenseToolStripMenuItem_Click);
		this.sourceToolStripMenuItem.Name = "sourceToolStripMenuItem";
		this.sourceToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
		this.sourceToolStripMenuItem.Text = "Source Code (Opens in browser)";
		this.sourceToolStripMenuItem.Click += new System.EventHandler(sourceToolStripMenuItem_Click);
		this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
		this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
		this.settingsToolStripMenuItem.Text = "Settings";
		this.settingsToolStripMenuItem.Click += new System.EventHandler(settingsToolStripMenuItem_Click);
		this.m_ButtonResetFit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_ButtonResetFit.Location = new System.Drawing.Point(280, 54);
		this.m_ButtonResetFit.Name = "m_ButtonResetFit";
		this.m_ButtonResetFit.Size = new System.Drawing.Size(112, 48);
		this.m_ButtonResetFit.TabIndex = 3;
		this.m_ButtonResetFit.Text = "Reset";
		this.m_ButtonResetFit.UseVisualStyleBackColor = true;
		this.m_ButtonResetFit.Click += new System.EventHandler(m_ButtonResetFit_Click);
		this.m_ComboBoxShipType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_ComboBoxShipType.FormattingEnabled = true;
		this.m_ComboBoxShipType.Location = new System.Drawing.Point(16, 64);
		this.m_ComboBoxShipType.Name = "m_ComboBoxShipType";
		this.m_ComboBoxShipType.Size = new System.Drawing.Size(248, 28);
		this.m_ComboBoxShipType.TabIndex = 4;
		this.m_ComboBoxShipType.DropDown += new System.EventHandler(m_ComboBoxShipType_DropDown);
		this.m_ComboBoxShipType.SelectedIndexChanged += new System.EventHandler(m_ComboBoxShipType_SelectedIndexChanged);
		this.m_ComboBoxShipType.TextUpdate += new System.EventHandler(m_ComboBoxShipType_TextUpdate);
		this.m_ButtonCopyEFT.Location = new System.Drawing.Point(8, 672);
		this.m_ButtonCopyEFT.Name = "m_ButtonCopyEFT";
		this.m_ButtonCopyEFT.Size = new System.Drawing.Size(304, 32);
		this.m_ButtonCopyEFT.TabIndex = 6;
		this.m_ButtonCopyEFT.Text = "Copy EFT fit";
		this.m_ButtonCopyEFT.UseVisualStyleBackColor = true;
		this.m_ButtonCopyEFT.Click += new System.EventHandler(m_ButtonCopyEFT_Click);
		this.m_TextBoxShieldHP.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
		this.m_TextBoxShieldHP.ForeColor = System.Drawing.SystemColors.WindowText;
		this.m_TextBoxShieldHP.Location = new System.Drawing.Point(376, 128);
		this.m_TextBoxShieldHP.Multiline = false;
		this.m_TextBoxShieldHP.Name = "m_TextBoxShieldHP";
		this.m_TextBoxShieldHP.ReadOnly = true;
		this.m_TextBoxShieldHP.Size = new System.Drawing.Size(80, 24);
		this.m_TextBoxShieldHP.TabIndex = 7;
		this.m_TextBoxShieldHP.Text = "";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
		this.label1.Location = new System.Drawing.Point(328, 136);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(47, 17);
		this.label1.TabIndex = 8;
		this.label1.Text = "Shield";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
		this.label2.Location = new System.Drawing.Point(328, 168);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(46, 17);
		this.label2.TabIndex = 9;
		this.label2.Text = "Armor";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
		this.label3.Location = new System.Drawing.Point(336, 200);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(32, 17);
		this.label3.TabIndex = 10;
		this.label3.Text = "Hull";
		this.m_TextBoxArmorHP.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
		this.m_TextBoxArmorHP.Location = new System.Drawing.Point(376, 160);
		this.m_TextBoxArmorHP.Multiline = false;
		this.m_TextBoxArmorHP.Name = "m_TextBoxArmorHP";
		this.m_TextBoxArmorHP.ReadOnly = true;
		this.m_TextBoxArmorHP.Size = new System.Drawing.Size(80, 24);
		this.m_TextBoxArmorHP.TabIndex = 11;
		this.m_TextBoxArmorHP.Text = "";
		this.m_TextBoxHullHP.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
		this.m_TextBoxHullHP.Location = new System.Drawing.Point(376, 192);
		this.m_TextBoxHullHP.Multiline = false;
		this.m_TextBoxHullHP.Name = "m_TextBoxHullHP";
		this.m_TextBoxHullHP.ReadOnly = true;
		this.m_TextBoxHullHP.Size = new System.Drawing.Size(80, 24);
		this.m_TextBoxHullHP.TabIndex = 12;
		this.m_TextBoxHullHP.Text = "";
		this.m_TextBoxShieldResistsCold.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
		this.m_TextBoxShieldResistsCold.Location = new System.Drawing.Point(472, 128);
		this.m_TextBoxShieldResistsCold.Multiline = false;
		this.m_TextBoxShieldResistsCold.Name = "m_TextBoxShieldResistsCold";
		this.m_TextBoxShieldResistsCold.ReadOnly = true;
		this.m_TextBoxShieldResistsCold.Size = new System.Drawing.Size(232, 24);
		this.m_TextBoxShieldResistsCold.TabIndex = 13;
		this.m_TextBoxShieldResistsCold.Text = "";
		this.m_TextBoxShieldResistsHot.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
		this.m_TextBoxShieldResistsHot.Location = new System.Drawing.Point(720, 128);
		this.m_TextBoxShieldResistsHot.Multiline = false;
		this.m_TextBoxShieldResistsHot.Name = "m_TextBoxShieldResistsHot";
		this.m_TextBoxShieldResistsHot.ReadOnly = true;
		this.m_TextBoxShieldResistsHot.Size = new System.Drawing.Size(232, 24);
		this.m_TextBoxShieldResistsHot.TabIndex = 14;
		this.m_TextBoxShieldResistsHot.Text = "";
		this.m_TextBoxArmorResistsCold.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
		this.m_TextBoxArmorResistsCold.Location = new System.Drawing.Point(472, 160);
		this.m_TextBoxArmorResistsCold.Multiline = false;
		this.m_TextBoxArmorResistsCold.Name = "m_TextBoxArmorResistsCold";
		this.m_TextBoxArmorResistsCold.ReadOnly = true;
		this.m_TextBoxArmorResistsCold.Size = new System.Drawing.Size(232, 24);
		this.m_TextBoxArmorResistsCold.TabIndex = 15;
		this.m_TextBoxArmorResistsCold.Text = "";
		this.m_TextBoxArmorResistsHot.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
		this.m_TextBoxArmorResistsHot.Location = new System.Drawing.Point(720, 160);
		this.m_TextBoxArmorResistsHot.Multiline = false;
		this.m_TextBoxArmorResistsHot.Name = "m_TextBoxArmorResistsHot";
		this.m_TextBoxArmorResistsHot.ReadOnly = true;
		this.m_TextBoxArmorResistsHot.Size = new System.Drawing.Size(232, 24);
		this.m_TextBoxArmorResistsHot.TabIndex = 16;
		this.m_TextBoxArmorResistsHot.Text = "";
		this.m_TextBoxHullResistsCold.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
		this.m_TextBoxHullResistsCold.Location = new System.Drawing.Point(472, 192);
		this.m_TextBoxHullResistsCold.Multiline = false;
		this.m_TextBoxHullResistsCold.Name = "m_TextBoxHullResistsCold";
		this.m_TextBoxHullResistsCold.ReadOnly = true;
		this.m_TextBoxHullResistsCold.Size = new System.Drawing.Size(232, 24);
		this.m_TextBoxHullResistsCold.TabIndex = 17;
		this.m_TextBoxHullResistsCold.Text = "";
		this.m_TextBoxHullResistsHot.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
		this.m_TextBoxHullResistsHot.Location = new System.Drawing.Point(720, 192);
		this.m_TextBoxHullResistsHot.Multiline = false;
		this.m_TextBoxHullResistsHot.Name = "m_TextBoxHullResistsHot";
		this.m_TextBoxHullResistsHot.ReadOnly = true;
		this.m_TextBoxHullResistsHot.Size = new System.Drawing.Size(232, 24);
		this.m_TextBoxHullResistsHot.TabIndex = 18;
		this.m_TextBoxHullResistsHot.Text = "";
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(472, 104);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(78, 13);
		this.label4.TabIndex = 19;
		this.label4.Text = "------ COLD ------";
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(720, 104);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(72, 13);
		this.label5.TabIndex = 20;
		this.label5.Text = "------ HOT ------";
		this.m_TextBoxEHPMjolnirCold.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_TextBoxEHPMjolnirCold.Location = new System.Drawing.Point(528, 260);
		this.m_TextBoxEHPMjolnirCold.Multiline = false;
		this.m_TextBoxEHPMjolnirCold.Name = "m_TextBoxEHPMjolnirCold";
		this.m_TextBoxEHPMjolnirCold.ReadOnly = true;
		this.m_TextBoxEHPMjolnirCold.Size = new System.Drawing.Size(120, 24);
		this.m_TextBoxEHPMjolnirCold.TabIndex = 21;
		this.m_TextBoxEHPMjolnirCold.Text = "";
		this.m_TextBoxEHPMjolnirHot.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_TextBoxEHPMjolnirHot.Location = new System.Drawing.Point(768, 260);
		this.m_TextBoxEHPMjolnirHot.Multiline = false;
		this.m_TextBoxEHPMjolnirHot.Name = "m_TextBoxEHPMjolnirHot";
		this.m_TextBoxEHPMjolnirHot.ReadOnly = true;
		this.m_TextBoxEHPMjolnirHot.Size = new System.Drawing.Size(120, 24);
		this.m_TextBoxEHPMjolnirHot.TabIndex = 22;
		this.m_TextBoxEHPMjolnirHot.Text = "";
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.SystemColors.ScrollBar;
		this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.label6.Location = new System.Drawing.Point(360, 260);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(61, 20);
		this.label6.TabIndex = 23;
		this.label6.Text = "Mjolnir";
		this.toolTip1.SetToolTip(this.label6, "Purifier");
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.SystemColors.ScrollBar;
		this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.label7.Location = new System.Drawing.Point(360, 292);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(49, 20);
		this.label7.TabIndex = 26;
		this.label7.Text = "Nova";
		this.toolTip1.SetToolTip(this.label7, "Hound");
		this.m_TextBoxEHPNovaHot.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_TextBoxEHPNovaHot.Location = new System.Drawing.Point(768, 292);
		this.m_TextBoxEHPNovaHot.Multiline = false;
		this.m_TextBoxEHPNovaHot.Name = "m_TextBoxEHPNovaHot";
		this.m_TextBoxEHPNovaHot.ReadOnly = true;
		this.m_TextBoxEHPNovaHot.Size = new System.Drawing.Size(120, 24);
		this.m_TextBoxEHPNovaHot.TabIndex = 25;
		this.m_TextBoxEHPNovaHot.Text = "";
		this.m_TextBoxEHPNovaCold.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_TextBoxEHPNovaCold.Location = new System.Drawing.Point(528, 292);
		this.m_TextBoxEHPNovaCold.Multiline = false;
		this.m_TextBoxEHPNovaCold.Name = "m_TextBoxEHPNovaCold";
		this.m_TextBoxEHPNovaCold.ReadOnly = true;
		this.m_TextBoxEHPNovaCold.Size = new System.Drawing.Size(120, 24);
		this.m_TextBoxEHPNovaCold.TabIndex = 24;
		this.m_TextBoxEHPNovaCold.Text = "";
		this.label8.AutoSize = true;
		this.label8.BackColor = System.Drawing.SystemColors.ActiveBorder;
		this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.label8.Location = new System.Drawing.Point(360, 332);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(93, 20);
		this.label8.TabIndex = 29;
		this.label8.Text = "Antimatter";
		this.toolTip1.SetToolTip(this.label8, "T1 Cat");
		this.m_TextBoxEHPAntimatterHot.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_TextBoxEHPAntimatterHot.Location = new System.Drawing.Point(768, 332);
		this.m_TextBoxEHPAntimatterHot.Multiline = false;
		this.m_TextBoxEHPAntimatterHot.Name = "m_TextBoxEHPAntimatterHot";
		this.m_TextBoxEHPAntimatterHot.ReadOnly = true;
		this.m_TextBoxEHPAntimatterHot.Size = new System.Drawing.Size(120, 24);
		this.m_TextBoxEHPAntimatterHot.TabIndex = 28;
		this.m_TextBoxEHPAntimatterHot.Text = "";
		this.m_TextBoxEHPAntimatterCold.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_TextBoxEHPAntimatterCold.Location = new System.Drawing.Point(528, 332);
		this.m_TextBoxEHPAntimatterCold.Multiline = false;
		this.m_TextBoxEHPAntimatterCold.Name = "m_TextBoxEHPAntimatterCold";
		this.m_TextBoxEHPAntimatterCold.ReadOnly = true;
		this.m_TextBoxEHPAntimatterCold.Size = new System.Drawing.Size(120, 24);
		this.m_TextBoxEHPAntimatterCold.TabIndex = 27;
		this.m_TextBoxEHPAntimatterCold.Text = "";
		this.label9.AutoSize = true;
		this.label9.BackColor = System.Drawing.SystemColors.ActiveBorder;
		this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.label9.Location = new System.Drawing.Point(360, 366);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(45, 20);
		this.label9.TabIndex = 32;
		this.label9.Text = "Void";
		this.toolTip1.SetToolTip(this.label9, "T2 Cat");
		this.m_TextBoxEHPVoidHot.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_TextBoxEHPVoidHot.Location = new System.Drawing.Point(768, 364);
		this.m_TextBoxEHPVoidHot.Multiline = false;
		this.m_TextBoxEHPVoidHot.Name = "m_TextBoxEHPVoidHot";
		this.m_TextBoxEHPVoidHot.ReadOnly = true;
		this.m_TextBoxEHPVoidHot.Size = new System.Drawing.Size(120, 24);
		this.m_TextBoxEHPVoidHot.TabIndex = 31;
		this.m_TextBoxEHPVoidHot.Text = "";
		this.m_TextBoxEHPVoidCold.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_TextBoxEHPVoidCold.Location = new System.Drawing.Point(528, 364);
		this.m_TextBoxEHPVoidCold.Multiline = false;
		this.m_TextBoxEHPVoidCold.Name = "m_TextBoxEHPVoidCold";
		this.m_TextBoxEHPVoidCold.ReadOnly = true;
		this.m_TextBoxEHPVoidCold.Size = new System.Drawing.Size(120, 24);
		this.m_TextBoxEHPVoidCold.TabIndex = 30;
		this.m_TextBoxEHPVoidCold.Text = "";
		this.label10.AutoSize = true;
		this.label10.BackColor = System.Drawing.SystemColors.ActiveBorder;
		this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.label10.Location = new System.Drawing.Point(360, 404);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(126, 20);
		this.label10.TabIndex = 35;
		this.label10.Text = "Multifrequency";
		this.toolTip1.SetToolTip(this.label10, "Coercer");
		this.m_TextBoxEHPMultifreqHot.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_TextBoxEHPMultifreqHot.Location = new System.Drawing.Point(768, 404);
		this.m_TextBoxEHPMultifreqHot.Multiline = false;
		this.m_TextBoxEHPMultifreqHot.Name = "m_TextBoxEHPMultifreqHot";
		this.m_TextBoxEHPMultifreqHot.ReadOnly = true;
		this.m_TextBoxEHPMultifreqHot.Size = new System.Drawing.Size(120, 24);
		this.m_TextBoxEHPMultifreqHot.TabIndex = 34;
		this.m_TextBoxEHPMultifreqHot.Text = "";
		this.m_TextBoxEHPMultifreqCold.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_TextBoxEHPMultifreqCold.Location = new System.Drawing.Point(528, 404);
		this.m_TextBoxEHPMultifreqCold.Multiline = false;
		this.m_TextBoxEHPMultifreqCold.Name = "m_TextBoxEHPMultifreqCold";
		this.m_TextBoxEHPMultifreqCold.ReadOnly = true;
		this.m_TextBoxEHPMultifreqCold.Size = new System.Drawing.Size(120, 24);
		this.m_TextBoxEHPMultifreqCold.TabIndex = 33;
		this.m_TextBoxEHPMultifreqCold.Text = "";
		this.label11.AutoSize = true;
		this.label11.BackColor = System.Drawing.SystemColors.ScrollBar;
		this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.label11.Location = new System.Drawing.Point(360, 444);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(46, 20);
		this.label11.TabIndex = 38;
		this.label11.Text = "EMP";
		this.toolTip1.SetToolTip(this.label11, "Autocannon Thrasher");
		this.m_TextBoxEHPEMPHot.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_TextBoxEHPEMPHot.Location = new System.Drawing.Point(768, 444);
		this.m_TextBoxEHPEMPHot.Multiline = false;
		this.m_TextBoxEHPEMPHot.Name = "m_TextBoxEHPEMPHot";
		this.m_TextBoxEHPEMPHot.ReadOnly = true;
		this.m_TextBoxEHPEMPHot.Size = new System.Drawing.Size(120, 24);
		this.m_TextBoxEHPEMPHot.TabIndex = 37;
		this.m_TextBoxEHPEMPHot.Text = "";
		this.m_TextBoxEHPEMPCold.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_TextBoxEHPEMPCold.Location = new System.Drawing.Point(528, 444);
		this.m_TextBoxEHPEMPCold.Multiline = false;
		this.m_TextBoxEHPEMPCold.Name = "m_TextBoxEHPEMPCold";
		this.m_TextBoxEHPEMPCold.ReadOnly = true;
		this.m_TextBoxEHPEMPCold.Size = new System.Drawing.Size(120, 24);
		this.m_TextBoxEHPEMPCold.TabIndex = 36;
		this.m_TextBoxEHPEMPCold.Text = "";
		this.label12.AutoSize = true;
		this.label12.BackColor = System.Drawing.SystemColors.ScrollBar;
		this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.label12.Location = new System.Drawing.Point(360, 476);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(63, 20);
		this.label12.TabIndex = 41;
		this.label12.Text = "Fusion";
		this.toolTip1.SetToolTip(this.label12, "Autocannon Thrasher");
		this.m_TextBoxEHPFusionHot.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_TextBoxEHPFusionHot.Location = new System.Drawing.Point(768, 476);
		this.m_TextBoxEHPFusionHot.Multiline = false;
		this.m_TextBoxEHPFusionHot.Name = "m_TextBoxEHPFusionHot";
		this.m_TextBoxEHPFusionHot.ReadOnly = true;
		this.m_TextBoxEHPFusionHot.Size = new System.Drawing.Size(120, 24);
		this.m_TextBoxEHPFusionHot.TabIndex = 40;
		this.m_TextBoxEHPFusionHot.Text = "";
		this.m_TextBoxEHPFusionCold.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_TextBoxEHPFusionCold.Location = new System.Drawing.Point(528, 476);
		this.m_TextBoxEHPFusionCold.Multiline = false;
		this.m_TextBoxEHPFusionCold.Name = "m_TextBoxEHPFusionCold";
		this.m_TextBoxEHPFusionCold.ReadOnly = true;
		this.m_TextBoxEHPFusionCold.Size = new System.Drawing.Size(120, 24);
		this.m_TextBoxEHPFusionCold.TabIndex = 39;
		this.m_TextBoxEHPFusionCold.Text = "";
		this.label13.AutoSize = true;
		this.label13.BackColor = System.Drawing.SystemColors.ScrollBar;
		this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.label13.Location = new System.Drawing.Point(360, 508);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(132, 20);
		this.label13.TabIndex = 44;
		this.label13.Text = "Phased Plasma";
		this.toolTip1.SetToolTip(this.label13, "Autocannon Thrasher");
		this.m_TextBoxEHPPhasedPlasmaHot.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_TextBoxEHPPhasedPlasmaHot.Location = new System.Drawing.Point(768, 508);
		this.m_TextBoxEHPPhasedPlasmaHot.Multiline = false;
		this.m_TextBoxEHPPhasedPlasmaHot.Name = "m_TextBoxEHPPhasedPlasmaHot";
		this.m_TextBoxEHPPhasedPlasmaHot.ReadOnly = true;
		this.m_TextBoxEHPPhasedPlasmaHot.Size = new System.Drawing.Size(120, 24);
		this.m_TextBoxEHPPhasedPlasmaHot.TabIndex = 43;
		this.m_TextBoxEHPPhasedPlasmaHot.Text = "";
		this.m_TextBoxEHPPhasedPlasmaCold.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_TextBoxEHPPhasedPlasmaCold.Location = new System.Drawing.Point(528, 508);
		this.m_TextBoxEHPPhasedPlasmaCold.Multiline = false;
		this.m_TextBoxEHPPhasedPlasmaCold.Name = "m_TextBoxEHPPhasedPlasmaCold";
		this.m_TextBoxEHPPhasedPlasmaCold.ReadOnly = true;
		this.m_TextBoxEHPPhasedPlasmaCold.Size = new System.Drawing.Size(120, 24);
		this.m_TextBoxEHPPhasedPlasmaCold.TabIndex = 42;
		this.m_TextBoxEHPPhasedPlasmaCold.Text = "";
		this.label14.AutoSize = true;
		this.label14.BackColor = System.Drawing.SystemColors.ScrollBar;
		this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.label14.Location = new System.Drawing.Point(360, 540);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(40, 20);
		this.label14.TabIndex = 47;
		this.label14.Text = "Hail";
		this.toolTip1.SetToolTip(this.label14, "Autocannon Thrasher");
		this.m_TextBoxEHPHailHot.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_TextBoxEHPHailHot.Location = new System.Drawing.Point(768, 540);
		this.m_TextBoxEHPHailHot.Multiline = false;
		this.m_TextBoxEHPHailHot.Name = "m_TextBoxEHPHailHot";
		this.m_TextBoxEHPHailHot.ReadOnly = true;
		this.m_TextBoxEHPHailHot.Size = new System.Drawing.Size(120, 24);
		this.m_TextBoxEHPHailHot.TabIndex = 46;
		this.m_TextBoxEHPHailHot.Text = "";
		this.m_TextBoxEHPHailCold.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_TextBoxEHPHailCold.Location = new System.Drawing.Point(528, 540);
		this.m_TextBoxEHPHailCold.Multiline = false;
		this.m_TextBoxEHPHailCold.Name = "m_TextBoxEHPHailCold";
		this.m_TextBoxEHPHailCold.ReadOnly = true;
		this.m_TextBoxEHPHailCold.Size = new System.Drawing.Size(120, 24);
		this.m_TextBoxEHPHailCold.TabIndex = 45;
		this.m_TextBoxEHPHailCold.Text = "";
		this.m_FitText.Location = new System.Drawing.Point(8, 128);
		this.m_FitText.Name = "m_FitText";
		this.m_FitText.ReadOnly = true;
		this.m_FitText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
		this.m_FitText.Size = new System.Drawing.Size(304, 432);
		this.m_FitText.TabIndex = 48;
		this.m_FitText.Text = "";
		this.m_checkBoxPassive.AutoSize = true;
		this.m_checkBoxPassive.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_checkBoxPassive.Location = new System.Drawing.Point(364, 635);
		this.m_checkBoxPassive.Name = "m_checkBoxPassive";
		this.m_checkBoxPassive.Size = new System.Drawing.Size(89, 24);
		this.m_checkBoxPassive.TabIndex = 49;
		this.m_checkBoxPassive.Text = "Passive";
		this.m_checkBoxPassive.UseVisualStyleBackColor = true;
		this.m_checkBoxPassive.CheckedChanged += new System.EventHandler(m_checkBoxPassive_CheckedChanged);
		this.m_BackgroundWorkerPrices.DoWork += new System.ComponentModel.DoWorkEventHandler(BackgroundWorkerPrices_DoWork);
		this.m_BackgroundWorkerPrices.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(BackgroundWorkerPrices_RunWorkerCompleted);
		this.m_ValueHullText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_ValueHullText.Location = new System.Drawing.Point(152, 568);
		this.m_ValueHullText.Name = "m_ValueHullText";
		this.m_ValueHullText.ReadOnly = true;
		this.m_ValueHullText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
		this.m_ValueHullText.Size = new System.Drawing.Size(160, 24);
		this.m_ValueHullText.TabIndex = 51;
		this.m_ValueHullText.Text = "";
		this.m_ValueFittingsText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_ValueFittingsText.Location = new System.Drawing.Point(152, 592);
		this.m_ValueFittingsText.Name = "m_ValueFittingsText";
		this.m_ValueFittingsText.ReadOnly = true;
		this.m_ValueFittingsText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
		this.m_ValueFittingsText.Size = new System.Drawing.Size(160, 24);
		this.m_ValueFittingsText.TabIndex = 52;
		this.m_ValueFittingsText.Text = "";
		this.m_ValueTotalText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_ValueTotalText.Location = new System.Drawing.Point(152, 616);
		this.m_ValueTotalText.Name = "m_ValueTotalText";
		this.m_ValueTotalText.ReadOnly = true;
		this.m_ValueTotalText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
		this.m_ValueTotalText.Size = new System.Drawing.Size(160, 24);
		this.m_ValueTotalText.TabIndex = 53;
		this.m_ValueTotalText.Text = "";
		this.m_ValueCanDropText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_ValueCanDropText.Location = new System.Drawing.Point(152, 640);
		this.m_ValueCanDropText.Name = "m_ValueCanDropText";
		this.m_ValueCanDropText.ReadOnly = true;
		this.m_ValueCanDropText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
		this.m_ValueCanDropText.Size = new System.Drawing.Size(160, 24);
		this.m_ValueCanDropText.TabIndex = 54;
		this.m_ValueCanDropText.Text = "";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.label15.Location = new System.Drawing.Point(72, 568);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(39, 16);
		this.label15.TabIndex = 55;
		this.label15.Text = "Hull:";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.label16.Location = new System.Drawing.Point(72, 592);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(62, 16);
		this.label16.TabIndex = 56;
		this.label16.Text = "Fittings:";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.label17.Location = new System.Drawing.Point(72, 616);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(48, 16);
		this.label17.TabIndex = 57;
		this.label17.Text = "Total:";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.label18.Location = new System.Drawing.Point(72, 640);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(75, 16);
		this.label18.TabIndex = 58;
		this.label18.Text = "Can drop:";
		this.m_BackgroundWorkerUpdate.DoWork += new System.ComponentModel.DoWorkEventHandler(BackgroundWorkerUpdate_DoWork);
		this.m_BackgroundWorkerUpdate.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(BackgroundWorkerUpdate_RunWorkerCompleted);
		this.m_checkBoxADCActive.AutoSize = true;
		this.m_checkBoxADCActive.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_checkBoxADCActive.Location = new System.Drawing.Point(365, 683);
		this.m_checkBoxADCActive.Name = "m_checkBoxADCActive";
		this.m_checkBoxADCActive.Size = new System.Drawing.Size(210, 24);
		this.m_checkBoxADCActive.TabIndex = 59;
		this.m_checkBoxADCActive.Text = "ADC active (if present)";
		this.m_checkBoxADCActive.UseVisualStyleBackColor = true;
		this.m_checkBoxADCActive.CheckedChanged += new System.EventHandler(m_checkBoxADCActive_CheckedChanged);
		this.m_History.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.m_History.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
		this.m_History.FormattingEnabled = true;
		this.m_History.Location = new System.Drawing.Point(8, 728);
		this.m_History.Name = "m_History";
		this.m_History.Size = new System.Drawing.Size(944, 23);
		this.m_History.TabIndex = 60;
		this.m_History.SelectedIndexChanged += new System.EventHandler(m_History_SelectedIndexChanged);
		this.m_checkBoxSTK.AutoSize = true;
		this.m_checkBoxSTK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_checkBoxSTK.Location = new System.Drawing.Point(631, 635);
		this.m_checkBoxSTK.Name = "m_checkBoxSTK";
		this.m_checkBoxSTK.Size = new System.Drawing.Size(122, 24);
		this.m_checkBoxSTK.TabIndex = 61;
		this.m_checkBoxSTK.Text = "Ships to Kill";
		this.m_checkBoxSTK.UseVisualStyleBackColor = true;
		this.m_checkBoxSTK.CheckedChanged += new System.EventHandler(m_checkBoxSTK_CheckedChanged);
		this.m_comboBoxSysSecurity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.m_comboBoxSysSecurity.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_comboBoxSysSecurity.FormattingEnabled = true;
		this.m_comboBoxSysSecurity.Items.AddRange(new object[14]
		{
			"0.5", "0.5p", "0.6", "0.6p", "0.7", "0.7p", "0.8", "0.8p", "0.9", "0.9p",
			"1.0", "1.0p", "Jita", "Jitap"
		});
		this.m_comboBoxSysSecurity.Location = new System.Drawing.Point(709, 681);
		this.m_comboBoxSysSecurity.Name = "m_comboBoxSysSecurity";
		this.m_comboBoxSysSecurity.Size = new System.Drawing.Size(83, 28);
		this.m_comboBoxSysSecurity.TabIndex = 62;
		this.m_comboBoxSysSecurity.SelectedIndexChanged += new System.EventHandler(m_ComboBoxSysSecurity_SelectedIndexChanged);
		this.labelSysSecurity.AutoSize = true;
		this.labelSysSecurity.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.labelSysSecurity.Location = new System.Drawing.Point(592, 685);
		this.labelSysSecurity.Name = "labelSysSecurity";
		this.labelSysSecurity.Size = new System.Drawing.Size(108, 20);
		this.labelSysSecurity.TabIndex = 63;
		this.labelSysSecurity.Text = "Sys Security";
		this.labelDPS.AutoSize = true;
		this.labelDPS.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.labelDPS.Location = new System.Drawing.Point(798, 224);
		this.labelDPS.Name = "labelDPS";
		this.labelDPS.Size = new System.Drawing.Size(45, 20);
		this.labelDPS.TabIndex = 64;
		this.labelDPS.Text = "DPS";
		this.labelRoF.AutoSize = true;
		this.labelRoF.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.labelRoF.Location = new System.Drawing.Point(889, 224);
		this.labelRoF.Name = "labelRoF";
		this.labelRoF.Size = new System.Drawing.Size(43, 20);
		this.labelRoF.TabIndex = 65;
		this.labelRoF.Text = "RoF";
		this.labelSTK.AutoSize = true;
		this.labelSTK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.labelSTK.Location = new System.Drawing.Point(636, 224);
		this.labelSTK.Name = "labelSTK";
		this.labelSTK.Size = new System.Drawing.Size(103, 20);
		this.labelSTK.TabIndex = 66;
		this.labelSTK.Text = "Ships to Kill";
		this.m_textBox_DPS_Mjolnir.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_textBox_DPS_Mjolnir.Location = new System.Drawing.Point(785, 260);
		this.m_textBox_DPS_Mjolnir.Multiline = false;
		this.m_textBox_DPS_Mjolnir.Name = "m_textBox_DPS_Mjolnir";
		this.m_textBox_DPS_Mjolnir.Size = new System.Drawing.Size(77, 24);
		this.m_textBox_DPS_Mjolnir.TabIndex = 67;
		this.m_textBox_DPS_Mjolnir.Text = "";
		this.m_textBox_DPS_Mjolnir.TextChanged += new System.EventHandler(m_textBox_DPS_Mjolnir_ValueChanged);
		this.m_textBox_RoF_Mjolnir.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_textBox_RoF_Mjolnir.Location = new System.Drawing.Point(878, 260);
		this.m_textBox_RoF_Mjolnir.Multiline = false;
		this.m_textBox_RoF_Mjolnir.Name = "m_textBox_RoF_Mjolnir";
		this.m_textBox_RoF_Mjolnir.Size = new System.Drawing.Size(77, 24);
		this.m_textBox_RoF_Mjolnir.TabIndex = 68;
		this.m_textBox_RoF_Mjolnir.Text = "";
		this.m_textBox_RoF_Mjolnir.TextChanged += new System.EventHandler(m_textBox_RoF_Mjolnir_ValueChanged);
		this.m_textBox_DPS_Nova.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_textBox_DPS_Nova.Location = new System.Drawing.Point(785, 292);
		this.m_textBox_DPS_Nova.Multiline = false;
		this.m_textBox_DPS_Nova.Name = "m_textBox_DPS_Nova";
		this.m_textBox_DPS_Nova.Size = new System.Drawing.Size(77, 24);
		this.m_textBox_DPS_Nova.TabIndex = 69;
		this.m_textBox_DPS_Nova.Text = "";
		this.m_textBox_DPS_Nova.TextChanged += new System.EventHandler(m_textBox_DPS_Nova_ValueChanged);
		this.m_textBox_DPS_Antimatter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_textBox_DPS_Antimatter.Location = new System.Drawing.Point(785, 332);
		this.m_textBox_DPS_Antimatter.Multiline = false;
		this.m_textBox_DPS_Antimatter.Name = "m_textBox_DPS_Antimatter";
		this.m_textBox_DPS_Antimatter.Size = new System.Drawing.Size(77, 24);
		this.m_textBox_DPS_Antimatter.TabIndex = 70;
		this.m_textBox_DPS_Antimatter.Text = "";
		this.m_textBox_DPS_Antimatter.TextChanged += new System.EventHandler(m_textBox_DPS_Antimatter_ValueChanged);
		this.m_textBox_DPS_Void.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_textBox_DPS_Void.Location = new System.Drawing.Point(785, 364);
		this.m_textBox_DPS_Void.Multiline = false;
		this.m_textBox_DPS_Void.Name = "m_textBox_DPS_Void";
		this.m_textBox_DPS_Void.Size = new System.Drawing.Size(77, 24);
		this.m_textBox_DPS_Void.TabIndex = 71;
		this.m_textBox_DPS_Void.Text = "";
		this.m_textBox_DPS_Void.TextChanged += new System.EventHandler(m_textBox_DPS_Void_ValueChanged);
		this.m_textBox_DPS_Multifrequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_textBox_DPS_Multifrequency.Location = new System.Drawing.Point(785, 404);
		this.m_textBox_DPS_Multifrequency.Multiline = false;
		this.m_textBox_DPS_Multifrequency.Name = "m_textBox_DPS_Multifrequency";
		this.m_textBox_DPS_Multifrequency.Size = new System.Drawing.Size(77, 24);
		this.m_textBox_DPS_Multifrequency.TabIndex = 72;
		this.m_textBox_DPS_Multifrequency.Text = "";
		this.m_textBox_DPS_Multifrequency.TextChanged += new System.EventHandler(m_textBox_DPS_Multifrequency_ValueChanged);
		this.m_textBox_DPS_EMP.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_textBox_DPS_EMP.Location = new System.Drawing.Point(785, 444);
		this.m_textBox_DPS_EMP.Multiline = false;
		this.m_textBox_DPS_EMP.Name = "m_textBox_DPS_EMP";
		this.m_textBox_DPS_EMP.Size = new System.Drawing.Size(77, 24);
		this.m_textBox_DPS_EMP.TabIndex = 73;
		this.m_textBox_DPS_EMP.Text = "";
		this.m_textBox_DPS_EMP.TextChanged += new System.EventHandler(m_textBox_DPS_EMP_ValueChanged);
		this.m_textBox_DPS_Fusion.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_textBox_DPS_Fusion.Location = new System.Drawing.Point(785, 476);
		this.m_textBox_DPS_Fusion.Multiline = false;
		this.m_textBox_DPS_Fusion.Name = "m_textBox_DPS_Fusion";
		this.m_textBox_DPS_Fusion.Size = new System.Drawing.Size(77, 24);
		this.m_textBox_DPS_Fusion.TabIndex = 74;
		this.m_textBox_DPS_Fusion.Text = "";
		this.m_textBox_DPS_Fusion.TextChanged += new System.EventHandler(m_textBox_DPS_Fusion_ValueChanged);
		this.m_textBox_DPS_Phased_Plasma.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_textBox_DPS_Phased_Plasma.Location = new System.Drawing.Point(785, 508);
		this.m_textBox_DPS_Phased_Plasma.Multiline = false;
		this.m_textBox_DPS_Phased_Plasma.Name = "m_textBox_DPS_Phased_Plasma";
		this.m_textBox_DPS_Phased_Plasma.Size = new System.Drawing.Size(77, 24);
		this.m_textBox_DPS_Phased_Plasma.TabIndex = 75;
		this.m_textBox_DPS_Phased_Plasma.Text = "";
		this.m_textBox_DPS_Phased_Plasma.TextChanged += new System.EventHandler(m_textBox_DPS_Phased_Plasma_ValueChanged);
		this.m_textBox_DPS_Hail.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_textBox_DPS_Hail.Location = new System.Drawing.Point(785, 540);
		this.m_textBox_DPS_Hail.Multiline = false;
		this.m_textBox_DPS_Hail.Name = "m_textBox_DPS_Hail";
		this.m_textBox_DPS_Hail.Size = new System.Drawing.Size(77, 24);
		this.m_textBox_DPS_Hail.TabIndex = 76;
		this.m_textBox_DPS_Hail.Text = "";
		this.m_textBox_DPS_Hail.TextChanged += new System.EventHandler(m_textBox_DPS_Hail_ValueChanged);
		this.m_textBox_RoF_Nova.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_textBox_RoF_Nova.Location = new System.Drawing.Point(878, 292);
		this.m_textBox_RoF_Nova.Multiline = false;
		this.m_textBox_RoF_Nova.Name = "m_textBox_RoF_Nova";
		this.m_textBox_RoF_Nova.Size = new System.Drawing.Size(77, 24);
		this.m_textBox_RoF_Nova.TabIndex = 77;
		this.m_textBox_RoF_Nova.Text = "";
		this.m_textBox_RoF_Nova.TextChanged += new System.EventHandler(m_textBox_RoF_Nova_ValueChanged);
		this.m_textBox_RoF_Antimatter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_textBox_RoF_Antimatter.Location = new System.Drawing.Point(878, 332);
		this.m_textBox_RoF_Antimatter.Multiline = false;
		this.m_textBox_RoF_Antimatter.Name = "m_textBox_RoF_Antimatter";
		this.m_textBox_RoF_Antimatter.Size = new System.Drawing.Size(77, 24);
		this.m_textBox_RoF_Antimatter.TabIndex = 78;
		this.m_textBox_RoF_Antimatter.Text = "";
		this.m_textBox_RoF_Antimatter.TextChanged += new System.EventHandler(m_textBox_RoF_Antimatter_ValueChanged);
		this.m_textBox_RoF_Void.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_textBox_RoF_Void.Location = new System.Drawing.Point(878, 364);
		this.m_textBox_RoF_Void.Multiline = false;
		this.m_textBox_RoF_Void.Name = "m_textBox_RoF_Void";
		this.m_textBox_RoF_Void.Size = new System.Drawing.Size(77, 24);
		this.m_textBox_RoF_Void.TabIndex = 79;
		this.m_textBox_RoF_Void.Text = "";
		this.m_textBox_RoF_Void.TextChanged += new System.EventHandler(m_textBox_RoF_Void_ValueChanged);
		this.m_textBox_RoF_Multifrequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_textBox_RoF_Multifrequency.Location = new System.Drawing.Point(878, 404);
		this.m_textBox_RoF_Multifrequency.Multiline = false;
		this.m_textBox_RoF_Multifrequency.Name = "m_textBox_RoF_Multifrequency";
		this.m_textBox_RoF_Multifrequency.Size = new System.Drawing.Size(77, 24);
		this.m_textBox_RoF_Multifrequency.TabIndex = 80;
		this.m_textBox_RoF_Multifrequency.Text = "";
		this.m_textBox_RoF_Multifrequency.TextChanged += new System.EventHandler(m_textBox_RoF_Multifrequency_ValueChanged);
		this.m_textBox_RoF_EMP.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_textBox_RoF_EMP.Location = new System.Drawing.Point(878, 444);
		this.m_textBox_RoF_EMP.Multiline = false;
		this.m_textBox_RoF_EMP.Name = "m_textBox_RoF_EMP";
		this.m_textBox_RoF_EMP.Size = new System.Drawing.Size(77, 24);
		this.m_textBox_RoF_EMP.TabIndex = 81;
		this.m_textBox_RoF_EMP.Text = "";
		this.m_textBox_RoF_EMP.TextChanged += new System.EventHandler(m_textBox_RoF_EMP_ValueChanged);
		this.m_textBox_RoF_Fusion.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_textBox_RoF_Fusion.Location = new System.Drawing.Point(878, 476);
		this.m_textBox_RoF_Fusion.Multiline = false;
		this.m_textBox_RoF_Fusion.Name = "m_textBox_RoF_Fusion";
		this.m_textBox_RoF_Fusion.Size = new System.Drawing.Size(77, 24);
		this.m_textBox_RoF_Fusion.TabIndex = 82;
		this.m_textBox_RoF_Fusion.Text = "";
		this.m_textBox_RoF_Fusion.TextChanged += new System.EventHandler(m_textBox_RoF_Fusion_ValueChanged);
		this.m_textBox_RoF_Phased_Plasma.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_textBox_RoF_Phased_Plasma.Location = new System.Drawing.Point(878, 508);
		this.m_textBox_RoF_Phased_Plasma.Multiline = false;
		this.m_textBox_RoF_Phased_Plasma.Name = "m_textBox_RoF_Phased_Plasma";
		this.m_textBox_RoF_Phased_Plasma.Size = new System.Drawing.Size(77, 24);
		this.m_textBox_RoF_Phased_Plasma.TabIndex = 83;
		this.m_textBox_RoF_Phased_Plasma.Text = "";
		this.m_textBox_RoF_Phased_Plasma.TextChanged += new System.EventHandler(m_textBox_RoF_Phased_Plasma_ValueChanged);
		this.m_textBox_RoF_Hail.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_textBox_RoF_Hail.Location = new System.Drawing.Point(878, 540);
		this.m_textBox_RoF_Hail.Multiline = false;
		this.m_textBox_RoF_Hail.Name = "m_textBox_RoF_Hail";
		this.m_textBox_RoF_Hail.Size = new System.Drawing.Size(77, 24);
		this.m_textBox_RoF_Hail.TabIndex = 84;
		this.m_textBox_RoF_Hail.Text = "";
		this.m_textBox_RoF_Hail.TextChanged += new System.EventHandler(m_textBox_RoF_Hail_ValueChanged);
		this.m_radioPassive.AutoSize = true;
		this.m_radioPassive.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold);
		this.m_radioPassive.Location = new System.Drawing.Point(365, 635);
		this.m_radioPassive.Margin = new System.Windows.Forms.Padding(2);
		this.m_radioPassive.Name = "m_radioPassive";
		this.m_radioPassive.Size = new System.Drawing.Size(88, 24);
		this.m_radioPassive.TabIndex = 85;
		this.m_radioPassive.Text = "Passive";
		this.m_radioPassive.UseVisualStyleBackColor = true;
		this.m_radioPassive.CheckedChanged += new System.EventHandler(m_radioPassive_CheckedChanged);
		this.m_radioCold.AutoSize = true;
		this.m_radioCold.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold);
		this.m_radioCold.Location = new System.Drawing.Point(453, 635);
		this.m_radioCold.Margin = new System.Windows.Forms.Padding(2);
		this.m_radioCold.Name = "m_radioCold";
		this.m_radioCold.Size = new System.Drawing.Size(63, 24);
		this.m_radioCold.TabIndex = 86;
		this.m_radioCold.Text = "Cold";
		this.m_radioCold.UseVisualStyleBackColor = true;
		this.m_radioCold.CheckedChanged += new System.EventHandler(m_radioPassive_CheckedChanged);
		this.m_radioHot.AutoSize = true;
		this.m_radioHot.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold);
		this.m_radioHot.Location = new System.Drawing.Point(518, 635);
		this.m_radioHot.Margin = new System.Windows.Forms.Padding(2);
		this.m_radioHot.Name = "m_radioHot";
		this.m_radioHot.Size = new System.Drawing.Size(56, 24);
		this.m_radioHot.TabIndex = 87;
		this.m_radioHot.Text = "Hot";
		this.m_radioHot.UseVisualStyleBackColor = true;
		this.m_radioHot.CheckedChanged += new System.EventHandler(m_radioPassive_CheckedChanged);
		this.panel1.BackColor = System.Drawing.SystemColors.ActiveBorder;
		this.panel1.Location = new System.Drawing.Point(350, 324);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(618, 114);
		this.panel1.TabIndex = 90;
		this.panel2.BackColor = System.Drawing.SystemColors.ScrollBar;
		this.panel2.Location = new System.Drawing.Point(350, 438);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(618, 133);
		this.panel2.TabIndex = 91;
		this.panel3.BackColor = System.Drawing.SystemColors.ScrollBar;
		this.panel3.Location = new System.Drawing.Point(350, 248);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(618, 77);
		this.panel3.TabIndex = 91;
		this.m_textBox_RoF_VoidL.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_textBox_RoF_VoidL.Location = new System.Drawing.Point(878, 584);
		this.m_textBox_RoF_VoidL.Multiline = false;
		this.m_textBox_RoF_VoidL.Name = "m_textBox_RoF_VoidL";
		this.m_textBox_RoF_VoidL.Size = new System.Drawing.Size(77, 24);
		this.m_textBox_RoF_VoidL.TabIndex = 96;
		this.m_textBox_RoF_VoidL.Text = "";
		this.m_textBox_RoF_VoidL.TextChanged += new System.EventHandler(m_textBox_RoF_VoidL_ValueChanged);
		this.m_textBox_DPS_VoidL.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_textBox_DPS_VoidL.Location = new System.Drawing.Point(785, 584);
		this.m_textBox_DPS_VoidL.Multiline = false;
		this.m_textBox_DPS_VoidL.Name = "m_textBox_DPS_VoidL";
		this.m_textBox_DPS_VoidL.Size = new System.Drawing.Size(77, 24);
		this.m_textBox_DPS_VoidL.TabIndex = 95;
		this.m_textBox_DPS_VoidL.Text = "";
		this.m_textBox_DPS_VoidL.TextChanged += new System.EventHandler(m_textBox_DPS_VoidL_ValueChanged);
		this.label19.AutoSize = true;
		this.label19.BackColor = System.Drawing.SystemColors.ActiveBorder;
		this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.label19.Location = new System.Drawing.Point(360, 586);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(60, 20);
		this.label19.TabIndex = 94;
		this.label19.Text = "Void L";
		this.toolTip1.SetToolTip(this.label19, "Talos (no drones)");
		this.m_TextBoxEHPVoidLHot.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_TextBoxEHPVoidLHot.Location = new System.Drawing.Point(768, 584);
		this.m_TextBoxEHPVoidLHot.Multiline = false;
		this.m_TextBoxEHPVoidLHot.Name = "m_TextBoxEHPVoidLHot";
		this.m_TextBoxEHPVoidLHot.ReadOnly = true;
		this.m_TextBoxEHPVoidLHot.Size = new System.Drawing.Size(120, 24);
		this.m_TextBoxEHPVoidLHot.TabIndex = 93;
		this.m_TextBoxEHPVoidLHot.Text = "";
		this.m_TextBoxEHPVoidLCold.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_TextBoxEHPVoidLCold.Location = new System.Drawing.Point(528, 584);
		this.m_TextBoxEHPVoidLCold.Multiline = false;
		this.m_TextBoxEHPVoidLCold.Name = "m_TextBoxEHPVoidLCold";
		this.m_TextBoxEHPVoidLCold.ReadOnly = true;
		this.m_TextBoxEHPVoidLCold.Size = new System.Drawing.Size(120, 24);
		this.m_TextBoxEHPVoidLCold.TabIndex = 92;
		this.m_TextBoxEHPVoidLCold.Text = "";
		this.panel4.BackColor = System.Drawing.SystemColors.ActiveBorder;
		this.panel4.Location = new System.Drawing.Point(350, 570);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(618, 54);
		this.panel4.TabIndex = 91;
		this.labelSeconds.AutoSize = true;
		this.labelSeconds.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.labelSeconds.Location = new System.Drawing.Point(798, 685);
		this.labelSeconds.Name = "labelSeconds";
		this.labelSeconds.Size = new System.Drawing.Size(79, 20);
		this.labelSeconds.TabIndex = 97;
		this.labelSeconds.Text = "Seconds";
		this.labelBR.AutoSize = true;
		this.labelBR.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.labelBR.ForeColor = System.Drawing.SystemColors.HotTrack;
		this.labelBR.Location = new System.Drawing.Point(13, 640);
		this.labelBR.Name = "labelBR";
		this.labelBR.Size = new System.Drawing.Size(29, 16);
		this.labelBR.TabIndex = 98;
		this.labelBR.Text = "BR";
		this.labelBR.Visible = false;
		this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.button1.Location = new System.Drawing.Point(475, 63);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(68, 29);
		this.button1.TabIndex = 99;
		this.button1.Text = "3 Exp";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.button2.Location = new System.Drawing.Point(559, 63);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(68, 29);
		this.button2.TabIndex = 100;
		this.button2.Text = "3 Bulk";
		this.button2.UseVisualStyleBackColor = true;
		this.button2.Click += new System.EventHandler(button2_Click);
		this.m_checkBoxManualEHP.AutoSize = true;
		this.m_checkBoxManualEHP.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_checkBoxManualEHP.Location = new System.Drawing.Point(350, 222);
		this.m_checkBoxManualEHP.Name = "m_checkBoxManualEHP";
		this.m_checkBoxManualEHP.Size = new System.Drawing.Size(127, 24);
		this.m_checkBoxManualEHP.TabIndex = 101;
		this.m_checkBoxManualEHP.Text = "Manual EHP";
		this.m_checkBoxManualEHP.UseVisualStyleBackColor = true;
		this.m_checkBoxManualEHP.CheckedChanged += new System.EventHandler(m_checkBoxManualEHP_CheckedChanged);
		this.m_richTextBoxManualEHP.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
		this.m_richTextBoxManualEHP.Location = new System.Drawing.Point(472, 221);
		this.m_richTextBoxManualEHP.Multiline = false;
		this.m_richTextBoxManualEHP.Name = "m_richTextBoxManualEHP";
		this.m_richTextBoxManualEHP.Size = new System.Drawing.Size(120, 24);
		this.m_richTextBoxManualEHP.TabIndex = 102;
		this.m_richTextBoxManualEHP.Text = "";
		this.m_richTextBoxManualEHP.TextChanged += new System.EventHandler(m_richTextBoxManualEHP_TextChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.SystemColors.ControlLight;
		base.ClientSize = new System.Drawing.Size(968, 772);
		base.Controls.Add(this.m_richTextBoxManualEHP);
		base.Controls.Add(this.m_checkBoxManualEHP);
		base.Controls.Add(this.button2);
		base.Controls.Add(this.button1);
		base.Controls.Add(this.labelBR);
		base.Controls.Add(this.labelSeconds);
		base.Controls.Add(this.m_textBox_RoF_VoidL);
		base.Controls.Add(this.m_textBox_DPS_VoidL);
		base.Controls.Add(this.label19);
		base.Controls.Add(this.m_TextBoxEHPVoidLHot);
		base.Controls.Add(this.m_TextBoxEHPVoidLCold);
		base.Controls.Add(this.m_checkBoxSTK);
		base.Controls.Add(this.m_radioHot);
		base.Controls.Add(this.m_radioCold);
		base.Controls.Add(this.m_radioPassive);
		base.Controls.Add(this.m_textBox_RoF_Hail);
		base.Controls.Add(this.m_textBox_RoF_Phased_Plasma);
		base.Controls.Add(this.m_textBox_RoF_Fusion);
		base.Controls.Add(this.m_textBox_RoF_EMP);
		base.Controls.Add(this.m_textBox_RoF_Multifrequency);
		base.Controls.Add(this.m_textBox_RoF_Void);
		base.Controls.Add(this.m_textBox_RoF_Antimatter);
		base.Controls.Add(this.m_textBox_RoF_Nova);
		base.Controls.Add(this.m_textBox_DPS_Hail);
		base.Controls.Add(this.m_textBox_DPS_Phased_Plasma);
		base.Controls.Add(this.m_textBox_DPS_Fusion);
		base.Controls.Add(this.m_textBox_DPS_EMP);
		base.Controls.Add(this.m_textBox_DPS_Multifrequency);
		base.Controls.Add(this.m_textBox_DPS_Void);
		base.Controls.Add(this.m_textBox_DPS_Antimatter);
		base.Controls.Add(this.m_textBox_DPS_Nova);
		base.Controls.Add(this.m_textBox_RoF_Mjolnir);
		base.Controls.Add(this.m_textBox_DPS_Mjolnir);
		base.Controls.Add(this.labelSTK);
		base.Controls.Add(this.labelRoF);
		base.Controls.Add(this.labelDPS);
		base.Controls.Add(this.labelSysSecurity);
		base.Controls.Add(this.m_comboBoxSysSecurity);
		base.Controls.Add(this.m_History);
		base.Controls.Add(this.m_checkBoxADCActive);
		base.Controls.Add(this.label18);
		base.Controls.Add(this.label17);
		base.Controls.Add(this.label16);
		base.Controls.Add(this.label15);
		base.Controls.Add(this.m_ValueCanDropText);
		base.Controls.Add(this.m_ValueTotalText);
		base.Controls.Add(this.m_ValueFittingsText);
		base.Controls.Add(this.m_ValueHullText);
		base.Controls.Add(this.m_checkBoxPassive);
		base.Controls.Add(this.m_FitText);
		base.Controls.Add(this.label14);
		base.Controls.Add(this.m_TextBoxEHPHailHot);
		base.Controls.Add(this.m_TextBoxEHPHailCold);
		base.Controls.Add(this.label13);
		base.Controls.Add(this.m_TextBoxEHPPhasedPlasmaHot);
		base.Controls.Add(this.m_TextBoxEHPPhasedPlasmaCold);
		base.Controls.Add(this.label12);
		base.Controls.Add(this.m_TextBoxEHPFusionHot);
		base.Controls.Add(this.m_TextBoxEHPFusionCold);
		base.Controls.Add(this.label11);
		base.Controls.Add(this.m_TextBoxEHPEMPHot);
		base.Controls.Add(this.m_TextBoxEHPEMPCold);
		base.Controls.Add(this.label10);
		base.Controls.Add(this.m_TextBoxEHPMultifreqHot);
		base.Controls.Add(this.m_TextBoxEHPMultifreqCold);
		base.Controls.Add(this.label9);
		base.Controls.Add(this.m_TextBoxEHPVoidHot);
		base.Controls.Add(this.m_TextBoxEHPVoidCold);
		base.Controls.Add(this.label8);
		base.Controls.Add(this.m_TextBoxEHPAntimatterHot);
		base.Controls.Add(this.m_TextBoxEHPAntimatterCold);
		base.Controls.Add(this.label7);
		base.Controls.Add(this.m_TextBoxEHPNovaHot);
		base.Controls.Add(this.m_TextBoxEHPNovaCold);
		base.Controls.Add(this.label6);
		base.Controls.Add(this.m_TextBoxEHPMjolnirHot);
		base.Controls.Add(this.m_TextBoxEHPMjolnirCold);
		base.Controls.Add(this.label5);
		base.Controls.Add(this.label4);
		base.Controls.Add(this.m_TextBoxHullResistsHot);
		base.Controls.Add(this.m_TextBoxHullResistsCold);
		base.Controls.Add(this.m_TextBoxArmorResistsHot);
		base.Controls.Add(this.m_TextBoxArmorResistsCold);
		base.Controls.Add(this.m_TextBoxShieldResistsHot);
		base.Controls.Add(this.m_TextBoxShieldResistsCold);
		base.Controls.Add(this.m_TextBoxHullHP);
		base.Controls.Add(this.m_TextBoxArmorHP);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.m_TextBoxShieldHP);
		base.Controls.Add(this.m_ButtonCopyEFT);
		base.Controls.Add(this.m_ComboBoxShipType);
		base.Controls.Add(this.m_ButtonResetFit);
		base.Controls.Add(this.menuStrip1);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel4);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MainMenuStrip = this.menuStrip1;
		base.MaximizeBox = false;
		this.MinimumSize = new System.Drawing.Size(984, 580);
		base.Name = "Form1";
		this.Text = "Miniluv fit scanner";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Form1_FormClosing);
		base.Load += new System.EventHandler(Form1_Load);
		this.menuStrip1.ResumeLayout(false);
		this.menuStrip1.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	private void UpdateHistoryFit()
	{
		if (!m_InsideUpdate)
		{
			m_InsideUpdate = true;
			if (m_FitScanProcessor.ValidFit)
			{
				m_HistoryManager.OnFitChanged(m_FitScanProcessor.ShipName, m_FitScanProcessor.HighSlots, m_FitScanProcessor.HighPowerModules, m_FitScanProcessor.MediumSlots, m_FitScanProcessor.MediumPowerModules, m_FitScanProcessor.LowSlots, m_FitScanProcessor.LowPowerModules, m_FitScanProcessor.RigSlots, m_FitScanProcessor.Rigs, m_FitScanProcessor.SubsystemSlots, m_FitScanProcessor.SubsystemModules);
				UpdateHistoryList();
			}
			m_InsideUpdate = false;
		}
	}

	private void UpdateHistoryTank(float EHP)
	{
		if (m_FitScanProcessor.ValidFit && m_History.SelectedIndex == 0)
		{
			m_HistoryManager.OnEHPChanged(EHP);
			UpdateHistoryList();
		}
	}

	private void UpdateHistoryPrice(float price)
	{
		if (m_FitScanProcessor.ValidFit && m_History.SelectedIndex == 0)
		{
			m_HistoryManager.OnPriceChanged(price);
			UpdateHistoryList();
		}
	}

	private void m_History_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!m_bIgnoreIndexChanges)
		{
			m_bInsideIndexChange = true;
			m_FitScanProcessor.NewPaste(m_HistoryManager.GetFitAt(m_History.SelectedIndex), m_checkBoxPassive.Checked, m_checkBoxADCActive.Checked);
			m_bInsideIndexChange = false;
		}
	}

	private void UpdateHistoryList()
	{
		List<string> list = new List<string>();
		for (int i = 0; i < m_HistoryManager.Count; i++)
		{
			list.Add(m_HistoryManager.GetSummaryAt(i));
		}
		m_bIgnoreIndexChanges = true;
		m_History.Items.Clear();
		ComboBox.ObjectCollection items = m_History.Items;
		object[] items2 = list.ToArray();
		items.AddRange(items2);
		if (list.Count > 0)
		{
			m_History.SelectedIndex = 0;
		}
		m_bIgnoreIndexChanges = false;
	}

	private void OnNewItemsWithUnknownPrices()
	{
		if (!ConfigHelper.Instance.GetPrices)
		{
			return;
		}
		m_Guard.WaitOne();
		foreach (string itemsWithUnknownPrice in m_FitScanProcessor.ItemsWithUnknownPrices)
		{
			if (!m_ItemsWithUnknownPrices.ContainsKey(itemsWithUnknownPrice))
			{
				m_ItemsWithUnknownPrices.Add(itemsWithUnknownPrice, 1);
			}
		}
		bool flag = m_ItemsWithUnknownPrices.Count == 0;
		m_Guard.ReleaseMutex();
		if (!m_BackgroundWorkerPrices.IsBusy && !flag)
		{
			RestartWorker();
		}
	}

	private void RestartWorker()
	{
		Debug.Assert(!m_BackgroundWorkerPrices.IsBusy);
		m_Guard.WaitOne();
		List<string> list = m_ItemsWithUnknownPrices.Keys.ToList();
		m_ItemsWithUnknownPrices.Clear();
		m_Guard.ReleaseMutex();
		if (list.Count > 0)
		{
			m_BackgroundWorkerPrices.RunWorkerAsync(list);
		}
	}

	private void BackgroundWorkerPrices_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		Debug.Assert(!m_BackgroundWorkerPrices.IsBusy);
		if (e.Error == null && e.Result is IReadOnlyDictionary<string, double> { Count: >0 } readOnlyDictionary)
		{
			m_FitScanProcessor.ConsumeNewPrices(readOnlyDictionary);
		}
		RestartWorker();
	}

	private void BackgroundWorkerPrices_DoWork(object sender, DoWorkEventArgs e)
	{
		IList<string> items = (IList<string>)e.Argument;
		e.Result = FetchPrices(items);
	}

	private IReadOnlyDictionary<string, double> FetchPrices(IList<string> items)
	{
		try
		{
			IPriceProvider priceProvider = CreatePriceProvider();
			if (priceProvider == null)
			{
				return new Dictionary<string, double>();
			}
			return priceProvider.GetPricesAsync(items, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
				.GetResult();
		}
		catch (Exception)
		{
			return new Dictionary<string, double>();
		}
	}

	private IPriceProvider CreatePriceProvider()
	{
		string priceProvider = ConfigHelper.Instance.PriceProvider;
		if (string.Equals(priceProvider, "Janice", StringComparison.OrdinalIgnoreCase))
		{
			string janiceApiKey = ConfigHelper.Instance.JaniceApiKey;
			if (string.IsNullOrWhiteSpace(janiceApiKey))
			{
				return null;
			}
			return new JanicePriceProvider(janiceApiKey);
		}
		if (string.Equals(priceProvider, "Fuzzwork", StringComparison.OrdinalIgnoreCase))
		{
			return new FuzzworkPriceProvider(m_FitScanProcessor);
		}
		return new GoonpraisalPriceProvider();
	}

	private void RefreshShipCombo(string filterText = null, bool keepTypedText = false)
	{
		if (m_FitScanProcessor != null)
		{
			string text = (keepTypedText ? m_ComboBoxShipType.Text : (filterText ?? ""));
			IReadOnlyCollection<string> readOnlyCollection;
			if (!string.IsNullOrEmpty(text))
			{
				readOnlyCollection = m_FitScanProcessor.SuggestNames(text);
			}
			else
			{
				IReadOnlyCollection<string> allShipNames = m_FitScanProcessor.GetAllShipNames();
				readOnlyCollection = allShipNames;
			}
			IReadOnlyCollection<string> collection = readOnlyCollection;
			m_ComboBoxItems = new List<string>(collection);
			m_BindingSource.DataSource = null;
			m_BindingSource.DataSource = m_ComboBoxItems;
			m_ComboBoxShipType.DataSource = null;
			m_ComboBoxShipType.DataSource = m_BindingSource;
			m_ComboBoxShipType.SelectedIndex = -1;
			if (keepTypedText)
			{
				m_ComboBoxShipType.Text = text;
				m_ComboBoxShipType.SelectionStart = text.Length;
				m_ComboBoxShipType.SelectionLength = 0;
			}
		}
	}

	private void m_ComboBoxShipType_TextUpdate(object sender, EventArgs e)
	{
		string text = m_ComboBoxShipType.Text;
		RefreshShipCombo(text, keepTypedText: true);
		m_ComboBoxShipType.DroppedDown = true;
		System.Windows.Forms.Cursor.Current = Cursors.Default;
	}

	private void m_ComboBoxShipType_DropDown(object sender, EventArgs e)
	{
		if (m_ComboBoxItems == null || m_ComboBoxItems.Count == 0)
		{
			RefreshShipCombo(m_ComboBoxShipType.Text, keepTypedText: true);
		}
	}

	private void m_ComboBoxShipType_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (m_ComboBoxShipType.SelectedIndex >= 0)
		{
			string text = m_ComboBoxShipType.Text;
			m_FitScanProcessor.SetShipName(text, m_checkBoxPassive.Checked, m_checkBoxADCActive.Checked);
			Label label = labelBR;
			int visible;
			switch (text)
			{
			default:
				visible = ((text == "Viator") ? 1 : 0);
				break;
			case "Crane":
			case "Prorator":
			case "Prowler":
				visible = 1;
				break;
			}
			label.Visible = (byte)visible != 0;
		}
	}

	private void OnFitValueChanged()
	{
		Tuple<float, float, float, float> fitValue = m_FitScanProcessor.FitValue;
		m_ValueHullText.Clear();
		m_ValueHullText.AppendText($"{fitValue.Item1:N0}");
		m_ValueHullText.SelectAll();
		m_ValueHullText.SelectionAlignment = HorizontalAlignment.Right;
		m_ValueHullText.SelectionLength = 0;
		m_ValueFittingsText.Clear();
		m_ValueFittingsText.AppendText($"{fitValue.Item2:N0}");
		m_ValueFittingsText.SelectAll();
		m_ValueFittingsText.SelectionAlignment = HorizontalAlignment.Right;
		m_ValueFittingsText.SelectionLength = 0;
		m_ValueTotalText.Clear();
		m_ValueTotalText.AppendText($"{fitValue.Item3:N0}");
		m_ValueTotalText.SelectAll();
		m_ValueTotalText.SelectionAlignment = HorizontalAlignment.Right;
		m_ValueTotalText.SelectionLength = 0;
		m_ValueCanDropText.Clear();
		m_ValueCanDropText.AppendText($"{fitValue.Item4:N0}");
		m_ValueCanDropText.SelectAll();
		m_ValueCanDropText.SelectionAlignment = HorizontalAlignment.Right;
		m_ValueCanDropText.SelectionLength = 0;
		UpdateHistoryPrice(fitValue.Item3);
	}

	private void OnShipFitChanged()
	{
		m_FitText.Clear();
		if (!m_FitScanProcessor.ValidFit)
		{
			m_FitText.AppendText("INVALID FIT" + Environment.NewLine + Environment.NewLine);
			int textLength = m_FitText.TextLength;
			m_FitText.SelectAll();
			m_FitText.SelectionColor = Color.Red;
			m_FitText.SelectionLength = 0;
		}
		m_FitText.AppendText(m_FitScanProcessor.EFTFit);
		HighlightFit();
		if (!m_bInsideIndexChange)
		{
			UpdateHistoryFit();
		}
		if (ConfigHelper.Instance.ActivateOnFitUpdate && !base.TopMost)
		{
			Activate();
		}
	}

	private void HighlightFit()
	{
		if (ConfigHelper.Instance.Highlight && m_FitScanProcessor.FullFitKnown)
		{
			m_FitText.BackColor = Color.LightGreen;
		}
		else if (ConfigHelper.Instance.Highlight && m_FitScanProcessor.FullTankKnown)
		{
			m_FitText.BackColor = Color.Yellow;
		}
		else
		{
			m_FitText.BackColor = Color.White;
		}
	}

	private void OnShipTankChanged()
	{
		if (m_checkBoxSTK.Checked)
		{
			OnShipTankChangedSTK();
		}
		else
		{
			OnShipTankChangedHotCold();
		}
		float eHP = EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResists, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResists, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResists, EhpCalculator.AmmoUniform);
		UpdateHistoryTank(eHP);
	}

	private void OnShipTankChangedSTK()
	{
		if (m_checkBoxManualEHP.Checked)
		{
			m_TextBoxShieldHP.Text = "";
			m_TextBoxArmorHP.Text = "";
			m_TextBoxHullHP.Text = "";
			m_TextBoxShieldResistsCold.Text = "";
			m_TextBoxArmorResistsCold.Text = "";
			m_TextBoxHullResistsCold.Text = "";
			FormatEHP(m_TextBoxEHPMjolnirCold, ConfigHelper.Instance.Manual_EHP);
			FormatEHP(m_TextBoxEHPNovaCold, ConfigHelper.Instance.Manual_EHP);
			FormatEHP(m_TextBoxEHPAntimatterCold, ConfigHelper.Instance.Manual_EHP);
			FormatEHP(m_TextBoxEHPVoidCold, ConfigHelper.Instance.Manual_EHP);
			FormatEHP(m_TextBoxEHPVoidLCold, ConfigHelper.Instance.Manual_EHP);
			FormatEHP(m_TextBoxEHPMultifreqCold, ConfigHelper.Instance.Manual_EHP);
			FormatEHP(m_TextBoxEHPEMPCold, ConfigHelper.Instance.Manual_EHP);
			FormatEHP(m_TextBoxEHPFusionCold, ConfigHelper.Instance.Manual_EHP);
			FormatEHP(m_TextBoxEHPPhasedPlasmaCold, ConfigHelper.Instance.Manual_EHP);
			FormatEHP(m_TextBoxEHPHailCold, ConfigHelper.Instance.Manual_EHP);
		}
		else
		{
			m_TextBoxShieldHP.Text = $"{m_FitScanProcessor.ShieldHP}";
			m_TextBoxArmorHP.Text = $"{m_FitScanProcessor.ArmorHP}";
			m_TextBoxHullHP.Text = $"{m_FitScanProcessor.HullHP}";
			if (m_radioHot.Checked)
			{
				FormatResists(m_TextBoxShieldResistsCold, m_FitScanProcessor.ShieldResistsHeated);
				FormatResists(m_TextBoxArmorResistsCold, m_FitScanProcessor.ArmorResistsHeated);
				FormatResists(m_TextBoxHullResistsCold, m_FitScanProcessor.HullResistsHeated);
				FormatEHP(m_TextBoxEHPMjolnirCold, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResistsHeated, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResistsHeated, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResistsHeated, EhpCalculator.AmmoMjolnir));
				FormatEHP(m_TextBoxEHPNovaCold, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResistsHeated, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResistsHeated, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResistsHeated, EhpCalculator.AmmoNova));
				FormatEHP(m_TextBoxEHPAntimatterCold, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResistsHeated, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResistsHeated, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResistsHeated, EhpCalculator.AmmoAntimatter));
				FormatEHP(m_TextBoxEHPVoidCold, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResistsHeated, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResistsHeated, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResistsHeated, EhpCalculator.AmmoVoid));
				FormatEHP(m_TextBoxEHPVoidLCold, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResistsHeated, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResistsHeated, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResistsHeated, EhpCalculator.AmmoVoid));
				FormatEHP(m_TextBoxEHPMultifreqCold, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResistsHeated, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResistsHeated, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResistsHeated, EhpCalculator.AmmoMultifreq));
				FormatEHP(m_TextBoxEHPEMPCold, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResistsHeated, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResistsHeated, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResistsHeated, EhpCalculator.AmmoEMP));
				FormatEHP(m_TextBoxEHPFusionCold, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResistsHeated, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResistsHeated, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResistsHeated, EhpCalculator.AmmoFusion));
				FormatEHP(m_TextBoxEHPPhasedPlasmaCold, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResistsHeated, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResistsHeated, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResistsHeated, EhpCalculator.AmmoPhasedPlasma));
				FormatEHP(m_TextBoxEHPHailCold, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResistsHeated, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResistsHeated, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResistsHeated, EhpCalculator.AmmoHail));
			}
			else
			{
				FormatResists(m_TextBoxShieldResistsCold, m_FitScanProcessor.ShieldResists);
				FormatResists(m_TextBoxArmorResistsCold, m_FitScanProcessor.ArmorResists);
				FormatResists(m_TextBoxHullResistsCold, m_FitScanProcessor.HullResists);
				FormatEHP(m_TextBoxEHPMjolnirCold, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResists, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResists, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResists, EhpCalculator.AmmoMjolnir));
				FormatEHP(m_TextBoxEHPNovaCold, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResists, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResists, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResists, EhpCalculator.AmmoNova));
				FormatEHP(m_TextBoxEHPAntimatterCold, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResists, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResists, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResists, EhpCalculator.AmmoAntimatter));
				FormatEHP(m_TextBoxEHPVoidCold, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResists, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResists, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResists, EhpCalculator.AmmoVoid));
				FormatEHP(m_TextBoxEHPVoidLCold, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResists, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResists, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResists, EhpCalculator.AmmoVoid));
				FormatEHP(m_TextBoxEHPMultifreqCold, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResists, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResists, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResists, EhpCalculator.AmmoMultifreq));
				FormatEHP(m_TextBoxEHPEMPCold, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResists, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResists, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResists, EhpCalculator.AmmoEMP));
				FormatEHP(m_TextBoxEHPFusionCold, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResists, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResists, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResists, EhpCalculator.AmmoFusion));
				FormatEHP(m_TextBoxEHPPhasedPlasmaCold, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResists, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResists, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResists, EhpCalculator.AmmoPhasedPlasma));
				FormatEHP(m_TextBoxEHPHailCold, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResists, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResists, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResists, EhpCalculator.AmmoHail));
			}
		}
		string text = m_comboBoxSysSecurity.Text;
		FormatEHP(m_TextBoxEHPMjolnirHot, int.Parse(m_gankShips.NumShipToKill(text, ConfigHelper.Instance.DPS_Mjolnir, ConfigHelper.Instance.RoF_Mjolnir, int.Parse(m_TextBoxEHPMjolnirCold.Text, NumberStyles.AllowThousands))));
		FormatEHP(m_TextBoxEHPNovaHot, int.Parse(m_gankShips.NumShipToKill(text, ConfigHelper.Instance.DPS_Nova, ConfigHelper.Instance.RoF_Nova, int.Parse(m_TextBoxEHPNovaCold.Text, NumberStyles.AllowThousands))));
		FormatEHP(m_TextBoxEHPAntimatterHot, int.Parse(m_gankShips.NumShipToKill(text, ConfigHelper.Instance.DPS_Antimatter, ConfigHelper.Instance.RoF_Antimatter, int.Parse(m_TextBoxEHPAntimatterCold.Text, NumberStyles.AllowThousands))));
		FormatEHP(m_TextBoxEHPVoidHot, int.Parse(m_gankShips.NumShipToKill(text, ConfigHelper.Instance.DPS_Void, ConfigHelper.Instance.RoF_Void, int.Parse(m_TextBoxEHPVoidCold.Text, NumberStyles.AllowThousands))));
		FormatEHP(m_TextBoxEHPVoidLHot, int.Parse(m_gankShips.NumShipToKill(text, ConfigHelper.Instance.DPS_VoidL, ConfigHelper.Instance.RoF_VoidL, int.Parse(m_TextBoxEHPVoidLCold.Text, NumberStyles.AllowThousands))));
		FormatEHP(m_TextBoxEHPMultifreqHot, int.Parse(m_gankShips.NumShipToKill(text, ConfigHelper.Instance.DPS_Multifrequency, ConfigHelper.Instance.RoF_Multifrequency, int.Parse(m_TextBoxEHPMultifreqCold.Text, NumberStyles.AllowThousands))));
		FormatEHP(m_TextBoxEHPEMPHot, int.Parse(m_gankShips.NumShipToKill(text, ConfigHelper.Instance.DPS_EMP, ConfigHelper.Instance.RoF_EMP, int.Parse(m_TextBoxEHPEMPCold.Text, NumberStyles.AllowThousands))));
		FormatEHP(m_TextBoxEHPFusionHot, int.Parse(m_gankShips.NumShipToKill(text, ConfigHelper.Instance.DPS_Fusion, ConfigHelper.Instance.RoF_Fusion, int.Parse(m_TextBoxEHPFusionCold.Text, NumberStyles.AllowThousands))));
		FormatEHP(m_TextBoxEHPPhasedPlasmaHot, int.Parse(m_gankShips.NumShipToKill(text, ConfigHelper.Instance.DPS_Phased_Plasma, ConfigHelper.Instance.RoF_Phased_Plasma, int.Parse(m_TextBoxEHPPhasedPlasmaCold.Text, NumberStyles.AllowThousands))));
		FormatEHP(m_TextBoxEHPHailHot, int.Parse(m_gankShips.NumShipToKill(text, ConfigHelper.Instance.DPS_Hail, ConfigHelper.Instance.RoF_Hail, int.Parse(m_TextBoxEHPHailCold.Text, NumberStyles.AllowThousands))));
	}

	private void OnShipTankChangedHotCold()
	{
		m_TextBoxShieldHP.Text = $"{m_FitScanProcessor.ShieldHP}";
		m_TextBoxArmorHP.Text = $"{m_FitScanProcessor.ArmorHP}";
		m_TextBoxHullHP.Text = $"{m_FitScanProcessor.HullHP}";
		FormatResists(m_TextBoxShieldResistsCold, m_FitScanProcessor.ShieldResists);
		FormatResists(m_TextBoxArmorResistsCold, m_FitScanProcessor.ArmorResists);
		FormatResists(m_TextBoxHullResistsCold, m_FitScanProcessor.HullResists);
		FormatEHP(m_TextBoxEHPMjolnirCold, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResists, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResists, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResists, EhpCalculator.AmmoMjolnir));
		FormatEHP(m_TextBoxEHPNovaCold, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResists, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResists, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResists, EhpCalculator.AmmoNova));
		FormatEHP(m_TextBoxEHPAntimatterCold, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResists, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResists, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResists, EhpCalculator.AmmoAntimatter));
		FormatEHP(m_TextBoxEHPVoidCold, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResists, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResists, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResists, EhpCalculator.AmmoVoid));
		FormatEHP(m_TextBoxEHPMultifreqCold, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResists, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResists, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResists, EhpCalculator.AmmoMultifreq));
		FormatEHP(m_TextBoxEHPEMPCold, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResists, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResists, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResists, EhpCalculator.AmmoEMP));
		FormatEHP(m_TextBoxEHPFusionCold, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResists, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResists, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResists, EhpCalculator.AmmoFusion));
		FormatEHP(m_TextBoxEHPPhasedPlasmaCold, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResists, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResists, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResists, EhpCalculator.AmmoPhasedPlasma));
		FormatEHP(m_TextBoxEHPHailCold, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResists, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResists, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResists, EhpCalculator.AmmoHail));
		if (m_FitScanProcessor.PassiveTank)
		{
			m_TextBoxShieldResistsHot.Clear();
			m_TextBoxArmorResistsHot.Clear();
			m_TextBoxHullResistsHot.Clear();
			m_TextBoxEHPMjolnirHot.Clear();
			m_TextBoxEHPNovaHot.Clear();
			m_TextBoxEHPAntimatterHot.Clear();
			m_TextBoxEHPVoidHot.Clear();
			m_TextBoxEHPMultifreqHot.Clear();
			m_TextBoxEHPEMPHot.Clear();
			m_TextBoxEHPFusionHot.Clear();
			m_TextBoxEHPPhasedPlasmaHot.Clear();
			m_TextBoxEHPHailHot.Clear();
			return;
		}
		m_TextBoxShieldResistsHot.Enabled = true;
		m_TextBoxArmorResistsHot.Enabled = true;
		m_TextBoxHullResistsHot.Enabled = true;
		m_TextBoxEHPMjolnirHot.Enabled = true;
		m_TextBoxEHPNovaHot.Enabled = true;
		m_TextBoxEHPAntimatterHot.Enabled = true;
		m_TextBoxEHPVoidHot.Enabled = true;
		m_TextBoxEHPMultifreqHot.Enabled = true;
		m_TextBoxEHPEMPHot.Enabled = true;
		m_TextBoxEHPFusionHot.Enabled = true;
		m_TextBoxEHPPhasedPlasmaHot.Enabled = true;
		m_TextBoxEHPHailHot.Enabled = true;
		FormatResists(m_TextBoxShieldResistsHot, m_FitScanProcessor.ShieldResistsHeated);
		FormatResists(m_TextBoxArmorResistsHot, m_FitScanProcessor.ArmorResistsHeated);
		FormatResists(m_TextBoxHullResistsHot, m_FitScanProcessor.HullResistsHeated);
		FormatEHP(m_TextBoxEHPMjolnirHot, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResistsHeated, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResistsHeated, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResistsHeated, EhpCalculator.AmmoMjolnir));
		FormatEHP(m_TextBoxEHPNovaHot, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResistsHeated, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResistsHeated, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResistsHeated, EhpCalculator.AmmoNova));
		FormatEHP(m_TextBoxEHPAntimatterHot, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResistsHeated, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResistsHeated, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResistsHeated, EhpCalculator.AmmoAntimatter));
		FormatEHP(m_TextBoxEHPVoidHot, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResistsHeated, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResistsHeated, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResistsHeated, EhpCalculator.AmmoVoid));
		FormatEHP(m_TextBoxEHPMultifreqHot, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResistsHeated, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResistsHeated, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResistsHeated, EhpCalculator.AmmoMultifreq));
		FormatEHP(m_TextBoxEHPEMPHot, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResistsHeated, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResistsHeated, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResistsHeated, EhpCalculator.AmmoEMP));
		FormatEHP(m_TextBoxEHPFusionHot, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResistsHeated, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResistsHeated, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResistsHeated, EhpCalculator.AmmoFusion));
		FormatEHP(m_TextBoxEHPPhasedPlasmaHot, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResistsHeated, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResistsHeated, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResistsHeated, EhpCalculator.AmmoPhasedPlasma));
		FormatEHP(m_TextBoxEHPHailHot, EhpCalculator.GetEHP(m_FitScanProcessor.ShieldHP, m_FitScanProcessor.ShieldResistsHeated, m_FitScanProcessor.ArmorHP, m_FitScanProcessor.ArmorResistsHeated, m_FitScanProcessor.HullHP, m_FitScanProcessor.HullResistsHeated, EhpCalculator.AmmoHail));
	}

	private void FormatResists(RichTextBox box, Dictionary<ShipModel.RESIST, float> Resists)
	{
		box.Clear();
		AppendText(box, $"{Resists[ShipModel.RESIST.EM] * 100f:0.0}%", Color.Blue);
		AppendText(box, " / ", Color.Empty);
		AppendText(box, $"{Resists[ShipModel.RESIST.THERMAL] * 100f:0.0}%", Color.Red);
		AppendText(box, " / ", Color.Empty);
		AppendText(box, $"{Resists[ShipModel.RESIST.KINETIC] * 100f:0.0}%", Color.DarkGray);
		AppendText(box, " / ", Color.Empty);
		AppendText(box, $"{Resists[ShipModel.RESIST.EXPLOSIVE] * 100f:0.0}%", Color.Orange);
		box.SelectAll();
		box.SelectionAlignment = HorizontalAlignment.Center;
		box.SelectionLength = 0;
	}

	private void FormatEHP(RichTextBox box, float EHP)
	{
		box.Clear();
		box.AppendText($"{EHP:N0}");
		box.SelectAll();
		box.SelectionAlignment = HorizontalAlignment.Right;
		box.SelectionLength = 0;
	}

	private void AppendText(RichTextBox box, string text, Color color)
	{
		int textLength = box.TextLength;
		box.AppendText(text);
		int textLength2 = box.TextLength;
		box.Select(textLength, textLength2 - textLength);
		box.SelectionColor = color;
		box.SelectionLength = 0;
	}
}
