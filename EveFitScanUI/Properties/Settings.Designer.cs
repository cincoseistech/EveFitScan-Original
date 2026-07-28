using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace EveFitScanUI.Properties;

[CompilerGenerated]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
internal sealed class Settings : ApplicationSettingsBase
{
	private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

	public static Settings Default => defaultInstance;

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public int WindowWidth
	{
		get
		{
			return (int)this["WindowWidth"];
		}
		set
		{
			this["WindowWidth"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public int WindowHeight
	{
		get
		{
			return (int)this["WindowHeight"];
		}
		set
		{
			this["WindowHeight"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public int WindowPositionX
	{
		get
		{
			return (int)this["WindowPositionX"];
		}
		set
		{
			this["WindowPositionX"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public int WindowPositionY
	{
		get
		{
			return (int)this["WindowPositionY"];
		}
		set
		{
			this["WindowPositionY"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool AlwaysOnTop
	{
		get
		{
			return (bool)this["AlwaysOnTop"];
		}
		set
		{
			this["AlwaysOnTop"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool PassiveTank
	{
		get
		{
			return (bool)this["PassiveTank"];
		}
		set
		{
			this["PassiveTank"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool STK
	{
		get
		{
			return (bool)this["STK"];
		}
		set
		{
			this["STK"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public int SysSecurity
	{
		get
		{
			return (int)this["SysSecurity"];
		}
		set
		{
			this["SysSecurity"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool GetPrices
	{
		get
		{
			return (bool)this["GetPrices"];
		}
		set
		{
			this["GetPrices"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("Goonpraisal")]
	public string PriceProvider
	{
		get
		{
			return (string)this["PriceProvider"];
		}
		set
		{
			this["PriceProvider"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string JaniceApiKey
	{
		get
		{
			return (string)this["JaniceApiKey"];
		}
		set
		{
			this["JaniceApiKey"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool ADCActive
	{
		get
		{
			return (bool)this["ADCActive"];
		}
		set
		{
			this["ADCActive"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool Highlight
	{
		get
		{
			return (bool)this["Highlight"];
		}
		set
		{
			this["Highlight"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool ActivateOnFitUpdate
	{
		get
		{
			return (bool)this["ActivateOnFitUpdate"];
		}
		set
		{
			this["ActivateOnFitUpdate"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("10000")]
	public int Manual_EHP
	{
		get
		{
			return (int)this["Manual_EHP"];
		}
		set
		{
			this["Manual_EHP"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("false")]
	public bool Is_Manual_EHP
	{
		get
		{
			return (bool)this["Is_Manual_EHP"];
		}
		set
		{
			this["Is_Manual_EHP"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public int DPS_Mjolnir
	{
		get
		{
			return (int)this["DPS_Mjolnir"];
		}
		set
		{
			this["DPS_Mjolnir"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public int DPS_Nova
	{
		get
		{
			return (int)this["DPS_Nova"];
		}
		set
		{
			this["DPS_Nova"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public int DPS_Antimatter
	{
		get
		{
			return (int)this["DPS_Antimatter"];
		}
		set
		{
			this["DPS_Antimatter"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public int DPS_Void
	{
		get
		{
			return (int)this["DPS_Void"];
		}
		set
		{
			this["DPS_Void"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public int DPS_VoidL
	{
		get
		{
			return (int)this["DPS_VoidL"];
		}
		set
		{
			this["DPS_VoidL"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public int DPS_Multifrequency
	{
		get
		{
			return (int)this["DPS_Multifrequency"];
		}
		set
		{
			this["DPS_Multifrequency"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public int DPS_EMP
	{
		get
		{
			return (int)this["DPS_EMP"];
		}
		set
		{
			this["DPS_EMP"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public int DPS_Fusion
	{
		get
		{
			return (int)this["DPS_Fusion"];
		}
		set
		{
			this["DPS_Fusion"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public int DPS_Phased_Plasma
	{
		get
		{
			return (int)this["DPS_Phased_Plasma"];
		}
		set
		{
			this["DPS_Phased_Plasma"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public int DPS_Hail
	{
		get
		{
			return (int)this["DPS_Hail"];
		}
		set
		{
			this["DPS_Hail"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public double RoF_Mjolnir
	{
		get
		{
			return (double)this["RoF_Mjolnir"];
		}
		set
		{
			this["RoF_Mjolnir"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public double RoF_Nova
	{
		get
		{
			return (double)this["RoF_Nova"];
		}
		set
		{
			this["RoF_Nova"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public double RoF_Antimatter
	{
		get
		{
			return (double)this["RoF_Antimatter"];
		}
		set
		{
			this["RoF_Antimatter"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public double RoF_Void
	{
		get
		{
			return (double)this["RoF_Void"];
		}
		set
		{
			this["RoF_Void"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public double RoF_VoidL
	{
		get
		{
			return (double)this["RoF_VoidL"];
		}
		set
		{
			this["RoF_VoidL"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public double RoF_Multifrequency
	{
		get
		{
			return (double)this["RoF_Multifrequency"];
		}
		set
		{
			this["RoF_Multifrequency"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public double RoF_EMP
	{
		get
		{
			return (double)this["RoF_EMP"];
		}
		set
		{
			this["RoF_EMP"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public double RoF_Fusion
	{
		get
		{
			return (double)this["RoF_Fusion"];
		}
		set
		{
			this["RoF_Fusion"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public double RoF_Phased_Plasma
	{
		get
		{
			return (double)this["RoF_Phased_Plasma"];
		}
		set
		{
			this["RoF_Phased_Plasma"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("0")]
	public double RoF_Hail
	{
		get
		{
			return (double)this["RoF_Hail"];
		}
		set
		{
			this["RoF_Hail"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("Cold")]
	public string PassiveColdHot
	{
		get
		{
			return (string)this["PassiveColdHot"];
		}
		set
		{
			this["PassiveColdHot"] = value;
		}
	}
}
